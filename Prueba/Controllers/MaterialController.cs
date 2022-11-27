using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PRueba.Controllers
{
    public class MaterialController : Controller
    {
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            List<Material>? materials = new List<Material>();
            materials = new List<Material>();
            string Uri = "http://localhost:5147/api/Material/GetMateriales";
            HttpClient cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                HttpResponseMessage response = await cliente.GetAsync(Uri);
                response.EnsureSuccessStatusCode();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string Content = await response.Content.ReadAsStringAsync();
                    materials = JsonConvert.DeserializeObject<List<Material>>(Content);
                }
            }
            catch (Exception ex)
            {
                materials = null;
                var es = ex.Message;
            }
            return View(materials);
        }
    }
}

