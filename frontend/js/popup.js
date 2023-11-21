function togglePopup() {
    const popup = document.getElementById('popup');
    const overlay = document.getElementById('overlay');
    if (popup.style.display === 'none' || popup.style.display === '') {
        popup.style.display = 'block';
        overlay.style.display = 'block';
        setTimeout(() => {
            popup.style.opacity = '1';
            overlay.style.opacity = '1';
        }, 10); 
    } else {
        popup.style.opacity = '0';
        overlay.style.opacity = '0';
        setTimeout(() => {
            popup.style.display = 'none';
            overlay.style.display = 'none';
        }, 300);
        removePopupContent();
    }
  }
  
// ----------------------------------Lenin------------------------------------ //

const salesElement = document.querySelector("#salesConsulta");
salesElement.addEventListener('click', () => {
    getOficinas();
});

async function getOficinas() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Ciudad/oficinas");
        let respJson = await reqData.json();
        addTr(["Ciudad", "Id Oficina"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = oficina.nombreCiudad;
            const celdaApellidoContacto = fila.insertCell();
            celdaApellidoContacto.textContent = oficina.idOficina;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

const oficinaPais = document.querySelector("#oficinaspais");
oficinaPais.addEventListener('click', () => {
    getOficinasPais();
});

async function getOficinasPais() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Pais/oficinasespaña");
        let respJson = await reqData.json();
        addTr(["Ciudad", "Id Oficina", "Telefono"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = oficina.ciudad;
            const celdaApellidoContacto = fila.insertCell();
            celdaApellidoContacto.textContent = oficina.id;
            const celdaTelefono = fila.insertCell();
            celdaTelefono.textContent = oficina.telefono;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

const empleados7x = document.querySelector("#empleado1");
empleados7x.addEventListener('click', () => {
    Empleados7();
});

async function Empleados7() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Empleado/empleados7");
        let respJson = await reqData.json();
        addTr(["Nombre", "Apellido", "Email"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = oficina.nombre;
            const celdaApellidoContacto = fila.insertCell();
            celdaApellidoContacto.textContent = oficina.apellido;
            const celdaTelefono = fila.insertCell();
            celdaTelefono.textContent = oficina.email;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

const x1 = document.querySelector("#jefe");
x1.addEventListener('click', () => {
    DirectorGeneral();
});

async function DirectorGeneral() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Empleado/empleadojefe");
        let respJson = await reqData.json();
        addTr(["Nombre", "Apellido", "Email", "Puesto"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = oficina.nombre;
            const celdaApellidoContacto = fila.insertCell();
            celdaApellidoContacto.textContent = oficina.apellido;
            const celdaTelefono = fila.insertCell();
            celdaTelefono.textContent = oficina.email;
            const celdaPuesto = fila.insertCell();
            celdaPuesto.textContent = oficina.puesto;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

const norep = document.querySelector("#norep");
norep.addEventListener('click', () => {
    noRepresentante();
});

async function noRepresentante() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Empleado/empleadosnorepresentante");
        let respJson = await reqData.json();
        addTr(["Nombre", "Apellido", "Puesto"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = oficina.nombre;
            const celdaApellidoContacto = fila.insertCell();
            celdaApellidoContacto.textContent = oficina.apellido;
            const celdaPuesto = fila.insertCell();
            celdaPuesto.textContent = oficina.puesto;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

const españoles = document.querySelector("#españoles");
españoles.addEventListener('click', () => {
    Españoles();
});

async function Españoles() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Pais/clientesespaña");
        let respJson = await reqData.json();
        addTr(["Nombre"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = oficina.nombre;
            const celdaApellidoContacto = fila.insertCell();
            celdaApellidoContacto.textContent = oficina.apellido;
            const celdaTelefono = fila.insertCell();
            celdaTelefono.textContent = oficina.email;
            const celdaPuesto = fila.insertCell();
            celdaPuesto.textContent = oficina.puesto;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

const estados = document.querySelector("#estados");
estados.addEventListener('click', () => {
    Estados();
});

async function Estados() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Estado/estados");
        let respJson = await reqData.json();
        addTr(["Nombre"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = oficina.nombre;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

const clientes2008 = document.querySelector("#clientes2008");
clientes2008.addEventListener('click', () => {
    Clientes2008();
});

async function Clientes2008() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Pago/clientes2008");
        let respJson = await reqData.json();
        addTr(["Id Cliente"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = oficina.id;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

const tarde = document.querySelector("#tarde");
tarde.addEventListener('click', () => {
    Tarde();
});

async function Tarde() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Pedido/pedidotarde");
        let respJson = await reqData.json();
        addTr(["Id Pedido", "Id Cliente", "Fecha Esperada", "Fecha Entrega"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = oficina.id;
            const celdaIdCliente = fila.insertCell();
            celdaIdCliente.textContent = oficina.idClienteFk;
            const celdaFechaEsperada = fila.insertCell();
            celdaFechaEsperada.textContent = oficina.fechaEsperada;
            const celdaFechaEntrega = fila.insertCell();
            celdaFechaEntrega.textContent = oficina.fechaEntrega;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

const dosdias = document.querySelector("#dosdias");
dosdias.addEventListener('click', () => {
    Dosdias();
});

async function Dosdias() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Pedido/pedido2diasantesAddDate");
        let respJson = await reqData.json();
        addTr(["Id","Fecha Pedido", "Fecha Esperada", "Fecha Entrega", "Comentarios"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = oficina.id;
            const celdaIdCliente = fila.insertCell();
            celdaIdCliente.textContent = oficina.idClienteFk;
            const celdaFechaEsperada = fila.insertCell();
            celdaFechaEsperada.textContent = oficina.fechaEsperada;
            const celdaFechaEntrega = fila.insertCell();
            celdaFechaEntrega.textContent = "Rechazado";
            const celdaComentarios = fila.insertCell();
            celdaComentarios.textContent = oficina.fechaEntrega;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

const pedidos2009 = document.querySelector("#pedidos2009");
pedidos2009.addEventListener('click', () => {
    Pedidos2009();
});

async function Pedidos2009() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Pedido/pedidorechazado2009");
        let respJson = await reqData.json();
        addTr(["Id","Fecha Pedido", "Fecha Esperada", "Fecha Entrega", "Comentarios"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = oficina.id;
            const celdaIdCliente = fila.insertCell();
            celdaIdCliente.textContent = oficina.fechaPedido;
            const celdaFechaEsperada = fila.insertCell();
            celdaFechaEsperada.textContent = oficina.fechaEsperada;
            const celdaFechaEntrega = fila.insertCell();
            celdaFechaEntrega.textContent = "Rechazado";
            const celdaComentarios = fila.insertCell();
            celdaComentarios.textContent = oficina.comentarios;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

const enero = document.querySelector("#enero");
enero.addEventListener('click', () => {
    Enero();
});

async function Enero() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Pedido/pedidoenero");
        let respJson = await reqData.json();
        addTr(["Id","Fecha Pedido", "Fecha Esperada", "Fecha Entrega", "Comentarios"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = oficina.id;
            const celdaIdCliente = fila.insertCell();
            celdaIdCliente.textContent = oficina.fechaPedido;
            const celdaFechaEsperada = fila.insertCell();
            celdaFechaEsperada.textContent = oficina.fechaEsperada;
            const celdaFechaEntrega = fila.insertCell();
            celdaFechaEntrega.textContent = oficina.fechaEntrega;
            const celdaComentarios = fila.insertCell();
            celdaComentarios.textContent = oficina.comentarios;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

const paypal = document.querySelector("#paypal");
paypal.addEventListener('click', () => {
    Paypal();
});

async function Paypal() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Pago/pagopaypal2008");
        let respJson = await reqData.json();
        addTr(["Id","Fecha Pago", "Total", "Forma Pago"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = oficina.id;
            const celdaIdCliente = fila.insertCell();
            celdaIdCliente.textContent = oficina.fechaPago;
            const celdaFechaEsperada = fila.insertCell();
            celdaFechaEsperada.textContent = oficina.total;
            const celdaFechaEntrega = fila.insertCell();
            celdaFechaEntrega.textContent = oficina.formaPago;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

const formapago = document.querySelector("#formapago");
formapago.addEventListener('click', () => {
    FormaPago();
});

async function FormaPago() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Formapago/formaspago");
        let respJson = await reqData.json();
        addTr(["Nombre"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = oficina.nombre;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

const ornamentales = document.querySelector("#ornamentales");
ornamentales.addEventListener('click', () => {
    Ornamentales();
});

async function Ornamentales() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Producto/productosornamentales100");
        let respJson = await reqData.json();
        addTr(["Nombre","Precio Venta", "Cantidad", "Dimensiones"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = oficina.nombre;
            const celdaIdCliente = fila.insertCell();
            celdaIdCliente.textContent = oficina.precioVenta;
            const celdaFechaEsperada = fila.insertCell();
            celdaFechaEsperada.textContent = oficina.cantidad;
            const celdaFechaEntrega = fila.insertCell();
            celdaFechaEntrega.textContent = oficina.dimensiones;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

const madrid1130 = document.querySelector("#madrid1130");
madrid1130.addEventListener('click', () => {
    Madrid1130();
});

async function Madrid1130() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Cliente/clientesmadrid11o30");
        let respJson = await reqData.json();
        addTr(["Nombre"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = oficina.nombre;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

const cantidadclientespais = document.querySelector("#cantidadclientespais");
cantidadclientespais.addEventListener('click', () => {
    CantidadClientesPais();
});

async function CantidadClientesPais() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Cliente/cantidadclientesbypais");
        let respJson = await reqData.json();
        addTr(["Pais", "Cantidad"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = oficina.pais;
            const cantidad = fila.insertCell();
            cantidad.textContent = oficina.count;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

const media2009 = document.querySelector("#media2009");
media2009.addEventListener('click', () => {
    Media2009();
});

async function Media2009() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Pago/pagos2009media");
        let respJson = await reqData.json();
        addTr(["Cantidad"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = oficina.cantidad;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

const pedidosestado = document.querySelector("#pedidosestado");
pedidosestado.addEventListener('click', () => {
    PedidosEstado();
});

async function PedidosEstado() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Pedido/pedidoscantidadestado");
        let respJson = await reqData.json();
        addTr(["Estado", "Cantidad"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = oficina.estado;
            const cantidad = fila.insertCell();
            cantidad.textContent = oficina.count;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

// const carobarato = document.querySelector("#carobarato");
// carobarato.addEventListener('click', () => {
//     CaroBarato();
// });

// async function CaroBarato() {
//     try {
//         let reqData = await fetch("http://localhost:5280/api/Producto/productobaratocaro");
//         let respJson = await reqData.json();
//         addTr(["Producto Caro", "Precio Caro", "Producto Barato", "Precio Barato"]);
//         // Obtén la referencia al cuerpo de la tabla
//         const cuerpoTabla = document.querySelector("#miTabla");
//         // Limpiar contenido existente en el cuerpo de la tabla
//         cuerpoTabla.innerHTML = "";
//         // Itera sobre los objetos en respJson y crea filas y celdas
//         respJson => {
//             // Crea una nueva fila
//             const fila = cuerpoTabla.insertRow();
//             // Crea celdas y asigna valores a cada celda
//             const celdaNombre = fila.insertCell();
//             celdaNombre.textContent = oficina.nombreCaro;
//             const cantidad = fila.insertCell();
//             cantidad.textContent = oficina.caro;
//             const nombrebarato = fila.insertCell();
//             nombrebarato.textContent = oficina.nombreBarato;
//             const barato = fila.insertCell();
//             barato.textContent = oficina.barato;
//         }
//     } catch (error) {
//         console.error('Error al obtener datos:', error);
//     }
// }

const cantidadempleados = document.querySelector("#cantidadempleados");
cantidadempleados.addEventListener('click', () => {
    CantidadEmpleados();
});

async function CantidadEmpleados() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Empleado/cantidadempleados");
        let respJson = await reqData.json();
        addTr(["Cantidad"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const cantidad = fila.insertCell();
            cantidad.textContent = oficina.cantidad;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

const cantidadclientes = document.querySelector("#cantidadclientes");
cantidadclientes.addEventListener('click', () => {
    CantidadClientes();
});

async function CantidadClientes() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Cliente/cantidadclientes");
        let respJson = await reqData.json();
        addTr(["Cantidad"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((oficina) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const cantidad = fila.insertCell();
            cantidad.textContent = oficina.cantidad;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}


/*-------------------------Brayan---------------------- */

// const MaxLimite = document.querySelector("#max-credito");
// MaxLimite.addEventListener('click', () => {
//     getPrecMax();
// });
// async function getCreditoMax() {
//     try {
//         let reqData = await fetch("http://localhost:5280/api/Cliente/clienteMaxCredito");
//         let respJson = await reqData.json();
//         addTr(["Nombre", "Precio Venta"]);
//         // Obtén la referencia al cuerpo de la tabla
//         const cuerpoTabla = document.querySelector("#miTabla");
//         // Limpiar contenido existente en el cuerpo de la tabla
//         cuerpoTabla.innerHTML = "";
//         // Itera sobre los objetos en respJson y crea filas y celdas
//         respJson.forEach((producto) => {
//             // Crea una nueva fila
//             const fila = cuerpoTabla.insertRow();
//             // Crea celdas y asigna valores a cada celda
//             const celdaNombre = fila.insertCell();
//             celdaNombre.textContent = producto.nombre;
//             const celdaLimiteCredito = fila.insertCell();
//             celdaLimiteCredito.textContent = producto;
//         });
//     } catch (error) {
//         console.error('Error al obtener datos:', error);
//     }
// }

// const PrecioMax = document.querySelector("#precio-max");
// PrecioMax.addEventListener('click', () => {
//     getCreditoMax();
// });
// async function getCreditoMax() {
//     try {
//         let reqData = await fetch("http://localhost:5280/api/Cliente/clienteMaxCredito");
//         let respJson = await reqData.json();
//         addTr(["Nombre", "Limite Credito"]);
//         // Obtén la referencia al cuerpo de la tabla
//         const cuerpoTabla = document.querySelector("#miTabla");
//         // Limpiar contenido existente en el cuerpo de la tabla
//         cuerpoTabla.innerHTML = "";
//         // Itera sobre los objetos en respJson y crea filas y celdas
//         respJson.forEach((oficina) => {
//             // Crea una nueva fila
//             const fila = cuerpoTabla.insertRow();
//             // Crea celdas y asigna valores a cada celda
//             const celdaNombre = fila.insertCell();
//             celdaNombre.textContent = oficina.nombre;
//             const celdaLimiteCredito = fila.insertCell();
//             celdaLimiteCredito.textContent = oficina.limiteCredito;
//         });
//     } catch (error) {
//         console.error('Error al obtener datos:', error);
//     }
// }

function addTr(lista) {
    let popup_top = document.getElementById("popup-top");

    lista.forEach(child => {
        let th = document.createElement('th');
        th.textContent = child; // Corrección: Usar '=' en lugar de '()'
        popup_top.appendChild(th);
    });
}
function removePopupContent() {
    
    let popup_top = document.getElementById("popup-top");
    popup_top.innerHTML = ''; // Establece el contenido HTML del elemento como vacío
    let content = document.getElementById("miTabla");
    content.innerHTML = '';
}