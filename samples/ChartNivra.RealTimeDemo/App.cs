namespace ChartNivra.RealTimeDemo;

using Microsoft.Maui.Controls;

public class App : Application
{
    public App()
    {
        MainPage = new NavigationPage(new MainPage());
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new NavigationPage(new MainPage()))
        {
            Title = "ChartNivra Real-Time Streaming"
        };
    }
}
