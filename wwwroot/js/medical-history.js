// Medical History Management JavaScript

document.addEventListener('DOMContentLoaded', function () {
    // Initialize AOS animation
    AOS.init();
    
    // Load all patients to populate the filter dropdown
    loadAllPatients();
    
    // Load all medical histories when page loads
    loadAllMedicalHistories();
});

// Global variables
let patientsList = [];
let visitsList = [];
let medicalHistoriesList = [];
let doctorsList = [];
let currentMedicalHistoryId = null;
let medicalHistoryTable = null;

// Function to load all patients
function loadAllPatients() {
    fetch('/api/Patient/PatientInfo')
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to fetch patients');
            }
            return response.json();
        })
        .then(data => {
            patientsList = data;
            
            // Populate patient filter dropdown
            populatePatientDropdowns(data);
            
            // Load all visits after patients are loaded
            loadAllVisits();
            
            // Load all doctors after patients are loaded
            loadAllDoctors();
        })
        .catch(error => {
            console.error('Error loading patients:', error);
            Swal.fire({
                title: 'Error!',
                text: 'Failed to load patients. Please try again later.',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        });
}

// Function to populate patient dropdowns
function populatePatientDropdowns(patients) {
    // Populate filter dropdown
    const filterPatient = document.getElementById('filterMedicalHistoryPatient');
    if (filterPatient) {
        // Keep the first option (All Patients)
        filterPatient.innerHTML = '<option value="">All Patients</option>';
        
        // Add patient options
        patients.forEach(patient => {
            const option = document.createElement('option');
            option.value = patient.id;
            option.textContent = `${patient.id} - ${patient.name}`;
            filterPatient.appendChild(option);
        });
        
        // Add event listener for filtering
        filterPatient.addEventListener('change', function() {
            filterMedicalHistoriesByPatient(this.value);
        });
    }
    
    // Populate form dropdown
    const medicalHistoryPatientId = document.getElementById('medical-history-patient-id');
    if (medicalHistoryPatientId) {
        // Keep the first option (Select Patient)
        medicalHistoryPatientId.innerHTML = '<option value="">Select Patient</option>';
        
        // Add patient options
        patients.forEach(patient => {
            const option = document.createElement('option');
            option.value = patient.id;
            option.textContent = `${patient.id} - ${patient.name}`;
            medicalHistoryPatientId.appendChild(option);
        });
        
        // Add event listener to update visits dropdown when patient changes
        medicalHistoryPatientId.addEventListener('change', function() {
            updateVisitsDropdown(this.value);
        });
    }
}

// Function to load all visits
function loadAllVisits() {
    fetch('/api/Patient/PatientVisit')
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to fetch visits');
            }
            return response.json();
        })
        .then(data => {
            visitsList = data;
        })
        .catch(error => {
            console.error('Error loading visits:', error);
            Swal.fire({
                title: 'Error!',
                text: 'Failed to load patient visits. Please try again later.',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        });
}

// Function to load all doctors (staff with doctor role)
function loadAllDoctors() {
    return fetch('/api/employees')
        .then(response => {
            if (!response.ok) {
                if (response.status === 404) {
                    return [];
                }
                throw new Error('Failed to fetch staff information');
            }
            return response.json();
        })
        .then(data => {
            // Filter staff to only include doctors
            doctorsList = data.filter(staff => 
                staff.role && staff.role.toLowerCase().includes('doctor'));
            
            // Populate doctor dropdown
            populateDoctorDropdown(doctorsList);
            
            // Log doctors list for debugging
            console.log("Doctors loaded:", doctorsList.length);
            
            return doctorsList; // Return the doctors list for chaining
        })
        .catch(error => {
            console.error('Error loading doctors:', error);
            // Don't show error to user, just log it
            return []; // Return empty array in case of error
        });
}

// Function to populate doctor dropdown
function populateDoctorDropdown(doctors) {
    const doctorDropdown = document.getElementById('medical-history-doctor-id');
    if (!doctorDropdown) return;
    
    // Keep the first option
    doctorDropdown.innerHTML = '<option value="">Select Doctor</option>';
    
    // Add doctor options
    if (doctors && doctors.length > 0) {
        doctors.forEach(doctor => {
            const option = document.createElement('option');
            option.value = doctor.id;
            option.textContent = `${doctor.name} - ${doctor.department || 'No Department'}`;
            // Store the department in a data attribute for easy access
            option.dataset.department = doctor.department || '';
            doctorDropdown.appendChild(option);
        });
        
        // Add change event to auto-fill department
        doctorDropdown.addEventListener('change', function() {
            const selectedOption = this.options[this.selectedIndex];
            const departmentField = document.getElementById('medical-history-department');
            
            if (selectedOption && selectedOption.dataset.department) {
                departmentField.value = selectedOption.dataset.department;
            }
        });
    } else {
        // No doctors found, add a default option
        const option = document.createElement('option');
        option.value = "0";
        option.textContent = "No doctors available";
        doctorDropdown.appendChild(option);
    }
}

// Function to update visits dropdown based on selected patient
function updateVisitsDropdown(patientId) {
    const visitsDropdown = document.getElementById('medical-history-visit-id');
    if (!visitsDropdown) return;
    
    // Reset dropdown
    visitsDropdown.innerHTML = '<option value="">Select Visit</option>';
    
    if (!patientId) return;
    
    // Filter visits by patient ID
    const patientVisits = visitsList.filter(visit => visit.patientId == patientId);
    
    if (patientVisits.length === 0) {
        // No visits found for this patient, add a message option
        const option = document.createElement('option');
        option.value = "";
        option.textContent = "No visits found for this patient";
        option.disabled = true;
        visitsDropdown.appendChild(option);
        
        // Show a message to the user
        Swal.fire({
            title: 'No Visits Found',
            text: 'This patient has no recorded visits. Please create a visit for this patient first.',
            icon: 'info',
            confirmButtonText: 'OK'
        });
        
        return;
    }
    
    // Add visit options and sort by date (newest first)
    patientVisits
        .sort((a, b) => new Date(b.visitDate) - new Date(a.visitDate))
        .forEach(visit => {
            const visitDate = new Date(visit.visitDate).toLocaleString();
            const option = document.createElement('option');
            option.value = visit.id;
            option.textContent = `Visit #${visit.id} - ${visitDate}`;
            visitsDropdown.appendChild(option);
        });
    
    // Enable the dropdown
    visitsDropdown.disabled = false;
}

// Function to filter medical histories by patient ID
function filterMedicalHistoriesByPatient(patientId) {
    if (!patientId) {
        // If no patient selected (All Patients), show all medical histories
        renderMedicalHistoryTable(medicalHistoriesList);
        return;
    }
    
    // Filter medical histories by patient ID
    const filteredHistories = medicalHistoriesList.filter(history => history.patientId == patientId);
    renderMedicalHistoryTable(filteredHistories);
}

// Function to load all medical histories
function loadAllMedicalHistories() {
    // Create a promise for fetching medical histories
    const medicalHistoriesPromise = fetch('/api/Patient/MedicalHistory')
        .then(response => {
            if (!response.ok) {
                // Check if it's a "No Medical History" response
                if (response.status === 404) {
                    return response.json().then(data => {
                        if (data && data.message === "No Medical History") {
                            // This is not an error, just no records found
                            console.log("No medical history records found");
                            return []; // Return empty array
                        } else {
                            throw new Error('Not found: ' + (data.message || 'Unknown error'));
                        }
                    });
                }
                throw new Error('Failed to fetch medical histories');
            }
            return response.json();
        })
        .catch(error => {
            console.error('Error loading medical histories:', error);
            
            // Don't show error for "No Medical History" case - it's already handled
            if (!error.message.includes("No Medical History")) {
                Swal.fire({
                    title: 'Error!',
                    text: 'Failed to load medical histories. Please try again later.',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }
            
            return []; // Return empty array in case of error
        });
    
    // Make sure doctors are loaded before rendering
    // If doctors list is empty, load it. Otherwise, use the existing list
    const doctorsPromise = doctorsList.length > 0 ? Promise.resolve(doctorsList) : loadAllDoctors();
    
    // Wait for both promises to resolve
    Promise.all([medicalHistoriesPromise, doctorsPromise])
        .then(([histories, doctors]) => {
            console.log(`Rendering table with ${histories.length} histories and ${doctors.length} doctors`);
            medicalHistoriesList = histories;
            renderMedicalHistoryTable(histories);
        });
}

// Function to render medical history table
function renderMedicalHistoryTable(histories) {
    const tableBody = document.getElementById('medical-history-table-body');
    const noResult = document.querySelector('.noresult');
    
    if (histories.length === 0) {
        tableBody.innerHTML = '';
        noResult.style.display = 'block';
        return;
    }
    
    noResult.style.display = 'none';
    
    // Initialize list.js if not already initialized
    if (!medicalHistoryTable) {
        medicalHistoryTable = new List('medicalHistoryList', {
            valueNames: ['id', 'patientVisitId', 'doctorId', 'doctorName', 'department', 'reasonForVisit', 'diagnosis', 'treatment', 'prescribedMedication', 'createdDate'],
            page: 10,
            pagination: true
        });
    }
    
    // Clear existing data
    medicalHistoryTable.clear();
    
    // Add new data
    histories.forEach(history => {
        // Format dates
        const createdDate = new Date(history.createdDate).toLocaleString();
        
        // Find doctor name from doctorsList
        const doctor = doctorsList.find(d => d.id === history.doctorId);
        console.log(doctorsList); // Debug log
        const doctorName = doctor ? doctor.name : 'Unknown';
        
        medicalHistoryTable.add({
            id: history.id,
            patientVisitId: history.patientVisitId,
            doctorId: history.doctorId,
            doctorName: doctorName,
            department: history.department || 'Not specified',
            reasonForVisit: history.reasonForVisit || 'Not specified',
            diagnosis: history.diagnosis || 'Not specified',
            treatment: history.treatment || 'Not specified',
            prescribedMedication: history.prescribedMedication || 'Not specified',
            createdDate: createdDate,
            DT_Row_Data: history
        });
    });
    
    // Update UI to show data
    updateMedicalHistoryTableUI();
}

// Function to update medical history table UI
function updateMedicalHistoryTableUI() {
    const tableBody = document.getElementById('medical-history-table-body');
    tableBody.innerHTML = '';
    
    if (medicalHistoryTable && medicalHistoryTable.items.length > 0) {
        medicalHistoryTable.items.forEach(item => {
            const history = item.values();
            const row = document.createElement('tr');
            
            // Add vertical alignment to all cells to ensure middle alignment
            // Don't truncate text anymore - show full content
            row.innerHTML = `
                <td class="id align-middle">${history.id}</td>
                <td class="patientVisitId align-middle">${history.patientVisitId}</td>
                <td class="doctorName align-middle">${history.doctorName}</td>
                <td class="departmentCustom align-middle">${history.department}</td>
                <td class="reasonForVisit align-middle">${history.reasonForVisit}</td>
                <td class="diagnosis align-middle">${history.diagnosis}</td>
                <td class="treatment align-middle">${history.treatment}</td>
                <td class="prescribedMedication align-middle">${history.prescribedMedication}</td>
                <td class="createdDate align-middle">${history.createdDate}</td>
                <td class="align-middle">
                    <div class="d-flex gap-2">
                        <button class="btn btn-sm btn-soft-primary edit-item-btn" onclick="editMedicalHistory(${history.id})">
                            <i class="ri-pencil-fill align-bottom"></i>
                        </button>
                        <button class="btn btn-sm btn-soft-danger remove-item-btn" onclick="confirmDeleteMedicalHistory(${history.id})">
                            <i class="ri-delete-bin-fill align-bottom"></i>
                        </button>
                    </div>
                </td>
            `;
            
            tableBody.appendChild(row);
        });
    }
}

// Function to show medical history form for adding a new record
function showMedicalHistoryForm() {
    currentMedicalHistoryId = null;
    document.getElementById('medical-history-form-title').textContent = 'Create Medical History';
    document.getElementById('medical-history-form-element').reset();
    
    // Clear validation
    const form = document.getElementById('medical-history-form-element');
    form.classList.remove('was-validated');
    
    // Show modal
    const modal = new bootstrap.Modal(document.getElementById('medical-history-form-modal'));
    modal.show();
}

// Function to edit a medical history record
function editMedicalHistory(historyId) {
    currentMedicalHistoryId = historyId;
    document.getElementById('medical-history-form-title').textContent = 'Edit Medical History';
    
    // Get medical history data
    fetch(`/api/Patient/MedicalHistory/${historyId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to fetch medical history details');
            }
            return response.json();
        })
        .then(history => {
            console.log("Loaded history for editing:", history); // Debug log
            
            // Populate form
            document.getElementById('medical-history-id').value = historyId;
            
            // Find and set the patient ID based on patientVisitId
            const patientVisit = visitsList.find(v => v.id === history.patientVisitId);
            if (patientVisit) {
                const patientId = patientVisit.patientId;
                document.getElementById('medical-history-patient-id').value = patientId;
                
                // Update visits dropdown based on patient
                updateVisitsDropdown(patientId);
                
                // Set visit ID after dropdown is populated (with a slight delay)
                setTimeout(() => {
                    document.getElementById('medical-history-visit-id').value = history.patientVisitId;
                }, 200);
            }
            
            // Set doctor
            document.getElementById('medical-history-doctor-id').value = history.doctorId || '';
            
            // Set department
            document.getElementById('medical-history-department').value = history.department || '';
            
            // Set other fields
            document.getElementById('medical-history-condition').value = history.reasonForVisit || '';
            document.getElementById('medical-history-diagnosis').value = history.diagnosis || '';
            document.getElementById('medical-history-treatment').value = history.treatment || '';
            document.getElementById('medical-history-medication').value = history.prescribedMedication || '';
            
            // Show modal
            const modal = new bootstrap.Modal(document.getElementById('medical-history-form-modal'));
            modal.show();
        })
        .catch(error => {
            console.error('Error fetching medical history details:', error);
            Swal.fire({
                title: 'Error!',
                text: 'Failed to load medical history details. Please try again.',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        });
}

// Function to submit medical history form
function submitMedicalHistoryForm() {
    const form = document.getElementById('medical-history-form-element');
    
    // Validate form
    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return;
    }
    
    // Get form data
    const historyData = {
        patientVisitId: parseInt(document.getElementById('medical-history-visit-id').value),
        doctorId: parseInt(document.getElementById('medical-history-doctor-id').value || 0),
        department: document.getElementById('medical-history-department').value || '',
        reasonForVisit: document.getElementById('medical-history-condition').value || '',
        diagnosis: document.getElementById('medical-history-diagnosis').value || '',
        treatment: document.getElementById('medical-history-treatment').value || '',
        prescribedMedication: document.getElementById('medical-history-medication').value || ''
    };
    
    // Determine if create or update
    const isUpdate = currentMedicalHistoryId !== null;
    const url = isUpdate 
        ? `/api/Patient/MedicalHistory/${currentMedicalHistoryId}` 
        : '/api/Patient/MedicalHistory';
    const method = isUpdate ? 'PUT' : 'POST';
    
    // Send request
    fetch(url, {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(historyData)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error(`Failed to ${isUpdate ? 'update' : 'create'} medical history`);
        }
        return response.json();
    })
    .then(data => {
        // Hide modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('medical-history-form-modal'));
        modal.hide();
        
        // Remove backdrop manually
        const modalBackdrop = document.querySelector('.modal-backdrop');
        if (modalBackdrop) {
            modalBackdrop.remove();
        }
        document.body.classList.remove('modal-open');
        document.body.style.removeProperty('padding-right');
        
        // Show success message
        Swal.fire({
            title: 'Success!',
            text: `Medical history ${isUpdate ? 'updated' : 'created'} successfully.`,
            icon: 'success',
            confirmButtonText: 'OK'
        });
        
        // Reload medical histories
        loadAllMedicalHistories();
    })
    .catch(error => {
        console.error(`Error ${isUpdate ? 'updating' : 'creating'} medical history:`, error);
        Swal.fire({
            title: 'Error!',
            text: `Failed to ${isUpdate ? 'update' : 'create'} medical history. Please try again.`,
            icon: 'error',
            confirmButtonText: 'OK'
        });
    });
}

// Function to confirm deletion of a medical history record
function confirmDeleteMedicalHistory(historyId) {
    currentMedicalHistoryId = historyId;
    
    // Show confirmation modal
    const modal = new bootstrap.Modal(document.getElementById('delete-medical-history-modal'));
    modal.show();
    
    // Set up delete button event
    document.getElementById('delete-medical-history-btn').onclick = function() {
        deleteMedicalHistory(historyId);
    };
}

// Function to delete a medical history record
function deleteMedicalHistory(historyId) {
    fetch(`/api/Patient/MedicalHistory/${historyId}`, {
        method: 'DELETE'
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Failed to delete medical history');
        }
        return response.json();
    })
    .then(data => {
        // Hide modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('delete-medical-history-modal'));
        modal.hide();
        
        // Remove backdrop manually
        const modalBackdrop = document.querySelector('.modal-backdrop');
        if (modalBackdrop) {
            modalBackdrop.remove();
        }
        document.body.classList.remove('modal-open');
        document.body.style.removeProperty('padding-right');
        
        // Show success message
        Swal.fire({
            title: 'Deleted!',
            text: 'Medical history has been deleted successfully.',
            icon: 'success',
            confirmButtonText: 'OK'
        });
        
        // Reload medical histories
        loadAllMedicalHistories();
    })
    .catch(error => {
        console.error('Error deleting medical history:', error);
        
        // Hide modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('delete-medical-history-modal'));
        modal.hide();
        
        // Remove backdrop manually
        const modalBackdrop = document.querySelector('.modal-backdrop');
        if (modalBackdrop) {
            modalBackdrop.remove();
        }
        document.body.classList.remove('modal-open');
        document.body.style.removeProperty('padding-right');
        
        // Show error message
        Swal.fire({
            title: 'Error!',
            text: 'Failed to delete medical history. Please try again.',
            icon: 'error',
            confirmButtonText: 'OK'
        });
    });
}

// Add event listener for the add medical history button
document.addEventListener('DOMContentLoaded', function() {
    // Event listener for the add button
    const addMedicalHistoryBtn = document.querySelector('[data-bs-target="#medical-history-form-modal"]');
    if (addMedicalHistoryBtn) {
        addMedicalHistoryBtn.addEventListener('click', function() {
            showMedicalHistoryForm();
        });
    }
    
    // Event listener for the close button
    const closeBtn = document.querySelector('#medical-history-form-modal .btn-close');
    if (closeBtn) {
        closeBtn.addEventListener('click', function() {
            // Manually remove the backdrop and reset body classes
            setTimeout(() => {
                const modalBackdrop = document.querySelector('.modal-backdrop');
                if (modalBackdrop) {
                    modalBackdrop.remove();
                }
                document.body.classList.remove('modal-open');
                document.body.style.removeProperty('padding-right');
            }, 200); // Small delay to ensure Bootstrap has time to start its hiding animation
        });
    }
    
    // Event listener for the cancel button
    const cancelBtn = document.querySelector('#medical-history-form-modal .btn-light');
    if (cancelBtn) {
        cancelBtn.addEventListener('click', function() {
            // Manually remove the backdrop and reset body classes
            setTimeout(() => {
                const modalBackdrop = document.querySelector('.modal-backdrop');
                if (modalBackdrop) {
                    modalBackdrop.remove();
                }
                document.body.classList.remove('modal-open');
                document.body.style.removeProperty('padding-right');
            }, 200); // Small delay to ensure Bootstrap has time to start its hiding animation
        });
    }
});