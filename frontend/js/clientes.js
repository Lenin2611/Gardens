// import {performEndpoint} from './endpoints';

// async function gets() {
//     let end = performEndpoint("/cliente");
//     }
// gets();
let currentPage = 1;
let x =1;
const pageSize = 4; // Tamaño de la página

function previousPage() {
    if (currentPage > x) {
        currentPage -= x;
        updateTable();
    }
}

const Next = document.querySelector("#next");
Next.addEventListener('click', () => {
    nextPage();
});

const Previous = document.querySelector("#previous");
Previous.addEventListener('click', () => {
    previousPage();
});

function nextPage() {
    currentPage += x;
    updateTable();
}

function updateTable() {
    // Lógica para actualizar la tabla con la página actual
    clientes();
    document.getElementById("currentPage").textContent = currentPage;
}

async function clientes() {
    try {
        let reqData = await fetch(`http://localhost:5280/api/Cliente?pageIndex=${currentPage}&pageSize=${pageSize}`);
        let respJson = await reqData.json();
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((cliente) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = cliente.nombre;
            const celdaNombreContacto = fila.insertCell();
            celdaNombreContacto.textContent = cliente.nombreContacto;
            const celdaApellidoContacto = fila.insertCell();
            celdaApellidoContacto.textContent = cliente.apellidoContacto;
            const celdaTelefono = fila.insertCell();
            celdaTelefono.textContent = cliente.telefono;
            const celdaFax = fila.insertCell();
            celdaFax.textContent = cliente.fax;
            const celdaLimiteCredito = fila.insertCell();
            celdaLimiteCredito.textContent = cliente.limiteCredito;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

// Llama a la función después de cargar la página o en el momento adecuado
document.addEventListener("DOMContentLoaded", clientes);

async function ordenes() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Pedido");
        let respJson = await reqData.json();
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#ordenes");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((producto) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            
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
document.addEventListener("DOMContentLoaded", ordenes);


async function usuarios() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Auth/usuarios");
        let respJson = await reqData.json();

        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#usuarios");

        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";

        // Itera sobre los primeros 5 objetos en respJson y crea filas y celdas
        respJson.forEach((usuario) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();

            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = usuario.id;

            const celdaDireccion = fila.insertCell();
            celdaDireccion.textContent = usuario.nombreUsuario;

            const celdaNombreContacto = fila.insertCell();
            celdaNombreContacto.textContent = usuario.email;

            const celdaTelefono = fila.insertCell();
            celdaTelefono.textContent = usuario.idCliente;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

// Llama a la función después de cargar la página o en el momento adecuado
document.addEventListener("DOMContentLoaded", usuarios);
