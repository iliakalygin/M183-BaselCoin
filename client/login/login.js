document.getElementById('loginForm').addEventListener('submit', function(event) {
    event.preventDefault(); 
    
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;
  
    fetch('https://localhost:7108/api/Account/login', {
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
            localStorage.setItem('jwt', data.jwt);
            
            if(data.role == "admin") {
                window.location.href = '../session/admin.html';
            } else {
                window.location.href = '../session/user.html';
            }
        } else {
            // Display error message
            const errorMessageDiv = document.getElementById('errorMessage');
            errorMessageDiv.style.display = 'block';
            errorMessageDiv.textContent = 'Login failed. Please check your credentials and try again.';
        }
    })
    .catch((error) => {
        const errorMessageDiv = document.getElementById('errorMessage');
        errorMessageDiv.style.display = 'block';
        errorMessageDiv.textContent = 'An error occurred. Please try again later.';
    });
  });
  