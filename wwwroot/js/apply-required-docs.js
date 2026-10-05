{
    const VolunteerStatus = document.getElementById('VolunteerStatus');

    VolunteerStatus.addEventListener('change', function () {

        if (VolunteerStatus.value === 'Active') {
            volunteerDocsRequired = true;
            AdjustRequiredDocs();
        }
        else {
            volunteerDocsRequired = false;
            AdjustRequiredDocs();
        }

    });

    const EmploymentStatus = document.getElementById('EmploymentStatus');


    EmploymentStatus.addEventListener('change', function () {

        if (EmploymentStatus.value === 'EmployedHalftime' || EmploymentStatus.value === 'EmployedFulltime') {
            employmentDocsRequired = true;
            AdjustRequiredDocs();
        }
        else {
            employmentDocsRequired = false;
            AdjustRequiredDocs();

        }
    });

    const EducationStatus = document.getElementById('EducationStatus');


    EducationStatus.addEventListener('change', function () {

        if (EducationStatus.value === 'FullTime' || EducationStatus.value === 'PartTime') {
            educationDocsRequired = true;
            AdjustRequiredDocs();
        }
        else {
            educationDocsRequired = false;
            AdjustRequiredDocs();
        }
    });




    //Required documents

    let volunteerDocsRequired = false;
    let employmentDocsRequired = false;
    let educationDocsRequired = false;
    let caregiverDocsRequired = false;

    const requiredDocsSection = document.getElementById('requiredDocsSection');


    function AdjustRequiredDocs() {
        if (volunteerDocsRequired || employmentDocsRequired || educationDocsRequired || caregiverDocsRequired) {
            requiredDocsSection.classList.remove('d-none');
            requiredDocsSection.innerHTML = '';
        }
        else {
            requiredDocsSection.classList.add('d-none');
            requiredDocsSection.innerHTML = '';
            return;
        }

        if (volunteerDocsRequired) {
            requiredDocsSection.innerHTML += `
                <div class="alert alert-warning" role="alert">
                    <strong>Volunteer Documents:</strong> Please upload your volunteer hours documentation (e.g., time logs, supervisor verification).
                </div>
            `;
        }

        if (employmentDocsRequired) {
            requiredDocsSection.innerHTML += `
                <div class="alert alert-warning" role="alert">
                    <strong>Employment Documents:</strong> Please upload proof of employment (e.g., pay stubs, employment verification).
                </div>
            `;
        }

        if (educationDocsRequired) {
            requiredDocsSection.innerHTML += `
                <div class="alert alert-warning" role="alert">
                    <strong>Education Documents:</strong> Please upload your education documentation (e.g., transcripts).
                </div>
            `;
        }

        if (caregiverDocsRequired) {
            requiredDocsSection.innerHTML += `
                <div class="alert alert-warning" role="alert">
                    <strong>Caregiver Documents:</strong> Please upload your caregiver documentation (e.g., proof of relationship).
                </div>
            `;
        }

    }


}