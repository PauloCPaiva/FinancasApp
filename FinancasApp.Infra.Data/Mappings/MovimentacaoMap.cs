using FinancasApp.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancasApp.Infra.Data.Mappings
{
    public static class MovimentacaoMap
    {
        private static bool hasDone = false;

        public static void Configure()
        {
            if (hasDone) return;

            BsonClassMap.RegisterClassMap<Movimentacao>(map =>
            {
                map.AutoMap();

                //Nome da collection no banco de dados
                map.SetDiscriminator("movimentacoes");

                //Chave primária
                map.MapIdMember(m => m.Id)
                    .SetIdGenerator(GuidGenerator.Instance)
                    .SetSerializer(new GuidSerializer(BsonType.String));

                hasDone = true;
            });
        }
    }
}