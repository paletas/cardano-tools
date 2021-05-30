using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Silvestre.Cardano.Integration.DbSync.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Silvestre.Cardano.Integration.DbSyncAPI.Services
{
    internal class TransactionService : Transactions.TransactionsBase
    {
        private readonly ILogger<TransactionService> _logger;
        private readonly DatabaseProxy _databaseProxy;

        public TransactionService(ILogger<TransactionService> logger, DatabaseProxy databaseProxy)
        {
            _logger = logger;
            _databaseProxy = databaseProxy;
        }

        public override async Task<GetTransactionDetailsResponse> GetTransactionDetails(GetTransactionDetailsRequest request, ServerCallContext context)
        {
            var transactionOutputs = await _databaseProxy.GetTransactionOutput(request.TransactionHashId).ConfigureAwait(false);
            if (transactionOutputs.Any() == false) return new GetTransactionDetailsResponse();

            var transactionDetails = transactionOutputs.First();

            var transaction = transactionOutputs.GroupBy(transaction => transaction.TransactionId)
                .Select(groupTransactions =>
                {
                    var transactionDetails = groupTransactions.First();

                    return new Transaction
                    {
                        TransactionId = transactionDetails.TransactionId,
                        TransactionHash = transactionDetails.TransactionHash,
                        TransactionCount = transactionDetails.TransactionCount,
                        TransactionSize = transactionDetails.TransactionSize,
                        BlockNumber = transactionDetails.BlockNumber,
                        BlockSize = transactionDetails.BlockSize,
                        EpochNumber = transactionDetails.EpochNumber,
                        EpochSlotNumber = transactionDetails.EpochSlotNumber,
                        SlotNumber = transactionDetails.SlotNumber,
                        Deposit = transactionDetails.Deposit,
                        Fees = transactionDetails.Fees,
                        InvalidAfterBlock = transactionDetails.InvalidAfterBlock,
                        InvalidBeforeBlock = transactionDetails.InvalidBeforeBlock,
                        OutSum = transactionDetails.TransactionOutputTotal,
                        Timestamp = transactionDetails.BlockTimestamp.ToTimestamp(),
                        TransactionInId = transactionDetails.TransactionInId
                    };
                })
                .Single();

            foreach (var metadataDetail in transactionOutputs.Where(m => m.MetadataKey > 0))
                transaction.Metadata.Add(new TransactionMetadata { Key = metadataDetail.MetadataKey, Json = metadataDetail.MetadataJson });

            foreach (var outputDetail in transactionOutputs.Where(o => o.AddressTo != null))
                transaction.Output.Add(new TransactionOutput { Address = outputDetail.AddressTo, Amount = outputDetail.Amount });

            return new GetTransactionDetailsResponse { Transaction = transaction };
        }
    }
}
