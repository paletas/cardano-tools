using Dapper;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Silvestre.Cardano.Integration.DbSyncAPI.Database.Queries
{
    internal static class SetupQueries
    {
        private static readonly Version _CurrentVersion = new(1, 0, 0);

        private record DatabaseVersion(short Major, short Minor, short Revision);

        public static async Task<bool> RequiresUpgradeAsync(this DbConnection dbConnection)
        {
            const string CreateUpgradeTableQuery =
                @"CREATE TABLE IF NOT EXISTS public.silvestretool_upgrades
                (
                    version_major smallint NOT NULL,
                    version_minor smallint NOT NULL,
                    version_revision smallint NOT NULL,
                    ""when"" date NOT NULL
                )

                TABLESPACE pg_default;";

            const string CheckCurrentVersionQuery =
                @"SELECT version_major Major, version_minor Minor, version_revision Revision FROM public.silvestretool_upgrades";

            await dbConnection.ExecuteAsync(CreateUpgradeTableQuery).ConfigureAwait(false);
            var databaseVersion = await dbConnection.QuerySingleOrDefaultAsync<DatabaseVersion>(CheckCurrentVersionQuery);
            var currentVersion = new Version(databaseVersion?.Major ?? 0, databaseVersion?.Minor ?? 0, databaseVersion?.Revision ?? 0);

            return databaseVersion == null || currentVersion < _CurrentVersion;
        }

        public static async Task UpgradeDatabase(this DbConnection dbConnection)
        {
            const string UpgradeIndexesQuery =
                @"DROP INDEX public.idx_reward_block_id;
                CREATE INDEX idx_reward_block_id
                    ON public.reward USING btree
                    (block_id ASC NULLS LAST)
                    INCLUDE(amount)
                    TABLESPACE pg_default;

                DROP INDEX public.idx_tx_in_tx_out_id;
                CREATE INDEX idx_tx_in_tx_out_id
                    ON public.tx_in USING btree
                    (tx_out_id ASC NULLS LAST)
	                INCLUDE (tx_out_index)
                    TABLESPACE pg_default;

                DELETE FROM public.silvestretool_upgrades;
                INSERT INTO public.silvestretool_upgrades (version_major, version_minor, version_revision, ""when"") VALUES (@VersionMajor, @VersionMinor, @VersionRevision, @When);";

            await dbConnection.ExecuteAsync(UpgradeIndexesQuery, new { VersionMajor = _CurrentVersion.Major, VersionMinor = _CurrentVersion.Minor, VersionRevision = _CurrentVersion.Build, When = DateTime.Now }, commandTimeout: 500).ConfigureAwait(false);
        }
    }
}
