// Patient Visits Management JavaScript

document.addEventListener('DOMContentLoaded', function () {
    // Initialize AOS animation
    AOS.init();
    
    // Load all patients first, then load visits after patients are loaded
    loadAllPatients();
    
    // Initialize flatpickr for date inputs
    flatpickr("#visit-date", {
        enableTime: true,
        dateFormat: "Y-m-d H:i",
        time_24hr: true
    });
});

// Global variables
let patientsList = [];
let visitsList = [];
let currentVisitId = null;
let visitTable = null;

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
            
            // Now that we have patient data, load visits
            loadAllVisits();
        })
        .catch(error => {
            console.error('Error loading patients:', error);
            Swal.fire({
                title: 'Error!',
                text: 'Failed to load patients. Please try again later.',
                icon: 'error',
                confirmButtonText: 'OK'
            });
            
            // Load visits anyway to show at least the visits data
            loadAllVisits();
        });
}

// Function to populate patient dropdowns
function populatePatientDropdowns(patients) {
    // Populate filter dropdown
    const filterPatient = document.getElementById('filterPatient');
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
            filterVisitsByPatient(this.value);
        });
    }
    
    // Populate form dropdown
    const visitPatientId = document.getElementById('visit-patient-id');
    if (visitPatientId) {
        // Keep the first option (Select Patient)
        visitPatientId.innerHTML = '<option value="">Select Patient</option>';
        
        // Add patient options
        patients.forEach(patient => {
            const option = document.createElement('option');
            option.value = patient.id;
            option.textContent = `${patient.id} - ${patient.name}`;
            visitPatientId.appendChild(option);
        });
    }
}

// Function to filter visits by patient ID
function filterVisitsByPatient(patientId) {
    if (!patientId) {
        // If no patient selected (All Patients), show all visits
        renderVisitTable(visitsList);
        return;
    }
    
    // Filter visits by patient ID
    const filteredVisits = visitsList.filter(visit => visit.patientId == patientId);
    renderVisitTable(filteredVisits);
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
            renderVisitTable(data);
        })
        .catch(error => {
            console.error('Error loading visits:', error);
            Swal.fire({
                title: 'Error!',
                text: 'Failed to load patient visits. Please try again later.',
                icon: 'error',
                confirmButtonText: 'OK'
            });
            
            // Show empty table
            renderVisitTable([]);
        });
}

// Function to initialize List.js for pagination
function initializeListJs() {
    // Clear existing instance if it exists
    if (visitTable) {
        visitTable.clear();
        visitTable.destroy();
    }
    
    // Initialize with the correct approach
    visitTable = new List('visitList', {
        valueNames: ['id', 'patientId', 'patientName', 'visitDate', 'notes'],
        page: 10,
        pagination: true,
        item: function(values) {
            // Return the existing DOM element
            return values.elm;
        }
    });
}

// Function to render visit table
function renderVisitTable(visits) {
    const tableBody = document.getElementById('visit-table-body');
    const noResult = document.querySelector('.noresult');
    
    if (visits.length === 0) {
        tableBody.innerHTML = '';
        noResult.style.display = 'block';
        return;
    }
    
    noResult.style.display = 'none';
    
    // Clear the table first
    tableBody.innerHTML = '';
    
    // Create HTML elements for each visit
    visits.forEach(visit => {
        // Format the visit date
        const visitDate = new Date(visit.visitDate).toLocaleString();
        
        // Find patient name
        const patient = patientsList.find(p => p.id === visit.patientId);
        const patientName = patient ? patient.name : 'Unknown';
        
        // Create a new row
        const row = document.createElement('tr');
        
        row.innerHTML = `
            <td class="id">${visit.id}</td>
            <td class="patientId">${visit.patientId}</td>
            <td class="patientName">${patientName}</td>
            <td class="visitDate">${visitDate}</td>
            <td class="notes">${visit.notes || 'No notes'}</td>
            <td>
                <div class="dropdown">
                    <button class="btn btn-soft-secondary btn-sm dropdown" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                        <i class="ri-more-fill align-middle"></i>
                    </button>
                    <ul class="dropdown-menu dropdown-menu-end">
                        <li><a class="dropdown-item edit-item-btn" href="javascript:void(0);" onclick="editVisit(${visit.id})"><i class="ri-pencil-fill align-bottom me-2 text-muted"></i> Edit</a></li>
                        <li><a class="dropdown-item remove-item-btn" href="javascript:void(0);" onclick="confirmDeleteVisit(${visit.id})"><i class="ri-delete-bin-fill align-bottom me-2 text-muted"></i> Delete</a></li>
                    </ul>
                </div>
            </td>
        `;
        
        tableBody.appendChild(row);
    });
    
    // Initialize List.js after the table is populated
    initializeListJs();
}

