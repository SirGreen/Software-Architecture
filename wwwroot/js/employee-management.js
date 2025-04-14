document.addEventListener("DOMContentLoaded", () => {
    fetchEmployees();
});

function fetchEmployees() {
    fetch("/api/employees")
        .then(response => response.json())
        .then(data => {
            const tableBody = document.getElementById("employee-table-body");
            tableBody.innerHTML = "";
            data.forEach(employee => {
                // Escape and serialize the license and certificates data to avoid JSON parsing issues
                const escapedLicenses = employee.license ? JSON.stringify(employee.license).replace(/"/g, '&quot;') : '[]';
                const escapedCertificates = employee.certificates ? JSON.stringify(employee.certificates).replace(/"/g, '&quot;') : '[]';
                
                const row = document.createElement("tr");
                row.innerHTML = `
                    <td>${employee.id}</td>
                    <td>${employee.name}</td>
                    <td>${employee.role}</td>
                    <td>${employee.department}</td>
                    <td>
                        <button class="btn btn-warning" onclick="editEmployee(${employee.id})">Edit</button>
                        <button class="btn btn-danger" onclick="deleteEmployee(${employee.id})">Delete</button>
                        <button class="btn btn-info" onclick="showEmployeeCredentials(${employee.id}, '${employee.name.replace(/'/g, "\\'")}', ${escapedLicenses}, ${escapedCertificates})">View Credentials</button>
                    </td>
                `;
                tableBody.appendChild(row);
            });
        })
        .catch(error => console.error("Error fetching employees:", error));
}

function showCreateEmployeeForm() {
    document.getElementById("form-title").innerText = "Create Employee";
    document.getElementById("employee-id").value = "";
    document.getElementById("employee-name").value = "";
    document.getElementById("employee-role").value = "";
    document.getElementById("employee-department").value = "";
    document.getElementById("employee-form").style.display = "block";
}

function hideEmployeeForm() {
    document.getElementById("employee-form").style.display = "none";
}

function submitEmployeeForm() {
    const id = document.getElementById("employee-id").value;
    const name = document.getElementById("employee-name").value;
    const role = document.getElementById("employee-role").value;
    const department = document.getElementById("employee-department").value;

    const employee = { id, name, role, department };

    const method = id ? "PUT" : "POST";
    const url = id ? `/api/employees/${id}` : "/api/employees";

    fetch(url, {
        method,
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(employee)
    })
        .then(response => {
            if (response.ok) {
                fetchEmployees();
                hideEmployeeForm();
            } else {
                console.error("Error saving employee:", response);
            }
        })
        .catch(error => console.error("Error saving employee:", error));
}

function editEmployee(id) {
    fetch(`/api/employees/${id}`)
        .then(response => response.json())
        .then(employee => {
            document.getElementById("form-title").innerText = "Edit Employee";
            document.getElementById("employee-id").value = employee.id;
            document.getElementById("employee-name").value = employee.name;
            document.getElementById("employee-role").value = employee.role;
            document.getElementById("employee-department").value = employee.department;
            document.getElementById("employee-form").style.display = "block";
        })
        .catch(error => console.error("Error fetching employee:", error));
}

function deleteEmployee(id) {
    if (confirm("Are you sure you want to delete this employee?")) {
        fetch(`/api/employees/${id}`, { method: "DELETE" })
            .then(response => {
                if (response.ok) {
                    fetchEmployees();
                } else {
                    console.error("Error deleting employee:", response);
                }
            })
            .catch(error => console.error("Error deleting employee:", error));
    }
}

function showEmployeeCredentials(employeeId, employeeName, licenses, certificates) {
    document.getElementById('selected-employee-name').innerText = employeeName;
    document.getElementById('employee-credentials').dataset.employeeId = employeeId;
    
    const tableBody = document.getElementById("credentials-table-body");
    tableBody.innerHTML = "";
    
    // Add licenses to the table with specific License attributes
    if (licenses && licenses.length > 0) {
        licenses.forEach(license => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td>${license.id}</td>
                <td>License</td>
                <td>${license.name}</td>
                <td>${formatDate(license.issueDate)}</td>
                <td>${formatDate(license.expirationDate)}</td>
                <td>
                    <div class="btn-group">
                        <button class="btn btn-warning btn-sm" onclick="renewCredential(${license.id})">Renew</button>
                        <button class="btn btn-info btn-sm" onclick="editCredential(${license.id}, 'License', '${license.name.replace(/'/g, "\\'")}', '${license.issuingBody?.replace(/'/g, "\\'")}', '${formatDate(license.issueDate)}', '${formatDate(license.expirationDate)}', '${license.number?.replace(/'/g, "\\'")}', '${license.restriction?.replace(/'/g, "\\'")}')" title="Number: ${license.number || 'N/A'}, Restriction: ${license.restriction || 'N/A'}">Edit</button>
                        <button class="btn btn-danger btn-sm" onclick="deleteCredential(${license.id}, 'License')">Delete</button>
                    </div>
                </td>
            `;
            tableBody.appendChild(row);
        });
    }
    
    // Add certificates to the table with specific Certificate attributes
    if (certificates && certificates.length > 0) {
        certificates.forEach(certificate => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td>${certificate.id}</td>
                <td>Certificate</td>
                <td>${certificate.name}</td>
                <td>${formatDate(certificate.issueDate)}</td>
                <td>${formatDate(certificate.expirationDate)}</td>
                <td>
                    <div class="btn-group">
                        <button class="btn btn-warning btn-sm" onclick="renewCredential(${certificate.id})">Renew</button>
                        <button class="btn btn-info btn-sm" onclick="editCredential(${certificate.id}, 'Certificate', '${certificate.name.replace(/'/g, "\\'")}', '${certificate.issuingBody?.replace(/'/g, "\\'")}', '${formatDate(certificate.issueDate)}', '${formatDate(certificate.expirationDate)}', '${certificate.level?.replace(/'/g, "\\'")}', '${certificate.version?.replace(/'/g, "\\'")}')" title="Level: ${certificate.level || 'N/A'}, Version: ${certificate.version || 'N/A'}">Edit</button>
                        <button class="btn btn-danger btn-sm" onclick="deleteCredential(${certificate.id}, 'Certificate')">Delete</button>
                    </div>
                </td>
            `;
            tableBody.appendChild(row);
        });
    }
    
    document.getElementById('employee-credentials').style.display = 'block';
}

