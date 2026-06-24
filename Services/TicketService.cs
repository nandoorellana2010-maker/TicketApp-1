using SQLite;
using TicketApp.Models;

namespace TicketApp.Services
{
    public class TicketService
    {
        private SQLiteAsyncConnection _db;

        private async Task Init()
        {
            if (_db is not null) return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "tickets.db3");
            _db = new SQLiteAsyncConnection(dbPath);
            await _db.CreateTableAsync<Ticket>();
        }

        public async Task<List<Ticket>> GetTicketsAsync()
        {
            await Init();
            return await _db.Table<Ticket>().ToListAsync();
        }

        public async Task<Ticket> GetTicketByIdAsync(int id)
        {
            await Init();
            return await _db.FindAsync<Ticket>(id);
        }

        public async Task<int> GuardarTicketAsync(Ticket ticket)
        {
            await Init();

            // Si es nuevo, se inserta
            if (ticket.Id == 0)
            {
                return await _db.InsertAsync(ticket);
            }
            else
            {
                return await _db.UpdateAsync(ticket);
            }
        }

        public async Task<int> EliminarTicketAsync(Ticket ticket)
        {
            await Init();
            return await _db.DeleteAsync(ticket);
        }

        public async Task<int> CambiarEstadoAsync(Ticket ticket, Estado nuevoEstado)
        {
            await Init();

            if (ticket.Estado != nuevoEstado)
            {
                ticket.Estado = nuevoEstado;

                if (nuevoEstado != Estado.Abierto)
                {
                    ticket.FechaUltimoCambioEstado = DateTime.Now;
                }

                return await _db.UpdateAsync(ticket);
            }
            return 0; // 
        }
    }
}
