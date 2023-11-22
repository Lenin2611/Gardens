let currentPage = 1;
let x =1;
const pageSize = 4; // Tamaño de la página

function previousPage() {
    if (currentPage > x) {
        currentPage -= x;
        updateTable();
    }
}

const Next = document.querySelector("#nexto");
Next.addEventListener('click', () => {
    nextPage();
});

const Previous = document.querySelector("#previouso");
Previous.addEventListener('click', () => {
    previousPage();
});

function nextPage() {
    currentPage += x;
    updateTable();
}

function updateTable() {
    // Lógica para actualizar la tabla con la página actual
    oficina();
    document.getElementById("currentPageo").textContent = currentPage;
}

async function oficina() {
    try {
        let reqData = await fetch(`http://localhost:5280/api/Oficina?pageIndex=${currentPage}&pageSize=${pageSize}`);
        let respJson = await reqData.json();
        const cuerpoTabla = document.querySelector("#oficinas");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = oficina.id;
            const celdaNombreContacto = fila.insertCell();
            celdaNombreContacto.textContent = oficina.telefono;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

// Llama a la función después de cargar la página o en el momento adecuado
document.addEventListener("DOMContentLoaded", oficina);