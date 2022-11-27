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
    public class ProveedorController : Controller
    {
        // GET: /<controller>/
        //public IActionResult Index()
        //{
        //    return View();
        //}

        ProveedorDAL proveedorDAL = new ProveedorDAL();

        [HttpGet]
        public async Task<List<Proveedor>> GetProveedores()
        {
            List<Proveedor> proveedors = await proveedorDAL.GetProveedorsAsync();
            return proveedors;
        }

        [HttpPost]
        public async Task<bool> InsertProveedor([FromBody] Proveedor proveedor)
        {
            bool respuesta = await proveedorDAL.InsertProveedor(proveedor);
            return respuesta;
        }

        
    }
}

