using FinancasApp.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancasApp.Domain.Interfaces.Services
{
    public interface IMovimentacaoService
    {
        MovimentacaoResponse Criar(MovimentacaoRequest request);
        MovimentacaoResponse Alterar(Guid id, MovimentacaoRequest request);
        MovimentacaoResponse Excluir(Guid id);

        List<MovimentacaoResponse> ConsultarPorDatas(string dataMin, string dataMax);
        MovimentacaoResponse? ObterPorId(Guid id);
    }
}