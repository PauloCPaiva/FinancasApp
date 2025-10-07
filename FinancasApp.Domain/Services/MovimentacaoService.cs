using FinancasApp.Domain.Dtos;
using FinancasApp.Domain.Entities;
using FinancasApp.Domain.Interfaces.Messages;
using FinancasApp.Domain.Interfaces.Repositories;
using FinancasApp.Domain.Interfaces.Services;
using FinancasApp.Domain.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinancasApp.Domain.Services
{
    public class MovimentacaoService(IMovimentacaoRepository repository, IMessageProducer message) : IMovimentacaoService
    {
        public MovimentacaoResponse Criar(MovimentacaoRequest request)
        {
            var movimentacao = new Movimentacao
            {
                Nome = request.Nome,
                Valor = (decimal)request.Valor,
                Data = DateOnly.Parse(request.Data),
                Tipo = (TipoMovimentacao)request.Tipo
            };

            var validator = new MovimentacaoValidator();
            var result = validator.Validate(movimentacao);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            repository.Add(movimentacao);

            message.Send(movimentacao);

            return ToResponse(movimentacao);
        }

        public MovimentacaoResponse Alterar(Guid id, MovimentacaoRequest request)
        {
            var movimentacao = repository.GetById(id);

            if (movimentacao == null)
                throw new ApplicationException("Movimentação não encontrada para edição.");

            movimentacao.Nome = request.Nome;
            movimentacao.Valor = (decimal)request.Valor;
            movimentacao.Data = DateOnly.Parse(request.Data);
            movimentacao.Tipo = (TipoMovimentacao)request.Tipo;

            var validator = new MovimentacaoValidator();
            var result = validator.Validate(movimentacao);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            repository.Update(movimentacao);

            return ToResponse(movimentacao);
        }

        public MovimentacaoResponse Excluir(Guid id)
        {
            var movimentacao = repository.GetById(id);

            if (movimentacao == null)
                throw new ApplicationException("Movimentação não encontrada para exclusão.");

            repository.Delete(id);

            return ToResponse(movimentacao);
        }

        public List<MovimentacaoResponse> ConsultarPorDatas(string dataMin, string dataMax)
        {
            var dtMin = DateOnly.Parse(dataMin);
            var dtMax = DateOnly.Parse(dataMax);

            var movimentacoes = repository.GetByDatas(dtMin, dtMax);

            return movimentacoes.Select(ToResponse).ToList();
        }

        public MovimentacaoResponse? ObterPorId(Guid id)
        {
            var movimentacao = repository.GetById(id);
            return movimentacao != null ? ToResponse(movimentacao) : null;
        }

        private MovimentacaoResponse ToResponse(Movimentacao movimentacao)
        {
            return new MovimentacaoResponse(
                    movimentacao.Id,
                    movimentacao.Nome,
                    (double)movimentacao.Valor,
                    movimentacao.Data.ToString("dd/MM/yyyy"),
                    (int)movimentacao.Tipo
                );
        }
    }
}