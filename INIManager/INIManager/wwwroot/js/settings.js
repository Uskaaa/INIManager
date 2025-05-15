window.initSettings = () => {
    console.log('Settings page initialized');
    // Tab switching functionality
    const tabButtons = document.querySelectorAll('.tab-button');
    const tabPanes = document.querySelectorAll('.tab-pane');

    tabButtons.forEach(button => {
        button.addEventListener('click', () => {
            // Remove active class from all buttons and panes
            tabButtons.forEach(btn => btn.classList.remove('active'));
            tabPanes.forEach(pane => pane.classList.remove('active'));

            // Add active class to clicked button and corresponding pane
            button.classList.add('active');
            const tabId = button.getAttribute('data-tab');
            document.getElementById(tabId).classList.add('active');
        });
    });

    // Settings functionality
    const toggleSwitches = document.querySelectorAll('.toggle-switch input');
    const sliderRange = document.querySelector('.slider-range');

    // Save settings to localStorage
    function saveSetting(key, value) {
        localStorage.setItem(key, value);
    }

    // Load settings from localStorage
    function loadSettings() {
        toggleSwitches.forEach(switchEl => {
            const savedValue = localStorage.getItem(switchEl.id);
            if (savedValue !== null) {
                switchEl.checked = savedValue === 'true';
            }
        });

        const savedFontSize = localStorage.getItem('fontSize');
        if (savedFontSize) {
            sliderRange.value = savedFontSize;
            document.documentElement.style.fontSize = `${savedFontSize}px`;
        }
    }

    // Event listeners for toggle switches
    toggleSwitches.forEach(switchEl => {
        switchEl.addEventListener('change', () => {
            saveSetting(switchEl.id, switchEl.checked);

            // Special handling for dark mode
            if (switchEl.id === 'dark-mode') {
                document.body.classList.toggle('dark-mode', switchEl.checked);
                saveSetting('darkMode', switchEl.checked);
            }
        });
    });

    // Event listener for font size slider
    sliderRange.addEventListener('input', () => {
        const fontSize = sliderRange.value;
        document.documentElement.style.fontSize = `${fontSize}px`;
        saveSetting('fontSize', fontSize);
    });

    // Load saved settings when page loads
    loadSettings();

    // Check for saved dark mode preference
    const darkMode = localStorage.getItem('darkMode') === 'true';
    if (darkMode) {
        document.body.classList.add('dark-mode');
        document.getElementById('dark-mode').checked = true;
    }
}; 