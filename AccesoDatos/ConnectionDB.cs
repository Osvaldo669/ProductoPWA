using System;
namespace AccesoDatos
{
	public class ConnectionDB
	{
		private readonly string Server = "20.110.177.121,1433";
		private readonly string User = "osva";
		private readonly string NameDB = "PagosObras2022";
		private readonly string Pass = "admin1234";
		public string? CadenaConexion = "";


		public ConnectionDB()
		{
			CadenaConexion = "Data Source = "+Server+";" +
				" Initial Catalog = "+NameDB+";" +
				" User ID = "+User+";" +
				" Password = "+Pass;

        }
	}
}

