document.addEventListener('drop', (e) => {
    console.log('Native drop event fired');
});
document.addEventListener('dragover', (e) => {
    e.preventDefault(); // Wichtig für Drop
    console.log('Native dragover event fired');
});