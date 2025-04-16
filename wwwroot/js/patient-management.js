// Patient Management JavaScript

document.addEventListener('DOMContentLoaded', function () {
    // Initialize AOS animation
    AOS.init();
    
    // Load all patients when page loads
    loadAllPatients();
    
    // Initialize flatpickr for date inputs
    flatpickr("#patient-dob", {
        dateFormat: "Y-m-d",
        maxDate: new Date()
    });
});

// Global variables
let patientsList = [];
let currentPatientId = null;
let patientTable = null;

// Function to initialize List.js for pagination
function initializeListJs() {
    // Clear existing instance if it exists
    if (patientTable) {
        patientTable.clear();
        patientTable.destroy();
    }
    
    // Initialize with the correct approach
    patientTable = new List('patientList', {
        valueNames: ['id', 'name', 'email', 'phone', 'gender', 'dob', 'address'],
        page: 10,
        pagination: true,
        item: function(values) {
            // Return the existing DOM element
            return values.elm;
        }
    });
}

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
            renderPatientTable(data);
        })
        .catch(error => {
            console.error('Error loading patients:', error);
            Swal.fire({
                title: 'Error!',
                text: 'Failed to load patients. Please try again later.',
                icon: 'error',
                confirmButtonText: 'OK'
            });
            
            // Show empty table
            renderPatientTable([]);
        });
}

// Function to render patient table
function renderPatientTable(patients) {
    const tableBody = document.getElementById('patient-table-body');
    const noResult = document.querySelector('.noresult');
    
    if (patients.length === 0) {
        tableBody.innerHTML = '';
        noResult.style.display = 'block';
        return;
    }
    
    noResult.style.display = 'none';
    
    // Clear the table first
    tableBody.innerHTML = '';
    
    // Create HTML elements for each patient
    patients.forEach(patient => {
        // Format the date of birth
        const dob = patient.dateOfBirth ? new Date(patient.dateOfBirth).toLocaleDateString() : 'N/A';
        
        // Create a new row
        const row = document.createElement('tr');
        
        row.innerHTML = `
            <td class="id">${patient.id}</td>
            <td class="name">${patient.name || 'N/A'}</td>
            <td class="email">${patient.email || 'N/A'}</td>
            <td class="phone">${patient.phoneNumber || 'N/A'}</td>
            <td class="gender">${patient.gender || 'N/A'}</td>
            <td class="dob">${dob}</td>
            <td class="address">${patient.address || 'N/A'}</td>
            <td>
                <div class="dropdown">
                    <button class="btn btn-soft-secondary btn-sm dropdown" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                        <i class="ri-more-fill align-middle"></i>
                    </button>
                    <ul class="dropdown-menu dropdown-menu-end">
                        <li><a class="dropdown-item view-item-btn" href="javascript:void(0);" onclick="viewPatient(${patient.id})"><i class="ri-eye-fill align-bottom me-2 text-muted"></i> View</a></li>
                        <li><a class="dropdown-item edit-item-btn" href="javascript:void(0);" onclick="editPatient(${patient.id})"><i class="ri-pencil-fill align-bottom me-2 text-muted"></i> Edit</a></li>
                        <li><a class="dropdown-item remove-item-btn" href="javascript:void(0);" onclick="confirmDeletePatient(${patient.id})"><i class="ri-delete-bin-fill align-bottom me-2 text-muted"></i> Delete</a></li>
                    </ul>
                </div>
            </td>
        `;
        
        tableBody.appendChild(row);
    });
    
    // Initialize List.js after the table is populated
    initializeListJs();
}

// Function to show patient form for adding a new patient
function showPatientForm() {
    currentPatientId = null;
    document.getElementById('form-title').textContent = 'Create Patient';
    document.getElementById('patient-form-element').reset();
    
    // Clear validation
    const form = document.getElementById('patient-form-element');
    form.classList.remove('was-validated');
    
    // Show modal
    const modal = new bootstrap.Modal(document.getElementById('patient-form-modal'));
    modal.show();
}

