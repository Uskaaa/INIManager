window.initHome = () => {
    console.log('Home page initialized');
    
    // Add event listeners for action buttons
    document.querySelectorAll('.btn').forEach(button => {
        button.addEventListener('click', (e) => {
            const action = e.currentTarget.title;
            const row = e.currentTarget.closest('tr');
            const id = row.querySelector('td:first-child').textContent;

            // Add click animation
            e.currentTarget.style.transform = 'translateY(2px)';
            setTimeout(() => {
                e.currentTarget.style.transform = 'translateY(0px)';
            }, 200);

            switch (action) {
                case 'Edit':
                    console.log(`Edit configuration ${id}`);
                    break;
                case 'Export':
                    console.log(`Export configuration ${id}`);
                    break;
                case 'Delete':
                    console.log(`Delete configuration ${id}`);
                    break;
            }
        });
    });
};