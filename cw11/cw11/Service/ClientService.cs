using cw11.Data;
using cw11.Dbo;
using cw11.Models;
using Microsoft.EntityFrameworkCore;

namespace cw11.Service;

public class ClientService : IClientService
{
    private readonly EfdatabaseContext _context;

    public ClientService(EfdatabaseContext context)
    {
        _context = context;
    }

    public async Task DeleteClientIfHaveNotTrips(int IdClient)
    {
        var client = await _context.Clients.FindAsync(IdClient);
        if (client == null)
        {
            throw new NotFoundEx("Nie istnieje klient o podanym ID");
        }
        
        bool trips = await _context.ClientTrips.AnyAsync(c => c.IdClient == IdClient);
        if (trips)
        {
            throw new ConflictEX("nei mozna usunac klienta ktory posiada zabookowane wycieczki");
        }
        
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        
    }

    
}