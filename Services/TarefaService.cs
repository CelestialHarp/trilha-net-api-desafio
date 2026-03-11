using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Services
{
    public class TarefaService
    {
        private readonly OrganizadorContext _context;

        public TarefaService(OrganizadorContext context)
        {
            _context = context; 
        }

        public async Task<Tarefa> ObterPorId(int id)
        {
            return await _context.Tarefas.FirstOrDefaultAsync(x => x.Id == id);
        }
        
        public async Task<List<Tarefa>> ObterTodas()
        {
            return await _context.Tarefas.ToListAsync();
        }

        public async Task<List<Tarefa>> ObterPorTitulo(string titulo)
        {
            return await _context.Tarefas.Where(x => EF.Functions.ILike(x.Titulo, $"%{titulo}%")).ToListAsync();
        }

        public async Task<List<Tarefa>> ObterPorData(DateTime data)
        {
            return await _context.Tarefas.Where(x => x.Data.Date == data.Date).ToListAsync();
        }

        public async Task<List<Tarefa>> ObterPorStatus(EnumStatusTarefa status)
        {
            return await _context.Tarefas.Where(x => x.Status == status).ToListAsync();
        }

        public async Task<Tarefa> Criar(Tarefa tarefa)
        {
            await _context.Tarefas.AddAsync(tarefa);
            await _context.SaveChangesAsync();
            return tarefa;
        }

        public async Task<Tarefa> Atualizar(int id, Tarefa tarefaAtualizada)
        {
            var tarefaBanco = await _context.Tarefas.FirstOrDefaultAsync(x => x.Id == id);
            if (tarefaBanco == null)
            {
                return null;
            }

            tarefaBanco.Titulo = tarefaAtualizada.Titulo;
            tarefaBanco.Descricao = tarefaAtualizada.Descricao;
            tarefaBanco.Data = tarefaAtualizada.Data;
            tarefaBanco.Status = tarefaAtualizada.Status;

            _context.Tarefas.Update(tarefaBanco);
            await _context.SaveChangesAsync();
            return tarefaBanco;
        }

        public async Task<Tarefa> Deletar(int id)
        {

            var tarefaBanco = await _context.Tarefas.FirstOrDefaultAsync(x => x.Id == id);
            
            if (tarefaBanco == null)
            {
                return null;
            }

            _context.Tarefas.Remove(tarefaBanco);
            await _context.SaveChangesAsync();
            return tarefaBanco;
        }











    }
}