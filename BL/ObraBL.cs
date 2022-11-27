using System;
using Entities;
using Newtonsoft.Json;
using System.Runtime.Intrinsics.Arm;
using System.Text;

namespace BL
{
	public class ObraBL
	{
        HttpClient httpClient;
        string URIBase = "http://20.110.177.121/app/api/Obra/";
        public string? Mensaje { get; set; }

        public ObraBL()
		{
			httpClient = new HttpClient();
		}

        public async Task<List<Obra>> GetObrasAsync()
        {
            string URI = URIBase + "GetObras";

            List<Obra>? obras = new List<Obra>();
            try
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync(URI);
                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var Content = await responseMessage.Content.ReadAsStringAsync();
                    obras = JsonConvert.DeserializeObject<List<Obra>>(Content);
                }
                else
                {
                    Mensaje = "No hay datos";
                }

            }
            catch (Exception ex)
            {
                Mensaje = "Error: " + ex.Message;
            }

            return obras;
        }

        public async Task<bool> AddObraAsync(Obra obra)
        {
            bool respuesta = false;
            string URI = URIBase + "InsertObra";

            try
            {
                var Content = new StringContent(JsonConvert.SerializeObject(obra),
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

