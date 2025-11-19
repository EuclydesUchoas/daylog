using Daylog.Infrastructure.Database.Factories.Creators;
using Daylog.Shared.Data.Enums;

namespace Daylog.Infrastructure.Database.Factories;

public interface IDatabaseFactory
{
    void StartDatabase(DatabaseStarterStrategyEnum strategy = DatabaseStarterStrategyEnum.DefaultCreator);

    IDatabaseCreator? GetDatabaseCreator();

    string? GetConnectionString();

    DatabaseProviderEnum GetDatabaseProvider();

    void RunMigrations(bool migrateUp = true);
}
