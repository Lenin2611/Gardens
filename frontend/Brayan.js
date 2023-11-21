function togglePopupp() {
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


const PrecioMax = document.querySelector("#precio-max");
PrecioMax.addEventListener('click', () => {
    getCreditoMax();
});

async function getCreditoMax() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Cliente/clienteMaxCredito");
        let respJson = await reqData.json();
        addTr(["Nombre", "Limite Credito"]);
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
            const celdaLimiteCredito = fila.insertCell();
            celdaLimiteCredito.textContent = cliente.limiteCredito;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}


const ProductoMaxStock = document.querySelector("#producto-max");
ProductoMaxStock.addEventListener('click', () => {
    getProductoMax();
});

async function getProductoMax() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Productos/productoMax");
        let respJson = await reqData.json();
        addTr(["Nombre", "Cantidad"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((p) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = p.nombre;
            const celdaCantidad = fila.insertCell();
            celdaCantidad.textContent = p.cantidad;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

const ProductoMinStock = document.querySelector("#producto-min");
ProductoMinStock.addEventListener('click', () => {
    getProductoMin();
});

async function getProductoMin() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Productos/productoMin");
        let respJson = await reqData.json();
        addTr(["Nombre", "Cantidad"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((p) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = p.nombre;
            const celdaCantidad = fila.insertCell();
            celdaCantidad.textContent = p.cantidad;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}

const albertoSoria = document.querySelector("#producto-min");
albertoSoria.addEventListener('click', () => {
    getProductoMin();
});

async function getAlberto() {
    try {
        let reqData = await fetch("http://localhost:5280/api/Empleado/empleadosAlberto");
        let respJson = await reqData.json();
        addTr(["Nombre", "Cantidad"]);
        // Obtén la referencia al cuerpo de la tabla
        const cuerpoTabla = document.querySelector("#miTabla");
        // Limpiar contenido existente en el cuerpo de la tabla
        cuerpoTabla.innerHTML = "";
        // Itera sobre los objetos en respJson y crea filas y celdas
        respJson.forEach((a) => {
            // Crea una nueva fila
            const fila = cuerpoTabla.insertRow();
            // Crea celdas y asigna valores a cada celda
            const celdaNombre = fila.insertCell();
            celdaNombre.textContent = p.nombre;
            const celdaCantidad = fila.insertCell();
            celdaCantidad.textContent = p.cantidad;
        });
    } catch (error) {
        console.error('Error al obtener datos:', error);
    }
}



const norep = document.querySelector("#norep");
norep.addEventListener('click', () => {
    noRepresentante();
});

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