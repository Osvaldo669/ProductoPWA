using System;
using Entities;
using Newtonsoft.Json;
using System.Runtime.Intrinsics.Arm;
using System.Text;

namespace BL
{
	public class NotaBL
	{
		HttpClient httpClient;
        string URIBase = "http://20.110.177.121/app/api/Nota/";
        public string? Mensaje { get; set; }

        public NotaBL()
		{
			httpClient = new HttpClient();
		}

        public async Task<List<Nota>> GetNotasAsync()
        {
            string URI = URIBase + "GetNotas";

            List<Nota>? notas = new List<Nota>();
            try
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync(URI);
                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var Content = await responseMessage.Content.ReadAsStringAsync();
                    notas = JsonConvert.DeserializeObject<List<Nota>>(Content);
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

            return notas;
        }

        public async Task<bool> AddNotaAsync(Nota nota)
        {
            bool respuesta = false;
            string URI = URIBase + "InsertNota";

            try
            {
                var Content = new StringContent(JsonConvert.SerializeObject(nota),
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

        public async Task<List<BusquedaEspecial>> GetBusquedas(int obra, string fecha1, string fecha2)
        {
            string URI = URIBase + "GetBusquedaEspecials/" + obra + "/" + fecha1 + "/" + fecha2;

            List<BusquedaEspecial>? notasE = new List<BusquedaEspecial>();
            try
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync(URI);
                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var Content = await responseMessage.Content.ReadAsStringAsync();
                    notasE = JsonConvert.DeserializeObject<List<BusquedaEspecial>>(Content);
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

            return notasE;
        }

        public async Task<List<BusquedaEspecial>> GetBusquedas2(int obra)
        {
            string URI = URIBase + "GetBusquedaEspecialsAsync2/" + obra ;

            List<BusquedaEspecial>? notasE = new List<BusquedaEspecial>();
            try
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync(URI);
                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var Content = await responseMessage.Content.ReadAsStringAsync();
                    notasE = JsonConvert.DeserializeObject<List<BusquedaEspecial>>(Content);
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

            return notasE;
        }

    }
}

