document.addEventListener('DOMContentLoaded', function () {
    // Initialize AOS animation library
    AOS.init({
        easing: 'ease-out-cubic',
        once: true,
        offset: 50,
        delay: 50,
    });

    // Initialize flatpickr for date inputs
    flatpickr("#employee-dob", {
        dateFormat: "Y-m-d",
        maxDate: new Date(),
        animate: true,
    });

    // Initialize choices.js for enhanced select dropdowns
    document.querySelectorAll('[data-choices]').forEach(element => {
        const choices = new Choices(element, {
            searchEnabled: element.getAttribute("data-choices-search-false") ? false : true,
            itemSelectText: '',
            shouldSort: false,
        });
    });

    // Check for selected elements
    let selectedEmployees = [];
    
    // Initialize checkAll
    document.getElementById("checkAll").addEventListener("change", function() {
        document.querySelectorAll('tbody .form-check-input').forEach(checkbox => {
            checkbox.checked = this.checked;
            handleEmployeeSelection(checkbox);
        });
        toggleMultipleDeleteButton();
    });

    // Load employees with animation
    loadEmployees();

    // Setup search functionality
    const searchBox = document.querySelector('.search');
    if (searchBox) {
        searchBox.addEventListener('input', function(e) {
            const searchTerm = e.target.value.toLowerCase();
            filterEmployees(searchTerm);
        });
    }

    // Department filter
    const departmentFilter = document.getElementById('filterDepartment');
    if (departmentFilter) {
        departmentFilter.addEventListener('change', function() {
            filterByDepartment(this.value);
        });
    }

    // Set up multiple delete button
    document.getElementById('remove-multiple-confirm-btn').addEventListener('click', function() {
        deleteMultipleEmployees(selectedEmployees);
    });
});

// Function to load employees from API
function loadEmployees() {
    fetch('/api/employees')
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to fetch employees');
            }
            return response.json();
        })
        .then(data => {
            renderEmployees(data);
            initializeListJs();
        })
        .catch(error => {
            console.error('Error:', error);
            showErrorToast('Failed to load employees. Please try again.');
            document.getElementById('employee-table-body').innerHTML = `
                <tr>
                    <td colspan="6" class="text-center">
                        <div class="d-flex align-items-center justify-content-center py-4">
                            <i class="ri-error-warning-line fs-24 text-danger me-2"></i>
                            <span>Error loading employees. Please refresh and try again.</span>
                        </div>
                    </td>
                </tr>
            `;
        });
}

// Function to render employees in the table
function renderEmployees(employees) {
    const tableBody = document.getElementById('employee-table-body');
    
    if (!employees || employees.length === 0) {
        document.querySelector('.noresult').style.display = 'block';
        tableBody.innerHTML = '';
        return;
    }
    
    document.querySelector('.noresult').style.display = 'none';
    
    let html = '';
    employees.forEach((employee, index) => {
        const delay = index * 50; // Stagger the animations
        html += `
            <tr class="animate__animated animate__fadeIn" style="animation-delay: ${delay}ms;">
                <td>
                    <div class="form-check">
                        <input class="form-check-input" type="checkbox" value="${employee.id}" onchange="handleEmployeeSelection(this)">
                    </div>
                </td>
                <td class="id">
                    <a href="javascript:void(0);" class="fw-medium link-primary employee-id-badge" data-id="${employee.id}">#EMP${employee.id.toString().padStart(3, '0')}</a>
                </td>
                <td class="name">
                    <div class="d-flex align-items-center">
                        <div class="flex-shrink-0">
                            <div class="avatar-circle" style="background-color: ${getRandomPastelColor(employee.name)}">
                                ${getInitials(employee.name)}
                            </div>
                        </div>
                        <div class="flex-grow-1 ms-2">
                            <h5 class="fs-14 mb-0">${employee.name}</h5>
                            <span class="text-muted">${employee.email || 'No email'}</span>
                        </div>
                    </div>
                </td>
                <td class="role">
                    <span class="badge bg-soft-${getRoleBadgeColor(employee.role)} text-${getRoleBadgeColor(employee.role)}">${employee.role || 'Unassigned'}</span>
                </td>
                <td class="departmentCustom">
                    <span class="department-badge bg-soft-info text-info">${employee.department || 'Unassigned'}</span>
                </td>
                <td>
                    <div class="dropdown">
                        <button class="btn btn-soft-secondary btn-sm dropdown" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                            <i class="ri-more-fill align-middle"></i>
                        </button>
                        <ul class="dropdown-menu dropdown-menu-end">
                            <li><a class="dropdown-item" href="javascript:void(0);" onclick="viewEmployeeDetails(${employee.id})"><i class="ri-eye-fill align-bottom me-2 text-muted"></i> View</a></li>
                            <li><a class="dropdown-item" href="javascript:void(0);" onclick="editEmployee(${employee.id})"><i class="ri-pencil-fill align-bottom me-2 text-muted"></i> Edit</a></li>
                            <li><a class="dropdown-item" href="javascript:void(0);" onclick="manageCredentials(${employee.id}, '${employee.name}')"><i class="ri-file-list-3-line align-bottom me-2 text-muted"></i> Credentials</a></li>
                            <li><a class="dropdown-item" href="javascript:void(0);" onclick="deleteEmployee(${employee.id})"><i class="ri-delete-bin-fill align-bottom me-2 text-muted"></i> Delete</a></li>
                        </ul>
                    </div>
                </td>
            </tr>
        `;
    });
    
    tableBody.innerHTML = html;

    // Add hover effects to employee rows
    document.querySelectorAll('#employee-table-body tr').forEach(row => {
        row.addEventListener('mouseenter', function() {
            this.classList.add('highlight-row');
        });
        row.addEventListener('mouseleave', function() {
            this.classList.remove('highlight-row');
        });
    });

    // Add click effect for employee badges
    document.querySelectorAll('.employee-id-badge').forEach(badge => {
        badge.addEventListener('click', function() {
            const employeeId = this.getAttribute('data-id');
            viewEmployeeDetails(employeeId);
        });
    });

    // Add custom styling
    addCustomStyling();
}

// Function to get random pastel color based on name
function getRandomPastelColor(str) {
    if (!str) str = "Unknown";
    // Generate a hash of the string
    let hash = 0;
    for (let i = 0; i < str.length; i++) {
        hash = str.charCodeAt(i) + ((hash << 5) - hash);
    }
    
    // Generate pastel color
    const h = hash % 360;
    return `hsl(${h}, 70%, 80%)`;
}

