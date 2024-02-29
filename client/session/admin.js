document.addEventListener('DOMContentLoaded', function() {
    const jwt = localStorage.getItem('jwt');
    if (!jwt) {
        window.location.href = '../login/login.html';
    } else {
        fetchUsers(); // Call fetch users on load
    }

    function fetchUsers() {
        // Simulating a fetch call to get users
        // Replace the URL with your actual endpoint
        fetch('https://yourapi.com/api/users', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${jwt}`
            }
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Session invalid');
            }
            return response.json();
        })
        .then(users => displayUsers(users))
        .catch(error => {
            console.error('Error fetching users:', error);
            localStorage.removeItem('jwt');
            window.location.href = '../login/login.html';
        });
    }

    document.getElementById('addUserForm').addEventListener('submit', function(e) {
        e.preventDefault();
        const userName = document.getElementById('userName').value;
        const userPassword = document.getElementById('userPassword').value;
        const userBalance = document.getElementById('userBalance').value;

        const formAlert = document.getElementById('formAlert');

        if (!userName || !userPassword || userBalance < 0) {
            formAlert.textContent = 'Please check your inputs.';
            formAlert.style.display = 'block';
            return;
        }

        // Simulated fetch call for adding a user
        // Replace URL with your actual endpoint and adjust the request as needed
        fetch('https://yourapi.com/api/addUser', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${jwt}`
            },
            body: JSON.stringify({
                username: userName,
                password: userPassword,
                balance: userBalance
            })
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to add user');
            }
            return response.json(); // Assuming the API returns a success message
        })
        .then(data => {
            // Handle success
            console.log('User added:', data);
            $('#addUserModal').modal('hide'); // Hide modal using jQuery (Bootstrap 5 dependency)
            fetchUsers(); // Refresh the user list
        })
        .catch(error => {
            // Handle error
            formAlert.textContent = error.message;
            formAlert.style.display = 'block';
        });
    });

    document.getElementById('logoutButton').addEventListener('click', function() {
        localStorage.removeItem('jwt');
        window.location.href = '../login/login.html';
    });
});

function displayUsers(users) {
    const usersList = document.getElementById('usersList');
    usersList.innerHTML = ''; // Clear existing users
    users.forEach(user => {
        usersList.innerHTML += `
            <li class="list-group-item">
                <div class="d-flex justify-content-between align-items-center">
                    <div>
                        <strong>Username:</strong> ${user.username} <br>
                        <strong>Balance:</strong> $${user.balance}
                    </div>
                    <div>
                        <button class="btn btn-secondary btn-sm me-2">Edit User</button>
                        <button class="btn btn-danger btn-sm">Delete User</button>
                    </div>
                </div>
            </li>
        `;
    });
}
