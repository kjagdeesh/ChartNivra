# Fixed Side Menu Design

## 🎯 Overview

Professional fixed side menu with adaptive responsiveness:
- **Desktop (≥1200px)**: Full-width sidebar (280px) with labels + icons
- **Tablet (768px-1200px)**: Collapsed sidebar (72px) showing icons only with tooltips
- **Mobile (<768px)**: Hidden sidebar with drawer toggle

---

## 📐 Architecture

### Components

**1. SideMenuStyles.cs** - Centralized design tokens
```csharp
Colors, Dimensions, Spacing, Breakpoints, Animation timings
```

**2. SideMenuItem.cs** - Individual menu item control
- Responsive icon + label layout
- Selected state with accent color
- Tap gesture handling
- Expansion/collapse support

**3. SideMenuLayout.cs** - Main sidebar container
- Header with logo, title, subtitle
- Scrollable menu content area
- Footer with copyright
- Responsive width adjustment
- Menu section grouping

**4. AppShellWithSideMenu.cs** - Integration page
- Grid layout: Sidebar | Content
- Lazy page loading
- Navigation routing

---

## 🎨 Visual Design

### Color Palette
```
Background:     #0F172A (Dark blue - main)
                #0A0F1E (Darker - header/footer)
                #1E293B (Card gray)
Text Primary:   #F8FAFC (Off-white)
Text Secondary: #94A3B8 (Gray-blue)
Accent:         #6366F1 (Indigo)
Accent Hover:   #4F46E5
Accent Active:  #4338CA
Border:         #334155 (Subtle gray)
```

### Typography
- **Logo**: 20px Bold
- **Menu Header**: 11px Bold
- **Menu Item**: 14px Regular
- **Footer**: 10px Regular

### Spacing System
```
Small:   8px
Medium: 16px
Large:  20px
```

---

## 📱 Responsive Behavior

### Desktop (≥1200px)
```
┌─────────────────────────┐
│280px │      Content     │
│      │                  │
│ ---- │                  │
│ Menu │                  │
│ ---- │                  │
└─────────────────────────┘
```
- Full labels visible
- Fixed position
- Scroll content only
- Icon + text visible

### Tablet (768px - 1200px)
```
┌──────────────────┐
│72px│  Content    │
│    │             │
│----|             │
│ ■■ │             │
│ ■■ │             │
└──────────────────┘
```
- Icons only
- Tooltips on hover (future)
- Compact 72px width
- Labels hidden

### Mobile (<768px)
```
┌───────────────────┐
│ ☰    Title  ⚙️    │
├───────────────────┤
│    Content        │
│                   │
└───────────────────┘
```
- Sidebar hidden (drawer)
- Menu accessed via hamburger icon
- Full screen content
- Overlay on nav open

---

## 🔧 Usage

### Basic Setup

1. **Replace AppShell in App.cs**:
```csharp
protected override Window CreateWindow(IActivationState? activationState)
{
    return new Window(new AppShellWithSideMenu())
    {
        Title = "ChartNivra Gallery"
    };
}
```

2. **Menu items are auto-added** via `AddMenuSection()` calls

### Adding Custom Sections

```csharp
_sideMenu.AddMenuSection("📈  MY CHARTS",
    ("📉", "Line Chart", new Command(() => NavigateTo(typeof(LineChartPage)))),
    ("📊", "Bar Chart", new Command(() => NavigateTo(typeof(BarChartPage))))
);
```

---

## 🎯 Key Features

✅ **Adaptive Layout**
- Auto-adjusts on resize
- SizeChanged event handler
- Smooth transitions

✅ **Semantic Sections**
- Category headers
- Visual dividers
- Clear grouping

✅ **Accessible**
- Clear visual hierarchy
- High contrast colors
- Proper spacing

✅ **Performant**
- Lazy page instantiation
- Scrollable menu only
- Minimal redraws

✅ **Professional**
- Modern color scheme
- Consistent spacing
- Refined typography

---

## 📋 Integration Checklist

- [ ] Copy SideMenuStyles.cs
- [ ] Copy SideMenuItem.cs
- [ ] Copy SideMenuLayout.cs
- [ ] Copy AppShellWithSideMenu.cs
- [ ] Update App.cs to use AppShellWithSideMenu
- [ ] Add page references in BuildNavigation()
- [ ] Test on desktop/tablet/mobile
- [ ] Add tooltips for tablet mode (optional)
- [ ] Add animations on selection (optional)
- [ ] Customize colors in SideMenuStyles

---

## 🚀 Future Enhancements

1. **Tooltips** - Show full label on hover in collapsed mode
2. **Animations** - Smooth expand/collapse transitions
3. **Search** - Command palette for quick navigation
4. **History** - Recently visited pages
5. **Customization** - Theme switcher in menu
6. **Persistence** - Save expanded/collapsed state
7. **Keyboard Nav** - Arrow keys to navigate menu

---

## 📐 Dimensions Reference

```
Desktop Sidebar Width:        280px
Collapsed Sidebar Width:       72px
Menu Item Height:              44px
Category Header Height:        32px
Icon Size:                     20px
Corner Radius:                  8px
Header Height (approx):       ~120px
Footer Height (approx):       ~60px
```

---

## 🎨 Color Reference

| Use | Color | Hex |
|-----|-------|-----|
| Background | Dark Blue | #0F172A |
| Header/Footer BG | Darker | #0A0F1E |
| Cards | Gray | #1E293B |
| Text | Light | #F8FAFC |
| Muted Text | Gray-Blue | #94A3B8 |
| Accent | Indigo | #6366F1 |
| Accent Hover | Indigo Dark | #4F46E5 |
| Borders | Subtle | #334155 |

---

## 📞 Notes

- All sizes/colors are in `SideMenuStyles.cs` for easy customization
- Breakpoints can be adjusted in SideMenuStyles
- Menu items use Command pattern for loose coupling
- Page instantiation happens on demand
