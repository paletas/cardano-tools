using Dapper;
using Silvestre.Cardano.Integration.DbSyncAPI.Database.Model;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Silvestre.Cardano.Integration.DbSyncAPI.Database.Queries
{
    internal static class TransactionQueries
    {
        public static async Task<IEnumerable<TransactionOutput>> GetTransactionOutputs(this DbConnection sqlConnection, CancellationToken cancellationToken, string transactionId)
        {
            const string QueryString =
                @"SELECT tx.id TransactionId, encode(tx.hash, 'hex') TransactionHash, 
					block.epoch_no EpochNumber, block.slot_no SlotNumber, block.epoch_slot_no EpochSlotNumber, block.block_no BlockNumber, block.tx_count TransactionCount, block.time BlockTimestamp, block.size BlockSize,
					tx.out_sum TransactionOutputTotal, tx.fee Fees, tx.deposit Deposit, tx.size Size, tx.invalid_before InvalidBeforeBlock, tx.invalid_hereafter InvalidAfterBlock,
					tx_metadata.key MetadataKey, tx_metadata.json MetadataJson,
					tx_out.address AddressTo, tx_out.value Amount,
					tx_in.tx_in_id TransactionInId
				FROM public.tx INNER JOIN public.block
						ON tx.block_id = block.id
					INNER JOIN public.tx_out 
						ON tx.id = tx_out.tx_id
					LEFT JOIN public.tx_metadata 
						ON tx.id = tx_metadata.tx_id
					LEFT JOIN public.tx_in
						ON tx_out.id = tx_in.tx_out_id
					LEFT JOIN public.withdrawal
						ON tx.id = withdrawal.tx_id
				WHERE tx.hash = decode(@TransactionId, 'hex')";

			var transactionCommand = new CommandDefinition(QueryString, new { TransactionId = transactionId }, commandType: System.Data.CommandType.Text, cancellationToken: cancellationToken);
			var transactionData = await sqlConnection.QueryAsync<TransactionOutput>(transactionCommand).ConfigureAwait(false);

            foreach (var transaction in transactionData)
            {
                transaction.BlockTimestamp = transaction.BlockTimestamp.ToUniversalTime();
            }

            return transactionData;
        }
    }
}
