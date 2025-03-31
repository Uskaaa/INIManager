window.initDragDrop = () => {
    console.log("Initializing Drag & Drop...");

    const sourceList = document.querySelector(".source-list");
    const targetList = document.querySelector(".target-list");
    const draggableLists = document.querySelectorAll(".draggable-list");

// Funktion zum Initialisieren der Drag-Events für Items
    function initDraggableItems(list) {
        const items = list.querySelectorAll(".item");
        items.forEach(item => {
            item.addEventListener("dragstart", () => {
                setTimeout(() => item.classList.add("dragging"), 0);
            });
            item.addEventListener("dragend", () => {
                item.classList.remove("dragging");
            });
        });
    }

// Funktion zum Sortieren (nur im Ziel-Container)
    function handleDragOver(e, list) {
        e.preventDefault();
        const draggingItem = document.querySelector(".dragging");
        if (!draggingItem || list !== targetList) return; // Nur im Ziel-Container sortieren

        const siblings = [...list.querySelectorAll(".item:not(.dragging)")];
        const nextSibling = siblings.find(sibling => {
            return e.clientY <= sibling.offsetTop + sibling.offsetHeight / 2;
        });
        list.insertBefore(draggingItem, nextSibling);
    }

// Funktion zum Verschieben zwischen Containern
    function handleDrop(e, target) {
        e.preventDefault();
        const draggingItem = document.querySelector(".dragging");
        if (!draggingItem) return;

        const sourceContainer = draggingItem.parentNode;
        if (sourceContainer !== target) {
            target.appendChild(draggingItem);
            initDraggableItems(target); // Neue Items im Ziel-Container draggable machen
        }
    }

// Event-Listener für beide Listen
    draggableLists.forEach(list => {
        initDraggableItems(list);

        list.addEventListener("dragover", (e) => {
            handleDragOver(e, list);
        });

        list.addEventListener("drop", (e) => {
            handleDrop(e, list);
        });

        list.addEventListener("dragenter", (e) => e.preventDefault());
    });
};