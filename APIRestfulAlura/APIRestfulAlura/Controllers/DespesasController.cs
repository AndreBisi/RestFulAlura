using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace APIRestfulAlura.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DespesasController : ControllerBase
    {

        [HttpGet(Name = "GetDespesas")]
        public IEnumerable<Despesas> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Despesas
            {
                Id = 1,
                Descricao = "Descrição",
                Valor = 0,
                DataReceita = DateTime.Now
            })
            .ToArray();
        }

        [HttpPost]
        public void Post([FromBody]Despesas despesa)
        {
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = DBAlura; User Id = postgres; Password = porcos128;"))
                {
                    //Abra a conexão com o PgSQL                  
                    pgsqlConnection.Open();
                    
                    string cmdInserir = String.Format("Insert Into tbDespesa values({0},'{1}',{2}, '{3}')", despesa.Id, despesa.Descricao, despesa.Valor, despesa.DataReceita.ToString("dd/MM/yyyy"));

                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection))
                    {
                        pgsqlcommand.ExecuteNonQuery();
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
