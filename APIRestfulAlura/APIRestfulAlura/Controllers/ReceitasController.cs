using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                DataReceita = DateTime.Now
            })
            .ToArray();
        }
    }
}
