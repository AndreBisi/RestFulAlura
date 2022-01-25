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

        [HttpGet]
        public ActionResult<List<Despesas>> GetLista()
        {
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = DBAlura; User Id = postgres; Password = porcos128;"))
                {
                    //Abra a conexão com o PgSQL                  
                    pgsqlConnection.Open();

                    string cmdSequence = "select * from tbDespesa order by despesaId";

                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdSequence, pgsqlConnection))
                    {
                        NpgsqlDataReader reader = pgsqlcommand.ExecuteReader();

                        var lista = new List<Despesas>();

                        while (reader.Read())
                        {
                            Despesas despesa = new Despesas();
                            despesa.Id = reader.GetInt32(0);
                            despesa.Descricao = reader.GetString(1);
                            despesa.Valor = reader.GetDecimal(2);
                            despesa.Data = reader.GetDateTime(3);

                            lista.Add(despesa);
                        }
                        return lista.ToList();
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

        [HttpGet("{Id}")]
        public IActionResult GetPorId(int Id)
        {
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = DBAlura; User Id = postgres; Password = porcos128;"))
                {
                    //Abra a conexão com o PgSQL                  
                    pgsqlConnection.Open();

                    string cmdSequence = "select * from tbDespesa where despesaId = " + Id;

                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdSequence, pgsqlConnection))
                    {

                        NpgsqlDataReader reader = pgsqlcommand.ExecuteReader();

                        while (reader.Read())
                        {
                            Despesas despesa = new Despesas();
                            despesa.Id = reader.GetInt32(0);
                            despesa.Descricao = reader.GetString(1);
                            despesa.Valor = reader.GetDecimal(2);
                            despesa.Data = reader.GetDateTime(3);
                            return Ok(despesa);

                        }
                        return NotFound();
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
        private bool validaDados(Despesas despesa)
        {
            using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = DBAlura; User Id = postgres; Password = porcos128;"))
            {
                try
                {
                    //Abra a conexão com o PgSQL                  
                    pgsqlConnection.Open();

                    string cmdSequence = String.Format("select * from tbdespesa where despesaDesc = '{0}' and   extract(month from despesaData ) = {1} and   extract(year from despesaData ) = {2} and despesaId <> {3}", despesa.Descricao, despesa.Data.Month, despesa.Data.Year, despesa.Id);

                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdSequence, pgsqlConnection))
                    {

                        NpgsqlDataReader reader = pgsqlcommand.ExecuteReader();

                        if (reader.HasRows)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
                    pgsqlConnection.Close();
                }

            }
        }

        private int getCodigo()
        {
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = DBAlura; User Id = postgres; Password = porcos128;"))
                {
                    //Abra a conexão com o PgSQL                  
                    pgsqlConnection.Open();

                    string cmdSequence = "select nextval('sqdespesa');";

                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdSequence, pgsqlConnection))
                    {

                        NpgsqlDataReader reader = pgsqlcommand.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                return reader.GetInt32(0);
                            }
                            return 1;
                        }
                        else
                        {
                            return 1;
                        }
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


        [HttpPut]
        public IActionResult Put([FromBody] Despesas despesa)
        {
            if (validaDados(despesa))
            {
                try
                {
                    using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = DBAlura; User Id = postgres; Password = porcos128;"))
                    {

                        //Abra a conexão com o PgSQL                  
                        pgsqlConnection.Open();

                        string cmdInserir = String.Format("update tbDespesa set despesaDesc = '{0}', despesaValor = {1}, despesaData = '{2}' where despesaId = {3}", despesa.Descricao, despesa.Valor, despesa.Data.ToString("dd/MM/yyyy"), despesa.Id);

                        using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection))
                        {
                            pgsqlcommand.ExecuteNonQuery();
                            return CreatedAtAction(nameof(despesa), new { Id = despesa.Id }, despesa);
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
            else
            {
                throw new Exception("Já existe despesa cadastrada para este mês");
            }
        }

        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = DBAlura; User Id = postgres; Password = porcos128;"))
                {

                    //Abra a conexão com o PgSQL                  
                    pgsqlConnection.Open();

                    string cmdInserir = String.Format("delete from tbDespesa where despesaId = {0}", Id);

                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection))
                    {
                        pgsqlcommand.ExecuteNonQuery();
                        return NoContent();
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
        public IActionResult Post([FromBody] Despesas despesa)
        {

            if (validaDados(despesa))
            {
                try
                {
                    using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = DBAlura; User Id = postgres; Password = porcos128;"))
                    {

                        //Abra a conexão com o PgSQL                  
                        pgsqlConnection.Open();

                        int codigo = getCodigo();

                        System.Console.WriteLine("codigo" + codigo);

                        string cmdInserir = String.Format("Insert Into tbDespesa values({0},'{1}',{2},'{3}')", codigo, despesa.Descricao, despesa.Valor, despesa.Data.ToString("dd/MM/yyyy"));

                        using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection))
                        {
                            pgsqlcommand.ExecuteNonQuery();
                            return Ok(despesa);
                            //return CreatedAtAction(nameof(despesa), new { Id = codigo }, despesa);
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
            else
            {
                throw new Exception("Já existe despesa cadastrada para este mês");
            }
        }
    }
}