// Function to update visits table UI
function updateVisitsTableUI() {
    const tableBody = document.getElementById('visit-table-body');
    tableBody.innerHTML = '';
    
    if (visitTable && visitTable.items.length > 0) {
        visitTable.items.forEach(item => {
            const visit = item.values();
            const row = document.createElement('tr');
            
            row.innerHTML = `
                <td class="id">${visit.id}</td>
                <td class="patientId">${visit.patientId}</td>
                <td class="patientName">${visit.patientName}</td>
                <td class="visitDate">${visit.visitDate}</td>
                <td class="notes">${visit.notes}</td>
                <td>
                    <div class="dropdown">
                        <button class="btn btn-soft-secondary btn-sm dropdown" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                            <i class="ri-more-fill align-middle"></i>
                        </button>
                        <ul class="dropdown-menu dropdown-menu-end">
                            <li><a class="dropdown-item edit-item-btn" href="javascript:void(0);" onclick="editVisit(${visit.id})"><i class="ri-pencil-fill align-bottom me-2 text-muted"></i> Edit</a></li>
                            <li><a class="dropdown-item remove-item-btn" href="javascript:void(0);" onclick="confirmDeleteVisit(${visit.id})"><i class="ri-delete-bin-fill align-bottom me-2 text-muted"></i> Delete</a></li>
                        </ul>
                    </div>
                </td>
            `;
            
            tableBody.appendChild(row);
        });
    }
}

// Function to show visit form for adding a new visit
function showVisitForm() {
    currentVisitId = null;
    document.getElementById('visit-form-title').textContent = 'Create Visit';
    document.getElementById('visit-form-element').reset();
    
    // Clear validation
    const form = document.getElementById('visit-form-element');
    form.classList.remove('was-validated');
    
    // Show modal
    const modal = new bootstrap.Modal(document.getElementById('visit-form-modal'));
    modal.show();
}

// Function to edit a visit
function editVisit(visitId) {
    currentVisitId = visitId;
    document.getElementById('visit-form-title').textContent = 'Edit Visit';
    
    // Get visit data
    fetch(`/api/Patient/PatientVisit/${visitId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to fetch visit details');
            }
            return response.json();
        })
        .then(visit => {
            // Populate form
            document.getElementById('visit-id').value = visitId;
            document.getElementById('visit-patient-id').value = visit.patientId;
            document.getElementById('visit-notes').value = visit.notes || '';
            
            // Format date for the date picker
            if (visit.visitDate) {
                const date = new Date(visit.visitDate);
                const formattedDate = date.toISOString().slice(0, 16);
                document.getElementById('visit-date').value = formattedDate;
            }
            
            // Show modal
            const modal = new bootstrap.Modal(document.getElementById('visit-form-modal'));
            modal.show();
        })
        .catch(error => {
            console.error('Error fetching visit details:', error);
            Swal.fire({
                title: 'Error!',
                text: 'Failed to load visit details. Please try again.',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        });
}

// Function to submit visit form
function submitVisitForm() {
    const form = document.getElementById('visit-form-element');
    
    // Validate form
    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return;
    }
    
    // Get form data and ensure proper date format
    const visitDateInput = document.getElementById('visit-date').value;
    // Parse and format the date to ensure ISO format compatibility with .NET DateTime
    const visitDate = new Date(visitDateInput);
    
    const visitData = {
        patientId: parseInt(document.getElementById('visit-patient-id').value),
        visitDate: visitDate.toISOString(),
        notes: document.getElementById('visit-notes').value || null // Ensure null instead of empty string if no notes
    };
    
    // Determine if create or update
    const isUpdate = currentVisitId !== null;
    const url = isUpdate 
        ? `/api/Patient/PatientVisit/${currentVisitId}` 
        : '/api/Patient/PatientVisit';
    const method = isUpdate ? 'PUT' : 'POST';
    
    // Send request
    fetch(url, {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(visitData)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error(`Failed to ${isUpdate ? 'update' : 'create'} visit`);
        }
        return response.json();
    })
    .then(data => {
        // Hide modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('visit-form-modal'));
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
            text: `Visit ${isUpdate ? 'updated' : 'created'} successfully.`,
            icon: 'success',
            confirmButtonText: 'OK'
        });
        
        // Reload visits
        loadAllVisits();
    })
    .catch(error => {
        console.error(`Error ${isUpdate ? 'updating' : 'creating'} visit:`, error);
        Swal.fire({
            title: 'Error!',
            text: `Failed to ${isUpdate ? 'update' : 'create'} visit. Please try again.`,
            icon: 'error',
            confirmButtonText: 'OK'
        });
    });
}

// Function to confirm deletion of a visit
function confirmDeleteVisit(visitId) {
    currentVisitId = visitId;
    
    // Show confirmation modal
    const modal = new bootstrap.Modal(document.getElementById('delete-visit-modal'));
    modal.show();
    
    // Set up delete button event
    document.getElementById('delete-visit-btn').onclick = function() {
        deleteVisit(visitId);
    };
}

// Function to delete a visit
function deleteVisit(visitId) {
    fetch(`/api/Patient/PatientVisit/${visitId}`, {
        method: 'DELETE'
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Failed to delete visit');
        }
        return response.json();
    })
    .then(data => {
        // Hide modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('delete-visit-modal'));
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
            text: 'Patient visit has been deleted successfully.',
            icon: 'success',
            confirmButtonText: 'OK'
        });
        
        // Reload visits
        loadAllVisits();
    })
    .catch(error => {
        console.error('Error deleting visit:', error);
        
        // Hide modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('delete-visit-modal'));
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
            text: 'Failed to delete visit. Please try again.',
            icon: 'error',
            confirmButtonText: 'OK'
        });
    });
}

// Add event listener for the add visit button
document.addEventListener('DOMContentLoaded', function() {
    const addVisitBtn = document.querySelector('[data-bs-target="#visit-form-modal"]');
    if (addVisitBtn) {
        addVisitBtn.addEventListener('click', function() {
            showVisitForm();
        });
    }
});