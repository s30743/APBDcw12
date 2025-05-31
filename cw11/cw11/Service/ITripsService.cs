using cw11.Dbo;

namespace cw11.Service;

public interface ITripsService
{
    Task<tripsGET> getTripsSortedByDate(int page = 1, int pageSize = 10);
    Task PutClientIntoTrip(int IdTrip,clientPOST clientPOST);
}