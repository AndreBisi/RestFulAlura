using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace APIRestfulAlura.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReceitasController : ControllerBase
    {

        [HttpGet(Name = "GetReceitas")]
        public IEnumerable<Receitas> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Receitas
            {
                Id = 1,
                Descricao = "Descrição",
                Valor = 0,
                Data = DateTime.Now
            })
            .ToArray();
        }

        private String getCodigo()
        {
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = DBAlura; User Id = postgres; Password = porcos128;"))
                {
                    //Abra a conexão com o PgSQL                  
                    pgsqlConnection.Open();

                    string cmdSequence = "select nextval('sqReceita');";

                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdSequence, pgsqlConnection))
                    {
                         NpgsqlDataReader reader = pgsqlcommand.ExecuteReader();
                         return reader["nextval"].ToString() ;
                        
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //pgsqlConnection.Close();
            }
        }
        [HttpPost]
        public IActionResult Post([FromBody] Receitas receita)
        {
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = DBAlura; User Id = postgres; Password = porcos128;"))
                {
                    //Abra a conexão com o PgSQL                  
                    pgsqlConnection.Open();

                    int codigo = Int32.Parse(getCodigo());

                    string cmdInserir = String.Format("Insert Into tbReceita values({0},'{1}',{2}, '{3}')", codigo, receita.Descricao, receita.Valor, receita.Data.ToString("dd/MM/yyyy"));

                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection))
                    {
                        pgsqlcommand.ExecuteNonQuery();
                        return CreatedAtAction(nameof(receita), new { Id = receita.Id }, receita);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //pgsqlConnection.Close();
            }

        }
    }

}
