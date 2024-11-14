
document.addEventListener('DOMContentLoaded', function () {
    const token = localStorage.getItem('authToken');
    if (token) {
        document.querySelectorAll('.nav-item a[href="/Home/LogIn"], .nav-item a[href="/Home/Register"]').forEach(link => {
            link.style.display = 'none';   // hides login and register buttons
        });

        const role = localStorage.getItem('role');
        if (role == "User") {
            document.getElementById('logoutLink').style.display = 'block';
            document.getElementById('showUserName').style.display = 'block';
            document.getElementById('showProperties').style.display = 'block';
            document.getElementById('showRepairs').style.display = 'block';
        }
        else if (role == "Admin") {
            document.getElementById('logoutLink').style.display = 'block';
            document.getElementById('adminOwner').style.display = 'block';
            document.getElementById('adminProperty').style.display = 'block';
        }
    }
});

// Handle login button click or form submission
function handleLogin(username, password) {
    fetch('/Auth/LogIn', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ username, password })
    })
        .then(response => response.json())
        .then(data => {
            if (data.error) {
                showErrorMessage(true, data.error); 
            } else {
                localStorage.setItem('authToken', data.token);
                localStorage.setItem('role', data.role);
                if (data.role == "User") {
                    window.location.href = '/Home/Index';
                } else if (data.role == "Admin") {
                    window.location.href = '/admin';
                }
            }
        })
        .catch(error => {
            console.error('Error during login:', error);
            console.log('Login failed: ' + error.message); 
            showErrorMessage(true, 'An error occurred during login.'); 
        });
}

function getAuthToken() {
    return localStorage.getItem('authToken');
}

function getProfile() {
    const token = getAuthToken();  // Fetch the token from localStorage

    if (!token) {
        console.error("No auth token found");
        return;
    }

    // Create the URL with the token as a query parameter
    const url = `/Owner/Profile?id=${encodeURIComponent(token)}`;

    // Redirect the user to the URL, which will call the controller's action and render the view
    window.location.href = url;  // This changes the URL and triggers a GET request to the server
}


function logOut() { // Logs out the user
    // Remove the token from localStorage
    localStorage.removeItem('authToken');
    localStorage.removeItem('role');
    // Redirect the user to the homepage
    window.location.href = '/Home/Index';  // or simply '/';
}


const openModalButton = document.getElementById('genericModalTrigger');
const modalBody = document.querySelector('#exampleModal .modal-body');
const modalElement = new bootstrap.Modal(document.getElementById('exampleModal'));

openModalButton.addEventListener('click', function () {
    // Dynamically load content into the modal
    modalBody.innerHTML = '<p>Here is the dynamic content!</p>';

    // Show the modal
    modalElement.show();
});

function loadModalContent(folder, viewName) {     // Calls home controller to dynamicly return the view you are asking for , it needs the folder of the file and the name
    fetch(`/Home/LoadModalContent?folder=${folder}&viewName=${viewName}`)
        .then(response => response.text())
        .then(data => {
            document.getElementById('modalContent').innerHTML = data;

            const modalElement = new bootstrap.Modal(document.getElementById('genericModal'));
            modalElement.show();
        })
        .catch(error => console.error('Error loading modal content:', error));  // shows an error if anything goes wrong
}

function showErrorMessage(show) {
    const errorMessage = document.getElementById('error-message');
    if (show) {
        errorMessage.style.display = 'block';  // Show error message
    } else {
        errorMessage.style.display = 'none';   // Hide error message
    }
}



