using System;
using Entities;
using System.Text;
using Newtonsoft.Json;

namespace BL
{
	public class MaterialBL
	{
		HttpClient httpClient;
        string URIBase = "http://20.110.177.121/app/api/Material/";
		public string? Mensaje { get; set; } 


        public MaterialBL()
		{
			httpClient = new HttpClient();
			
		}

		
		public async Task<List<Material>> GetMaterialsAsync()
		{
			string URI = URIBase+"GetMateriales";

            List<Material>? Lista_Materiales = new List<Material>();
            try
			{
				HttpResponseMessage responseMessage = await httpClient.GetAsync(URI);
				responseMessage.EnsureSuccessStatusCode();
				if(responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
				{
					var Content = await responseMessage.Content.ReadAsStringAsync();
					Lista_Materiales = JsonConvert.DeserializeObject<List<Material>>(Content);
				}
			}
			catch(Exception ex)
			{
				Mensaje = ex.Message;
			}

			return Lista_Materiales;
		}


		public async Task<bool> AddMaterialAsync(Material material)
		{
			bool respuesta = false;
            string URI = URIBase + "InsertMaterial";

			try
			{
				var Content = new StringContent(JsonConvert.SerializeObject(material),
					Encoding.UTF8, "application/json");
				HttpResponseMessage responseMessage = await httpClient.PostAsync(URI,Content);
				responseMessage.EnsureSuccessStatusCode();
				if(responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
				{
					respuesta = true;
				}
			}
			catch(Exception)
			{
				respuesta = false;
			}

			return respuesta;

        }
    }
}

