using FinancasApp.Domain.Entities;
using FinancasApp.Domain.Interfaces.Repositories;
using FinancasApp.Infra.Data.Contexts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancasApp.Infra.Data.Repositories
{
    public class MovimentacaoRepository(MongoDbContext context) : IMovimentacaoRepository
    {
        public void Add(Movimentacao movimentacao)
        {
            context.Movimentacoes.InsertOne(movimentacao);
        }

        public void Update(Movimentacao movimentacao)
        {
            var filter = Builders<Movimentacao>.Filter.Eq(m => m.Id, movimentacao.Id);
            context.Movimentacoes.ReplaceOne(filter, movimentacao);
        }

        public void Delete(Guid id)
        {
            var filter = Builders<Movimentacao>.Filter.Eq(m => m.Id, id);
            context.Movimentacoes.DeleteOne(filter);
        }

        public List<Movimentacao> GetByDatas(DateOnly dataMin, DateOnly dataMax)
        {
            var dtMin = dataMin.ToDateTime(new TimeOnly(0, 0));
            var dtMax = dataMax.ToDateTime(new TimeOnly(23, 59, 59));

            var filter = Builders<Movimentacao>.Filter.Gte(m => m.Data, dataMin)
                       & Builders<Movimentacao>.Filter.Lte(m => m.Data, dataMax);

            var sort = Builders<Movimentacao>.Sort.Ascending(m => m.Data);

            return context.Movimentacoes
                          .Find(filter)
                          .Sort(sort)
                          .ToList();
        }

        public Movimentacao? GetById(Guid id)
        {
            var filter = Builders<Movimentacao>.Filter.Eq(m => m.Id, id);
            return context.Movimentacoes.Find(filter).FirstOrDefault();
        }
    }
}