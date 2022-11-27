using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace AccesoDatos
{
	public class ObrasDAL
	{
		private readonly string? CadenaCon = null;

		public ObrasDAL()
		{
			ConnectionDB connectionDB = new ConnectionDB();
			CadenaCon = connectionDB.CadenaConexion;
		}

		public async Task<List<Obra>> GetObrasAsync()
		{
			List<Obra>? obras = null;

			using(SqlConnection connection = new SqlConnection(CadenaCon))
			{
				try
				{
					SqlCommand command = new SqlCommand();
					command.CommandText = "SELECT * FROM Obra";
					command.CommandType = System.Data.CommandType.Text;
					await connection.OpenAsync();
					command.Connection = connection;

                    using (var Reader = await command.ExecuteReaderAsync())
                    {
                        if (Reader.HasRows)
                        {
                            obras = new List<Obra>();
                            while (Reader.Read())
                            {
                                obras.Add(new Obra()
                                {
                                    Id_Obra = (int)Reader[0],
									Nombre_Obra = (string)Reader[1],
									Direccion = (string)Reader[2],
									FechaInicio = (DateTime)Reader[3],
									FechaFinal = (DateTime)Reader[4],
									Dueno = (string)Reader[5],
									Responsable = (string)Reader[6],
									Tel_resp = (string)Reader[7],
									Correo_res = (string)Reader[8]
                                });
                            }
                            await connection.CloseAsync();
                        }
                    }
                }
				catch(Exception ex)
				{
					obras = null;
					await connection.CloseAsync();
				}
			}

			return obras;
				 
		}


        public async Task<bool> InsertObra(Obra obra)
        {
            bool respuesta = false;

            SqlConnection conn = new SqlConnection(CadenaCon);

            try
            {
                SqlCommand commando = new SqlCommand();
                commando.CommandText = "INSERT INTO Obra(Nombre_Obra,Direccion,Fecha_ini,fecha_fin,Dueño,Responsable,Tel_resp,Correo_res) " +
                    "VALUES (@Nombre_Obra, @Direccion,@Fecha_ini,@fecha_fin,@Dueño,@Responsable,@Tel_resp,@Correo_res);";
                commando.CommandType = CommandType.Text;
                commando.Parameters.AddWithValue("@Nombre_Obra", obra.Nombre_Obra);
                commando.Parameters.AddWithValue("@Direccion", obra.Direccion);
                commando.Parameters.AddWithValue("@Fecha_ini", obra.FechaInicio);
                commando.Parameters.AddWithValue("@fecha_fin", obra.FechaFinal);
                commando.Parameters.AddWithValue("@Dueño", obra.Dueno);
                commando.Parameters.AddWithValue("@Responsable", obra.Responsable);
                commando.Parameters.AddWithValue("@Tel_resp", obra.Tel_resp);
                commando.Parameters.AddWithValue("@Correo_res", obra.Correo_res);

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

