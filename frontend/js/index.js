
document.addEventListener('DOMContentLoaded', function() {
  const token = getToken();
  const base64Url = token.split('.')[1]; // Obtiene la parte base64 del token
  const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/'); // Reemplaza caracteres especiales
  const jsonPayload = decodeURIComponent(atob(base64).split('').map((c) => {
      return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
  }).join(''));
  
  const decodedToken = JSON.parse(jsonPayload);
  
  console.log(decodedToken);
  const sideMenu = document.querySelector("aside");
  const menuBtn = document.querySelector("#menu-btn");
  const closeBtn = document.querySelector("#close-btn");
  const themeToggler = document.getElementById('theme-toggler');

  // show sidebar
  menuBtn.addEventListener("click", () => {
    sideMenu.style.display = "block";
  });

  // close sidebar
  closeBtn.addEventListener("click", () => {
    sideMenu.style.display = "none";
  });

  // change theme
  themeToggler.addEventListener("click", () => {
    document.body.classList.toggle("dark-theme-variables");

    // Asegúrate de que las clases se estén aplicando correctamente
    themeToggler.querySelector("span:nth-child(1)").classList.toggle("active");
    themeToggler.querySelector("span:nth-child(2)").classList.toggle("active");
  });

  // Función para realizar solicitudes al backend con el token
   // Función para realizar solicitudes al backend con el token Bearer
  async function fetchData(url) {
    const token = getToken();

    try {
      const response = await fetch(url, {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (response.ok) {
        const data = await response.json();
        // Manejar los datos obtenidos del backend
        console.log('Data from server:', data);
        // Por ejemplo, puedes llenar tablas con estos datos o realizar otras acciones en el frontend
      } else {
        throw new Error('Error en la solicitud al servidor');
      }
    } catch (error) {
      // Manejar errores de la solicitud
      console.error(error);
    }
  }
  function getToken() {
    return localStorage.getItem('token');
  }
});