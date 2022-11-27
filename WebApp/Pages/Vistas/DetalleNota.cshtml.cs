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
	public class DetalleNotaModel : PageModel
    {
        private readonly DetalleNotaBL detalleNota = new DetalleNotaBL();
        private readonly MaterialBL materialBL = new MaterialBL();
        private readonly NotaBL notaBL = new NotaBL();
        private readonly ObraBL obraBL = new ObraBL();
        private readonly ProveedorBL proveedorBl = new ProveedorBL();


        public IEnumerable<DetalleNotaInner>? ListaDetalleNotas { get; set; }
        public List<Obra>? obras { get; set; }
        public List<Proveedor>? proveedors { get; set; }
        public List<Material>? materials { get; set; }
        public List<Nota>? notas { get; set; }
        public string? Alerta { get; set; }


        public async Task OnGet()
        {
            ListaDetalleNotas = await detalleNota.GetDetalleNotasAsync();
            await LlenarSelects();

        }

        public async Task OnPost()
        {
            var select = Request.Form["Obra"];

            DetalleNota detalle = new DetalleNota()
            {
                ObraP = Convert.ToInt32(Request.Form["Obra"]),
                Provee = Convert.ToInt32(Request.Form["Proveedor"]),
                MaterialM = Convert.ToInt32(Request.Form["Material"]),
                NotaE = Convert.ToInt32(Request.Form["Nota"]),
                Cantidad = Convert.ToInt32(Request.Form["Cantidad"]),
                PrecioUnitario = Convert.ToDouble(Request.Form["Precio"]),
                Extra = Request.Form["Extra"]

            };

            bool respuesta = await detalleNota.AddDetalleNota(detalle);
            if (respuesta)
            {
                ListaDetalleNotas = await detalleNota.GetDetalleNotasAsync();
                await LlenarSelects();
                Alerta = "SE INSERTO CORRECTAMENTE UNA NOTA";
            }
            else
                Alerta = "Ocurrio un error al crear la nota";
        }

        private async Task LlenarSelects()
        {
            obras = await obraBL.GetObrasAsync();
            proveedors = await proveedorBl.GetProveedorsAsync();
            materials = await materialBL.GetMaterialsAsync();
            notas = await notaBL.GetNotasAsync();

        }


        public async Task<FileResult> OnPostGenerarExcel()
        {
            var data = await detalleNota.GetDetalleNotasAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Detalle Notas");
                worksheet.Cell(3, 1).Value = "Id_Detalle";
                worksheet.Cell(3, 2).Value = "Obra";
                worksheet.Cell(3, 3).Value = "Proveedor";
                worksheet.Cell(3, 4).Value = "Material";
                worksheet.Cell(3, 5).Value = "Nota-Extra";
                worksheet.Cell(3, 6).Value = "Cantidad";
                worksheet.Cell(3, 7).Value = "Precio Unitario";
                worksheet.Cell(3, 8).Value = "Extra";

                worksheet.Style.Font.Bold = true;
                worksheet.Cells("A3:H3").Style.Fill.BackgroundColor = XLColor.AliceBlue;
                worksheet.Column("A").Width = 25;
                worksheet.Column("B").Width = 25;
                worksheet.Column("C").Width = 25;
                worksheet.Column("D").Width = 25;
                worksheet.Column("E").Width = 25;
                worksheet.Column("F").Width = 25;
                worksheet.Column("G").Width = 25;
                worksheet.Column("H").Width = 25;

                var encabezado = worksheet.Range("A1:H2").Merge();
                worksheet.Cell("A1").Value = "Constructora OGS";
                worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                encabezado.Style.Fill.BackgroundColor = XLColor.BlueBell;


                int index = 4;
                foreach (var item in data)
                {
                    worksheet.Cell(index, 1).Value = item.Id_Detalle;
                    worksheet.Cell(index, 2).Value = item.ObraP;
                    worksheet.Cell(index, 3).Value = item.Provee;
                    worksheet.Cell(index, 4).Value = item.MaterialM;
                    worksheet.Cell(index, 5).Value = item.NotaE;
                    worksheet.Cell(index, 6).Value = item.Cantidad;
                    worksheet.Cell(index, 7).Value = item.PrecioUnitario;
                    worksheet.Cell(index, 8).Value = item.Extra;
                    index++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    var strDate = DateTime.Now.ToString("yyyyMMdd");
                    string filename = string.Format($"DetalleNota{strDate}.xlsx");
                    return File(content, contentType, filename);
                }

            }
        }
    }
}
