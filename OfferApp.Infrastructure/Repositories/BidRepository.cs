using Dapper;
using OfferApp.Core.Entities;
using OfferApp.Core.Repositories;
using System.Data;
using System.Reflection;

namespace OfferApp.Infrastructure.Repositories
{
    internal sealed class BidRepository : IRepository<Bid>
    {
        private readonly IDbConnection _dbConnection;

        public BidRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> Add(Bid entity)
        {
            var id = await _dbConnection.QuerySingleAsync<int>("""
                INSERT INTO bids (Id, Name, Description, Created, Count, FirstPrice, Published) 
                    VALUES (@Id, @Name, @Description, @Created, @Count, @FirstPrice, @Published);
                SELECT LAST_INSERT_ID();
                """, entity);
            SetId(entity, id);
            return id;
        }

        public Task Delete(Bid entity)
        {
            return _dbConnection.ExecuteAsync("""
                DELETE FROM bids WHERE Id = @Id
                """, new { entity.Id });
        }

        public Task<Bid?> Get(int id)
        {
            return _dbConnection.QuerySingleOrDefaultAsync<Bid?>(
                """
                SELECT Id, Name, Description, Created, Updated, Count, FirstPrice, LastPrice, Published 
                FROM bids 
                WHERE Id = @Id
                """, new { Id = id });
        }

        public async Task<IReadOnlyList<Bid>> GetAll()
        {
            return (await _dbConnection.QueryAsync<Bid>(
                """
                SELECT Id, Name, Description, Created, Updated, Count, FirstPrice, LastPrice, Published 
                FROM bids
                """)).ToList();
        }

        public async Task<bool> Update(Bid entity)
        {
            return (await _dbConnection.ExecuteAsync("""
                UPDATE bids SET Name = @Name, Description = @Description, 
                Created = @Created, Updated = @Updated, Count = @Count, 
                FirstPrice = @FirstPrice, LastPrice = @LastPrice, Published = @Published 
                WHERE Id = @Id
                """, entity)) > 0;
        }

        private static void SetId(Bid bid, int id)
        {
            var field = typeof(Bid).BaseType?.GetField($"<{nameof(BaseEntity.Id)}>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(bid, id);
        }
    }
}
