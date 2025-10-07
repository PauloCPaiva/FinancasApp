using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancasApp.Domain.Dtos
{
    /// <summary>
    /// Record para entrada de dados nos serviços do dominio.
    /// </summary>
    public record MovimentacaoRequest(
        string Nome,    //Nome da movimentação
        double Valor,   //Valor da movimentação
        string Data,    //Data da movimentação
        int Tipo        //Tipo da movimentação 
        )
    {

    }
}
