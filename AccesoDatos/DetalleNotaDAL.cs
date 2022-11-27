using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace AccesoDatos
{
	public class DetalleNotaDAL
	{
		private readonly string? CadenaConn = null;
		public DetalleNotaDAL()
		{
			ConnectionDB connectionDB = new ConnectionDB();
			CadenaConn = connectionDB.CadenaConexion;
		}

		public async Task<List<DetalleNotaInner>> GetDetalleNotasAsync()
		{
            List<DetalleNotaInner>? detalleNotas = null;

            SqlConnection conn = new SqlConnection(CadenaConn);
            try
            {
                SqlCommand commando = new SqlCommand();
                commando.CommandText = "SELECT D.id_Detalle, O.Nombre_Obra, P.RazonSoc, M.Nombre_Mat, N.Extra, D.Cantidad, D.PrecioUnitario, D.Extra FROM Detalle_Nota D " +
                    "JOIN Nota N ON D.Nota = N.id_Nota " +
                    "JOIN Proveedor P ON D.Prove = P.Id_Prove " +
                    "JOIN Material M ON D.Material = M.Id_Mate " +
                    "JOIN Obra O ON D.Obra = O.id_Obra";
                commando.CommandType = CommandType.Text;
                await conn.OpenAsync();
                commando.Connection = conn;
                using (var Reader = await commando.ExecuteReaderAsync())
                {
                    if (Reader.HasRows)
                    {
                        detalleNotas = new List<DetalleNotaInner>();
                        while (Reader.Read())
                        {
                            detalleNotas.Add(new DetalleNotaInner()
                            {
                                Id_Detalle = (int)Reader[0],
                                ObraP = Reader[1].ToString(),
                                Provee = Reader[2].ToString(),
                                MaterialM = Reader[3].ToString(),
                                NotaE = Reader[4].ToString(),
                                Cantidad = (int)Reader[5],
                                PrecioUnitario = (double)Reader[6],
                                Extra = Reader[7].ToString()
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


        public async Task<bool> InsertDN(DetalleNota detalleNota)
        {
            bool respuesta = false;

            SqlConnection conn = new SqlConnection(CadenaConn);

            try
            {
                SqlCommand commando = new SqlCommand();
                commando.CommandText = "INSERT INTO Detalle_Nota(Obra, Prove, Material, Nota, Cantidad, PrecioUnitario, Extra) VALUES (@Obra, @Prove,@Material ,@Nota, @Cantidad, @PrecioUnitario, @Extra);";
                commando.CommandType = CommandType.Text;
                commando.Parameters.AddWithValue("@Obra", detalleNota.ObraP);
                commando.Parameters.AddWithValue("@Prove", detalleNota.Provee);
                commando.Parameters.AddWithValue("@Material", detalleNota.MaterialM);
                commando.Parameters.AddWithValue("@Nota", detalleNota.NotaE);
                commando.Parameters.AddWithValue("@Cantidad", detalleNota.Cantidad);
                commando.Parameters.AddWithValue("@PrecioUnitario", detalleNota.PrecioUnitario);
                commando.Parameters.AddWithValue("@Extra", detalleNota.Extra);


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

