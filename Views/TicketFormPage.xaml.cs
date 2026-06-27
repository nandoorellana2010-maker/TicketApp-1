using TicketApp.Services;
using TicketApp.ViewModels;

namespace TicketApp.Views;

public partial class TicketFormPage : ContentPage
{
    public TicketFormPage() : this(new TicketFormViewModel(new TicketService()))
    {
    }

    public TicketFormPage(TicketFormViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}