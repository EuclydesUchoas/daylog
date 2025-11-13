namespace Daylog.Infrastructure.Database.Factories.Creators;

public interface IDatabaseCreator
{
    void CreateDatabase();

    bool ExistsDatabase();

    string? GetConnectionString();
}
