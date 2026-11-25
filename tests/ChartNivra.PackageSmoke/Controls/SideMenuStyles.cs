namespace ChartNivra.Gallery.Controls;

using Microsoft.Maui.Graphics;

/// <summary>Centralized styling constants for the fixed minimalist side menu - Light theme.</summary>
public static class SideMenuStyles
{
    // ── Colors (Light Theme) ──────────────────────────────────────────────
    public static readonly Color Background = Color.FromArgb("#FFFFFF");
    public static readonly Color BackgroundHover = Color.FromArgb("#F1F5F9");
    public static readonly Color BorderSubtle = Color.FromArgb("#E2E8F0");
    public static readonly Color BorderFocus = Color.FromArgb("#CBD5E1");
    public static readonly Color TextPrimary = Color.FromArgb("#0F172A");
    public static readonly Color TextSecondary = Color.FromArgb("#475569");
    public static readonly Color TextMuted = Color.FromArgb("#94A3B8");
    public static readonly Color Accent = Color.FromArgb("#6366F1");
    public static readonly Color AccentHover = Color.FromArgb("#4F46E5");
    public static readonly Color AccentSubtle = Color.FromArgb("#EEF2FF");

    // ── Dimensions ────────────────────────────────────────────────────────
    public const double SidebarWidth = 260;
    public const double MenuItemHeight = 40;
    public const double CategoryHeaderHeight = 28;
    public const double IconSize = 18;
    public const double CornerRadius = 6;

    // ── Spacing ───────────────────────────────────────────────────────────
    public const double PaddingSmall = 8;
    public const double PaddingMedium = 16;
    public const double PaddingLarge = 20;
    public const double SpacingItems = 2;
    public const double SpacingSections = 16;

    // ── Typography ────────────────────────────────────────────────────────
    public const double FontSizeTitle = 13;
    public const double FontSizeHeader = 11;
    public const double FontSizeCaption = 10;

    // ── Animation ─────────────────────────────────────────────────────────
    public const uint AnimationDuration = 150;
}
