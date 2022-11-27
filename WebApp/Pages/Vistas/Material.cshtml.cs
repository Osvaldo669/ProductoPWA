using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Net;
using BL;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Net.Mime;

namespace WebApp.Pages.Vistas
{
	public class MaterialModel : PageModel
    {
        public IEnumerable<Material>? Materials { get; set; }
        public string? Alerta { get; set; }

        private readonly MaterialBL materialBL = new MaterialBL();

       
        public async Task OnGet()
        {
            Materials = await materialBL.GetMaterialsAsync();
        }

        public async Task OnPost()
        {
            Material material = new Material()
            {
                Nombre_Mat = Request.Form["Nombre"],
                Marca = Request.Form["Marca"],
                Categoria = Request.Form["Categoria"],
                UnidadMedida = Request.Form["Medida"]

            };
            bool respuesta = await materialBL.AddMaterialAsync(material);
            if (respuesta)
            {
                Materials = await materialBL.GetMaterialsAsync();
                Alerta = "SE AGREGO EL MATERIAL CON EXITO";
            }
            else
            {
                Alerta = "Ocurrio un error al agregar";
            }
            
        }

        public async Task<FileResult> OnPostGenerarExcel()
        {
            var data = await materialBL.GetMaterialsAsync();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Material");
                worksheet.Cell(3, 1).Value ="ID Material";
                worksheet.Cell(3, 2).Value = "Nombre";
                worksheet.Cell(3, 3).Value = "Marca";
                worksheet.Cell(3, 4).Value = "Categoria";
                worksheet.Cell(3, 5).Value = "Unidad de Medida";

                worksheet.Style.Font.Bold = true;
                worksheet.Cells("A3:E3").Style.Fill.BackgroundColor = XLColor.AliceBlue;
                worksheet.Column("A").Width = 25;
                worksheet.Column("B").Width = 25;
                worksheet.Column("C").Width = 25;
                worksheet.Column("D").Width = 25;
                worksheet.Column("E").Width = 25;
                var encabezado = worksheet.Range("A1:E2").Merge();
                worksheet.Cell("A1").Value = "Constructora OGS";
                worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                encabezado.Style.Fill.BackgroundColor = XLColor.BlueBell;


                int index = 4;
                foreach(var material in data)
                {
                    worksheet.Cell(index, 1).Value = material.Id_Material;
                    worksheet.Cell(index, 2).Value = material.Nombre_Mat;
                    worksheet.Cell(index, 3).Value = material.Marca;
                    worksheet.Cell(index, 4).Value = material.Categoria;
                    worksheet.Cell(index, 5).Value = material.UnidadMedida;
                    index++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    var strDate = DateTime.Now.ToString("yyyyMMdd");
                    string filename = string.Format($"Material{strDate}.xlsx");
                     return File(content, contentType, filename);
                }

            }
        }




    }


}
