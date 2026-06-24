using SQLite;

namespace TicketApp.Models
{
    public enum Prioridad
    {
        Baja,
        Media,
        Alta
    }

    public enum Estado
    {
        Abierto,
        EnProceso,
        Resuelto,
        Cerrado
    }

    public class Ticket
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        [NotNull]
        public string Descripcion { get; set; } = string.Empty;

        [NotNull]
        public Prioridad Prioridad { get; set; }

        [NotNull]
        public Estado Estado { get; set; } = Estado.Abierto;

        public string Responsable { get; set; } = string.Empty;

        [NotNull]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaUltimoCambioEstado { get; set; }
    }
}
