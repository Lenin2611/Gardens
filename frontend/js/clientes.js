// import {performEndpoint} from './endpoints';

// async function gets() {
//     let end = performEndpoint("/cliente");
//     }
// gets();
async function clientes() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Cliente");
        let respJson = await reqData.json();
        // Obtén la referencia al cuerpo de la tabla
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

async function empleados() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Empleado");
        let respJson = await reqData.json();
        // Obtén la referencia al cuerpo de la tabla
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
            const celdaFax = fila.insertCell();
            celdaFax.textContent = empleado.fax;
            const celdaLimiteCredito = fila.insertCell();
            celdaLimiteCredito.textContent = empleado.limiteCredito;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}
// Llama a la función después de cargar la página o en el momento adecuado
document.addEventListener("DOMContentLoaded", empleados);

async function oficinas() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Oficina");
        let respJson = await reqData.json();
        // Obtén la referencia al cuerpo de la tabla
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
            const celdaApellidoContacto = fila.insertCell();
            celdaApellidoContacto.textContent = oficina.email;
            const celdaTelefono = fila.insertCell();
            celdaTelefono.textContent = oficina.idOficinaFk;
            const celdaFax = fila.insertCell();
            celdaFax.textContent = oficina.fax;
            const celdaLimiteCredito = fila.insertCell();
            celdaLimiteCredito.textContent = oficina.limiteCredito;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}
// Llama a la función después de cargar la página o en el momento adecuado
document.addEventListener("DOMContentLoaded", oficinas);

async function facturas() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Detallepedido");
        let respJson = await reqData.json();
        // Obtén la referencia al cuerpo de la tabla
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
            const celdaFax = fila.insertCell();
            celdaFax.textContent = factura.fax;
            const celdaLimiteCredito = fila.insertCell();
            celdaLimiteCredito.textContent = factura.limiteCredito;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}
// Llama a la función después de cargar la página o en el momento adecuado
document.addEventListener("DOMContentLoaded", facturas);

async function inventarios() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Producto");
        let respJson = await reqData.json();
        // Obtén la referencia al cuerpo de la tabla
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
document.addEventListener("DOMContentLoaded", inventarios);

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
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = producto.id;
            const celdaDireccion = fila.insertCell();
            celdaDireccion.textContent = producto.fechaPedido;
            const celdaNombreContacto = fila.insertCell();
            celdaNombreContacto.textContent = producto.fechaEsperada;
            if (producto.fechaEntrega == "1900-01-01" )
            {
                const celdaTelefono = fila.insertCell();
                celdaTelefono.textContent = "En Camino";
            } else {
                const celdaTelefono = fila.insertCell();
                celdaTelefono.textContent = producto.fechaEntrega;
            }
            const celdaTelefono = fila.insertCell();
            celdaTelefono.textContent = producto.comentarios;
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

async function pagos() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Pago/pagosOrdenadosPorFecha");
        let respJson = await reqData.json();
        
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#pagos");
        
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        
        // Itera sobre los primeros 5 objetos en respJson y crea filas y celdas
        respJson.slice(0, 5).forEach((pago) => {
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
document.addEventListener("DOMContentLoaded", pagos);


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
