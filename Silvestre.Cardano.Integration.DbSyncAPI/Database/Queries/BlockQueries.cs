using Dapper;
using Silvestre.Cardano.Integration.DbSyncAPI.Database.Model;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Silvestre.Cardano.Integration.DbSyncAPI.Database.Queries
{
	internal static class BlockQueries
	{
		public static async Task<Block> GetLatestBlockForEpoch(this DbConnection sqlConnection, CancellationToken cancellationToken, uint epochNumber)
		{
			const string QueryString =
				@"SELECT block.id as Id, block.hash as Hash, block.epoch_no as EpochNumber, block.slot_no as SlotNumber, block.epoch_slot_no as EpochSlotNumber, block.block_no as BlockNumber, block.previous_id as PreviousID, block.slot_leader_id as SlotLeaderId, block.size as Size, block.""time"" as Timestamp, block.tx_count TransactionCount
				FROM public.block
				WHERE epoch_no = @EpochNumber
				ORDER BY id DESC
				LIMIT 1";

			var blockCommand = new CommandDefinition(QueryString, new { EpochNumber = (long)epochNumber }, commandType: System.Data.CommandType.Text, cancellationToken: cancellationToken);
			var blockData = await sqlConnection.QuerySingleAsync<Block>(blockCommand).ConfigureAwait(false);
			blockData.Timestamp = blockData.Timestamp.ToUniversalTime();

			return blockData;
		}

		public static async Task<Block> GetLatestBlock(this DbConnection sqlConnection, CancellationToken cancellationToken)
		{
			const string QueryString =
				@"SELECT block.id as Id, block.hash as Hash, block.epoch_no as EpochNumber, block.slot_no as SlotNumber, block.epoch_slot_no as EpochSlotNumber, block.block_no as BlockNumber, block.previous_id as PreviousID, block.slot_leader_id as SlotLeaderId, block.size as Size, block.""time"" as Timestamp, block.tx_count TransactionCount
				FROM public.block
				ORDER BY id DESC
				LIMIT 1";

			var blockCommand = new CommandDefinition(QueryString, commandType: System.Data.CommandType.Text, cancellationToken: cancellationToken);
			var blockData = await sqlConnection.QuerySingleAsync<Block>(blockCommand).ConfigureAwait(false);
			blockData.Timestamp = blockData.Timestamp.ToUniversalTime();

			return blockData;
		}

		public static async Task<IEnumerable<BlockDetail>> GetEpochBlocks(this DbConnection sqlConnection, CancellationToken cancellationToken, uint epochNumber)
		{
			const string QueryString =
				@"SELECT block.id as Id, block.hash as Hash, block.epoch_no as EpochNumber, block.slot_no as SlotNumber, block.epoch_slot_no as EpochSlotNumber, block.block_no as BlockNumber, block.previous_id as PreviousID, block.slot_leader_id as SlotLeaderId, block.size as Size, block.""time"" as Timestamp, block.tx_count TransactionCount, SUM(tx.fee) TotalFees, SUM(tx.out_sum) TotalOutSum
				FROM public.block LEFT JOIN public.tx
						ON block.id = tx.block_id
				WHERE block.epoch_no = @EpochNumber
				GROUP BY block.id, block.hash, block.epoch_no, block.slot_no, block.epoch_slot_no, block.block_no, block.previous_id, block.slot_leader_id, block.size, block.time, block.tx_count
				ORDER BY time ASC";

			var blockCommand = new CommandDefinition(QueryString, new { EpochNumber = (long)epochNumber }, commandType: System.Data.CommandType.Text, cancellationToken: cancellationToken);
			var epochBlocks = await sqlConnection.QueryAsync<BlockDetail>(blockCommand).ConfigureAwait(false);
			foreach (var block in epochBlocks)
            {
				block.Timestamp = block.Timestamp.ToUniversalTime();
            }

			return epochBlocks;
		}
	}
}
