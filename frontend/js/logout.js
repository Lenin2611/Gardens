let logout = document.getElementById("logout");
logout.addEventListener("click", () => {
    localStorage.setItem('token', null);});