using cw11.Data;
using cw11.Dbo;
using cw11.Models;
using Microsoft.EntityFrameworkCore;

namespace cw11.Service;

public class TripsService : ITripsService
{
    private readonly EfdatabaseContext _databaseContext;

    public TripsService(EfdatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<tripsGET> getTripsSortedByDate(int page = 1, int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var totalTrips = await _databaseContext.Trips.CountAsync();
        var allPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

        var trips = await _databaseContext.Trips
            .OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TripDetails
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(c => new CountriesDetails
                {
                    Name = c.Name,
                }).ToList(),
                Clients = t.ClientTrips.Select(client => new ClientsDetails
                {
                    FirstName = client.IdClientNavigation.FirstName,
                    LastName = client.IdClientNavigation.LastName,
                }).ToList()
            })
            .ToListAsync();

        return new tripsGET
        {
            pageNum = page,
            pageSize = pageSize,
            allPages = allPages,
            trips = trips
        };
    }
    public async Task PutClientIntoTrip(int IdTrip ,clientPOST clientPOST)
    {
        var peselExists = await _databaseContext.Clients.FirstOrDefaultAsync(C => C.Pesel == clientPOST.Pesel);
        if (peselExists != null)
        {
            throw new ConflictEX("Klient o takim peselu juz istnieje");
        }

        var client = new Client
        {
            FirstName = clientPOST.FirstName,
            LastName = clientPOST.LastName,
            Email = clientPOST.Email,
            Telephone = clientPOST.Telephone,
            Pesel = clientPOST.Pesel,
        };
        _databaseContext.Clients.Add(client);
        await _databaseContext.SaveChangesAsync();
        
        var tripName = await _databaseContext.Trips.AnyAsync(t => t.Name == clientPOST.TripName);
        if (!tripName)
        {
            throw new NotFoundEx("Nie istnieje wycieckza o podanej nazwie");
        }
        
        var trip = await _databaseContext.Trips.FindAsync(IdTrip);
        if (trip == null || trip.DateFrom <= DateTime.UtcNow)
        {
            throw new NotFoundEx("Wycieczka nie sitnieje lub sie odbyla");
        }

        var cleintTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = IdTrip,
            RegisteredAt = DateTime.UtcNow,
            PaymentDate = clientPOST.PaymentDate
        };
        _databaseContext.ClientTrips.Add(cleintTrip);
        await _databaseContext.SaveChangesAsync();
    }
}