using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AccesoDatos;
using Entities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ObraController : Controller
    {
        // GET: /<controller>/
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private readonly ObrasDAL obrasDAL = new ObrasDAL();

        [HttpGet]
        public async Task<List<Obra>> GetObras()
        {
            List<Obra> obras = await obrasDAL.GetObrasAsync();
            return obras;
        }

        [HttpPost]
        public async Task<bool> InsertObra([FromBody]Obra obra)
        {
            bool respuesta = await obrasDAL.InsertObra(obra);
            return respuesta;
        }
    }
}

