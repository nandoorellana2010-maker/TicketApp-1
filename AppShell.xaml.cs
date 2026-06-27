using TicketApp.Views;

namespace TicketApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(TicketFormPage), typeof(TicketFormPage));
    }
}