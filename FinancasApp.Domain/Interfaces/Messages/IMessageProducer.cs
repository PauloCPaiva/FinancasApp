using FinancasApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancasApp.Domain.Interfaces.Messages
{
    public interface IMessageProducer
    {
        /// <summary>
        /// Método para enviar um registro de movimentação para a fila da mensageria
        /// </summary>
        void Send(Movimentacao movimentacao);
    }
}
