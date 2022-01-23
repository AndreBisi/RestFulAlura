using System;
using System.ComponentModel.DataAnnotations;

namespace APIRestfulAlura
{

    public class Despesas
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O valor deve ser informado")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "A data deve ser informada")]
        public DateTime Data { get; set; }

    }
}
