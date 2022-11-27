using System;
using System.Text;
using Entities;
using Newtonsoft.Json;


namespace BL
{
	public class ProveedorBL
	{
		string URIBase = "http://20.110.177.121/app/api/Proveedor/";
        HttpClient httpClient;
        public string? Mensaje { get; set; }

        public ProveedorBL()
		{
			httpClient = new HttpClient();
		}

		public async Task<List<Proveedor>> GetProveedorsAsync()
		{
			string URI = URIBase + "GetProveedores";

            List<Proveedor> ? proveedores = new List<Proveedor>();
			try
			{
				HttpResponseMessage responseMessage = await httpClient.GetAsync(URI);
				responseMessage.EnsureSuccessStatusCode();
				if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
				{
					var Content = await responseMessage.Content.ReadAsStringAsync();
					proveedores = JsonConvert.DeserializeObject<List<Proveedor>>(Content);
				}
				else
				{
					Mensaje = "No hay datos";
				}

			}
			catch(Exception ex)
			{
				Mensaje = "Error: " + ex.Message;
			}

			return proveedores;
		}

		public async Task<bool> AddProveedorAsync(Proveedor proveedor)
		{
            bool respuesta = false;
            string URI = URIBase + "InsertProveedor";

            try
            {
                var Content = new StringContent(JsonConvert.SerializeObject(proveedor),
                    Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await httpClient.PostAsync(URI, Content);
                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    respuesta = true;
                }
            }
            catch (Exception)
            {
                respuesta = false;
            }

            return respuesta;

        }
    
	}
}

