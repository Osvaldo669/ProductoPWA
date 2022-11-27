using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccesoDatos;
using Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NotaController : Controller
    {

        private NotaDAL Access = new NotaDAL();

        [HttpGet()]
        public async Task<List<Nota>> GetNotas()
        {
            List<Nota> notas = await Access.GetNotas();
            return notas;
        }

        [HttpPost]
        public async Task<bool> InsertNota([FromBody] Nota nota)
        {
            bool respuesta = await Access.InsertNote(nota);
            return respuesta;
        }

        [HttpGet("{obra}/{fecha1}/{fecha2}")]
        public async Task<List<BusquedaEspecial>> GetBusquedaEspecialsAsync(int obra, DateTime fecha1, DateTime fecha2)
        {
            List<BusquedaEspecial> busquedas = await Access.ConsultaEsp(obra, fecha1, fecha2);
            return busquedas;
        }

        [HttpGet("{obra}")]
        public async Task<List<BusquedaEspecial>> GetBusquedaEspecialsAsync2(int obra)
        {
            List<BusquedaEspecial> busquedas = await Access.ConsultaEsp2(obra);
            return busquedas;
        }
    }
}

