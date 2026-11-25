namespace ChartNivra.Gallery.Controls;

using Microsoft.Maui.Controls;
using System.Text.RegularExpressions;

public enum CodeLanguage { CSharp, Xaml }

/// <summary>
/// Reusable code-snippet viewer with syntax-styled monospace display and a Copy button.
/// </summary>
public class CodeSnippetView : ContentView
{
    private readonly Label _codeLabel;
    private readonly Label _languageLabel;
    private readonly Label _lineNumbers;
    private string _code = string.Empty;

    private static readonly Color BgCode    = Color.FromArgb("#212430");
    private static readonly Color BgHeader  = Color.FromArgb("#30333F");
    private static readonly Color BorderCol = Color.FromArgb("#414654");
    private static readonly Color TextCode  = Color.FromArgb("#F1F2F5");
    private static readonly Color TextMuted = Color.FromArgb("#93A2B3");
    private static readonly Color AccentGreen = Color.FromArgb("#10B981");
    private static readonly string EditorFont = DeviceInfo.Platform == DevicePlatform.WinUI ? "Consolas"
        : DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.MacCatalyst ? "Menlo"
        : "monospace";

    public static readonly BindableProperty CodeProperty =
        BindableProperty.Create(nameof(Code), typeof(string), typeof(CodeSnippetView), string.Empty,
            propertyChanged: (b, _, n) => ((CodeSnippetView)b).OnCodeChanged((string)n));

    public static readonly BindableProperty LanguageProperty =
        BindableProperty.Create(nameof(Language), typeof(CodeLanguage), typeof(CodeSnippetView), CodeLanguage.CSharp,
            propertyChanged: (b, _, _) => ((CodeSnippetView)b).OnLanguageChanged());

    public string Code
    {
        get => (string)GetValue(CodeProperty);
        set => SetValue(CodeProperty, value);
    }

    public CodeLanguage Language
    {
        get => (CodeLanguage)GetValue(LanguageProperty);
        set => SetValue(LanguageProperty, value);
    }

    public CodeSnippetView()
    {
        _codeLabel = new Label
        {
            FontFamily        = EditorFont,
            FontSize          = 14,
            LineHeight        = 1.4,
            LineBreakMode     = LineBreakMode.NoWrap,
        };
        _lineNumbers = new Label { FontFamily = EditorFont, FontSize = 14, LineHeight = 1.4, TextColor = Color.FromArgb("#758895"), HorizontalTextAlignment = TextAlignment.End, LineBreakMode = LineBreakMode.NoWrap, InputTransparent = true };

        var copyBtn = new Button
        {
            Text              = "⧉  Copy code",
            FontSize          = 13,
            FontAttributes    = FontAttributes.Bold,
            TextColor         = Color.FromArgb("#E2E8F0"),
            BackgroundColor   = Color.FromArgb("#414654"),
            BorderColor       = Color.FromArgb("#596171"),
            BorderWidth       = 1,
            CornerRadius      = 8,
            Padding           = new Thickness(14, 0),
            HeightRequest     = 36,
        };
        copyBtn.Clicked += OnCopyClicked;

        var header = new Grid
        {
            Padding         = new Thickness(18, 12),
            BackgroundColor = BgHeader,
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto },
            },
        };
        var dots = new HorizontalStackLayout { Spacing = 6, VerticalOptions = LayoutOptions.Center, Margin = new Thickness(0, 0, 14, 0) };
        foreach (var color in new[] { "#FF605C", "#FFBD44", "#00CA4E" })
            dots.Add(new BoxView { WidthRequest = 9, HeightRequest = 9, CornerRadius = 5, Color = Color.FromArgb(color) });
        _languageLabel = new Label { Text = "Sample.cs", FontFamily = EditorFont, FontSize = 13, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#E2E8F0"), VerticalOptions = LayoutOptions.Center };
        header.Add(dots, 0, 0);
        header.Add(_languageLabel, 1, 0);
        header.Add(copyBtn, 2, 0);

        var contentGrid = new Grid
        {
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Auto }, new RowDefinition { Height = GridLength.Auto }, new RowDefinition { Height = GridLength.Star },
            }
        };
        Content = new Border { StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 12 }, Stroke = BorderCol, StrokeThickness = 1, BackgroundColor = BgCode, Content = contentGrid };
        contentGrid.Add(header, 0, 0);
        contentGrid.Add(new BoxView { HeightRequest = 1, BackgroundColor = BorderCol }, 0, 1);
        contentGrid.Add(new ScrollView
        {
            Orientation = ScrollOrientation.Both,
            Content = new Grid
            {
                Padding = new Thickness(24, 22, 28, 24),
                ColumnSpacing = 24,
                ColumnDefinitions = new ColumnDefinitionCollection { new() { Width = GridLength.Auto }, new() { Width = GridLength.Auto } },
                Children = { _lineNumbers, _codeLabel }
            }
        }, 0, 2);
        Grid.SetColumn(_codeLabel, 1);
    }

    private void OnCodeChanged(string newCode)
    {
        _code = newCode ?? string.Empty;
        _lineNumbers.Text = string.Join("\n", Enumerable.Range(1, _code.Count(c => c == '\n') + 1).Select(number => number.ToString("00")));
        ApplySyntaxHighlighting();
    }

    private void OnLanguageChanged()
    {
        _languageLabel.Text = Language == CodeLanguage.Xaml ? "Sample.xaml" : "Sample.cs";
        ApplySyntaxHighlighting();
    }

    private void ApplySyntaxHighlighting()
    {
        var formatted = new FormattedString();
        var pattern = Language == CodeLanguage.Xaml
            ? "(<!--.*?-->)|(\"[^\"]*\"|'[^']*')|(</?[A-Za-z_][\\w:.-]*)|([A-Za-z_][\\w:.-]*(?=\\s*=))|(/?>)"
            : "(//.*$)|(\"[^\"]*\")|\\b(var|new|class|public|private|protected|return|using|namespace|true|false|null|void)\\b";
        var matches = Regex.Matches(_code, pattern, RegexOptions.Multiline);
        var position = 0;
        foreach (Match match in matches)
        {
            if (match.Index > position) formatted.Spans.Add(new Span { Text = _code[position..match.Index], TextColor = TextCode });
            var color = Language == CodeLanguage.Xaml
                ? match.Value.StartsWith("<!--") ? TextMuted
                    : match.Value.StartsWith("\"") || match.Value.StartsWith("'") ? Color.FromArgb("#E6E990")
                    : match.Value.StartsWith("<") || match.Value is ">" or "/>" ? Color.FromArgb("#F48ACB")
                    : Color.FromArgb("#70E7A0")
                : (match.Value.StartsWith("//") ? TextMuted : match.Value.StartsWith("\"") ? Color.FromArgb("#E6E990") : Color.FromArgb("#C4B5FD"));
            formatted.Spans.Add(new Span { Text = match.Value, TextColor = color });
            position = match.Index + match.Length;
        }
        if (position < _code.Length) formatted.Spans.Add(new Span { Text = _code[position..], TextColor = TextCode });
        _codeLabel.FormattedText = formatted;
    }

    private async void OnCopyClicked(object? sender, EventArgs e)
    {
        await Clipboard.Default.SetTextAsync(_code);
        if (sender is Button btn)
        {
            var original = btn.Text;
            btn.Text      = "✓  Copied!";
            btn.TextColor = AccentGreen;
            await Task.Delay(1800);
            btn.Text      = original;
            btn.TextColor = Color.FromArgb("#E2E8F0");
        }
    }
}
