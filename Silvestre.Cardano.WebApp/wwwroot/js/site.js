// Navbar Toggle
document.addEventListener('DOMContentLoaded', function () {

    // Get all "navbar-burger" elements
    var $navbarBurgers = Array.prototype.slice.call(document.querySelectorAll('.navbar-burger'), 0);

    // Check if there are any navbar burgers
    if ($navbarBurgers.length > 0) {

        // Add a click event on each of them
        $navbarBurgers.forEach(function ($el) {
            $el.addEventListener('click', function () {

                // Get the "main-nav" element
                var $target = document.getElementById('main-nav');
                var $targetButtonOpen = document.getElementById('main-nav-button-open');
                var $targetButtonClosed = document.getElementById('main-nav-button-closed');

                // Toggle the class on "main-nav"
                $target.classList.toggle('hidden');
                $targetButtonOpen.classList.toggle('hidden');
                $targetButtonClosed.classList.toggle('hidden');
            });
        });
    }
});

var jcardano = {
    constants: {
        formats: {
            ada: '₳0.[000000]a',
            percentage: '0.00%'
        }
    }
}