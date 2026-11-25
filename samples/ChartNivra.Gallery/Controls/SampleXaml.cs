namespace ChartNivra.Gallery.Controls;

/// <summary>XAML host paired with the chart setup shown in the C# tab.</summary>
internal static class SampleXaml
{
    public const string Host = """
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:charts="clr-namespace:ChartNivra.Maui.Controls;assembly=ChartNivra"
             x:Class="MyApp.SamplePage">
    <!-- Configure chartView in SamplePage.xaml.cs using the C# tab. -->
    <charts:ChartView x:Name="chartView"
                      HeightRequest="320" />
</ContentPage>
""";
}
