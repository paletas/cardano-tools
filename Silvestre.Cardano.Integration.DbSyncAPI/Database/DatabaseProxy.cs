using Silvestre.Cardano.Integration.DbSyncAPI.Database.Model;
using Silvestre.Cardano.Integration.DbSyncAPI.Database.Queries;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Silvestre.Cardano.Integration.DbSyncAPI
{
    internal class DatabaseProxy
    {
        private readonly DbProviderFactory _dbConnectionFactory;
        private readonly string _connectionString;

        public DatabaseProxy(DbProviderFactory connectionFactory, string connectionString)
        {
            this._dbConnectionFactory = connectionFactory;
            this._connectionString = connectionString;
        }

        private DbConnection GetConnection()
        {
            var connection = _dbConnectionFactory.CreateConnection();
            connection.ConnectionString = this._connectionString;
            return connection;
        }

        public Task<bool> RequiresDatabaseUpgrade()
        {
            return SetupQueries.RequiresUpgradeAsync(GetConnection());
        }

        public Task UpgradeDatabase()
        {
            return SetupQueries.UpgradeDatabase(GetConnection());
        }

        public Task<Epoch> GetCurrentEpoch(CancellationToken cancellationToken)
        {
            return EpochQueries.GetCurrentEpoch(this.GetConnection(), cancellationToken);
        }

        public Task<Epoch> GetEpoch(CancellationToken cancellationToken, uint epochNumber)
        {
            return EpochQueries.GetEpoch(this.GetConnection(), cancellationToken, epochNumber);
        }

        public Task<Block> GetLatestBlock(CancellationToken cancellationToken, uint epochNumber)
        {
            return BlockQueries.GetLatestBlockForEpoch(this.GetConnection(), cancellationToken, epochNumber);
        }

        public Task<Block> GetLatestBlock(CancellationToken cancellationToken)
        {
            return BlockQueries.GetLatestBlock(this.GetConnection(), cancellationToken);
        }

        public Task<(ulong Total, IEnumerable<StakePool> StakePools)> ListStakePools(CancellationToken cancellationToken, uint? epochNumber = null, uint offset = 0, uint limit = 100)
        {
            return StakePoolQueries.ListStakePools(this.GetConnection(), cancellationToken, epochNumber, offset, limit);
        }

        public Task<StakePool> GetStakePool(CancellationToken cancellationToken, string poolAddress)
        {
            return StakePoolQueries.GetStakePool(this.GetConnection(), cancellationToken, poolAddress);
        }

        public Task<IEnumerable<TransactionOutput>> GetTransactionOutput(CancellationToken cancellationToken, string transactionId)
        {
            return TransactionQueries.GetTransactionOutputs(this.GetConnection(), cancellationToken, transactionId);
        }

        public Task<IEnumerable<BlockDetail>> GetEpochBlocks(CancellationToken cancellationToken, uint epochNumber)
        {
            return BlockQueries.GetEpochBlocks(this.GetConnection(), cancellationToken, epochNumber);
        }

        public Task<EpochStatistics> GetEpochDelegationStatistics(CancellationToken cancellationToken, uint epochNumber)
        {
            return  EpochQueries.GetEpochDelegationStatistics(this.GetConnection(), cancellationToken, epochNumber);
        }

        public Task<EpochStatistics> GetEpochCirculationStatistics(CancellationToken cancellationToken, uint epochNumber)
        {
            return EpochQueries.GetEpochCirculationStatistics(this.GetConnection(), cancellationToken, epochNumber);
        }
    }
}
