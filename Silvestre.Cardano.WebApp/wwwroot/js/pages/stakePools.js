function listStakePools() {
    function stakePool(servicePool) {
        return {
            poolAddress: servicePool.poolAddress,
            ticker: servicePool.ticker,
            name: servicePool.name,
            website: servicePool.websiteUrl,
            maintenanceCost: servicePool.maintenanceCostInADA,
            marginPercentage: servicePool.marginPercentage * 100,
            pledge: servicePool.pledgeInADA,
            delegation: servicePool.delegatedInADA
        };
    }

    return {
        isLoading: false,
        initialized: false,
        errors: {
            fetching: false
        },
        stakePools: [],
        fetchStakePools(count, offset) {
            this.isLoading = true;
            fetch(`/api/v1/stakepool?count=${count}&offset=${offset}`)
                .then(result => {
                    if (result.ok) return result.json();
                    else {
                        this.errors.fetching = true;
                        return [];
                    }
                })
                .then(data => {
                    this.isLoading = false;

                    this.stakePools = data.map(stakePool);

                    this.initialized = true;
                });
        }
    };     
}

function featuredPool() {
    function StakePool(servicePool) {
        return {
            poolAddress: servicePool.poolAddress,
            ticker: servicePool.ticker,
            name: servicePool.name,
            website: servicePool.websiteUrl,
            maintenanceCost: servicePool.maintenanceCostInADA,
            marginPercentage: servicePool.marginPercentage * 100,
            pledge: servicePool.pledgeInADA,
            delegation: servicePool.delegatedInADA
        };
    }

    return {
        isLoading: false,
        initialized: false,
        errorFetching: false,
        stakePool: undefined,
        fetchStakePool(poolAddress) {
            this.isLoading = true;
            fetch(`/api/v1/stakepool/${poolAddress}`)
                .then(result => {
                    if (result.ok) return result.json();
                    else {
                        this.errorFetching = true;
                        return undefined;
                    }
                })
                .then(data => {
                    this.isLoading = false;

                    if (data !== undefined)
                        this.stakePool = new StakePool(data);

                    this.initialized = true;
                });
        }
    };
}