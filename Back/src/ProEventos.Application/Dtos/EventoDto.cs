using ProEventos.Application.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProEvento.Application.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="O campo {0} é obrigatório"),
        StringLength(50, MinimumLength = 4,
             ErrorMessage = "Número de caractéres invalido")
            ]
        public string Local { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string? DataEvento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório"),
        MinLength(4, ErrorMessage = "O campo {0} necessida de no mínimo 4 caracteres"),
        MaxLength(50, ErrorMessage ="O campo {0} suporta no máximo 50 caracteres! Favor ajustar")]
        public string Tema { get; set; }

        [Display(Name ="Quantidade de pessoas"),
        Required(ErrorMessage = "O campo {0} é obrigatório"),
        Range(10,120000, ErrorMessage ="A quantidade de pessoas deverá ter entre 10 e 120.000 pessoas")]
        public int QuantidadePessoas { get; set; }
        
        [Required(ErrorMessage ="O campo {0} é obrigatório"),
        RegularExpression(@".*\.(gif|jpe?g|bmp|png)$", ErrorMessage ="Não é uma imagem com o formato válido. Formatos aceitos: gif, jpeg, bmp ou png")    
        ]
        public string ImagemURL { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório"),
            Phone(ErrorMessage = "O campo {0} está com o número invalido")]
        public string Telefone { get; set; }
        
        [Display(Name ="e-mail"),
        EmailAddress(ErrorMessage = "O endereço de email não é válido"),
        MinLength(4, ErrorMessage = "O campo {0} necessida de no mínimo 4 caracteres"),
        MaxLength(30, ErrorMessage = "O campo {0} suporta no máximo 30 caracteres! Favor ajustar"),
        Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Email { get; set; }

        //exemplo para campo que não será utilizado no banco de dados, só está aqui para regra de negócio
        /*[NotMapped]
        public int ContagemDias{ get; set; }
        */

        public IEnumerable<LoteDto> lotes { get; set; }
        public IEnumerable<RedeSocialDto> RedeSociais { get; set; }
        public IEnumerable<PalestranteDto> Palestrantes{ get; set; }
    }
}
