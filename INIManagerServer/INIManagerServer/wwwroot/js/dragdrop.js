window.initDragDrop = () => {
    const targetList = document.querySelector(".target-list");
    const draggableLists = document.querySelectorAll(".draggable-list");
    const previewTextarea = document.querySelector(".preview");

    function updatePreview() {
        const itemsInTarget = targetList.querySelectorAll(".item");
        const itemTexts = Array.from(itemsInTarget).map(item => item.textContent.trim());
        previewTextarea.value = itemTexts.join("\n");
    }

    function initDraggableItems(list) {
        const items = list.querySelectorAll(".item");
        items.forEach(item => {
            item.addEventListener("dragstart", () => {
                setTimeout(() => item.classList.add("dragging"), 0);
            });
            item.addEventListener("dragend", () => {
                item.classList.remove("dragging");
                updatePreview();
            });
        });
    }

    function handleDragOver(e, list) {
        e.preventDefault();
        const draggingItem = document.querySelector(".dragging");
        if (!draggingItem) return;

        // Berechne die Mausposition relativ zur Liste
        const listRect = list.getBoundingClientRect();
        const mouseY = e.clientY - listRect.top;

        const siblings = [...list.querySelectorAll(".item:not(.dragging)")];

        // Finde das nächste Element basierend auf der Mausposition
        let nextSibling = null;
        for (const sibling of siblings) {
            const siblingRect = sibling.getBoundingClientRect();
            const siblingMiddleY = siblingRect.top + siblingRect.height / 2 - listRect.top;

            if (mouseY <= siblingMiddleY) {
                nextSibling = sibling;
                break;
            }
        }

        list.insertBefore(draggingItem, nextSibling);
    }

    function handleDrop(e, target) {
        e.preventDefault();
        const draggingItem = document.querySelector(".dragging");
        if (!draggingItem) return;

        // Prüfe, ob das Element bereits im Target ist oder nicht
        const sourceList = draggingItem.parentNode;
        if (sourceList !== target) {
            // Element kommt von einer anderen Liste
            const clone = draggingItem.cloneNode(true);
            target.appendChild(clone);
            initDraggableItems(target);
        }
        // So oder so wird die Vorschau aktualisiert
        updatePreview();
    }

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