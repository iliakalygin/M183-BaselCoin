document.getElementById('loginForm').addEventListener('submit', function(event) {
    event.preventDefault(); 
    
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    // Replace 'YOUR_API_ENDPOINT' with the actual endpoint you are using
    fetch('YOUR_API_ENDPOINT', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            username: username,
            password: password,
        }),
    })
    .then(response => response.json())
    .then(data => {
        if(data.jwt) {
            // Save the JWT to localStorage
            localStorage.setItem('jwt', data.jwt);
            
            // Redirect based on adminState
            if(data.adminState) {
                window.location.href = '../session/admin.html';
            } else {
                window.location.href = '../session/user.html';
            }
        } else {
            alert('Login failed. Please check your credentials and try again.');
        }
    })
    .catch((error) => {
        console.error('Error:', error);
        alert('An error occurred. Please try again later.');
    });
});