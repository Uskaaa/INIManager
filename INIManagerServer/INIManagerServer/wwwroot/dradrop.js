window.addEventListener("DOMContentLoaded", () => {
    const dragElements = document.getElementsByClassName("mud-drop-item");
    
    const dropZones = document.getElementsByClassName("mud-drop-zone");
    
    Array.from(dropZones).forEach(dropZone => {
        dropZone.addEventListener("dragover", (e) => {
            e.preventDefault();
            dropZone.classList.add("dragdrop");
        });

        dropZone.addEventListener("dragleave", () => {
            dropZone.classList.remove("dragdrop");
        });

        // Optional: Drop-Event für Vollständigkeit
        dropZone.addEventListener("drop", () => {
            dropZone.classList.remove("dragdrop");
        });
    });
});