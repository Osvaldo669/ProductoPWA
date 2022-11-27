using System.Diagnostics;
using System.Net.Http.Headers;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRueba.Models;

namespace PRueba.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

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

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

