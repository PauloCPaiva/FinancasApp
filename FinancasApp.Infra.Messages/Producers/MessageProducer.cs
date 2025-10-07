using FinancasApp.Domain.Entities;
using FinancasApp.Domain.Interfaces.Messages;
using FinancasApp.Infra.Messages.Settings;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinancasApp.Infra.Messages.Producers
{
    public class MessageProducer(RabbitMQSettings settings) : IMessageProducer
    {
        public void Send(Movimentacao movimentacao)
        {
            Task.Run(async () =>
            {
                //Criando os parâmetro para conexão com a fila do RabbitMQ
                var factory = new ConnectionFactory
                {
                    HostName = settings.HostName,
                    Port = settings.Port,
                    UserName = settings.UserName,
                    Password = settings.Password,
                    VirtualHost = settings.VirtualHost
                };

                //Fazendo a conexão..
                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                //Criando a fila caso ela não exista
                await channel.QueueDeclareAsync(
                    queue: settings.QueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                //Serializando em JSON os dados que serão gravados na fila
                var payload = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(movimentacao));

                //Gravar os dados na fila
                await channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: settings.QueueName,
                    body: payload
                );
            });
        }
    }
}