function epochViewer() {
    function epoch(serviceEpoch) {
        return {
            number: serviceEpoch.number,
            startedAt: serviceEpoch.startTime,
            endedAt: serviceEpoch.endTime,
            hasEnded: serviceEpoch.hasEnded,
            transactionsTotal: serviceEpoch.transactionsTotal,
            transactionsCount: serviceEpoch.transactionsCount,
            feesTotal: serviceEpoch.feesTotal
        };
    }

    function epochStatistics(serviceStatistics) {
        return {
            totalStakePools: serviceStatistics.totalStakePools,
            totalDelegations: serviceStatistics.totalDelegations,
            circulatingSupply: serviceStatistics.circulatingSupply,
            stakedSupply: serviceStatistics.stakedSupply,
            rewards: serviceStatistics.rewards,
            orphanedRewards: serviceStatistics.orphanedRewards
        };
    }

    return {
        epoch: {
            isLoading: false,
            initialized: false,
            errors: {
                fetching: false,
                any: function () {
                    var t = this;
                    return t.fetching;
                }
            },
            data: undefined
        },
        epochStatistics: {
            isLoading: false,
            initialized: false,
            errors: {
                fetching: false,
                any: function () {
                    var t = this;
                    return t.fetching;
                }
            },
            data: undefined
        },
        initialize(epochNumber) {
            this.fetchEpoch(epochNumber);
            this.fetchEpochStatistics(epochNumber);
        },
        fetchEpoch(epochNumber) {
            this.epoch.isLoading = true;

            fetch(`/api/v1/epoch/${epochNumber}`)
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
                            this.epoch.errors.fetching = true;
                        }
                        return undefined;
                    }
                })
                .then(data => {
                    if (data !== undefined) {
                        this.epoch.data = new epoch(data);
                    }

                    this.epoch.isLoading = false;
                    this.epoch.initialized = true;
                });
        },
        fetchEpochStatistics(epochNumber) {
            this.epochStatistics.isLoading = true;

            fetch(`/api/v1/epoch/${epochNumber}/statistics`)
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
                            this.epochStatistics.errors.fetching = true;
                        }
                        return undefined;
                    }
                })
                .then(data => {
                    if (data !== undefined) {
                        this.epochStatistics.data = new epochStatistics(data);
                    }

                    this.epochStatistics.isLoading = false;
                    this.epochStatistics.initialized = true;
                });
        }
    };
}

function epochHeatmap() {
    function block(serviceBlock) {
        return {
            hash: serviceBlock.hash,
            epochNumber: serviceBlock.epochNumber,
            slotNumber: serviceBlock.slotNumber,
            epochSlotNumber: serviceBlock.epochSlotNumber,
            number: serviceBlock.number,
            size: serviceBlock.size,
            timestamp: new Date(serviceBlock.timestamp),
            amountTransacted: serviceBlock.amountTransacted,
            fees: serviceBlock.fees
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
        epochNumber: undefined,
        epochStart: undefined,
        epochEnd: undefined,
        blocks: undefined,
        chart: undefined,
        fetchBlocks(epochNumber, startDate, endDate, chartElement, chartHeight) {
            this.isLoading = true

            this.epochNumber = epochNumber;
            this.epochStart = new Date(startDate);
            this.epochEnd = new Date(endDate);

            fetch(`/api/v1/epoch/${epochNumber}/block`)
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
                    if (data !== undefined) {
                        this.blocks = data.map(block);
                    }

                    this.setupChart(chartElement, chartHeight);

                    this.isLoading = false;
                    this.initialized = true;
                });
        },
        setupChart(chartElement, chartSize) {
            function sameDay(d1, d2) {
                return d1.getFullYear() === d2.getFullYear() &&
                    d1.getMonth() === d2.getMonth() &&
                    d1.getDate() === d2.getDate();
            }

            function addDays(date, days) {
                date = new Date(date.getTime());
                date.setDate(date.getDate() + days);
                return date;
            }

            function dataLabel(date) {
                return `${date.getDate()} - ${date.getMonth()}`;
            }

            function dataPoint(block) {
                var minute = block.timestamp.getMinutes() < 30 ? '00' : '30';

                return {
                    x: moment(`${block.timestamp.getHours().toPrecision(2)}:${minute}`, 'HH:mm').format('HH:mm'),
                    y: block.amountTransacted
                };
            }

            function filterBlocksByDate(labelsArray, blocks, date) {
                return _.orderBy(
                    _.unionBy(
                        _.map(
                            _.groupBy(
                                blocks.filter(block => sameDay(block.timestamp, date)).map(dataPoint),
                                dp => dp.x
                            ),
                            value => _.reduce(value, (feeSum, dataPoint) => { return { x: dataPoint.x, y: feeSum.y + dataPoint.y }; })
                        ),
                        labelsArray,
                        dataPoint => dataPoint.x
                    )
                , 'x', 'asc');
            }

            function getHeatmapRange() {
                function indexToHour(value) {
                    if (value % 2 === 0) {
                        return moment(`${Math.floor(value / 2).toPrecision(2)}:00`, 'HH:mm').format('HH:mm');
                    }
                    else {
                        return moment(`${Math.floor(value / 2).toPrecision(2)}:30`, 'HH:mm').format('HH:mm');
                    }
                }

                return _.range(48).map(value => { return { x: indexToHour(value), y: null }; });
            }

            function getSeriesByDate(blocks, date) {
                return {
                    name: dataLabel(date),
                    data: filterBlocksByDate(hoursArray, blocks, date)
                };
            }

            var hoursArray = getHeatmapRange();
            var chartData = {
                series: [
                    getSeriesByDate(this.blocks, addDays(this.epochStart, 5)),
                    getSeriesByDate(this.blocks, addDays(this.epochStart, 4)),
                    getSeriesByDate(this.blocks, addDays(this.epochStart, 3)),
                    getSeriesByDate(this.blocks, addDays(this.epochStart, 2)),
                    getSeriesByDate(this.blocks, addDays(this.epochStart, 1)),
                    getSeriesByDate(this.blocks, addDays(this.epochStart, 0))
                ],
                chart: {
                    height: chartSize,
                    type: 'heatmap'
                },
                dataLabels: {
                    enabled: false
                },
                colors: ["#008FFB"],
                xaxis: {
                    title: {
                        text: "Block Time"
                    }
                },
                yaxis: {
                    title: {
                        text: "Block Date"
                    }
                }
            };

            this.chart = new ApexCharts(chartElement, chartData);
            this.chart.render();
        }
    };
}