window.initDragDrop = () => {
    const targetList = document.querySelector(".target-list");
    const draggableLists = document.querySelectorAll(".draggable-list");
    
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
    
    function handleDragOver(e, list) {
        e.preventDefault();
        const draggingItem = document.querySelector(".dragging");
        if (!draggingItem || list !== targetList) return;

        const siblings = [...list.querySelectorAll(".item:not(.dragging)")];
        const nextSibling = siblings.find(sibling => {
            return e.clientY <= sibling.offsetTop + sibling.offsetHeight / 2;
        });
        list.insertBefore(draggingItem, nextSibling);
    }
    
    function handleDrop(e, target) {
        e.preventDefault();
        const draggingItem = document.querySelector(".dragging");
        if (!draggingItem) return;

        const sourceContainer = draggingItem.parentNode;
        if (sourceContainer !== target) {
            target.appendChild(draggingItem);
            initDraggableItems(target);
        }
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