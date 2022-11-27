using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Entities;
using AccesoDatos;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DetalleNotasController : Controller
    {
        // GET: /<controller>/
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private DetalleNotaDAL detalleNota = new DetalleNotaDAL();

        [HttpGet]
        public async Task<List<DetalleNotaInner>> GetDetalleNotasAsync()
        {
            List<DetalleNotaInner> detalleNotas = await detalleNota.GetDetalleNotasAsync();
            return detalleNotas;
        }

        [HttpPost]
        public async Task<bool> InsertDN([FromBody] DetalleNota detalle)
        {
            bool respuesta = await detalleNota.InsertDN(detalle);
            return respuesta;
        }

    }
}

