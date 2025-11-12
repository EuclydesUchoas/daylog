using Daylog.Infrastructure.Database.Factories.Creators;
using Daylog.Shared.Enums;

namespace Daylog.Infrastructure.Database.Factories;

public interface IDatabaseFactory
{
    void StartDatabase(bool legacyMode = false);

    IDatabaseCreator? GetDatabaseCreator();

    string? GetConnectionString();

    DatabaseProviderEnum GetDatabaseProvider();

    void RunMigrations(bool migrateUp = true);
}
