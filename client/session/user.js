document.addEventListener('DOMContentLoaded', function () {
    const jwt = localStorage.getItem('jwt');
    if (!jwt) {
        window.location.href = '../login/login.html';
    } else {
        fetch('https://localhost:7108/User', { // Replace with your actual API endpoint
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${jwt}`,
                'Content-Type': 'application/json'
            },
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            document.getElementById('username').textContent = data.username;
            document.getElementById('balance').textContent = data.balance;
        })
        .catch(error => {
            console.error('There has been a problem with your fetch operation:', error);
            window.location.href = '../login/login.html'; // Redirect on error
        });
    }

    // Logout functionality
    document.getElementById('logoutButton').addEventListener('click', function() {
        localStorage.removeItem('jwt');
        window.location.href = '../login/login.html';
    });
});
