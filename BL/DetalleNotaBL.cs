using System;
using System.Text;
using Entities;
using Newtonsoft.Json;

namespace BL
{
	public class DetalleNotaBL
	{
        HttpClient httpClient;
        string URIBase = "http://20.110.177.121/app/api/DetalleNotas/";
        public DetalleNotaBL()
		{
			httpClient = new HttpClient();
		}

		public async Task<List<DetalleNotaInner>> GetDetalleNotasAsync()
		{
            string URI = URIBase + "GetDetalleNotas";

            List<DetalleNotaInner>? list = new List<DetalleNotaInner>();
            try
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync(URI);
                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var Content = await responseMessage.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<DetalleNotaInner>>(Content);
                }
            }
            catch (Exception ex)
            {
                var Mes = ex.Message;
            }

            return list;
        }

        public async Task<bool> AddDetalleNota(DetalleNota detalle)
        {

            bool respuesta = false;
            string URI = URIBase + "InsertDN";

            try
            {
                var Content = new StringContent(JsonConvert.SerializeObject(detalle),
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

        //public async Task LlenarSelects()
        //{

        //}

    }
}

