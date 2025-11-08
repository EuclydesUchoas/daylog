var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("daylog-postgres")
    .WithDataVolume()
    .WithPgAdmin()
    .AddDatabase("daylog-postgres-db", "daylog");

var sqlServer = builder.AddSqlServer("daylog-sqlserver")
    .WithDataVolume()
    .AddDatabase("daylog-sqlserver-db", "daylog");

builder.AddProject<Projects.Daylog_Api>("daylog-api")
    .WithReference(postgres)
    .WithReference(sqlServer);

builder.Build().Run();
