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
	public class ProveedorModel : PageModel
    {
        private readonly ProveedorBL proveedorBl = new ProveedorBL();
        public IEnumerable<Proveedor>? proveedorsList { get; set; }
        public string? Alerta { get; set; }

        public async Task OnGet()
        {
            proveedorsList = await proveedorBl.GetProveedorsAsync();
        }

        public async Task OnPost()
        {
            Proveedor proveedor = new Proveedor() {
                RazonSoc = Request.Form["Razon"],
                Agente = Request.Form["Agente"],
                Direccion = Request.Form["Direccion"],
                Telefono = Request.Form["Telefono"],
                Correo = Request.Form["Correo"],
                Tipo_Material = Request.Form["Material"]
            };
            bool respuesta = await proveedorBl.AddProveedorAsync(proveedor);

            if (respuesta)
            {
                proveedorsList = await proveedorBl.GetProveedorsAsync();
                Alerta = "SE INSERTO CORRECTAMENTE EL PROVEEDOR";
            }
            else
                Alerta = "Ocurrio un error al crear un nuevo proveedor";

        }

        public async Task<FileResult> OnPostGenerarExcel()
        {
            var data = await proveedorBl.GetProveedorsAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Proveedores");
                worksheet.Cell(3, 1).Value = "Id Proveedor";
                worksheet.Cell(3, 2).Value = "Razon Social";
                worksheet.Cell(3, 3).Value = "Agente";
                worksheet.Cell(3, 4).Value = "Direccion";
                worksheet.Cell(3, 5).Value = "Telefono";
                worksheet.Cell(3, 6).Value = "Correo";
                worksheet.Cell(3, 7).Value = "Tipo de Material";
                

                worksheet.Style.Font.Bold = true;
                worksheet.Cells("A3:G3").Style.Fill.BackgroundColor = XLColor.AliceBlue;
                worksheet.Column("A").Width = 25;
                worksheet.Column("B").Width = 25;
                worksheet.Column("C").Width = 25;
                worksheet.Column("D").Width = 25;
                worksheet.Column("E").Width = 25;
                worksheet.Column("F").Width = 25;
                worksheet.Column("G").Width = 25;
               

                var encabezado = worksheet.Range("A1:G2").Merge();
                worksheet.Cell("A1").Value = "Constructora OGS";
                worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                encabezado.Style.Fill.BackgroundColor = XLColor.BlueBell;


                int index = 4;
                foreach (var item in data)
                {
                    worksheet.Cell(index, 1).Value = item.Id_Proveedor;
                    worksheet.Cell(index, 2).Value = item.RazonSoc;
                    worksheet.Cell(index, 3).Value = item.Agente;
                    worksheet.Cell(index, 4).Value = item.Direccion;
                    worksheet.Cell(index, 5).Value = item.Telefono;
                    worksheet.Cell(index, 6).Value = item.Correo;
                    worksheet.Cell(index, 7).Value = item.Tipo_Material;
                    
                    index++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    var strDate = DateTime.Now.ToString("yyyy-MM-dd");
                    string filename = string.Format($"Proveedores{strDate}.xlsx");
                    return File(content, contentType, filename);
                }

            }
        }
    }
}
