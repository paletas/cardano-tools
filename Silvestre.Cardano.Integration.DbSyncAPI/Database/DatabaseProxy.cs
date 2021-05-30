using Silvestre.Cardano.Integration.DbSyncAPI.Database.Model;
using Silvestre.Cardano.Integration.DbSyncAPI.Database.Queries;
using System.Collections.Generic;
using System.Data.Common;
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

        public Task<Epoch> GetCurrentEpoch()
        {
            return EpochQueries.GetCurrentEpoch(this.GetConnection());
        }

        public Task<Block> GetLatestBlock(uint epochNumber)
        {
            return BlockQueries.GetLatestBlockForEpoch(this.GetConnection(), epochNumber);
        }

        public Task<Block> GetLatestBlock()
        {
            return BlockQueries.GetLatestBlock(this.GetConnection());
        }

        public Task<(ulong Total, IEnumerable<StakePool> StakePools)> ListStakePools(uint offset = 0, uint limit = 100)
        {
            return StakePoolQueries.ListStakePools(this.GetConnection(), offset, limit);
        }

        public Task<StakePool> GetStakePool(string poolAddress)
        {
            return StakePoolQueries.GetStakePool(this.GetConnection(), poolAddress);
        }

        public Task<IEnumerable<TransactionOutput>> GetTransactionOutput(string transactionId)
        {
            return TransactionQueries.GetTransactionOutputs(this.GetConnection(), transactionId);
        }
    }
}