function formatDate(dateString) {
    if (!dateString) return '';
    const date = new Date(dateString);
    return date.toISOString().split('T')[0];
}

function showAddCredentialForm() {
    const employeeId = document.getElementById('employee-credentials').dataset.employeeId;
    document.getElementById("credential-form-title").innerText = "Add Credential";
    document.getElementById("credential-id").value = "";
    document.getElementById("credential-type").value = "License";
    document.getElementById("credential-name").value = "";
    document.getElementById("credential-issuing-body").value = "";
    document.getElementById("credential-issue-date").value = "";
    document.getElementById("credential-expiry-date").value = "";
    document.getElementById("credential-additional-field1").value = "";
    document.getElementById("credential-additional-field2").value = "";
    
    // Initialize the form with the additional fields based on type
    updateCredentialFormFields("License");
    
    document.getElementById("credential-form").style.display = "block";
}

function updateCredentialFormFields(type) {
    const field1Label = document.getElementById("credential-additional-field1-label");
    const field2Label = document.getElementById("credential-additional-field2-label");
    
    if (type === "License") {
        field1Label.innerText = "Number";
        field2Label.innerText = "Restriction";
    } else if (type === "Certificate") {
        field1Label.innerText = "Level";
        field2Label.innerText = "Version";
    }
}

function hideCredentialForm() {
    document.getElementById("credential-form").style.display = "none";
}

