using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicketApp.Models;
using TicketApp.Services;
using TicketApp.Views;

namespace TicketApp.ViewModels;

public partial class TicketsViewModel : ObservableObject
{
    private readonly TicketService _ticketService;

    [ObservableProperty]
    private ObservableCollection<Ticket> tickets = new();

    [ObservableProperty]
    private bool isBusy;

    public TicketsViewModel(TicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [RelayCommand]
    public async Task CargarTicketsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            var lista = await _ticketService.GetTicketsAsync();

            Tickets.Clear();

            foreach (var ticket in lista)
            {
                Tickets.Add(ticket);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task NuevoTicketAsync()
    {
        await Shell.Current.GoToAsync(nameof(TicketFormPage));
    }

    [RelayCommand]
    public async Task EditarTicketAsync(Ticket ticket)
    {
        if (ticket == null)
            return;

        await Shell.Current.GoToAsync($"{nameof(TicketFormPage)}?ticketId={ticket.Id}");
    }

    [RelayCommand]
    public async Task EliminarTicketAsync(Ticket ticket)
    {
        if (ticket == null)
            return;

        bool confirmar = await Shell.Current.DisplayAlert(
            "Confirmar eliminación",
            $"¿Deseas eliminar el ticket: {ticket.Titulo}?",
            "Sí",
            "No"
        );

        if (!confirmar)
            return;

        await _ticketService.EliminarTicketAsync(ticket);
        await CargarTicketsAsync();
    }

    [RelayCommand]
    public async Task CambiarEstadoAsync(Ticket ticket)
    {
        if (ticket == null)
            return;

        Estado nuevoEstado = ObtenerSiguienteEstado(ticket.Estado);

        if (nuevoEstado == ticket.Estado)
        {
            await Shell.Current.DisplayAlert(
                "Ticket cerrado",
                "Este ticket ya está cerrado.",
                "OK"
            );
            return;
        }

        await _ticketService.CambiarEstadoAsync(ticket, nuevoEstado);
        await CargarTicketsAsync();
    }

    private Estado ObtenerSiguienteEstado(Estado estadoActual)
    {
        return estadoActual switch
        {
            Estado.Abierto => Estado.EnProceso,
            Estado.EnProceso => Estado.Resuelto,
            Estado.Resuelto => Estado.Cerrado,
            Estado.Cerrado => Estado.Cerrado,
            _ => Estado.Abierto
        };
    }
}