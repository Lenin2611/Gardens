// document.addEventListener('DOMContentLoaded', function () {
//     const clientesBody = document.getElementById('table');

//     // Hacer una solicitud GET al servidor para obtener los datos de clientes
//     fetch('https://localhost:5280/api/Cliente')
//         .then(response => {
//             if (!response.ok) {
//                 throw new Error(`Error de red: ${response.status}`);
//             }
//             return response.json();
//         })
//         .then(clientes => {
//             // Limpiar el cuerpo de la tabla
//             clientesBody.innerHTML = '';

//             // Iterar sobre los datos y agregar filas a la tabla
//             clientes.forEach(cliente => {
//                 const row = document.createElement('tr');
//                 row.innerHTML = `
//                     <td>${cliente.id}</td>
//                     <td>${cliente.nombre}</td>
//                     <td>${cliente.nombreContacto}</td>
//                     <td>${cliente.apellidoContacto}</td>
//                     <td>${cliente.telefono}</td>
//                     <td>${cliente.fax}</td>
//                     <td>${cliente.limiteCredito}</td>
//                 `;
//                 clientesBody.appendChild(row);
//             });
//         })
//         .catch(error => console.error(`Error al obtener datos: ${error.message}`));
// });


const hola = document.querySelector('#hola');

hola.addEventListener('click', async ()=>{
    let res = await fetch ('http://localhost:5280/api/Cliente')
    console.log(res)
})