window.initHome = () => {
    console.log('Home page initialized');
// Sample data for workstations
    const workstations = [
        'Workstation 1',
        'Workstation 2',
        'Workstation 3',
        'Workstation 4',
        'Workstation 5',
        'Workstation 6',
        'Workstation 7',
        'Workstation 8',
        'Workstation 9',
        'Workstation 10'
    ];

// Function to generate random timestamp
    function generateRandomTimestamp() {
        const now = new Date();
        const randomDays = Math.floor(Math.random() * 30);
        const randomHours = Math.floor(Math.random() * 24);
        const randomMinutes = Math.floor(Math.random() * 60);

        const date = new Date(now);
        date.setDate(date.getDate() - randomDays);
        date.setHours(randomHours);
        date.setMinutes(randomMinutes);

        return date.toLocaleString();
    }

// Generate more sample data for draft configurations
    const draftConfigurations = Array.from({length: 15}, (_, index) => ({
        id: index + 1,
        name: `Config ${String.fromCharCode(65 + index)}`,
        timestamp: generateRandomTimestamp()
    }));

// Generate more sample data for finished configurations
    const finishedConfigurations = Array.from({length: 12}, (_, index) => ({
        id: index + 1,
        name: `Finished Config ${String.fromCharCode(65 + index)}`,
        timestamp: generateRandomTimestamp()
    }));

// Function to create workstation dropdown
    function createWorkstationDropdown() {
        const select = document.createElement('select');
        workstations.forEach(workstation => {
            const option = document.createElement('option');
            option.value = workstation;
            option.textContent = workstation;
            select.appendChild(option);
        });

        // Add animation class when dropdown is opened
        select.addEventListener('focus', () => {
            select.classList.add('dropdown-open');
        });

        select.addEventListener('blur', () => {
            select.classList.remove('dropdown-open');
        });

        return select;
    }

// Function to create action buttons
    function createActionButtons() {
        const div = document.createElement('div');
        div.className = 'action-buttons';

        const editBtn = document.createElement('button');
        editBtn.className = 'btn btn-edit';
        editBtn.innerHTML = '<i class="fas fa-edit"></i>';
        editBtn.title = 'Edit';

        const exportBtn = document.createElement('button');
        exportBtn.className = 'btn btn-export';
        exportBtn.innerHTML = '<i class="fas fa-file-export"></i>';
        exportBtn.title = 'Export';

        const deleteBtn = document.createElement('button');
        deleteBtn.className = 'btn btn-delete';
        deleteBtn.innerHTML = '<i class="fas fa-trash"></i>';
        deleteBtn.title = 'Delete';

        div.appendChild(editBtn);
        div.appendChild(exportBtn);
        div.appendChild(deleteBtn);

        return div;
    }

// Function to populate table with animation
    function populateTable(tableId, configurations) {
        const tableBody = document.querySelector(`#${tableId} tbody`);
        tableBody.innerHTML = '';

        configurations.forEach((config, index) => {
            const row = document.createElement('tr');
            row.style.animationDelay = `${index * 0.1}s`;

            const idCell = document.createElement('td');
            idCell.textContent = config.id;

            const nameCell = document.createElement('td');
            nameCell.textContent = config.name;

            const workstationCell = document.createElement('td');
            workstationCell.appendChild(createWorkstationDropdown());

            const timestampCell = document.createElement('td');
            timestampCell.textContent = config.timestamp;

            const actionCell = document.createElement('td');
            actionCell.appendChild(createActionButtons());

            row.appendChild(idCell);
            row.appendChild(nameCell);
            row.appendChild(workstationCell);
            row.appendChild(timestampCell);
            row.appendChild(actionCell);

            tableBody.appendChild(row);
        });
    }

// Initialize tables with animation

    // Add loading animation
    document.querySelectorAll('.table-container').forEach(container => {
        container.style.opacity = '0';
    });

    // Fade in tables
    setTimeout(() => {
        document.querySelectorAll('.table-container').forEach(container => {
            container.style.opacity = '1';
        });
        populateTable('draftTable', draftConfigurations);
        populateTable('finishedTable', finishedConfigurations);
    }, 300);

    // Add event listeners for action buttons
    document.querySelectorAll('.btn').forEach(button => {
        button.addEventListener('click', (e) => {
            const action = e.currentTarget.title;
            const row = e.currentTarget.closest('tr');
            const id = row.querySelector('td:first-child').textContent;

            // Add click animation
            e.currentTarget.style.transform = 'translateY(2px)';
            setTimeout(() => {
                e.currentTarget.style.transform = '';
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