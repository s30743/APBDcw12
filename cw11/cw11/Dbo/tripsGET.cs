namespace cw11.Dbo;

public class tripsGET
{
    public int pageNum { get; set; }
    public int pageSize { get; set; }
    public int allPages { get; set; }
    public ICollection<TripDetails> trips { get; set; }
}

public class TripDetails
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public ICollection<CountriesDetails> Countries { get; set; }
    public ICollection<ClientsDetails> Clients { get; set; }
    
}

public class CountriesDetails
{
    public string Name { get; set; }
}

public class ClientsDetails
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}