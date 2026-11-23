namespace ChartNivra.Gallery.Controls;

using Microsoft.Maui.Controls;

/// <summary>
/// Reusable code-snippet viewer with syntax-styled monospace display and a Copy button.
/// </summary>
public class CodeSnippetView : ContentView
{
    private readonly Label _codeLabel;
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

    public string Code
    {
        get => (string)GetValue(CodeProperty);
        set => SetValue(CodeProperty, value);
    }

    public CodeSnippetView()
    {
        _codeLabel = new Label
        {
            FontFamily        = "Courier New",
            FontSize          = 12,
            TextColor         = TextCode,
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
        var langLabel = new Label { Text = "C#", FontSize = 12, TextColor = TextMuted, VerticalOptions = LayoutOptions.Center };
        header.Add(langLabel, 0, 0);
        header.Add(copyBtn, 1, 0);

        Content = new Frame
        {
            CornerRadius    = 10,
            Padding         = new Thickness(0),
            HasShadow       = false,
            BorderColor     = BorderCol,
            BackgroundColor = BgCode,
            Content = new VerticalStackLayout
            {
                Spacing = 0,
                Children =
                {
                    header,
                    new BoxView { HeightRequest = 1, BackgroundColor = BorderCol },
                    new ScrollView
                    {
                        Orientation = ScrollOrientation.Both,
                        Content     = new VerticalStackLayout
                        {
                            Padding  = new Thickness(16, 12),
                            Children = { _codeLabel }
                        }
                    }
                }
            }
        };
    }

    private void OnCodeChanged(string newCode)
    {
        _code = newCode ?? string.Empty;
        _codeLabel.Text = _code;
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
