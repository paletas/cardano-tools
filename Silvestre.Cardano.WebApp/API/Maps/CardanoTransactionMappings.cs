using Silvestre.Cardano.Integration.CardanoAPI;
using Silvestre.Cardano.WebApp.API.ServiceModel.Transactions;
using System.Linq;

namespace Silvestre.Cardano.WebApp.API.Maps
{
    public static class CardanoTransactionMappings
    {
        public static Transaction ToTransaction(this CardanoTransaction cardanoTransaction)
        {
            return new Transaction
            {
                TransactionHash = cardanoTransaction.TransactionHash,
                EpochNumber = cardanoTransaction.EpochNumber,
                SlotNumber = cardanoTransaction.SlotNumber,
                EpochSlotNumber = cardanoTransaction.EpochSlotNumber,
                BlockNumber = cardanoTransaction.BlockNumber,
                TransactionCount = cardanoTransaction.TransactionCount,
                Timestamp = cardanoTransaction.Timestamp,
                BlockSize = cardanoTransaction.BlockSize,
                TransactionOutputTotal = cardanoTransaction.TransactionOutputTotal.Quantity,
                Fees = cardanoTransaction.Fees.Quantity,
                Deposit = cardanoTransaction.Deposit.Quantity,
                TransactionSize = cardanoTransaction.TransactionSize,
                InvalidBeforeBlock = cardanoTransaction.InvalidBeforeBlock,
                InvalidAfterBlock = cardanoTransaction.InvalidAfterBlock,
                TransactionInId = cardanoTransaction.TransactionInId,
                Output = cardanoTransaction.Output.Select(ToTransactionOutput).ToArray(),
                Metadata = cardanoTransaction.Metadata.Select(ToTransactionMetadata).ToArray()
            };
        }

        public static TransactionOutput ToTransactionOutput(this CardanoTransactionOutput cardanoTransactionOutput)
        {
            return new TransactionOutput
            {
                AddressTo =  cardanoTransactionOutput.AddressTo,
                Amount = cardanoTransactionOutput.Output.Quantity
            };
        }

        public static TransactionMetadata ToTransactionMetadata(this CardanoTransactionMetadata cardanoTransactionMetadata)
        {
            return new TransactionMetadata
            {
                Key = cardanoTransactionMetadata.MetadataKey,
                Json = cardanoTransactionMetadata.MetadataJson
            };
        }
    }
}