// Function to edit a patient
function editPatient(patientId) {
    currentPatientId = patientId;
    document.getElementById('form-title').textContent = 'Edit Patient';
    
    // Get patient data
    fetch(`/api/Patient/PatientInfo/${patientId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to fetch patient details');
            }
            return response.json();
        })
        .then(patient => {
            // Populate form
            document.getElementById('patient-id').value = patientId;
            document.getElementById('patient-name').value = patient.name || '';
            document.getElementById('patient-email').value = patient.email || '';
            document.getElementById('patient-phone').value = patient.phoneNumber || '';
            document.getElementById('patient-address').value = patient.address || '';
            document.getElementById('patient-gender').value = patient.gender === 'Male' ? 'true' : 'false';
            document.getElementById('patient-insurance').value = patient.healthInsuranceId || '';
            
            // Format date for the date picker
            if (patient.dateOfBirth) {
                const date = new Date(patient.dateOfBirth);
                const formattedDate = date.toISOString().split('T')[0];
                document.getElementById('patient-dob').value = formattedDate;
            }
            
            // Show modal
            const modal = new bootstrap.Modal(document.getElementById('patient-form-modal'));
            modal.show();
        })
        .catch(error => {
            console.error('Error fetching patient details:', error);
            Swal.fire({
                title: 'Error!',
                text: 'Failed to load patient details. Please try again.',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        });
}

// Function to submit patient form
function submitPatientForm() {
    const form = document.getElementById('patient-form-element');
    
    // Validate form
    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return;
    }
    
    // Get form data
    const patientData = {
        name: document.getElementById('patient-name').value,
        email: document.getElementById('patient-email').value,
        phoneNumber: document.getElementById('patient-phone').value,
        address: document.getElementById('patient-address').value,
        gender: document.getElementById('patient-gender').value === 'true',
        healthInsuranceId: document.getElementById('patient-insurance').value,
        dateOfBirth: document.getElementById('patient-dob').value
    };
    
    // Determine if create or update
    const isUpdate = currentPatientId !== null;
    const url = isUpdate 
        ? `/api/Patient/PatientInfo/${currentPatientId}` 
        : '/api/Patient/PatientInfo';
    const method = isUpdate ? 'PUT' : 'POST';
    
    // Send request
    fetch(url, {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(patientData)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error(`Failed to ${isUpdate ? 'update' : 'create'} patient`);
        }
        return response.json();
    })
    .then(data => {
        // Hide modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('patient-form-modal'));
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
            text: `Patient ${isUpdate ? 'updated' : 'created'} successfully.`,
            icon: 'success',
            confirmButtonText: 'OK'
        });
        
        // Reload patients
        loadAllPatients();
    })
    .catch(error => {
        console.error(`Error ${isUpdate ? 'updating' : 'creating'} patient:`, error);
        Swal.fire({
            title: 'Error!',
            text: `Failed to ${isUpdate ? 'update' : 'create'} patient. Please try again.`,
            icon: 'error',
            confirmButtonText: 'OK'
        });
    });
}

// Function to confirm deletion of a patient
function confirmDeletePatient(patientId) {
    currentPatientId = patientId;
    
    // Show confirmation modal
    const modal = new bootstrap.Modal(document.getElementById('delete-confirmation-modal'));
    modal.show();
    
    // Set up delete button event
    document.getElementById('delete-patient-btn').onclick = function() {
        deletePatient(patientId);
    };
}

// Function to delete a patient
function deletePatient(patientId) {
    fetch(`/api/Patient/PatientInfo/${patientId}`, {
        method: 'DELETE'
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Failed to delete patient');
        }
        return response.json();
    })
    .then(data => {
        // Hide modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('delete-confirmation-modal'));
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
            text: 'Patient has been deleted successfully.',
            icon: 'success',
            confirmButtonText: 'OK'
        });
        
        // Reload patients
        loadAllPatients();
    })
    .catch(error => {
        console.error('Error deleting patient:', error);
        
        // Hide modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('delete-confirmation-modal'));
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
            text: 'Failed to delete patient. Please try again.',
            icon: 'error',
            confirmButtonText: 'OK'
        });
    });
}

// Add event listener for the add patient button
document.addEventListener('DOMContentLoaded', function() {
    const addPatientBtn = document.querySelector('[data-bs-target="#patient-form-modal"]');
    if (addPatientBtn) {
        addPatientBtn.addEventListener('click', function() {
            showPatientForm();
        });
    }
});