window.initConfigurator = (workstationsJson, dotNetHelper) => {
    console.log('Configurator started!');
    const workstations = workstationsJson;
    let configuration = [];

    const targetList = document.querySelector(".target-list");
    const draggableLists = document.querySelectorAll(".draggable-list");

    const textareaHardware = document.getElementById('preview-content-hardware');
    const textareaParams = document.getElementById('preview-content-params');
    const textareaDefault = document.getElementById('preview-content-defaults');
    const textareaM2kSys = document.getElementById('preview-content-m2ksys');

    const textHardware = "hardwareini\nharwarefdalsdkf\nlaskdjfldfkj\ndlaksjdfk\n\n";
    const textParams = "params\nharwarefdalsdkf\nlaskdjfldfkj\ndlaksjdfk\n\n";
    const textDefault = "defaults\nharwarefdalsdkf\nlaskdjfldfkj\ndlaksjdfk\n\n";
    const textM2kSys = "m2ksys\nharwarefdalsdkf\nlaskdjfldfkj\ndlaksjdfk\n\n";
    
    textareaHardware.value += textHardware;
    textareaParams.value += textParams;
    textareaDefault.value += textDefault;
    textareaM2kSys.value += textM2kSys;

    window.saveConfiguration = async function () {
        return configuration;
    };

    if (workstations && workstations.length > 0) {
        const sortedWorkstations = [...workstations].sort((a, b) => a.sequence - b.sequence);

        // Erstelle für jede Workstation ein entsprechendes Element in der Zielliste
        sortedWorkstations.forEach(workstation => {
            // Suche nach dem entsprechenden Element in den Quell-Listen
            const sourceLists = Array.from(draggableLists);
            const sourceItems = sourceLists.flatMap(list =>
                Array.from(list.querySelectorAll('.item'))
            );

            // Finde das Item, dessen Text dem Namen der Workstation entspricht
            const matchingItem = sourceItems.find(item =>
                item.textContent.trim() === workstation.name
            );

            if (matchingItem) {
                // Clone das Item und füge es zur Zielliste hinzu
                const clone = matchingItem.cloneNode(true);
                targetList.appendChild(clone);

                // Entferne das Original-Item aus seiner Quell-Liste
                matchingItem.remove();
            }
        });

        updatePreview(true);

    }
    
    function updatePreview(fromStart) {
        const itemsInTarget = targetList.querySelectorAll(".item");
        const itemTexts = Array.from(itemsInTarget).map(item => item.textContent.trim());

        textareaHardware.value = textHardware + (itemTexts.join("\n"));
        textareaParams.value = textParams + (itemTexts.join("\n"));
        textareaDefault.value = textDefault + (itemTexts.join("\n"));
        textareaM2kSys.value = textM2kSys + (itemTexts.join("\n"));
        
        configuration = Array.from(itemsInTarget).map((item, index) => ({
            text: item.textContent.trim(),
            index: index
        }));
        if (!fromStart) {
            dotNetHelper.invokeMethodAsync('OnUserInteraction');
        }
    }

    function initDraggableItems(list) {
        const items = list.querySelectorAll(".item");
        items.forEach(item => {
            item.addEventListener("dragstart", () => {
                setTimeout(() => item.classList.add("dragging"), 0);
            });
            item.addEventListener("dragend", () => {
                item.classList.remove("dragging");
                updatePreview(false);
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
        updatePreview(false);
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

    document.querySelectorAll('.preview-tab').forEach(tab => {
        tab.addEventListener('click', function () {
            document.querySelectorAll('.preview-tab').forEach(t => t.classList.remove('active'));
            document.querySelectorAll('.preview-content-tab').forEach(tc => tc.classList.remove('active'));
            tab.classList.add('active');
            document.getElementById('preview-content-' + tab.dataset.tab).classList.add('active');
            
        });
    });
};