// Function to get initials from name
function getInitials(name) {
    if (!name) return "NA";
    const parts = name.split(' ');
    if (parts.length === 1) return parts[0].charAt(0).toUpperCase();
    return (parts[0].charAt(0) + parts[parts.length - 1].charAt(0)).toUpperCase();
}

// Function to get role badge color
function getRoleBadgeColor(role) {
    const roleColors = {
        'Doctor': 'success',
        'Nurse': 'info',
        'Technician': 'warning',
        'Administrator': 'primary',
        'Receptionist': 'secondary',
        'Specialist': 'danger'
    };
    
    return roleColors[role] || 'dark';
}

// Utility function to format date
function formatDate(dateString) {
    if (!dateString) return 'N/A';
    
    const date = new Date(dateString);
    if (isNaN(date.getTime())) return 'Invalid Date';
    
    return new Intl.DateTimeFormat('en-US', {
        year: 'numeric',
        month: 'short',
        day: 'numeric'
    }).format(date);
}

// Function to add custom styling
function addCustomStyling() {
    // Add styling for avatar circles
    const style = document.createElement('style');
    style.textContent = `
        .avatar-circle {
            width: 36px;
            height: 36px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-weight: bold;
            color: #333;
            transition: all 0.3s ease;
        }
        .avatar-circle:hover {
            transform: scale(1.1);
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }
        .department-badge {
            padding: 5px 10px;
            border-radius: 4px;
            font-size: 14px;
            font-weight: 500;
        }
        /* Role badge styling - increased size */
        .role .badge {
            font-size: 14px !important;
            padding: 6px 12px !important;
            font-weight: 600 !important;
            letter-spacing: 0.3px;
            display: inline-block;
            box-shadow: 0 2px 4px rgba(0,0,0,0.05);
            transition: all 0.2s ease;
        }
        .role .badge:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 6px rgba(0,0,0,0.1);
        }
        .highlight-row {
            background-color: rgba(0, 105, 255, 0.04);
            transition: background-color 0.3s ease;
        }
        .status-indicator {
            width: 10px;
            height: 10px;
            border-radius: 50%;
            display: inline-block;
            margin-right: 5px;
        }
        .status-active {
            background-color: #0ab39c;
            box-shadow: 0 0 10px #0ab39c;
            animation: pulse 2s infinite;
        }
        .status-expired {
            background-color: #f06548;
        }
        .status-warning {
            background-color: #f7b84b;
        }
        @keyframes pulse {
            0% {
                box-shadow: 0 0 0 0 rgba(10, 179, 156, 0.7);
            }
            70% {
                box-shadow: 0 0 0 6px rgba(10, 179, 156, 0);
            }
            100% {
                box-shadow: 0 0 0 0 rgba(10, 179, 156, 0);
            }
        }
        
        /* Custom animation for modal */
        .modal.fade .modal-dialog {
            transition: transform 0.3s ease-out, opacity 0.3s;
            transform: translateY(-20px);
            opacity: 0;
        }
        .modal.show .modal-dialog {
            transform: translateY(0);
            opacity: 1;
        }
        
        /* Custom form input effects */
        .form-control:focus, .form-select:focus {
            box-shadow: 0 0 0 0.15rem rgba(0, 105, 255, 0.25);
            border-color: #0069ff;
            transition: all 0.3s ease;
        }
        
        /* Table row hover effects */
        #employeeTable tbody tr {
            transition: all 0.2s ease;
        }
        #employeeTable tbody tr:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.05);
            z-index: 1;
            position: relative;
        }
    `;
    document.head.appendChild(style);
}

// Function to initialize List.js for pagination
function initializeListJs() {
    // Clear existing instance if it exists
    if (employeeList) {
        employeeList.clear();
        employeeList.destroy();
    }
    
    // Initialize with the correct approach
    employeeList = new List('employeeList', {
        valueNames: ['id', 'name', 'role', 'departmentCustom'],
        page: 10,
        pagination: true,
        item: function(values) {
            // Return the existing DOM element
            return values.elm;
        }
    });
    
    // Add event listener for pagination
    employeeList.on('updated', function() {
        // Make the page changes animate
        const items = document.querySelectorAll('#employee-table-body tr');
        items.forEach((item, index) => {
            item.classList.add('animate__animated', 'animate__fadeIn');
            item.style.animationDelay = (index * 50) + 'ms';
        });
    });
}

// Function to filter employees by search term
function filterEmployees(searchTerm) {
    const rows = document.querySelectorAll('#employee-table-body tr');
    let foundMatch = false;
    
    rows.forEach(row => {
        const name = row.querySelector('.name')?.textContent.toLowerCase() || '';
        const role = row.querySelector('.role')?.textContent.toLowerCase() || '';
        const department = row.querySelector('.departmentCustom')?.textContent.toLowerCase() || '';
        const id = row.querySelector('.id')?.textContent.toLowerCase() || '';
        
        if (name.includes(searchTerm) || role.includes(searchTerm) || 
            department.includes(searchTerm) || id.includes(searchTerm)) {
            row.style.display = '';
            foundMatch = true;
            // Add highlight animation
            row.classList.add('animate__animated', 'animate__pulse');
            setTimeout(() => {
                row.classList.remove('animate__animated', 'animate__pulse');
            }, 1000);
        } else {
            row.style.display = 'none';
        }
    });
    
    // Show/hide no result message
    document.querySelector('.noresult').style.display = foundMatch ? 'none' : 'block';
}

// Function to filter by department
function filterByDepartment(department) {
    const rows = document.querySelectorAll('#employee-table-body tr');
    let foundMatch = false;
    
    rows.forEach(row => {
        const deptCell = row.querySelector('.departmentCustom')?.textContent.trim() || '';
        
        if (!department || deptCell.includes(department)) {
            row.style.display = '';
            foundMatch = true;
            // Add fade-in animation
            row.classList.add('animate__animated', 'animate__fadeIn');
            setTimeout(() => {
                row.classList.remove('animate__animated', 'animate__fadeIn');
            }, 1000);
        } else {
            row.style.display = 'none';
        }
    });
    
    // Show/hide no result message
    document.querySelector('.noresult').style.display = foundMatch ? 'none' : 'block';
}

// Function to handle employee selection
function handleEmployeeSelection(checkbox) {
    const employeeId = parseInt(checkbox.value);
    if (checkbox.checked) {
        if (!selectedEmployees.includes(employeeId)) {
            selectedEmployees.push(employeeId);
            // Add subtle animation to the row
            checkbox.closest('tr').classList.add('selected-row');
        }
    } else {
        selectedEmployees = selectedEmployees.filter(id => id !== employeeId);
        checkbox.closest('tr').classList.remove('selected-row');
    }
    
    toggleMultipleDeleteButton();
}

