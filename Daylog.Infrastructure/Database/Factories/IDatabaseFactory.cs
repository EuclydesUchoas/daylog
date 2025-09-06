using Daylog.Application.Shared.Enums;
using Daylog.Infrastructure.Database.Factories.Creators;

namespace Daylog.Infrastructure.Database.Factories;

public interface IDatabaseFactory
{
    void StartDatabase();

    IDatabaseCreator? GetDatabaseCreator();

    string? GetConnectionString();

    DatabaseProviderEnum GetDatabaseProvider();

    void RunMigrations(bool migrateUp = true);
}
