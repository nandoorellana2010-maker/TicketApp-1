using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicketApp.Models;
using TicketApp.Services;

namespace TicketApp.ViewModels;

[QueryProperty(nameof(TicketId), "ticketId")]
public partial class TicketFormViewModel : ObservableObject
{
    private readonly TicketService _ticketService;
    private Ticket? ticketActual;

    public List<Prioridad> Prioridades { get; } = new()
    {
        Prioridad.Baja,
        Prioridad.Media,
        Prioridad.Alta
    };

    public List<Estado> Estados { get; } = new()
    {
        Estado.Abierto,
        Estado.EnProceso,
        Estado.Resuelto,
        Estado.Cerrado
    };

    [ObservableProperty]
    private string ticketId = string.Empty;

    [ObservableProperty]
    private string titulo = string.Empty;

    [ObservableProperty]
    private string descripcion = string.Empty;

    [ObservableProperty]
    private Prioridad prioridad = Prioridad.Media;

    [ObservableProperty]
    private Estado estado = Estado.Abierto;

    [ObservableProperty]
    private string responsable = string.Empty;

    [ObservableProperty]
    private bool esEdicion;

    public TicketFormViewModel(TicketService ticketService)
    {
        _ticketService = ticketService;
    }

    partial void OnTicketIdChanged(string value)
    {
        if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, out int id))
        {
            _ = CargarTicketAsync(id);
        }
    }

    private async Task CargarTicketAsync(int id)
    {
        var ticket = await _ticketService.GetTicketByIdAsync(id);

        if (ticket == null)
            return;

        ticketActual = ticket;

        Titulo = ticket.Titulo;
        Descripcion = ticket.Descripcion;
        Prioridad = ticket.Prioridad;
        Estado = ticket.Estado;
        Responsable = ticket.Responsable;
        EsEdicion = true;
    }

    [RelayCommand]
    public async Task GuardarTicketAsync()
    {
        if (string.IsNullOrWhiteSpace(Titulo))
        {
            await Shell.Current.DisplayAlert("Validación", "El título es obligatorio.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(Descripcion))
        {
            await Shell.Current.DisplayAlert("Validación", "La descripción es obligatoria.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(Responsable))
        {
            await Shell.Current.DisplayAlert("Validación", "El responsable es obligatorio.", "OK");
            return;
        }

        if (ticketActual == null)
        {
            var nuevoTicket = new Ticket
            {
                Titulo = Titulo,
                Descripcion = Descripcion,
                Prioridad = Prioridad,
                Estado = Estado.Abierto,
                Responsable = Responsable,
                FechaCreacion = DateTime.Now,
                FechaUltimoCambioEstado = null
            };

            await _ticketService.GuardarTicketAsync(nuevoTicket);
        }
        else
        {
            bool cambioEstado = ticketActual.Estado != Estado;

            ticketActual.Titulo = Titulo;
            ticketActual.Descripcion = Descripcion;
            ticketActual.Prioridad = Prioridad;
            ticketActual.Responsable = Responsable;

            if (cambioEstado)
            {
                await _ticketService.CambiarEstadoAsync(ticketActual, Estado);
            }
            else
            {
                await _ticketService.GuardarTicketAsync(ticketActual);
            }
        }

        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    public async Task CancelarAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}