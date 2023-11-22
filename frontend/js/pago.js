let currentPage = 1;
let x =1;
const pageSize = 4; // Tamaño de la página

function previousPage() {
    if (currentPage > x) {
        currentPage -= x;
        updateTable();
    }
}

const Next = document.querySelector("#nexta");
Next.addEventListener('click', () => {
    nextPage();
});

const Previous = document.querySelector("#previousa");
Previous.addEventListener('click', () => {
    previousPage();
});

function nextPage() {
    currentPage += x;
    updateTable();
}

function updateTable() {
    // Lógica para actualizar la tabla con la página actual
    pago();
    document.getElementById("currentPagea").textContent = currentPage;
}

async function pago() {
    try {
        let reqData = await fetch(`http://localhost:5280/api/Pago?pageIndex=${currentPage}&pageSize=${pageSize}`);
        let respJson = await reqData.json();
        const cuerpoTabla = document.querySelector("#pagos");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((pago) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = pago.id;
            const celdaDireccion = fila.insertCell();
            celdaDireccion.textContent = pago.fechaPago;
            const celdaNombreContacto = fila.insertCell();
            celdaNombreContacto.textContent = pago.total;
            const celdaTelefono = fila.insertCell();
            celdaTelefono.textContent = pago.idClienteFk;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

// Llama a la función después de cargar la página o en el momento adecuado
document.addEventListener("DOMContentLoaded", pago);