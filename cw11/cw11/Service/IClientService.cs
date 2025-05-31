using cw11.Dbo;

namespace cw11.Service;

public interface IClientService
{
    Task DeleteClientIfHaveNotTrips(int IdClient);
    
}