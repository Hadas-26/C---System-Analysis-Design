const documentationInput = document.getElementById('documentation');
const fileList = document.getElementById('fileList');

let selectedFiles = [];

documentationInput.addEventListener('change', function () {
    selectedFiles = Array.from(documentationInput.files);
    updateFileList();
});

function updateFileList() {
    fileList.innerHTML = '';
    selectedFiles.forEach((file, index) => {
        const row = document.createElement('div');
        row.classList = 'd-flex justify-content-between align-items-center mb-2';

        row.innerHTML = `
                <span>${file.name}</span>
                <button type="button" class="btn btn-danger btn-sm" onclick="removeFile(${index})">Remove</button>
            `;
        fileList.appendChild(row);
    });
}

function removeFile(index) {

    selectedFiles.splice(index, 1);

    const dataTransfer = new DataTransfer();

    selectedFiles.forEach(file => dataTransfer.items.add(file));

    documentationInput.files = dataTransfer.files;

    updateFileList();
}