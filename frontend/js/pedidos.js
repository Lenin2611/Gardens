let currentPage = 1;
let x =1;
const pageSize = 4; // Tamaño de la página

function previousPage() {
    if (currentPage > x) {
        currentPage -= x;
        updateTable();
    }
}

const Next = document.querySelector("#nextp");
Next.addEventListener('click', () => {
    nextPage();
});

const Previous = document.querySelector("#previousp");
Previous.addEventListener('click', () => {
    previousPage();
});

function nextPage() {
    currentPage += x;
    updateTable();
}

function updateTable() {
    // Lógica para actualizar la tabla con la página actual
    pedido();
    document.getElementById("currentPagep").textContent = currentPage;
}

async function pedido() {
    try {
        let reqData = await fetch(`http://localhost:5280/api/Pedido?pageIndex=${currentPage}&pageSize=${pageSize}`);
        let respJson = await reqData.json();
        const cuerpoTabla = document.querySelector("#pedidos");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((pedido) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = pedido.id;
            const celdaDireccion = fila.insertCell();
            celdaDireccion.textContent = pedido.fechaPedido;
            const celdaNombreContacto = fila.insertCell();
            celdaNombreContacto.textContent = pedido.fechaEsperada;
            if (pedido.fechaEntrega == "1900-01-01") {
                const celdaTelefono = fila.insertCell();
                celdaTelefono.textContent = "En Camino";
            } else {
                const celdaTelefono = fila.insertCell();
                celdaTelefono.textContent = pedido.fechaEntrega;
            }
            const celdaTelefono = fila.insertCell();
            celdaTelefono.textContent = pedido.comentarios;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

// Llama a la función después de cargar la página o en el momento adecuado
document.addEventListener("DOMContentLoaded", pedido);