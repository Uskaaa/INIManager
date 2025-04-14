window.initNavbar = () => {
    const navItems = document.querySelectorAll('.nav-item');
    const hamburgerBtn = document.querySelector('.hamburger-btn');
    const sidebar = document.querySelector('.sidebar');
    const body = document.body;

    // Create overlay element
    const overlay = document.createElement('div');
    overlay.className = 'overlay';
    body.appendChild(overlay);

    // Toggle sidebar on hamburger button click
    hamburgerBtn.addEventListener('click', () => {
        sidebar.classList.toggle('active');
        overlay.classList.toggle('active');
    });

    // Close sidebar when clicking overlay
    overlay.addEventListener('click', () => {
        sidebar.classList.remove('active');
        overlay.classList.remove('active');
    });

    navItems.forEach(item => {
        item.addEventListener('click', (e) => {
            e.preventDefault();

            // Remove active class from all items
            navItems.forEach(navItem => navItem.classList.remove('active'));

            // Add active class to clicked item
            item.classList.add('active');

            // Close sidebar on mobile after clicking
            if (window.innerWidth <= 768) {
                sidebar.classList.remove('active');
                overlay.classList.remove('active');
            }

            // Here you can add your navigation logic
            const page = item.querySelector('span').textContent.toLowerCase();
            console.log(`Navigating to: ${page}`);
            // Add your navigation logic here
        });
    });


    // Close sidebar when window is resized to desktop
    window.addEventListener('resize', () => {
        if (window.innerWidth > 768) {
            sidebar.classList.remove('active');
            overlay.classList.remove('active');
        }
    });
};

window.handleRouteChange = function (newUrl) {
    console.log("Route changed to:", newUrl);
    
    const navItems = document.querySelectorAll('.nav-item');
    navItems.forEach(navItem => navItem.classList.remove('active'));
    navItems.forEach(item => {

        if (newUrl.toLowerCase().includes(item.id)) {
            item.classList.add('active');
        }
    });
};