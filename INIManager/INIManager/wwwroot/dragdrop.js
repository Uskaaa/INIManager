window.dragDropHandler = {
    init: (dotNetObject) => {
        const drag = document.getElementById('drag');
        const drop = document.getElementById('drop');

        drag.addEventListener('dragstart', (e) => {
            console.log('Drag started');
            e.dataTransfer.setData('text/plain', 'Test Data');
            dotNetObject.invokeMethodAsync('OnDragStart', 'Drag started via JS');
        });

        drop.addEventListener('dragover', (e) => {
            e.preventDefault();
            console.log('Drag over');
            dotNetObject.invokeMethodAsync('OnDragOver', 'Drag over via JS');
        });

        drop.addEventListener('drop', (e) => {
            e.preventDefault();
            const data = e.dataTransfer.getData('text/plain');
            console.log('Dropped: ' + data);
            dotNetObject.invokeMethodAsync('OnDrop', 'Dropped: ' + data);
        });
    }
};