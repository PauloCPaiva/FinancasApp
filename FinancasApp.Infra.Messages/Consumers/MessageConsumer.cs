using FinancasApp.Domain.Entities;
using FinancasApp.Infra.Messages.Settings;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinancasApp.Infra.Messages.Consumers
{
    public class MessageConsumer(RabbitMQSettings settings, SmtpSettings smtpSettings) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Criando os parâmetro para conexão com o servidor do RabbitMQ
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

            //Acessando a fila
            await channel.QueueDeclareAsync(
                queue: settings.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            //consumidor (leitura da fila)
            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, args) =>
            {
                //ler cada movimentação gravado na fila
                var payload = Encoding.UTF8.GetString(args.Body.ToArray());
                var movimentacao = JsonSerializer.Deserialize<Movimentacao>(payload);

                try
                {
                    await EnviarEmailAsync(movimentacao);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }

                //remover a mensagem da fila
                await channel.BasicAckAsync(args.DeliveryTag, false);
            };

            //executando o consumer
            await channel.BasicConsumeAsync(
                queue: settings.QueueName,
                autoAck: false,
                consumer: consumer
            );

            //Mantendo o método em execução
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task EnviarEmailAsync(Movimentacao movimentacao)
        {
            using var smtp = new SmtpClient(smtpSettings.Host, smtpSettings.Port)
            {
                EnableSsl = smtpSettings.EnableSsl,
                Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password)
            };

            var tipoDescricao = movimentacao.Tipo == TipoMovimentacao.Receita ? "Receita" : "Despesa";

            var mensagem = new MailMessage
            {
                From = new MailAddress(smtpSettings.From),
                Subject = $"Nova movimentação cadastrada - {movimentacao.Nome}",
                Body = $@"
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                background-color: #f9f9f9;
                                color: #333;
                                padding: 20px;
                            }}
                            .container {{
                                background-color: #fff;
                                border-radius: 8px;
                                box-shadow: 0 2px 6px rgba(0,0,0,0.1);
                                padding: 20px;
                                max-width: 500px;
                                margin: auto;
                            }}
                            h2 {{
                                color: #0078D7;
                            }}
                            table {{
                                width: 100%;
                                border-collapse: collapse;
                                margin-top: 15px;
                            }}
                            th, td {{
                                padding: 8px 12px;
                                border-bottom: 1px solid #ddd;
                            }}
                            th {{
                                text-align: left;
                                background-color: #f1f1f1;
                            }}
                            .footer {{
                                margin-top: 20px;
                                font-size: 12px;
                                color: #777;
                                text-align: center;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h2>Nova movimentação cadastrada</h2>
                            <p>Uma nova movimentação foi registrada no sistema:</p>

                            <table>
                                <tr>
                                    <th>ID</th>
                                    <td>{movimentacao.Id}</td>
                                </tr>
                                <tr>
                                    <th>Nome</th>
                                    <td>{movimentacao.Nome}</td>
                                </tr>
                                <tr>
                                    <th>Tipo</th>
                                    <td>{tipoDescricao}</td>
                                </tr>
                                <tr>
                                    <th>Valor</th>
                                    <td>{movimentacao.Valor:C}</td>
                                </tr>
                                <tr>
                                    <th>Data</th>
                                    <td>{movimentacao.Data:dd/MM/yyyy}</td>
                                </tr>
                            </table>

                            <div class='footer'>
                                <p>FinançasApp - Controle financeiro automatizado</p>
                                <p>{DateTime.Now:dd/MM/yyyy HH:mm}</p>
                            </div>
                        </div>
                    </body>
                    </html>
                ",
                IsBodyHtml = true
            };

            mensagem.To.Add(smtpSettings.To);

            await smtp.SendMailAsync(mensagem);
        }
    }
}