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
    private string _code = string.Empty;

    private static readonly Color BgCode    = Color.FromArgb("#0D1117");
    private static readonly Color BgHeader  = Color.FromArgb("#161B22");
    private static readonly Color BorderCol = Color.FromArgb("#30363D");
    private static readonly Color TextCode  = Color.FromArgb("#E6EDF3");
    private static readonly Color TextMuted = Color.FromArgb("#8B949E");
    private static readonly Color AccentGreen = Color.FromArgb("#10B981");

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
            FontFamily        = "Courier New",
            FontSize          = 12,
            LineBreakMode     = LineBreakMode.NoWrap,
        };

        var copyBtn = new Button
        {
            Text              = "⎘  Copy",
            FontSize          = 12,
            TextColor         = TextMuted,
            BackgroundColor   = Colors.Transparent,
            BorderColor       = BorderCol,
            BorderWidth       = 1,
            CornerRadius      = 6,
            Padding           = new Thickness(10, 4),
            HeightRequest     = 32,
        };
        copyBtn.Clicked += OnCopyClicked;

        var header = new Grid
        {
            Padding         = new Thickness(16, 10),
            BackgroundColor = BgHeader,
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto },
            },
        };
        _languageLabel = new Label { Text = "C#  ·  SOURCE", FontSize = 11, FontAttributes = FontAttributes.Bold, CharacterSpacing = 1, TextColor = Color.FromArgb("#7DD3FC"), VerticalOptions = LayoutOptions.Center };
        header.Add(_languageLabel, 0, 0);
        header.Add(copyBtn, 1, 0);

        Content = new Frame
        {
            CornerRadius    = 10,
            Padding         = new Thickness(0),
            HasShadow       = false,
            BorderColor     = BorderCol,
            BackgroundColor = BgCode,
            Content = new Grid
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = GridLength.Auto }, new RowDefinition { Height = GridLength.Auto }, new RowDefinition { Height = GridLength.Star },
                }
            }
        };
        var contentGrid = (Grid)((Frame)Content).Content;
        contentGrid.Add(header, 0, 0);
        contentGrid.Add(new BoxView { HeightRequest = 1, BackgroundColor = BorderCol }, 0, 1);
        contentGrid.Add(new ScrollView
        {
            Orientation = ScrollOrientation.Both,
            Content = new VerticalStackLayout
            {
                Padding = new Thickness(16, 12),
                Children = { _codeLabel }
            }
        }, 0, 2);
    }

    private void OnCodeChanged(string newCode)
    {
        _code = newCode ?? string.Empty;
        ApplySyntaxHighlighting();
    }

    private void OnLanguageChanged()
    {
        _languageLabel.Text = Language == CodeLanguage.Xaml ? "XAML  ·  MARKUP" : "C#  ·  SOURCE";
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
                    : match.Value.StartsWith("\"") || match.Value.StartsWith("'") ? Color.FromArgb("#A7F3D0")
                    : match.Value.StartsWith("<") || match.Value is ">" or "/>" ? Color.FromArgb("#7DD3FC")
                    : Color.FromArgb("#C4B5FD")
                : (match.Value.StartsWith("//") ? TextMuted : match.Value.StartsWith("\"") ? Color.FromArgb("#A7F3D0") : Color.FromArgb("#C4B5FD"));
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
            btn.TextColor = Color.FromArgb("#8B949E");
        }
    }
}
