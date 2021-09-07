using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class JobRepository : IJobRepository
    {
        private readonly SunriseContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public JobRepository(SunriseContext context, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<Job> GetJobByIdAsync(string id)
        {
            return await _context.Jobs
                .Include(j => j.CreatedByStaff)
                .FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task<IReadOnlyList<Job>> GetJobsAsync()
        {
            return await _context.Jobs
                .Include(j => j.CreatedByStaff)
                .ToListAsync();
        }
    }
}