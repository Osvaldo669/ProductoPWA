using System;
namespace Entities
{
	public class Obra
	{
		public int Id_Obra { get; set; }
		public string? Nombre_Obra { get; set; }
		public string?  Direccion { get; set; }
		public DateTime FechaInicio { get; set; }
		public DateTime FechaFinal { get; set; }
		public string? Dueno { get; set; }
		public string? Responsable { get; set; }
		public string? Tel_resp { get; set; }
		public string? Correo_res { get; set; }
	}
}

