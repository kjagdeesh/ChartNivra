namespace ChartNivra.Demo;

using Microsoft.Maui.Controls;

public class App : Application
{
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new NavigationPage(new MainPage()))
        {
            Title = "ChartNivra Gallery"
        };
    }
}
