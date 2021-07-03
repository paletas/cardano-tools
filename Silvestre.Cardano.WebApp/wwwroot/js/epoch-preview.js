function epochPreview() {
    function epoch(number, blockCount, epochSlotNumber, maxEpochSlots, endDate) {
        return {
            number: number,
            blockCount: blockCount,
            slotNumber: epochSlotNumber,
            maximumSlots: maxEpochSlots,
            endDate: Date.parse(endDate)
        };
    }

    return {
        initialized: false,
        errors: {
            fetching: false
        },
        epoch: undefined,
        signalConnection: undefined,
        initFetchEpoch: function () {
            var epochViewer = this;

            this.signalConnection = new signalR.HubConnectionBuilder()
                .configureLogging(signalR.LogLevel.Information)
                .withUrl("/rt/blocks")
                .build();

            this.signalConnection.start()
                .then(function () { console.log("EpochViewer: SignalR connected"); })
                .catch(function (err) { return console.error("EpochViewer: " + err.toString()); });

            this.signalConnection.on("NewEpoch", function (epochNumber) {
                epochViewer.updateEpoch();
            });

            this.signalConnection.on("NewBlock", function (block) {
                if (epochViewer.epoch === undefined) return;

                epochViewer.epoch.slotNumber = block.epochSlotNumber;
            });

            epochViewer.updateEpoch();
        },

        updateEpoch: function () {
            fetch(`/api/v1/epoch`)
                .then(result => {
                    if (result.ok) return result.json();
                    else {
                        this.errors.fetching = true;
                        return [];
                    }
                })
                .then(data => {
                    if (this.errors.fetching === true) {
                        this.epoch = undefined;
                        this.initialized = false;
                    }
                    else {
                        this.epoch = epoch(data.number, data.blockCount, data.currentSlotNumber, data.maximumSlots, data.endTime);
                        Alpine.store('currentEpoch', this.epoch);

                        this.initialized = true;
                    }
                });
        }
    };
}