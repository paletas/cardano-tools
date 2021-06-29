function listStakePools(stakePoolsPerPage, epochNumber) {
    function stakePool(servicePool, fetchMetadata) {
        return {
            poolAddress: servicePool.poolAddress,
            ticker: servicePool.ticker,
            name: servicePool.name,
            website: servicePool.websiteUrl,
            maintenanceCost: servicePool.maintenanceCostInADA,
            marginPercentage: servicePool.marginPercentage * 100,
            pledge: servicePool.pledgeInADA,
            delegation: servicePool.delegatedInADA,
            isLoading: false,
            initialized: false,
            metadataUrl: servicePool.metadataUrl,
            hasMetadataError: false
        };
    }

    function getAddress(epochNumber, quantity, offset) {
        var address = `/api/v1/stakepool?count=${quantity}&offset=${offset}`;
        if (epochNumber !== undefined && epochNumber !== null)
            address += `&epoch=${epochNumber}`;
        return address;
    }

    return {
        isLoading: false,
        initialized: false,
        currentPage: undefined,
        quantityPerPage: stakePoolsPerPage,
        errors: {
            fetching: false
        },
        stakePools: [],
        totalStakePools: undefined,
        pagination: new pagination(),
        fetchStakePoolsPage(page) {
            this.isLoading = true;
            var offset = this.quantityPerPage * (page - 1);

            fetch(getAddress(epochNumber, this.quantityPerPage, offset))
                .then(result => {
                    if (result.ok) return result.json();
                    else {
                        this.errors.fetching = true;
                        return [];
                    }
                })
                .then(data => {
                    this.totalStakePools = data.total;
                    this.stakePools = data.stakePools.map(stakePool);
                    this.currentPage = page;

                    if (this.pagination.initialized)
                        this.pagination.setMetrics(this.totalStakePools / this.quantityPerPage, this.quantityPerPage, this.totalStakePools);
                    else
                        this.pagination.setupPagination(this.totalStakePools / this.quantityPerPage, this.quantityPerPage, this.totalStakePools, this.fetchStakePoolsPage.bind(this));

                    for (var ix = 0; ix < this.stakePools.length; ix++) {
                        this.fetchStakePoolMetadata(this.stakePools[ix]);
                    }

                    this.isLoading = false;
                    this.initialized = true;
                });
        },
        fetchStakePoolMetadata(stakePool) {
            if (stakePool.initialized === true) return;

            stakePool.isLoading = true;

            fetch(`/api/v1/stakepool/metadata?url=${encodeURI(stakePool.metadataUrl)}`, { redirect: 'error', mode: 'no-cors' })
                .then(async result => {
                    if (result.ok) {
                        var jsonMetadata = await result.json();

                        stakePool.ticker = jsonMetadata.ticker;
                        stakePool.name = jsonMetadata.name;
                        stakePool.website = jsonMetadata.homepage;

                        stakePool.initialized = true;
                    }
                    else {
                        stakePool.hasMetadataError = true;
                    }

                    stakePool.isLoading = false;
                })
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