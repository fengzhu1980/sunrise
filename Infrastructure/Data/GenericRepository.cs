using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly SunriseContext _context;
        public GenericRepository(SunriseContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void AddList(IReadOnlyList<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }

        public void DeleteList(IReadOnlyList<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void UpdateList(IReadOnlyList<T> entities)
        {
            _context.Set<T>().AttachRange(entities);
            _context.Entry(entities).State = EntityState.Modified;
        }

        // public void DetachLocal(T entity, string entryId)
        // {
        //     var local = _context.Set<T>()
        //         .Local
        //         .FirstOrDefault(entry => entry.Id.Equals(entryId));
        //     if (local != null)
        //     {
        //         _context.Entry(local).State = EntityState.Detached;
        //     }
        //     _context.Entry(entity).State = EntityState.Modified;
        // }
    }
}