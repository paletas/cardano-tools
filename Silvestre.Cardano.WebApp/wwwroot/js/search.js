function search() {
    return {
        initialized: false,
        setupSearch: function () {
            this.initialized = true;
        },
        search: function (transactionId) {
            window.location.href = `/transaction/${transactionId}`;
        }
    };
}