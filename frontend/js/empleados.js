let currentPage = 1;
let x =1;
const pageSize = 4; // Tamaño de la página

function previousPage() {
    if (currentPage > x) {
        currentPage -= x;
        updateTable();
    }
}

const Next = document.querySelector("#nexte");
Next.addEventListener('click', () => {
    nextPage();
});

const Previous = document.querySelector("#previouse");
Previous.addEventListener('click', () => {
    previousPage();
});

function nextPage() {
    currentPage += x;
    updateTable();
}

function updateTable() {
    // Lógica para actualizar la tabla con la página actual
    empleado();
    document.getElementById("currentPagee").textContent = currentPage;
}

async function empleado() {
    try {
        let reqData = await fetch(`http://localhost:5280/api/Empleado?pageIndex=${currentPage}&pageSize=${pageSize}`);
        let respJson = await reqData.json();
        const cuerpoTabla = document.querySelector("#empleados");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((empleado) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = empleado.nombre;
            const celdaNombreContacto = fila.insertCell();
            celdaNombreContacto.textContent = empleado.apellido;
            const celdaApellidoContacto = fila.insertCell();
            celdaApellidoContacto.textContent = empleado.email;
            const celdaTelefono = fila.insertCell();
            celdaTelefono.textContent = empleado.idOficinaFk;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

// Llama a la función después de cargar la página o en el momento adecuado
document.addEventListener("DOMContentLoaded", empleado);