let currentPage = 1;
let x =1;
const pageSize = 4; // Tamaño de la página

function previousPage() {
    if (currentPage > x) {
        currentPage -= x;
        updateTable();
    }
}

const Next = document.querySelector("#nextf");
Next.addEventListener('click', () => {
    nextPage();
});

const Previous = document.querySelector("#previousf");
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
    document.getElementById("currentPagef").textContent = currentPage;
}

async function empleado() {
    try {
        let reqData = await fetch(`http://localhost:5280/api/Detallepedido?pageIndex=${currentPage}&pageSize=${pageSize}`);
        let respJson = await reqData.json();
        const cuerpoTabla = document.querySelector("#facturas");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((factura) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = factura.idProductoFk;
            const celdaNombreContacto = fila.insertCell();
            celdaNombreContacto.textContent = factura.cantidad;
            const celdaApellidoContacto = fila.insertCell();
            celdaApellidoContacto.textContent = factura.precioUnidad;
            const celdaTelefono = fila.insertCell();
            celdaTelefono.textContent = factura.numeroLinea;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

// Llama a la función después de cargar la página o en el momento adecuado
document.addEventListener("DOMContentLoaded", empleado);