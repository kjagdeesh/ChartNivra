namespace ChartNivra.Gallery.Controls;

/// <summary>Shared breakpoints for gallery pages. Values are in device-independent units.</summary>
public static class ResponsiveLayout
{
    // 600 keeps the layout comfortable on phones in either orientation while
    // allowing tablets and desktop windows to retain their denser layout.
    public const double CompactWidth = 600;

    public static bool IsCompact(double width) => width > 0 && width < CompactWidth;
    public static Thickness PageMargin(bool compact) => compact
        ? new Thickness(12, 12)
        : new Thickness(24, 20);
}
