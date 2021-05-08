function pagination() {
    return {
        initialized: false,
        displayPages: undefined,
        currentPage: undefined,
        itemsPerPage: undefined,
        totalItems: undefined,
        totalPages: undefined,
        callback: undefined,
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

            this.currentPage = pageNumber;
            this.setupDisplayPages(pageNumber);

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
            }
            else {
                var to = Math.min(currentPage + 2, this.totalPages);
                var from = Math.max(currentPage - (this.displayPages.length - (to - currentPage)), 0);
                for (ix = 0; ix < this.displayPages.length; ix++) {
                    this.displayPages[ix] = from + ix + 1;
                }
            }
        }
    };
}