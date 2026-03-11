using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrilhaApiDesafio.Models;
//Exemplod e como seria o DTO, que me tomaria várias linhas com atribuições desnecessáriamente, já que esse código n vai ser realmente usado. Provavelmente depois, quando n tiver absolutamente nada pra fazer, eu implemente.
namespace TrilhaApiDesafio.DTOs
{
    public record CreateTarefaDTO
    (
        int Id,
        string Titulo,
        string Descricao,
        DateTime Data,
        EnumStatusTarefa Status
    );



}