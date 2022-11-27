using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL;
using ClosedXML.Excel;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Vistas
{
	public class NotaModel : PageModel
    {
        private readonly NotaBL notaBL = new NotaBL();
        private readonly ObraBL obraBL = new ObraBL();

        public IEnumerable<Nota>? notasList { get; set; }
        public IEnumerable<BusquedaEspecial>? busquedas { get; set; }
        public IEnumerable<BusquedaEspecial>? busquedas2 { get; set; }
        public IEnumerable<Obra>? obras { get; set; }

        public string? Alerta { get; set; }

        public async Task OnGet()
        {
            notasList = await notaBL.GetNotasAsync();
            obras = await obraBL.GetObrasAsync();
        }

        public async Task OnPost()
        {
            
            Nota nota = new Nota() {
                Fecha = DateTime.Today,
                Extra = Request.Form["Extra"]
            };

            bool respuesta = await notaBL.AddNotaAsync(nota);
            if (respuesta)
            {
                notasList = await notaBL.GetNotasAsync();
                obras = await obraBL.GetObrasAsync();
                Alerta = "SE INSERTO CORRECTAMENTE UNA NOTA";
            }
            else
                Alerta = "Ocurrio un error al crear la nota";
        }


        public async Task OnPostOnClick()
        {
            notasList = await notaBL.GetNotasAsync();
            obras = await obraBL.GetObrasAsync();

            try
            {
                string fecha1 = Request.Form["Fecha1"];
                string fecha2 = Request.Form["Fecha2"];
                int obra= Convert.ToInt32(Request.Form["Obra"]);

                busquedas = await notaBL.GetBusquedas(obra, fecha1, fecha2);
            }
            catch(Exception)
            {
                Alerta = "Ocurrio un error";
            }
        }

        public async Task OnPostTwoClick()
        {
            notasList = await notaBL.GetNotasAsync();
            obras = await obraBL.GetObrasAsync();

            try
            {
                int obra = Convert.ToInt32(Request.Form["ObraB"]);

                busquedas2 = await notaBL.GetBusquedas2(obra);
            }
            catch (Exception)
            {
                Alerta = "Ocurrio un error";
            }
        }


        public async Task<FileResult> OnPostGenerarExcel()
        {
            var data = await notaBL.GetNotasAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Notas");
                worksheet.Cell(3, 1).Value = "Id Nota";
                worksheet.Cell(3, 2).Value = "Fecha";
                worksheet.Cell(3, 3).Value = "Extra";

                worksheet.Style.Font.Bold = true;
                worksheet.Cells("A3:C3").Style.Fill.BackgroundColor = XLColor.AliceBlue;
                worksheet.Column("A").Width = 25;
                worksheet.Column("B").Width = 25;
                worksheet.Column("C").Width = 25;


                var encabezado = worksheet.Range("A1:C2").Merge();
                worksheet.Cell("A1").Value = "Constructora OGS";
                worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                encabezado.Style.Fill.BackgroundColor = XLColor.BlueBell;


                int index = 4;
                foreach (var item in data)
                {
                    worksheet.Cell(index, 1).Value = item.Id_Nota;
                    worksheet.Cell(index, 2).Value = item.Fecha;
                    worksheet.Cell(index, 3).Value = item.Extra;
                    index++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    var strDate = DateTime.Now.ToString("yyyyMMdd");
                    string filename = string.Format($"Notas{strDate}.xlsx");
                    return File(content, contentType, filename);
                }

            }
        }

    }
}
