namespace Daylog.Infrastructure.Database.Factories.Creators;

public interface IDatabaseCreator
{
    string AdminUsername { get; }

    [Obsolete("Use IAppDbContext.CreateDatabaseIfNotExists instead.")]
    void CreateDatabase();

    [Obsolete("Use IAppDbContext.DatabaseExists instead.")]
    bool ExistsDatabase();

    string? GetConnectionString();
}
