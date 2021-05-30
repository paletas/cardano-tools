function transactionViewer() {
    function transaction(serviceTransaction) {
        function transactionOutput (serviceTransactionOutput) {
            return {
                addressTo: serviceTransactionOutput.addressTo,
                amount: serviceTransactionOutput.amount
            };
        }
         
        function transactionMetadata(serviceTransactionMetadata) {
            return {
                key: serviceTransactionMetadata.key,
                json: serviceTransactionMetadata.json,
                data: JSON.parse(serviceTransactionMetadata.json)
            };
        }

        return {
            transactionAddress: serviceTransaction.transactionAddress,
            epochNumber: serviceTransaction.epochNumber,
            slotNumber: serviceTransaction.slotNumber,
            epochSlotNumber: serviceTransaction.epochSlotNumber,
            blockNumber: serviceTransaction.blockNumber,
            transactionCount: serviceTransaction.transactionCount,
            timestamp: serviceTransaction.timestamp,
            blockSize: serviceTransaction.blockSize,
            transactionOutputTotal: serviceTransaction.transactionOutputTotal,
            fees: serviceTransaction.fees,
            deposit: serviceTransaction.deposit,
            transactionSize: serviceTransaction.transactionSize,
            invalidBeforeBlock: serviceTransaction.invalidBeforeBlock,
            invalidAfterBlock: serviceTransaction.invalidAfterBlock,
            output: serviceTransaction.output.map(transactionOutput),
            metadata: serviceTransaction.metadata.map(transactionMetadata)
        };
    }

    return {
        isLoading: false,
        initialized: false,
        errors: {
            fetching: false,
            any: function () {
                var t = this;
                return t.fetching;
            }
        },
        transaction: undefined,
        fetchTransaction(transactionId) {
            this.isLoading = true;

            fetch(`/api/v1/transaction/${transactionId}`)
                .then(result => {
                    if (result.ok) {
                        if (result === undefined) {
                            return undefined;
                        }
                        else {
                            return result.json();
                        }
                    }
                    else {
                        if (result.status !== 404) {
                            this.errors.fetching = true;
                        }
                        return undefined;
                    }
                })
                .then(data => {
                    if (data !== undefined)
                        this.transaction = new transaction(data);

                    this.isLoading = false;
                    this.initialized = true;
                });
        }
    };
}