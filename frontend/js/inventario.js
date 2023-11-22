let currentPage = 1;
let x =1;
const pageSize = 4; // Tamaño de la página

function previousPage() {
    if (currentPage > x) {
        currentPage -= x;
        updateTable();
    }
}

const Next = document.querySelector("#nexti");
Next.addEventListener('click', () => {
    nextPage();
});

const Previous = document.querySelector("#previousi");
Previous.addEventListener('click', () => {
    previousPage();
});

function nextPage() {
    currentPage += x;
    updateTable();
}

function updateTable() {
    // Lógica para actualizar la tabla con la página actual
    inventario();
    document.getElementById("currentPagei").textContent = currentPage;
}

async function inventario() {
    try {
        let reqData = await fetch(`http://localhost:5280/api/Producto?pageIndex=${currentPage}&pageSize=${pageSize}`);
        let respJson = await reqData.json();
        const cuerpoTabla = document.querySelector("#inventarios");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((producto) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = producto.nombre;
            const celdaNombreContacto = fila.insertCell();
            celdaNombreContacto.textContent = producto.dimensiones;
            const celdaApellidoContacto = fila.insertCell();
            celdaApellidoContacto.textContent = producto.precioVenta;
            const celdaTelefono = fila.insertCell();
            celdaTelefono.textContent = producto.cantidad;
            const celdaFax = fila.insertCell();
            celdaFax.textContent = producto.cantidadMin;
            const celdaLimiteCredito = fila.insertCell();
            celdaLimiteCredito.textContent = producto.cantidadMax;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

// Llama a la función después de cargar la página o en el momento adecuado
document.addEventListener("DOMContentLoaded", inventario);