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
    public class MaterialController : Controller
    {
        // GET: /<controller>/
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private MaterialDAL materialDAL = new MaterialDAL();

        [HttpGet]
        public async Task<List<Material>> GetMateriales()
        {
            List < Material > materiales = await materialDAL.GetMaterialsAsync();
            return materiales;
        }


        [HttpPost]
        public async Task<bool> InsertMaterial([FromBody] Material material)
        {
            bool respuesta = await materialDAL.InsertMaterial(material);

            return respuesta;
        }


        
    }
}

