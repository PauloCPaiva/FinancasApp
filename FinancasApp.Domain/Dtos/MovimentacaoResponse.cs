using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancasApp.Domain.Dtos
{
    /// <summary>
    /// Record para saída de dados dos serviços do dominio.
    /// </summary>
    public record MovimentacaoResponse(
        Guid Id,         //Id da movimentação
        string Nome,    //Nome da movimentação
        double Valor,   //Valor da movimentação
        string Data,    //Data da movimentação
        int Tipo        //Tipo da movimentação 
        )
    {
    }
}
