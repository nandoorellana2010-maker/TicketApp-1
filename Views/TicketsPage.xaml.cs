using TicketApp.Services;
using TicketApp.ViewModels;

namespace TicketApp.Views;

public partial class TicketsPage : ContentPage
{
    private readonly TicketsViewModel _viewModel;

    public TicketsPage() : this(new TicketsViewModel(new TicketService()))
    {
    }

    public TicketsPage(TicketsViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.CargarTicketsAsync();
    }
}