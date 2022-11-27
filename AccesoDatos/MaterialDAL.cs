using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace AccesoDatos
{
	public class MaterialDAL
	{
		private readonly string? CadenaConn = null;
		public MaterialDAL()
		{
			ConnectionDB connectionDB = new ConnectionDB();
			CadenaConn = connectionDB.CadenaConexion;
		}

		public async Task<List<Material>> GetMaterialsAsync()
		{
			List<Material>? materials = null;

            SqlConnection conn = new SqlConnection(CadenaConn);
            try
            {
                SqlCommand commando = new SqlCommand();
                commando.CommandText = "Select * From Material";
                commando.CommandType = CommandType.Text;
                await conn.OpenAsync();
                commando.Connection = conn;
                using (var Reader = await commando.ExecuteReaderAsync())
                {
                    if (Reader.HasRows)
                    {
                        materials = new List<Material>();
                        while (Reader.Read())
                        {
                            materials.Add(new Material()
                            {
                                Id_Material = (int)Reader[0],
                                Nombre_Mat = Reader[1].ToString(),
                                Marca = (string)Reader[2],
                                Categoria = (string)Reader[3],
                                UnidadMedida = (string)Reader[4]

                            });
                        }

                        await conn.CloseAsync();
                    }
                }

            }
            catch (Exception)
            {
                materials = null;
                await conn.CloseAsync();
            }


            return materials;
        }
        

        public async Task<bool> InsertMaterial(Material material)
        {
            bool respuesta = false;

            SqlConnection conn = new SqlConnection(CadenaConn);

            try
            {
                SqlCommand commando = new SqlCommand();
                commando.CommandText = "INSERT INTO Material(Nombre_Mat, Marca, Categoria, UnidadMedida) VALUES (@Nombre_Mat, @Categoria,@Marca ,@UnidadMedida);";
                commando.CommandType = CommandType.Text;
                commando.Parameters.AddWithValue("@Nombre_Mat", material.Nombre_Mat);
                commando.Parameters.AddWithValue("@Categoria", material.Categoria);
                commando.Parameters.AddWithValue("@Marca", material.Marca);
                commando.Parameters.AddWithValue("@UnidadMedida", material.UnidadMedida);

                await conn.OpenAsync();
                commando.Connection = conn;
                int estado = await commando.ExecuteNonQueryAsync();
                if (estado > 0)
                {
                    respuesta = true;
                }
                await conn.CloseAsync();
            }
            catch(Exception)
            {
                respuesta = false;
            }
            return respuesta;

        }
	}
}

