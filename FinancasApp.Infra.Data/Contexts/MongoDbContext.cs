using FinancasApp.Domain.Entities;
using FinancasApp.Infra.Data.Mappings;
using FinancasApp.Infra.Data.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancasApp.Infra.Data.Contexts
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        //Método construtor
        public MongoDbContext(MongoDbSettings settings)
        {
            //Conexão com o banco de dados
            var mongoUrl = new MongoUrl(settings.ConnectionString);
            var client = new MongoClient(mongoUrl);
            _database = client.GetDatabase(mongoUrl.DatabaseName);

            //Aplicar os mapeamentos
            MovimentacaoMap.Configure();
        }

        //Método para acessar a collection de movimentações
        //e programar os métodos de CRUD para a entidade
        public IMongoCollection<Movimentacao> Movimentacoes
        {
            get { return _database.GetCollection<Movimentacao>("movimentacoes"); }
        }
    }
}