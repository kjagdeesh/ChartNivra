namespace ChartNivra.Gallery;

using Microsoft.Maui.Controls;

public class App : Application
{
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell())
        {
            Title = "ChartNivra Gallery"
        };
    }
}