// Function to toggle multiple delete button visibility
function toggleMultipleDeleteButton() {
    const deleteBtn = document.getElementById('remove-multiple-btn');
    if (selectedEmployees.length > 0) {
        deleteBtn.disabled = false;
        deleteBtn.classList.add('animate__animated', 'animate__heartBeat');
        setTimeout(() => {
            deleteBtn.classList.remove('animate__animated', 'animate__heartBeat');
        }, 1000);
    } else {
        deleteBtn.disabled = true;
    }
}

// Function to delete multiple employees
function deleteMultipleEmployees(employeeIds) {
    if (!employeeIds || employeeIds.length === 0) {
        return;
    }
    
    // Close modal
    const modal = bootstrap.Modal.getInstance(document.getElementById('removeMultipleEmployeeModal'));
    modal.hide();
    
    // Show loading state
    Swal.fire({
        title: 'Deleting...',
        text: 'Removing selected employees',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
    
    // Delete employees one by one
    const deletePromises = employeeIds.map(id => 
        fetch(`/api/employees/${id}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            }
        })
    );
    
    Promise.all(deletePromises)
        .then(() => {
            Swal.fire({
                title: 'Deleted!',
                text: 'Employees have been deleted successfully.',
                icon: 'success',
                showConfirmButton: false,
                timer: 1500
            }).then(() => {
                // Reset selected employees
                selectedEmployees = [];
                // Reload employees
                loadEmployees();
            });
        })
        .catch(error => {
            console.error('Error:', error);
            Swal.fire({
                title: 'Error!',
                text: 'Failed to delete employees. Please try again.',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        });
}

// Function to delete a single employee
function deleteEmployee(employeeId) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/api/employees/${employeeId}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to delete employee');
                }
                
                Swal.fire({
                    title: 'Deleted!',
                    text: 'Employee has been deleted.',
                    icon: 'success',
                    showConfirmButton: false,
                    timer: 1500,
                    didClose: () => {
                        // Find and remove the deleted row with animation
                        const row = document.querySelector(`input[value="${employeeId}"]`).closest('tr');
                        row.classList.add('animate__animated', 'animate__fadeOutRight');
                        setTimeout(() => {
                            loadEmployees(); // Reload all data
                        }, 500);
                    }
                });
            })
            .catch(error => {
                console.error('Error:', error);
                Swal.fire({
                    title: 'Error!',
                    text: 'Failed to delete employee. Please try again.',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            });
        }
    });
}

// Function to show error toast
function showErrorToast(message) {
    Swal.fire({
        icon: 'error',
        title: 'Error',
        text: message,
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true
    });
}

// Add event listener to document to show animations when scrolling
document.addEventListener('scroll', function() {
    const elements = document.querySelectorAll('[data-aos]');
    elements.forEach(element => {
        const elementPosition = element.getBoundingClientRect();
        if (elementPosition.top < window.innerHeight && elementPosition.bottom > 0) {
            element.classList.add('aos-animate');
        }
    });
});

// Function to view employee details
function viewEmployeeDetails(employeeId) {
    // Show loading spinner
    Swal.fire({
        title: 'Loading...',
        text: 'Fetching employee details',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
    
    // Fetch employee details from API
    fetch(`/api/employees/${employeeId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to fetch employee details');
            }
            return response.json();
        })
        .then(employee => {
            Swal.close();
            
            // Create and show modal with employee details
            const modal = new bootstrap.Modal(document.getElementById('viewEmployeeModal') || createViewEmployeeModal());
            
            // Populate modal with employee data
            document.getElementById('view-employee-name').textContent = employee.name || 'N/A';
            document.getElementById('view-employee-id').textContent = `#EMP${employee.id.toString().padStart(3, '0')}`;
            document.getElementById('view-employee-role').textContent = employee.role || 'Unassigned';
            document.getElementById('view-employee-department').textContent = employee.department || 'Unassigned';
            document.getElementById('view-employee-email').textContent = employee.email || 'N/A';
            document.getElementById('view-employee-phone').textContent = employee.phoneNumber || 'N/A';
            document.getElementById('view-employee-dob').textContent = formatDate(employee.dateOfBirth) || 'N/A';
            document.getElementById('view-employee-address').textContent = employee.address || 'N/A';
            document.getElementById('view-employee-gender').textContent = employee.gender || 'N/A';
            
            // Set avatar
            const avatarElement = document.getElementById('view-employee-avatar');
            avatarElement.style.backgroundColor = getRandomPastelColor(employee.name);
            avatarElement.textContent = getInitials(employee.name);
            
            // Show credentials if any (combine License and Certificates)
            const credentialsContainer = document.getElementById('view-employee-credentials');
            
            // Check if either licenses or certificates exist
            const hasLicenses = employee.license && employee.license.length > 0;
            const hasCertificates = employee.certificates && employee.certificates.length > 0;
            
            if (hasLicenses || hasCertificates) {
                let credentialsHtml = '<div class="table-responsive mt-3"><table class="table table-bordered table-sm">';
                credentialsHtml += '<thead><tr><th>Type</th><th>Number</th><th>Issue Date</th><th>Expiry Date</th><th>Status</th></tr></thead><tbody>';
                
                // Process licenses
                if (hasLicenses) {
                    employee.license.forEach(license => {
                        const now = new Date();
                        const expiryDate = new Date(license.expirationDate);
                        let status = 'Active';
                        let statusClass = 'status-active';
                        
                        if (expiryDate < now) {
                            status = 'Expired';
                            statusClass = 'status-expired';
                        } else if ((expiryDate - now) / (1000 * 60 * 60 * 24) < 30) {
                            status = 'Expiring Soon';
                            statusClass = 'status-warning';
                        }
                        
                        credentialsHtml += `
                            <tr>
                                <td>License</td>
                                <td>${license.number || 'N/A'}</td>
                                <td>${formatDate(license.issueDate)}</td>
                                <td>${formatDate(license.expirationDate)}</td>
                                <td><span class="status-indicator ${statusClass}"></span>${status}</td>
                            </tr>
                        `;
                    });
                }
                
                // Process certificates
                if (hasCertificates) {
                    employee.certificates.forEach(cert => {
                        const now = new Date();
                        const expiryDate = new Date(cert.expirationDate);
                        let status = 'Active';
                        let statusClass = 'status-active';
                        
                        if (expiryDate < now) {
                            status = 'Expired';
                            statusClass = 'status-expired';
                        } else if ((expiryDate - now) / (1000 * 60 * 60 * 24) < 30) {
                            status = 'Expiring Soon';
                            statusClass = 'status-warning';
                        }
                        
                        credentialsHtml += `
                            <tr>
                                <td>Certificate</td>
                                <td>${cert.number || 'N/A'}</td>
                                <td>${formatDate(cert.issueDate)}</td>
                                <td>${formatDate(cert.expirationDate)}</td>
                                <td><span class="status-indicator ${statusClass}"></span>${status}</td>
                            </tr>
                        `;
                    });
                }
                
                credentialsHtml += '</tbody></table></div>';
                credentialsContainer.innerHTML = credentialsHtml;
            } else {
                credentialsContainer.innerHTML = '<p class="text-muted mt-3">No credentials found for this employee.</p>';
            }
            
            // Show the modal
            modal.show();
            
            console.log("Employee data loaded:", employee); // Debug log
        })
        .catch(error => {
            console.error('Error:', error);
            Swal.fire({
                title: 'Error!',
                text: 'Failed to load employee details. Please try again.',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        });
}

// Function to create view employee modal if it doesn't exist
function createViewEmployeeModal() {
    const modalHtml = `
        <div class="modal fade" id="viewEmployeeModal" tabindex="-1" aria-labelledby="viewEmployeeModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-lg">
                <div class="modal-content">
                    <div class="modal-header bg-light">
                        <h5 class="modal-title" id="viewEmployeeModalLabel">Employee Details</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-4 text-center mb-3">
                                <div class="avatar-circle mx-auto" id="view-employee-avatar" style="width: 80px; height: 80px; font-size: 24px;"></div>
                                <h4 class="mt-3" id="view-employee-name"></h4>
                                <p class="text-muted mb-1" id="view-employee-id"></p>
                                <div class="mt-2">
                                    <span class="badge bg-soft-primary text-primary me-1" id="view-employee-role"></span>
                                    <span class="badge bg-soft-info text-info" id="view-employee-department"></span>
                                </div>
                            </div>
                            <div class="col-md-8">
                                <div class="card">
                                    <div class="card-header">
                                        <h5 class="card-title mb-0">Personal Information</h5>
                                    </div>
                                    <div class="card-body">
                                        <div class="row mb-2">
                                            <div class="col-5 fw-medium">Email:</div>
                                            <div class="col-7" id="view-employee-email"></div>
                                        </div>
                                        <div class="row mb-2">
                                            <div class="col-5 fw-medium">Phone:</div>
                                            <div class="col-7" id="view-employee-phone"></div>
                                        </div>
                                        <div class="row mb-2">
                                            <div class="col-5 fw-medium">Gender:</div>
                                            <div class="col-7" id="view-employee-gender"></div>
                                        </div>
                                        <div class="row mb-2">
                                            <div class="col-5 fw-medium">Date of Birth:</div>
                                            <div class="col-7" id="view-employee-dob"></div>
                                        </div>
                                        <div class="row mb-2">
                                            <div class="col-5 fw-medium">Address:</div>
                                            <div class="col-7" id="view-employee-address"></div>
                                        </div>
                                    </div>
                                </div>
                                
                                <div class="card mt-3">
                                    <div class="card-header d-flex justify-content-between align-items-center">
                                        <h5 class="card-title mb-0">Credentials & Licenses</h5>
                                        <button class="btn btn-sm btn-primary" onclick="manageCredentials(document.getElementById('view-employee-id').textContent.replace('#EMP', ''), document.getElementById('view-employee-name').textContent)">
                                            <i class="ri-add-line align-bottom me-1"></i> Manage
                                        </button>
                                    </div>
                                    <div class="card-body" id="view-employee-credentials">
                                        <!-- Credentials will be displayed here -->
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-light" data-bs-dismiss="modal">Close</button>
                        <button type="button" class="btn btn-primary" onclick="editEmployee(document.getElementById('view-employee-id').textContent.replace('#EMP', ''))">
                            <i class="ri-pencil-fill align-bottom me-1"></i> Edit
                        </button>
                    </div>
                </div>
            </div>
        </div>
    `;
    
    document.body.insertAdjacentHTML('beforeend', modalHtml);
    return document.getElementById('viewEmployeeModal');
}

// Function to edit an employee
function editEmployee(employeeId) {
    // Close the view modal if it's open
    const viewModal = bootstrap.Modal.getInstance(document.getElementById('viewEmployeeModal'));
    if (viewModal) {
        viewModal.hide();
    }
    
    // Show loading spinner
    Swal.fire({
        title: 'Loading...',
        text: 'Fetching employee data',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
    
    // Fetch employee details from API
    fetch(`/api/employees/${employeeId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to fetch employee details');
            }
            return response.json();
        })
        .then(employee => {
            Swal.close();
            
            // Show the employee form modal
            const modal = new bootstrap.Modal(document.getElementById('employee-form-modal'));
            
            // Update form title and button text for edit mode
            document.getElementById('form-title').textContent = 'Edit Employee';
            document.getElementById('add-btn').innerHTML = '<i class="ri-save-line align-bottom me-1"></i> Update Employee';
            
            // Populate form with employee data
            document.getElementById('employee-id').value = employee.id;
            document.getElementById('employee-name').value = employee.name || '';
            document.getElementById('employee-email').value = employee.email || '';
            document.getElementById('employee-phone').value = employee.phoneNumber || '';
            document.getElementById('employee-address').value = employee.address || '';
            
            // Set date of birth (handle API date format)
            if (employee.dateOfBirth) {
                const dobDate = new Date(employee.dateOfBirth);
                if (!isNaN(dobDate.getTime())) {
                    // Format date as YYYY-MM-DD for input[type="date"]
                    const dobFormatted = dobDate.toISOString().split('T')[0];
                    document.getElementById('employee-dob').value = dobFormatted;
                    
                    // Also initialize flatpickr if it's being used
                    if (window.flatpickr) {
                        const flatpickrInstance = flatpickr("#employee-dob", {
                            dateFormat: "Y-m-d",
                            maxDate: new Date(),
                            defaultDate: dobFormatted
                        });
                    }
                }
            }
            
            // Set gender (true = Male, false = Female)
            const genderValue = employee.gender === "Male" ? "true" : "false";
            document.getElementById('employee-gender').value = genderValue;
            
            // Set department and role
            document.getElementById('employee-department').value = employee.department || '';
            document.getElementById('employee-role').value = employee.role || '';
            
            // Reinitialize enhanced selects if using Choices.js
            if (window.Choices) {
                document.querySelectorAll('#employee-form-modal select').forEach(select => {
                    if (!select.classList.contains('choices__input')) {
                        new Choices(select, {
                            searchEnabled: true,
                            itemSelectText: '',
                            shouldSort: false,
                        });
                    }
                });
            }
            
            // Show the modal
            modal.show();
            
            // Add a subtle entrance animation
            const modalContent = document.querySelector('#employee-form-modal .modal-content');
            modalContent.classList.add('animate__animated', 'animate__fadeInUp');
            setTimeout(() => {
                modalContent.classList.remove('animate__animated', 'animate__fadeInUp');
            }, 500);
        })
        .catch(error => {
            console.error('Error:', error);
            Swal.fire({
                title: 'Error!',
                text: 'Failed to load employee data. Please try again.',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        });
}

// Function to show create employee form
function showCreateEmployeeForm() {
    // Reset the form
    document.getElementById('employee-form-element').reset();
    document.getElementById('employee-id').value = '';
    
    // Update form title and button text
    document.getElementById('form-title').textContent = 'Create Employee';
    document.getElementById('add-btn').innerHTML = '<i class="ri-save-line align-bottom me-1"></i> Add Employee';
    
    // Initialize select dropdowns with enhanced UI
    document.querySelectorAll('#employee-form-modal select').forEach(select => {
        if (window.Choices && !select.classList.contains('choices__input')) {
            new Choices(select, {
                searchEnabled: true,
                itemSelectText: '',
                shouldSort: false,
            });
        }
    });
    
    // Initialize flatpickr for date input
    flatpickr("#employee-dob", {
        dateFormat: "Y-m-d",
        maxDate: new Date(),
        animate: true
    });
    
    // Show the modal with animation
    const modal = new bootstrap.Modal(document.getElementById('employee-form-modal'));
    modal.show();
    
    // Add a subtle entrance animation
    const modalContent = document.querySelector('#employee-form-modal .modal-content');
    modalContent.classList.add('animate__animated', 'animate__fadeInUp');
    setTimeout(() => {
        modalContent.classList.remove('animate__animated', 'animate__fadeInUp');
    }, 500);
}

// Function to hide employee form
function hideEmployeeForm() {
    const modal = bootstrap.Modal.getInstance(document.getElementById('employee-form-modal'));
    if (modal) {
        modal.hide();
    }
    
    // Reset form validation state
    document.getElementById('employee-form-element').classList.remove('was-validated');
}

// Function to submit employee form
function submitEmployeeForm() {
    const form = document.getElementById('employee-form-element');
    
    // Form validation
    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return;
    }
    
    // Get form values
    const employeeId = document.getElementById('employee-id').value;
    const employeeData = {
        id: employeeId ? parseInt(employeeId) : 0,
        name: document.getElementById('employee-name').value,
        email: document.getElementById('employee-email').value,
        phoneNumber: document.getElementById('employee-phone').value,
        gender: document.getElementById('employee-gender').value === 'true', // Convert "true"/"false" string to boolean
        dateOfBirth: document.getElementById('employee-dob').value,
        address: document.getElementById('employee-address').value,
        department: document.getElementById('employee-department').value,
        role: document.getElementById('employee-role').value
    };
    
    // Show loading state
    const submitBtn = document.getElementById('add-btn');
    const originalText = submitBtn.innerHTML;
    submitBtn.disabled = true;
    submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-1" role="status" aria-hidden="true"></span> Saving...';
    
    // Determine if creating or updating
    const isUpdate = employeeId && employeeId.trim() !== '';
    const method = isUpdate ? 'PUT' : 'POST';
    const url = isUpdate ? `/api/employees/${employeeId}` : '/api/employees';
    
    // Send request
    fetch(url, {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(employeeData)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Failed to save employee');
        }
        return response.json();
    })
    .then(data => {
        // Close modal
        hideEmployeeForm();
        
        // Show success message
        Swal.fire({
            title: isUpdate ? 'Updated!' : 'Created!',
            text: isUpdate ? 'Employee has been updated successfully.' : 'Employee has been created successfully.',
            icon: 'success',
            showConfirmButton: false,
            timer: 1500
        }).then(() => {
            // Reload employees to reflect changes
            loadEmployees();
        });
    })
    .catch(error => {
        console.error('Error:', error);
        Swal.fire({
            title: 'Error!',
            text: `Failed to ${isUpdate ? 'update' : 'create'} employee. Please try again.`,
            icon: 'error',
            confirmButtonText: 'OK'
        });
    })
    .finally(() => {
        // Reset button state
        submitBtn.disabled = false;
        submitBtn.innerHTML = originalText;
    });
}

// Function to manage employee credentials
function manageCredentials(employeeId, employeeName) {
    // Close the view modal if it's open
    const viewModal = bootstrap.Modal.getInstance(document.getElementById('viewEmployeeModal'));
    if (viewModal) {
        viewModal.hide();
    }
    
    // Show the credential management modal
    const modal = new bootstrap.Modal(document.getElementById('credential-management-modal'));
    
    // Set employee information
    document.getElementById('selected-employee-name').textContent = employeeName;
    
    // Add a hidden input with employee ID for later use
    if (!document.getElementById('credential-employee-id')) {
        const idInput = document.createElement('input');
        idInput.type = 'hidden';
        idInput.id = 'credential-employee-id';
        idInput.value = employeeId;
        document.getElementById('credential-management-modal').appendChild(idInput);
    } else {
        document.getElementById('credential-employee-id').value = employeeId;
    }
    
    // Load employee credentials
    loadEmployeeCredentials(employeeId);
    
    // Show the modal
    modal.show();
}

// Function to load employee credentials
function loadEmployeeCredentials(employeeId) {
    const licensesTableBody = document.getElementById('licenses-table-body');
    const certificatesTableBody = document.getElementById('certificates-table-body');
    
    // Show loading state
    licensesTableBody.innerHTML = `
        <tr>
            <td colspan="8" class="text-center">
                <div class="d-flex align-items-center justify-content-center py-3">
                    <div class="spinner-border text-primary spinner-border-sm me-2" role="status">
                        <span class="visually-hidden">Loading...</span>
                    </div>
                    <span>Loading licenses...</span>
                </div>
            </td>
        </tr>
    `;
    
    certificatesTableBody.innerHTML = `
        <tr>
            <td colspan="9" class="text-center">
                <div class="d-flex align-items-center justify-content-center py-3">
                    <div class="spinner-border text-info spinner-border-sm me-2" role="status">
                        <span class="visually-hidden">Loading...</span>
                    </div>
                    <span>Loading certificates...</span>
                </div>
            </td>
        </tr>
    `;
    
    // Fetch employee details with credentials
    fetch(`/api/employees/${employeeId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to fetch employee credentials');
            }
            return response.json();
        })
        .then(employee => {
            // Process licenses
            const hasLicenses = employee.license && employee.license.length > 0;
            
            if (!hasLicenses) {
                licensesTableBody.innerHTML = `
                    <tr>
                        <td colspan="8" class="text-center">
                            <div class="py-4">
                                <div class="avatar-sm mx-auto mb-3">
                                    <div class="avatar-title rounded-circle bg-light text-primary fs-24">
                                        <i class="ri-award-line"></i>
                                    </div>
                                </div>
                                <h6 class="mt-2">No Licenses Found</h6>
                                <p class="text-muted small mb-0">No license records found for this employee.</p>
                                <button class="btn btn-sm btn-soft-primary mt-2" onclick="showAddCredentialForm('License')">
                                    <i class="ri-add-line align-bottom me-1"></i> Add License
                                </button>
                            </div>
                        </td>
                    </tr>
                `;
            } else {
                let licensesHtml = '';
                
                employee.license.forEach((license, index) => {
                    const now = new Date();
                    const expiryDate = new Date(license.expirationDate);
                    let status = 'Active';
                    let statusClass = 'success';
                    
                    if (expiryDate < now) {
                        status = 'Expired';
                        statusClass = 'danger';
                    } else if ((expiryDate - now) / (1000 * 60 * 60 * 24) < 30) {
                        status = 'Expiring Soon';
                        statusClass = 'warning';
                    }
                    
                    licensesHtml += `
                        <tr class="animate__animated animate__fadeIn" style="animation-delay: ${index * 50}ms;">
                            <td>${license.id || '-'}</td>
                            <td>${license.number || 'N/A'}</td>
                            <td>${license.issuingBody || 'N/A'}</td>
                            <td>${formatDate(license.issueDate)}</td>
                            <td>${formatDate(license.expirationDate)}</td>
                            <td>${license.restriction || 'None'}</td>
                            <td><span class="badge bg-${statusClass}-subtle text-${statusClass}">${status}</span></td>
                            <td>
                                <div class="hstack gap-2">
                                    ${expiryDate < now ? 
                                        `<button type="button" class="btn btn-sm btn-soft-warning" onclick="renewCredential(${license.id})">
                                            <i class="ri-refresh-line"></i>
                                        </button>` : ''}
                                    <button type="button" class="btn btn-sm btn-soft-danger" onclick="deleteCredential(${license.id})">
                                        <i class="ri-delete-bin-line"></i>
                                    </button>
                                </div>
                            </td>
                        </tr>
                    `;
                });
                
                licensesTableBody.innerHTML = licensesHtml;
            }
            
            // Process certificates
            const hasCertificates = employee.certificates && employee.certificates.length > 0;
            
            if (!hasCertificates) {
                certificatesTableBody.innerHTML = `
                    <tr>
                        <td colspan="9" class="text-center">
                            <div class="py-4">
                                <div class="avatar-sm mx-auto mb-3">
                                    <div class="avatar-title rounded-circle bg-light text-info fs-24">
                                        <i class="ri-file-list-3-line"></i>
                                    </div>
                                </div>
                                <h6 class="mt-2">No Certificates Found</h6>
                                <p class="text-muted small mb-0">No certificate records found for this employee.</p>
                                <button class="btn btn-sm btn-soft-info mt-2" onclick="showAddCredentialForm('Certificate')">
                                    <i class="ri-add-line align-bottom me-1"></i> Add Certificate
                                </button>
                            </div>
                        </td>
                    </tr>
                `;
            } else {
                let certificatesHtml = '';
                
                employee.certificates.forEach((cert, index) => {
                    const now = new Date();
                    const expiryDate = new Date(cert.expirationDate);
                    let status = 'Active';
                    let statusClass = 'success';
                    
                    if (expiryDate < now) {
                        status = 'Expired';
                        statusClass = 'danger';
                    } else if ((expiryDate - now) / (1000 * 60 * 60 * 24) < 30) {
                        status = 'Expiring Soon';
                        statusClass = 'warning';
                    }
                    
                    certificatesHtml += `
                        <tr class="animate__animated animate__fadeIn" style="animation-delay: ${index * 50}ms;">
                            <td>${cert.id || '-'}</td>
                            <td>${cert.name || 'N/A'}</td>
                            <td>${cert.issuingBody || 'N/A'}</td>
                            <td>${formatDate(cert.issueDate)}</td>
                            <td>${formatDate(cert.expirationDate)}</td>
                            <td>${cert.level || 'N/A'}</td>
                            <td>${cert.version || 'N/A'}</td>
                            <td><span class="badge bg-${statusClass}-subtle text-${statusClass}">${status}</span></td>
                            <td>
                                <div class="hstack gap-2">
                                    ${expiryDate < now ? 
                                        `<button type="button" class="btn btn-sm btn-soft-warning" onclick="renewCredential(${cert.id})">
                                            <i class="ri-refresh-line"></i>
                                        </button>` : ''}
                                    <button type="button" class="btn btn-sm btn-soft-danger" onclick="deleteCredential(${cert.id})">
                                        <i class="ri-delete-bin-line"></i>
                                    </button>
                                </div>
                            </td>
                        </tr>
                    `;
                });
                
                certificatesTableBody.innerHTML = certificatesHtml;
            }
            
            console.log("Employee credentials loaded:", {licenses: employee.license, certificates: employee.certificates}); // Debug log
        })
        .catch(error => {
            console.error('Error fetching credentials:', error);
            
            // Show error in both tables
            const errorHtml = `
                <tr>
                    <td colspan="9" class="text-center">
                        <div class="py-3">
                            <div class="avatar-sm mx-auto mb-3">
                                <div class="avatar-title rounded-circle bg-soft-danger text-danger fs-24">
                                    <i class="ri-error-warning-line"></i>
                                </div>
                            </div>
                            <h6 class="mt-2">Error Loading Credentials</h6>
                            <p class="text-muted small mb-0">Failed to load credentials. Please try again.</p>
                        </div>
                    </td>
                </tr>
            `;
            
            licensesTableBody.innerHTML = errorHtml;
            certificatesTableBody.innerHTML = errorHtml;
        });
}

// Function to show the add credential form
function showAddCredentialForm(preSelectedType = '') {
    // Create modal if it doesn't exist
    let modal = document.getElementById('add-credential-modal');
    if (!modal) {
        const modalHtml = `
            <div class="modal fade" id="add-credential-modal" tabindex="-1" aria-labelledby="add-credential-modal-label" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="add-credential-modal-label">Add New Credential</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <form id="credential-form" class="needs-validation" novalidate>
                                <div class="mb-3">
                                    <label for="credential-type" class="form-label">Credential Type</label>
                                    <select class="form-select" id="credential-type" required onchange="toggleCredentialTypeFields()">
                                        <option value="">Select Type</option>
                                        <option value="License">License</option>
                                        <option value="Certificate">Certificate</option>
                                    </select>
                                    <div class="invalid-feedback">Please select a credential type.</div>
                                </div>
                                
                                <div class="mb-3">
                                    <label for="credential-name" class="form-label">Credential Name</label>
                                    <input type="text" class="form-control" id="credential-name" placeholder="Enter credential name" required>
                                    <div class="invalid-feedback">Please enter a credential name.</div>
                                </div>
                                
                                <div class="mb-3">
                                    <label for="credential-number" class="form-label">Number/ID</label>
                                    <input type="text" class="form-control" id="credential-number" placeholder="Enter credential number or ID" required>
                                    <div class="invalid-feedback">Please enter a credential number.</div>
                                </div>
                                
                                <div class="mb-3">
                                    <label for="credential-issuing-body" class="form-label">Issuing Body/Authority</label>
                                    <input type="text" class="form-control" id="credential-issuing-body" placeholder="Enter issuing organization">
                                    <div class="invalid-feedback">Please enter the issuing body.</div>
                                </div>
                                
                                <!-- License specific field -->
                                <div class="mb-3 credential-type-field" id="license-fields" style="display:none;">
                                    <label for="credential-restriction" class="form-label">Restrictions (if any)</label>
                                    <input type="text" class="form-control" id="credential-restriction" placeholder="Enter any license restrictions">
                                </div>
                                
                                <!-- Certificate specific fields -->
                                <div class="credential-type-field" id="certificate-fields" style="display:none;">
                                    <div class="mb-3">
                                        <label for="credential-level" class="form-label">Certificate Level</label>
                                        <select class="form-select" id="credential-level">
                                            <option value="">Select Level</option>
                                            <option value="Basic">Basic</option>
                                            <option value="Intermediate">Intermediate</option>
                                            <option value="Advanced">Advanced</option>
                                            <option value="Expert">Expert</option>
                                        </select>
                                    </div>
                                    <div class="mb-3">
                                        <label for="credential-version" class="form-label">Version/Edition</label>
                                        <input type="text" class="form-control" id="credential-version" placeholder="Enter version or edition">
                                    </div>
                                </div>
                                
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="mb-3">
                                            <label for="credential-issue-date" class="form-label">Issue Date</label>
                                            <input type="date" class="form-control" id="credential-issue-date" required>
                                            <div class="invalid-feedback">Please specify the issue date.</div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="mb-3">
                                            <label for="credential-expiry-date" class="form-label">Expiry Date</label>
                                            <input type="date" class="form-control" id="credential-expiry-date" required>
                                            <div class="invalid-feedback">Please specify the expiry date.</div>
                                        </div>
                                    </div>
                                </div>
                                
                                <div class="mb-3">
                                    <label for="credential-notes" class="form-label">Notes (Optional)</label>
                                    <textarea class="form-control" id="credential-notes" rows="3" placeholder="Additional information about this credential"></textarea>
                                </div>
                            </form>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-light" data-bs-dismiss="modal">Cancel</button>
                            <button type="button" class="btn btn-primary" id="save-credential-btn" onclick="saveCredential()">Save Credential</button>
                        </div>
                    </div>
                </div>
            </div>
        `;
        
        document.body.insertAdjacentHTML('beforeend', modalHtml);
        modal = document.getElementById('add-credential-modal');
        
        // Initialize date pickers if flatpickr is available
        if (window.flatpickr) {
            flatpickr("#credential-issue-date", {
                dateFormat: "Y-m-d",
                maxDate: new Date(),
                animate: true
            });
            
            flatpickr("#credential-expiry-date", {
                dateFormat: "Y-m-d",
                minDate: new Date(),
                animate: true
            });
        }
    }
    
    // Reset the form
    document.getElementById('credential-form').reset();
    document.getElementById('credential-form').classList.remove('was-validated');
    
    // Hide type-specific fields
    document.querySelectorAll('.credential-type-field').forEach(field => {
        field.style.display = 'none';
    });
    
    // Pre-select credential type if provided
    if (preSelectedType) {
        document.getElementById('credential-type').value = preSelectedType;
        toggleCredentialTypeFields();
    }
    
    // Show the modal
    const bsModal = new bootstrap.Modal(modal);
    bsModal.show();
}

// Function to toggle credential type fields
function toggleCredentialTypeFields() {
    const selectedType = document.getElementById('credential-type').value;
    
    // Hide all type-specific fields
    document.querySelectorAll('.credential-type-field').forEach(field => {
        field.style.display = 'none';
    });
    
    // Show fields based on credential type
    if (selectedType === 'License') {
        document.getElementById('license-fields').style.display = 'block';
    } else if (selectedType === 'Certificate') {
        document.getElementById('certificate-fields').style.display = 'block';
    }
}

// Function to save a new credential
function saveCredential() {
    const form = document.getElementById('credential-form');
    
    // Form validation
    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return;
    }
    
    const employeeId = document.getElementById('credential-employee-id').value;
    const credentialType = document.getElementById('credential-type').value;
    
    // Base credential data common to both License and Certificate
    const credentialData = {
        id: 0, // New credential, ID will be assigned by the server
        name: document.getElementById('credential-name').value,
        issuingBody: document.getElementById('credential-issuing-body').value,
        issueDate: document.getElementById('credential-issue-date').value,
        expirationDate: document.getElementById('credential-expiry-date').value,
        
        // Add the CertificateType property expected by the backend
        certificateType: credentialType,
        
        // Initialize all type-specific properties with empty strings
        licenseNumber: "",
        licenseRestriction: "",
        certificateLevel: "",
        certificateVersion: ""
    };
    
    // Set type-specific properties based on credential type
    if (credentialType === 'License') {
        credentialData.licenseNumber = document.getElementById('credential-number').value;
        credentialData.licenseRestriction = document.getElementById('credential-restriction').value || "";
    } else if (credentialType === 'Certificate') {
        // Note: In CredentialForm, the expected value is "Credential", not "Certificate"
        credentialData.certificateType = "Credential";
        credentialData.certificateLevel = document.getElementById('credential-level').value || "";
        credentialData.certificateVersion = document.getElementById('credential-version').value || "";
    }
    
    // Show loading state
    const saveBtn = document.getElementById('save-credential-btn');
    const originalText = saveBtn.textContent;
    saveBtn.disabled = true;
    saveBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-1" role="status" aria-hidden="true"></span> Saving...';
    
    // Send API request to add credential
    fetch(`/api/employees/${employeeId}/credentials`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(credentialData)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Failed to add credential');
        }
        return response.json();
    })
    .then(data => {
        // Close the modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('add-credential-modal'));
        modal.hide();
        
        // Show success message
        Swal.fire({
            title: 'Success!',
            text: `${credentialType} has been added successfully.`,
            icon: 'success',
            showConfirmButton: false,
            timer: 1500
        });
        
        // Reload credentials
        loadEmployeeCredentials(employeeId);
    })
    .catch(error => {
        console.error('Error adding credential:', error);
        Swal.fire({
            title: 'Error!',
            text: 'Failed to add credential. Please try again.',
            icon: 'error',
            confirmButtonText: 'OK'
        });
    })
    .finally(() => {
        // Reset button state
        saveBtn.disabled = false;
        saveBtn.textContent = originalText;
    });
}

// Function to delete a credential
function deleteCredential(credentialId) {
    Swal.fire({
        title: 'Are you sure?',
        text: "This credential will be permanently deleted!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            // Show loading state
            Swal.fire({
                title: 'Deleting...',
                text: 'Removing credential',
                allowOutsideClick: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });
            
            // Send delete request to API
            fetch(`/api/employees/credentials/${credentialId}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to delete credential');
                }
                
                Swal.fire({
                    title: 'Deleted!',
                    text: 'Credential has been deleted successfully.',
                    icon: 'success',
                    showConfirmButton: false,
                    timer: 1500
                });
                
                // Reload credentials for the current employee
                const employeeId = document.getElementById('credential-employee-id').value;
                loadEmployeeCredentials(employeeId);
            })
            .catch(error => {
                console.error('Error deleting credential:', error);
                Swal.fire({
                    title: 'Error!',
                    text: 'Failed to delete credential. Please try again.',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            });
        }
    });
}

// Function to renew a credential
function renewCredential(credentialId) {
    // Create modal if it doesn't exist
    let modal = document.getElementById('renew-credential-modal');
    if (!modal) {
        const modalHtml = `
            <div class="modal fade" id="renew-credential-modal" tabindex="-1" aria-labelledby="renew-credential-modal-label" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="renew-credential-modal-label">Renew Credential</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <form id="renew-credential-form" class="needs-validation" novalidate>
                                <input type="hidden" id="renew-credential-id">
                                <div class="mb-3">
                                    <label for="renew-credential-date" class="form-label">New Expiration Date</label>
                                    <input type="date" class="form-control" id="renew-credential-date" required>
                                    <div class="invalid-feedback">Please specify a new expiration date.</div>
                                </div>
                            </form>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-light" data-bs-dismiss="modal">Cancel</button>
                            <button type="button" class="btn btn-primary" id="save-renew-btn" onclick="submitRenewCredential()">Renew</button>
                        </div>
                    </div>
                </div>
            </div>
        `;
        
        document.body.insertAdjacentHTML('beforeend', modalHtml);
        modal = document.getElementById('renew-credential-modal');
        
        // Initialize date picker if flatpickr is available
        if (window.flatpickr) {
            flatpickr("#renew-credential-date", {
                dateFormat: "Y-m-d",
                minDate: new Date(),
                animate: true
            });
        }
    }
    
    // Reset the form
    document.getElementById('renew-credential-form').reset();
    document.getElementById('renew-credential-form').classList.remove('was-validated');
    
    // Set the credential ID
    document.getElementById('renew-credential-id').value = credentialId;
    
    // Set the minimum date to tomorrow
    const tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    const tomorrowString = tomorrow.toISOString().split('T')[0];
    document.getElementById('renew-credential-date').min = tomorrowString;
    
    // Show the modal
    const bsModal = new bootstrap.Modal(modal);
    bsModal.show();
}

// Function to back to employee details from credential management modal
function backToEmployeeDetails() {
    // Close the credential management modal
    const credentialModal = bootstrap.Modal.getInstance(document.getElementById('credential-management-modal'));
    if (credentialModal) {
        credentialModal.hide();
    }
    
    // Get the employee ID from the hidden input
    const employeeId = document.getElementById('credential-employee-id').value;
    
    // Get the employee name
    const employeeName = document.getElementById('selected-employee-name').textContent;
    
    // Show the employee details
    viewEmployeeDetails(employeeId);
}

// Function to submit credential renewal
function submitRenewCredential() {
    const form = document.getElementById('renew-credential-form');
    
    // Form validation
    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return;
    }
    
    const credentialId = document.getElementById('renew-credential-id').value;
    const newExpirationDate = document.getElementById('renew-credential-date').value;
    
    // Prepare the renewal data
    const renewalData = {
        credId: parseInt(credentialId),
        newExprDate: newExpirationDate
    };
    
    // Show loading state
    const saveBtn = document.getElementById('save-renew-btn');
    const originalText = saveBtn.textContent;
    saveBtn.disabled = true;
    saveBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-1" role="status" aria-hidden="true"></span> Renewing...';
    
    // Send API request to renew credential
    fetch(`/api/employees/credentials`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(renewalData)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Failed to renew credential');
        }
        return response.json();
    })
    .then(data => {
        // Close the modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('renew-credential-modal'));
        modal.hide();
        
        // Show success message
        Swal.fire({
            title: 'Success!',
            text: 'Credential has been renewed successfully.',
            icon: 'success',
            showConfirmButton: false,
            timer: 1500
        });
        
        // Reload credentials for the current employee
        const employeeId = document.getElementById('credential-employee-id').value;
        loadEmployeeCredentials(employeeId);
    })
    .catch(error => {
        console.error('Error renewing credential:', error);
        Swal.fire({
            title: 'Error!',
            text: 'Failed to renew credential. Please try again.',
            icon: 'error',
            confirmButtonText: 'OK'
        });
    })
    .finally(() => {
        // Reset button state
        saveBtn.disabled = false;
        saveBtn.textContent = originalText;
    });
}