namespace Silvestre.Cardano.Integration.DbSyncAPI.Database
{
    public static class DatabaseConfigurationExtensions
    {
        public static void SetupDatabase(this IApplicationBuilder applicationBuilder)
        {
            var databaseProxy = (DatabaseProxy)applicationBuilder.ApplicationServices.GetService(typeof(DatabaseProxy));

            if (databaseProxy.RequiresDatabaseUpgrade().GetAwaiter().GetResult())
                databaseProxy.UpgradeDatabase().GetAwaiter().GetResult();
        }
    }
}
