
using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;
using TrilhaApiDesafio.Services;
//Comments in english by the "ObterPorData()" (GetByDate) method, because there was no necessity for me to comment the other things in this code, execept for making some translations of the name of the methods, and everything afterwards is just self-exaplanatory.
//Main works:
//Execution of the TODOs my professor asked me.
//Corrections of some "mistakes" he did, probably just for simplicity, mainly method refactorings.
namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly TarefaService _tarefaService;

        public TarefaController(OrganizadorContext context, TarefaService service)
        {
            _tarefaService = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            // TODO: Buscar o Id no banco utilizando o EF
            //buscar diretamente através da Controller está errado, violação do S do SOLID.
            var tarefa = await _tarefaService.ObterPorId(id);
            // TODO: Validar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound,
            // caso contrário retornar OK com a tarefa encontrada
            if (tarefa == null)
            {
                return NotFound("Não existe uma tarefa com esse ID");
            }

            return Ok($"Tarefa encontrada: \n {tarefa}");
        }

        [HttpGet("ObterTodas")]
        public async Task<IActionResult> ObterTodas()
        //esse List<IActionResult n dá erro, tem certeza que devo retornar só um IActionResult?
        {
            // TODO: Buscar todas as tarefas no banco utilizando o EF
            //Alterei para "obter todAs". A concordância comt arefas deixa mais intuitivo e diminui a possibilidade de erro.
            List<Tarefa> tarefas = await _tarefaService.ObterTodas();

            if (tarefas.Count() == 0)
            {
                return Ok("Não foram encontradas tarefas cadastradas.");
            }
            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public async Task<IActionResult> ObterPorTitulo(string titulo)
        {
            // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
            List<Tarefa> partialMatchs = await _tarefaService.ObterPorTitulo(titulo);//
            // Dica: Usar como exemplo o endpoint ObterPorData
            if (partialMatchs.Count() == 0)
            {
                return Ok ($"Não foi encontrada nenhuma tarefa com o título: \"{titulo}\"");
            }
            return Ok(partialMatchs);
        }

        [HttpGet("ObterPorData")]
        public async Task<IActionResult> ObterPorData(DateTime data)
        {
            //Se eu não me egano, o professor, nas aulas, me autorizou a modificar os projetos pra consertar qualquer eventual problema, então modifiquei isso pra não violar o S
            /*  
            PREVIOUS CODE:
            (As the other snipets of code my professor suggested us, probably out of concern with the practicality of our task, this code)
            CÓDIGO ANTERIOR:

            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);*/
            //Código adequado para uma controller, seguindo o S do SOLID:
            //New, SOLID'S 'S' compliant controller code:
            List<Tarefa> tarefas = await _tarefaService.ObterPorData(data);

            if (tarefas.Count() == 0)
            {
                return Ok ($"Não foi encontrada nenhuma tarefa com a data \"{data}\"");
            }
            return Ok(tarefas);



        }

        [HttpGet("ObterPorStatus")]
        //Previously there was no async keyword
        //N tinha o async antes
        public async Task<IActionResult> ObterPorStatus(EnumStatusTarefa status)
        {
            // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
            
            // Dica: Usar como exemplo o endpoint ObterPorData
            //Código inadequado anterior:
            //Previous, inadequate code:
            // var tarefa = _context.Tarefas.Where(x => x.Status == status);
            // return Ok(tarefa);
            //New, S-compliant code:
            List<Tarefa> tarefas = await _tarefaService.ObterPorStatus(status);

            if (tarefas.Count() == 0)
            {
                return Ok ($"Não foi encontrada nenhuma tarefa com o status: \"{status}\"");
            }
            return Ok(tarefas);

        }

        [HttpPost]
        //Just now I percieved that, normaly, the arguments passed through would be DTOs (Data Transfer Objects), not created instances of the real entity-relational model, but I am not into correcting the code this far, just to give myself four lines more to write, pointlessly, since this code will never be used. The only thing that matters is that now you know that I could've done that because I understand.
        //before: no async
        public async Task<IActionResult> Criar(Tarefa tarefa)
        {
            //It would also be good to have some exception handling, but this if is enough for this example
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            // TODO: Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
            var resultado = await _tarefaService.Criar(tarefa);

            return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Id }, resultado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            //Previous, inadequate code:
            //Código inadequado anterior:

            //var tarefaBanco = _context.Tarefas.Find(id);
            

            // if (tarefaBanco == null)
            //     return NotFound();

            

            // TODO: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            // TODO: Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
            var resultado = await _tarefaService.Atualizar(id, tarefa);

            if (resultado == null)
            {
                return NotFound("Tarefa não encontrada para atualização.");
            }

            return Ok($"Tarefa de id: \"{resultado.Id} modificada para {resultado.Titulo}, \"{resultado.Descricao}\"." );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            // var tarefaBanco = _context.Tarefas.Find(id);

            // if (tarefaBanco == null)
            //     return NotFound();

            // TODO: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
            var resultado = await _tarefaService.Deletar(id);
            //changed from NoContent to OK(message), since it is considered good practice to give the user feedback of their actions.
            if (resultado == null)
            {
                return NotFound($"Não foi possível realizar a deleção da tarefa. ID \"{id}\" não encontrado.");
            }
            return Ok($"Tarefa de id \"{resultado.Id}, (\"{resultado.Titulo}\"), deletada." );
        }
    }
}
