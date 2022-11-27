using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace AccesoDatos
{
	public class NotaDAL
	{
        private readonly string? CadenaConn = null;

        public NotaDAL()
		{
			ConnectionDB connection = new ConnectionDB();
			CadenaConn = connection.CadenaConexion;
		}

		public async Task<List<Nota>> GetNotas()
		{
			List<Nota>? notas = null;

			SqlConnection conn = new SqlConnection(CadenaConn);
			try
			{
				SqlCommand commando = new SqlCommand();
				commando.CommandText = "Select * From Nota";
				commando.CommandType = CommandType.Text;
				await conn.OpenAsync();
				commando.Connection = conn;
				using(var Reader = await commando.ExecuteReaderAsync())
				{
					if (Reader.HasRows)
					{
						notas = new List<Nota>();
						while(Reader.Read())
						{
							notas.Add(new Nota()
							{
								Id_Nota = (int)Reader[0],
								Fecha = (DateTime)Reader[1],
								Extra = Reader[2].ToString()
							});
						}

						await conn.CloseAsync();
					}
				}
				
			}
			catch(Exception ex)
			{
				var error = ex.Message;
				await conn.CloseAsync();
			}
			
			return notas;
		}


        public async Task<bool> InsertNote(Nota note)
        {
            bool respuesta = false;

            SqlConnection conn = new SqlConnection(CadenaConn);

            try
            {
                SqlCommand commando = new SqlCommand();
                commando.CommandText = "INSERT INTO Nota(Fecha, Extra) VALUES (@Fecha, @Extra);";
                commando.CommandType = CommandType.Text;
                commando.Parameters.AddWithValue("@Fecha", note.Fecha);
                commando.Parameters.AddWithValue("@Extra", note.Extra);

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

        public async Task<List<BusquedaEspecial>> ConsultaEsp(int obra, DateTime fechaI, DateTime fechaF)
        {
            List<BusquedaEspecial>? detalleNotas = null;

            SqlConnection conn = new SqlConnection(CadenaConn);
            try
            {
                SqlCommand commando = new SqlCommand();
                commando.CommandText = @"SELECT N.id_Nota, N.Fecha, N.Extra, D.Cantidad, D.PrecioUnitario FROM Nota N 
                                        INNER JOIN Detalle_Nota D ON n.id_Nota = D.Nota
                                        WHERE D.Obra = @Obra AND N.Fecha >= @FechaI AND N.Fecha <= @FechaF";

                commando.Parameters.AddWithValue("@Obra", obra);
                commando.Parameters.AddWithValue("@FechaI", fechaI);
                commando.Parameters.AddWithValue("@FechaF", fechaF);
                commando.CommandType = CommandType.Text;

                await conn.OpenAsync();
                commando.Connection = conn;
                using (var Reader = await commando.ExecuteReaderAsync())
                {
                    if (Reader.HasRows)
                    {
                        detalleNotas = new List<BusquedaEspecial>();
                        while (Reader.Read())
                        {
                            detalleNotas.Add(new BusquedaEspecial()
                            {
                                Id = (int)Reader[0],
                                Fecha = (DateTime)Reader[1],
                                Extra = Reader[2].ToString(),
                                Cantidad = (int)Reader[3],
                                PrecioUnitario = (double)Reader[4],
                            });
                        }

                        await conn.CloseAsync();
                    }
                }

            }
            catch (Exception)
            {
                detalleNotas = null;
                await conn.CloseAsync();
            }
            return detalleNotas;

        }

        public async Task<List<BusquedaEspecial>> ConsultaEsp2(int obra)
        {
            List<BusquedaEspecial>? detalleNotas = null;

            SqlConnection conn = new SqlConnection(CadenaConn);
            try
            {
                SqlCommand commando = new SqlCommand();
                commando.CommandText = @"SELECT N.id_Nota, N.Fecha, N.Extra, D.Cantidad, D.PrecioUnitario FROM Nota N 
                                        INNER JOIN Detalle_Nota D ON n.id_Nota = D.Nota
                                        WHERE D.Obra = @Obra ";

                commando.Parameters.AddWithValue("@Obra", obra);
                commando.CommandType = CommandType.Text;

                await conn.OpenAsync();
                commando.Connection = conn;
                using (var Reader = await commando.ExecuteReaderAsync())
                {
                    if (Reader.HasRows)
                    {
                        detalleNotas = new List<BusquedaEspecial>();
                        while (Reader.Read())
                        {
                            detalleNotas.Add(new BusquedaEspecial()
                            {
                                Id = (int)Reader[0],
                                Fecha = (DateTime)Reader[1],
                                Extra = Reader[2].ToString(),
                                Cantidad = (int)Reader[3],
                                PrecioUnitario = (double)Reader[4],
                            });
                        }

                        await conn.CloseAsync();
                    }
                }

            }
            catch (Exception)
            {
                detalleNotas = null;
                await conn.CloseAsync();
            }
            return detalleNotas;

        }

    }
}

