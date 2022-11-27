using System;
namespace Entities
{
	public class DetalleNota
	{
		public int Id_Detalle { get; set; }
		public int ObraP { get; set; }
		public int Provee { get; set; }
		public int MaterialM { get; set; }
		public int NotaE { get; set; }
		public int Cantidad { get; set; }
		public double PrecioUnitario { get; set; }
		public string? Extra { get; set; }
	}

    public class DetalleNotaInner
    {
        public int Id_Detalle { get; set; }
        public string? ObraP { get; set; }
        public string? Provee { get; set; }
        public string? MaterialM { get; set; }
        public string? NotaE { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }
        public string? Extra { get; set; }
    }
}

