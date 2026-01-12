using Npgsql;

namespace WebApi.Data;

public class ApplicationDbContext
{
    private readonly string connString = "Host=localhost;Port=5432;Database=exam120126;Username=postgres;Password=2345";

    public NpgsqlConnection Connection()=> new NpgsqlConnection(connString);
}
