using MarketPlace.DataLayer.Context;
using MarketPlace.DataLayer.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly MarketPlaceDbContext _context;
        private readonly DbSet<TEntity> _dbSet;


        public GenericRepository(MarketPlaceDbContext context)
        {
            _context = context;
            this._dbSet = _context.Set<TEntity>();
        }

        public async Task AddEntity(TEntity entity)
        {
            entity.CreateDate = System.DateTime.Now;
            entity.LastUpdateDate = entity.CreateDate;
            await _dbSet.AddAsync(entity);
        }

        public async Task<TEntity> GetEntityById(long entityId)
        {
            return await _dbSet.SingleOrDefaultAsync(s => s.Id == entityId && s.IsDelete == false);
        }

        public void UpdateEntity(TEntity entity)
        {
            entity.LastUpdateDate = System.DateTime.Now;
            _dbSet.Update(entity);
        }


        public async ValueTask DisposeAsync()
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
            }

        }

        public void DeleteEntity(TEntity entity)
        {
            entity.IsDelete = true;
            UpdateEntity(entity);
        }

        public async Task DeleteEntity(long entityId)
        {
            TEntity entity = await GetEntityById(entityId);
            if (entity != null) DeleteEntity(entity); 
        }

        public void DeletePermanent(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task DeletePermanent(long entityId)
        {
            TEntity entity =await GetEntityById(entityId);
            if(entity !=null) DeletePermanent(entity);
            
        }

        public IQueryable<TEntity> GetQuery()
        {
            return _dbSet.AsQueryable();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
