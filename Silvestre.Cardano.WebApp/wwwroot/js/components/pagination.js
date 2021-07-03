function pagination() {
    return {
        initialized: false,
        displayPages: undefined,
        currentPage: undefined,
        itemsPerPage: undefined,
        totalItems: undefined,
        totalPages: undefined,
        callback: undefined,
        display: {
            showFastBackwards: true,
            showBackwards: true,
            showForwards: false,
            showFastForwards: false
        },
        setupPagination: function (numberOfPages, itemsPerPage, totalItems, changePageCallback) {
            this.setMetrics(numberOfPages, itemsPerPage, totalItems);
            this.callback = (page) => changePageCallback(page);

            this.setCurrentPage(1, false);
            this.initialized = true;
        },
        setMetrics: function (numberOfPages, itemsPerPage, totalItems) {
            this.itemsPerPage = itemsPerPage;
            this.totalItems = totalItems;
            this.totalPages = Math.ceil(numberOfPages);
        },
        setCurrentPage: function (pageNumber, triggerCallback) {
            if (this.currentPage === pageNumber) return;

            this.setupDisplayPages(pageNumber);
            this.currentPage = pageNumber;

            if (triggerCallback) {
                this.callback(pageNumber);
            }
        },
        setupDisplayPages: function (currentPage) {
            if (currentPage > this.totalPages) return;

            this.displayPages = new Array(Math.min(this.totalPages, 5));
            var ix = 0;

            if (this.totalPages <= 5) {
                for (ix = 0; ix < this.displayPages.length; ix++) {
                    this.displayPages[ix] = ix + 1;
                }

                this.display.showFastBackwards = false;
                this.display.showBackwards = false;
                this.display.showForwards = false;
                this.display.showFastForwards = false;
            }
            else {
                var to = Math.min(currentPage + 2, this.totalPages);
                var from = Math.max(currentPage - (this.displayPages.length - (to - currentPage)), 0) + 1;
                for (ix = 0; ix < this.displayPages.length; ix++) {
                    this.displayPages[ix] = from + ix;
                }

                this.display.showFastBackwards = from > 1;
                this.display.showBackwards = currentPage > 1;
                this.display.showForwards = currentPage < this.totalPages;
                this.display.showFastForwards = to < this.totalPages;
            }
        }
    };
}