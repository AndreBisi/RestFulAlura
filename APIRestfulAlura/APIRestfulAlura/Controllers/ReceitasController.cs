using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APIRestfulAlura.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReceitasController : ControllerBase
    {

        [HttpGet]
        public ActionResult<List<Receitas>> Get()
        {
            using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = DBAlura; User Id = postgres; Password = porcos128;"))
            {
                try
                {
                    //Abra a conexão com o PgSQL                  
                    pgsqlConnection.Open();

                    string cmdSequence = "select * from tbReceita order by receitaId";

                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdSequence, pgsqlConnection))
                    {
                        NpgsqlDataReader reader = pgsqlcommand.ExecuteReader();

                        var lista = new List<Receitas>();

                        while (reader.Read())
                        {
                            Receitas receita = new Receitas();
                            receita.Id = reader.GetInt32(0);
                            receita.Descricao = reader.GetString(1);
                            receita.Valor = reader.GetDecimal(2);
                            receita.Data = reader.GetDateTime(3);

                            lista.Add(receita);
                        }
                        return lista.ToList();
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

        [HttpGet("{Id}")]
        public IActionResult GetPorId(int Id)
        {
            using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = DBAlura; User Id = postgres; Password = porcos128;"))
            {
                try
                {
                    {
                        //Abra a conexão com o PgSQL                  
                        pgsqlConnection.Open();

                        string cmdSequence = "select * from tbReceita where receitaId = " + Id;

                        using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdSequence, pgsqlConnection))
                        {

                            NpgsqlDataReader reader = pgsqlcommand.ExecuteReader();

                            while (reader.Read())
                            {
                                Receitas receita = new Receitas();
                                receita.Id = reader.GetInt32(0);
                                receita.Descricao = reader.GetString(1);
                                receita.Valor = reader.GetDecimal(2);
                                receita.Data = reader.GetDateTime(3);
                                return Ok(receita);

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
                    pgsqlConnection.Close();
                }
            }
        }

        private int getCodigo()
        {

            using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = DBAlura; User Id = postgres; Password = porcos128;"))
            {
                try
                {
                    //Abra a conexão com o PgSQL                  
                    pgsqlConnection.Open();

                    string cmdSequence = "select nextval('sqreceita');";

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

        private bool validaDados(Receitas receita)
        {
            using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = DBAlura; User Id = postgres; Password = porcos128;"))
            {
                try
                {
                    //Abra a conexão com o PgSQL                  
                    pgsqlConnection.Open();

                    string cmdSequence = String.Format("select * from tbReceita where receitaDesc = '{0}' and   extract(month from receitaData ) = {1} and   extract(year from receitaData ) = {2} and receitaId <> {3}", receita.Descricao, receita.Data.Month, receita.Data.Year, receita.Id);

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


        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = DBAlura; User Id = postgres; Password = porcos128;"))
            {
                try
                {

                    //Abra a conexão com o PgSQL                  
                    pgsqlConnection.Open();

                    string cmdInserir = String.Format("delete from tbReceita where receitaId = {0}", Id);

                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection))
                    {
                        pgsqlcommand.ExecuteNonQuery();
                        return NoContent();
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

        [HttpPost]
        public IActionResult Post([FromBody] Receitas receita)
        {

            if (validaDados(receita))
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = DBAlura; User Id = postgres; Password = porcos128;"))
                {
                    try
                    {

                        //Abra a conexão com o PgSQL                  
                        pgsqlConnection.Open();

                        int codigo = getCodigo();

                        System.Console.WriteLine("codigo" + codigo);

                        string cmdInserir = String.Format("Insert Into tbReceita values({0},'{1}',{2},'{3}')", codigo, receita.Descricao, receita.Valor, receita.Data.ToString("dd/MM/yyyy"));

                        using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection))
                        {
                            pgsqlcommand.ExecuteNonQuery();
                            return Ok(receita);
                            //return CreatedAtAction(nameof(despesa), new { Id = codigo }, despesa);
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
            else
            {
                throw new Exception("Já existe receita cadastrada para este mês");
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] Receitas receita)
        {
            if (validaDados(receita))
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = DBAlura; User Id = postgres; Password = porcos128;"))
                {
                    try
                    {

                        //Abra a conexão com o PgSQL                  
                        pgsqlConnection.Open();

                        string cmdInserir = String.Format("update tbReceita set receitaDesc = '{0}', receitaValor = {1}, receitaData = '{2}' where receitaId = {3}", receita.Descricao, receita.Valor, receita.Data.ToString("dd/MM/yyyy"), receita.Id);

                        using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection))
                        {
                            pgsqlcommand.ExecuteNonQuery();

                            return Ok(receita);
                            //return CreatedAtAction(nameof(receita), new { Id = receita.Id }, receita);
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
            else
            {
                throw new Exception("Já existe despesa cadastrada para este mês");
            }
        }
    }
}
