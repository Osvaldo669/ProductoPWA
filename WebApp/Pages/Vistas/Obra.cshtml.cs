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
	public class ObraModel : PageModel
    {
        private readonly ObraBL obraBL = new ObraBL();
        public IEnumerable<Obra>? Obras { get; set; }
        public string? Alerta { get; set; }

        public async Task OnGet()
        {
            Obras = await obraBL.GetObrasAsync();
        }

        public async Task OnPost()
        {
            Obra obra = new Obra() {
                Nombre_Obra = Request.Form["Nombre"],
                Direccion = Request.Form["Direccion"],
                FechaInicio = Convert.ToDateTime(Request.Form["FechaI"]),
                FechaFinal = Convert.ToDateTime(Request.Form["FechaF"]),
                Dueno = Request.Form["Dueno"],
                Responsable = Request.Form["Responsable"],
                Tel_resp = Request.Form["Telefono"],
                Correo_res= Request.Form["Correo"]
            };
            bool respuesta = await obraBL.AddObraAsync(obra);
            if (respuesta)
            {
                Obras = await obraBL.GetObrasAsync();
                Alerta = "SE INSERTO CORRECTAMENTE UNA NOTA";
            }
            else
                Alerta = "Ocurrio un error al crear la nota";
        }

        public async Task<FileResult> OnPostGenerarExcel()
        {
            var data = await obraBL.GetObrasAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Obras");
                worksheet.Cell(3, 1).Value = "Id Obra";
                worksheet.Cell(3, 2).Value = "Nombre Obra";
                worksheet.Cell(3, 3).Value = "Direccion";
                worksheet.Cell(3, 4).Value = "Fecha-Inicio";
                worksheet.Cell(3, 5).Value = "Fecha-Final";
                worksheet.Cell(3, 6).Value = "Dueño";
                worksheet.Cell(3, 7).Value = "Responsable";
                worksheet.Cell(3, 8).Value = "Telefono";
                worksheet.Cell(3, 9).Value = "Correo";

                worksheet.Style.Font.Bold = true;
                worksheet.Cells("A3:I3").Style.Fill.BackgroundColor = XLColor.AliceBlue;
                worksheet.Column("A").Width = 25;
                worksheet.Column("B").Width = 25;
                worksheet.Column("C").Width = 25;
                worksheet.Column("D").Width = 25;
                worksheet.Column("E").Width = 25;
                worksheet.Column("F").Width = 25;
                worksheet.Column("G").Width = 25;
                worksheet.Column("H").Width = 25;
                worksheet.Column("I").Width = 25;


                var encabezado = worksheet.Range("A1:I2").Merge();
                worksheet.Cell("A1").Value = "Constructora OGS";
                worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                encabezado.Style.Fill.BackgroundColor = XLColor.BlueBell;


                int index = 4;
                foreach (var item in data)
                {
                    worksheet.Cell(index, 1).Value = item.Id_Obra;
                    worksheet.Cell(index, 2).Value = item.Nombre_Obra;
                    worksheet.Cell(index, 3).Value = item.Direccion;
                    worksheet.Cell(index, 4).Value = item.FechaInicio;
                    worksheet.Cell(index, 5).Value = item.FechaFinal;
                    worksheet.Cell(index, 6).Value = item.Dueno;
                    worksheet.Cell(index, 7).Value = item.Responsable;
                    worksheet.Cell(index, 8).Value = item.Tel_resp;
                    worksheet.Cell(index, 9).Value = item.Correo_res;
                    index++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    var strDate = DateTime.Now.ToString("yyyy-MM-dd");
                    string filename = string.Format($"Obras{strDate}.xlsx");
                    return File(content, contentType, filename);
                }

            }
        }
    }
}
