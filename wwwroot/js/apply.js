{
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl)
    })

    const VolunteerStatus = document.getElementById('VolunteerStatus');
    const VolunteerHoursField = document.getElementById('VolunteerHoursField');

    VolunteerStatus.addEventListener('change', function () {

        if (VolunteerStatus.value === 'Active') {
            VolunteerHoursField.classList.remove('d-none');

        }
        else {
            VolunteerHoursField.classList.add('d-none');
            VolunteerHoursField.querySelector('input').value = ''; //Reset the input value when the field is hidden

        }

    });

    const EmploymentStatus = document.getElementById('EmploymentStatus');
    const EmploymentField = document.getElementById('EmploymentField');

    EmploymentStatus.addEventListener('change', function () {

        if (EmploymentStatus.value === 'EmployedHalftime' || EmploymentStatus.value === 'EmployedFulltime') {
            EmploymentField.classList.remove('d-none');
        }
        else {
            EmploymentField.classList.add('d-none');
            EmploymentField.querySelector('input').value = ''; //Reset the input value when the field is hidden
        }
    });

    const EducationStatus = document.getElementById('EducationStatus');
    const EducationField = document.getElementById('EducationField');

    EducationStatus.addEventListener('change', function () {

        if (EducationStatus.value === 'FullTime' || EducationStatus.value === 'PartTime') {
            EducationField.classList.remove('d-none');
        }
        else {
            EducationField.classList.add('d-none');
            EducationField.querySelector('input').value = ''; //Reset the input value when the field is hidden
        }
    });


    //Pertaining to documents

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


}