function editCredential(id, type, name, issuingBody, issueDate, expiryDate, field1, field2) {
    document.getElementById("credential-form-title").innerText = "Edit Credential";
    document.getElementById("credential-id").value = id;
    document.getElementById("credential-type").value = type;
    document.getElementById("credential-name").value = name;
    document.getElementById("credential-issuing-body").value = issuingBody || '';
    document.getElementById("credential-issue-date").value = issueDate;
    document.getElementById("credential-expiry-date").value = expiryDate;
    document.getElementById("credential-additional-field1").value = field1 || '';
    document.getElementById("credential-additional-field2").value = field2 || '';
    
    // Update field labels based on type
    updateCredentialFormFields(type);
    
    document.getElementById("credential-form").style.display = "block";
}

function submitCredentialForm() {
    const employeeId = document.getElementById("employee-credentials").dataset.employeeId;
    const credentialId = document.getElementById("credential-id").value;
    const type = document.getElementById("credential-type").value;
    const name = document.getElementById("credential-name").value;
    const issuingBody = document.getElementById("credential-issuing-body").value;
    const issueDate = document.getElementById("credential-issue-date").value;
    const expiryDate = document.getElementById("credential-expiry-date").value;
    const field1 = document.getElementById("credential-additional-field1").value;
    const field2 = document.getElementById("credential-additional-field2").value;

    const credential = { 
        type: type,
        name: name,
        issuingBody: issuingBody,
        issueDate: issueDate,
        expirationDate: expiryDate
    };

    // Add type-specific fields
    if (type === "License") {
        credential.number = field1;
        credential.restriction = field2;
    } else if (type === "Certificate") {
        credential.level = field1;
        credential.version = field2;
    }

    const method = credentialId ? "PUT" : "POST";
    const url = credentialId 
        ? `/api/employees/credentials/${credentialId}` 
        : `/api/employees/${employeeId}/credentials`;

    fetch(url, {
        method: method,
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(credential)
    })
        .then(response => {
            if (response.ok) {
                hideCredentialForm();
                // Refresh employee data to show updated credentials
                fetch(`/api/employees/${employeeId}`)
                    .then(response => response.json())
                    .then(employee => {
                        showEmployeeCredentials(
                            employee.id, 
                            employee.name, 
                            employee.license || [], 
                            employee.certificates || []
                        );
                    });
            } else {
                console.error("Error saving credential:", response);
            }
        })
        .catch(error => console.error("Error saving credential:", error));
}

function deleteCredential(credId, type) {
    if (confirm(`Are you sure you want to delete this ${type.toLowerCase()}?`)) {
        const employeeId = document.getElementById("employee-credentials").dataset.employeeId;
        
        fetch(`/api/employees/credentials/${credId}`, { 
            method: "DELETE" 
        })
            .then(response => {
                if (response.ok) {
                    // Refresh employee data to show updated credentials
                    fetch(`/api/employees/${employeeId}`)
                        .then(response => response.json())
                        .then(employee => {
                            showEmployeeCredentials(
                                employee.id, 
                                employee.name, 
                                employee.license || [], 
                                employee.certificates || []
                            );
                        });
                } else {
                    console.error(`Error deleting ${type.toLowerCase()}:`, response);
                }
            })
            .catch(error => console.error(`Error deleting ${type.toLowerCase()}:`, error));
    }
}

function renewCredential(credId) {
    const newExpiryDate = prompt("Enter new expiry date (YYYY-MM-DD):");
    if (newExpiryDate) {
        fetch(`/api/employees/credentials/${credId}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ CredId: credId, NewExprDate: newExpiryDate })
        })
            .then(response => {
                if (response.ok) {
                    const employeeId = document.getElementById("employee-credentials").dataset.employeeId;
                    // Refresh employee data to show updated credentials
                    fetch(`/api/employees/${employeeId}`)
                        .then(response => response.json())
                        .then(employee => {
                            showEmployeeCredentials(
                                employee.id, 
                                employee.name, 
                                employee.license || [], 
                                employee.certificates || []
                            );
                        });
                } else {
                    console.error("Error renewing credential:", response);
                }
            })
            .catch(error => console.error("Error renewing credential:", error));
    }
}