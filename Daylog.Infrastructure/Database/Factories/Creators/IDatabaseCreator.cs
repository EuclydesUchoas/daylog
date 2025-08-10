namespace Daylog.Infrastructure.Database.Factories.Creators;

public interface IDatabaseCreator
{
    string AdminUsername { get; }
    void CreateDatabase();
    bool ExistsDatabase();
    string? GetConnectionString();
}
