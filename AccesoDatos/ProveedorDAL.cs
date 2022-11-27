using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace AccesoDatos
{
	public class ProveedorDAL
	{
		private readonly string? CadenaConn = null;
		public ProveedorDAL()
		{
			ConnectionDB connectionDB = new ConnectionDB();
			CadenaConn = connectionDB.CadenaConexion;
		}

		public async Task<List<Proveedor>> GetProveedorsAsync()
		{
			List<Proveedor>? proveedors = null;

			using(SqlConnection connection = new SqlConnection(CadenaConn))
			{
				try
				{
					SqlCommand command = new SqlCommand();
					command.CommandText = "SELECT * FROM Proveedor";
					command.CommandType = System.Data.CommandType.Text;

					await connection.OpenAsync();
					command.Connection = connection;

					using(SqlDataReader Reader = await command.ExecuteReaderAsync())
					{
						if (Reader.HasRows)
						{
							proveedors = new List<Proveedor>();
							while (Reader.Read())
							{
								proveedors.Add(new Proveedor()
								{
									Id_Proveedor = (int)Reader[0],
									RazonSoc = (string)Reader[1],
									Agente = (string)Reader[2],
									Direccion = (string)Reader[3],
									Telefono = (string)Reader[4],
									Correo = (string)Reader[5],
									Tipo_Material=Reader[6].ToString(),
								});
							}

							await connection.CloseAsync();
						}
					}
				}
				catch(Exception ex)
				{
					proveedors = null;
					await connection.CloseAsync();
				}
			}
			return proveedors;
        }

        public async Task<bool> InsertProveedor(Proveedor proveedor)
        {
            bool respuesta = false;

            SqlConnection conn = new SqlConnection(CadenaConn);

            try
            {
                SqlCommand commando = new SqlCommand();
                commando.CommandText = "INSERT INTO Proveedor(RazonSoc,Agente,Direccion,Telefono,Correo,Tipo_Material)" +
					"VALUES (@RazonSoc, @Agente,@Direccion,@Telefono,@Correo,@Tipo_Material);";
                commando.CommandType = CommandType.Text;
                commando.Parameters.AddWithValue("@RazonSoc", proveedor.RazonSoc);
                commando.Parameters.AddWithValue("@Agente", proveedor.Agente);
                commando.Parameters.AddWithValue("@Direccion", proveedor.Direccion);
                commando.Parameters.AddWithValue("@Telefono", proveedor.Telefono);
                commando.Parameters.AddWithValue("@Correo", proveedor.Correo);
                commando.Parameters.AddWithValue("@Tipo_Material", proveedor.Tipo_Material);

                await conn.OpenAsync();
                commando.Connection = conn;

                int estado = await commando.ExecuteNonQueryAsync();
                if (estado > 0)
                {
                    respuesta = true;
                }
                await conn.CloseAsync();
            }
            catch (Exception)
            {
                respuesta = false;
            }
            return respuesta;

        }
    }
}

