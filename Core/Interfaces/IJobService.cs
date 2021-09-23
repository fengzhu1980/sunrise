using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IJobService
    {
        Task<bool> CreateNewJobStage(Stage model);
        Task<bool> UpdateJobStage(Stage model);
        Task<bool> CreateNewJob(Job model);
        Task<bool> UpdateJob(Job model, string staffId, string log);
        Task<bool> CreateNewJobNote(Note note, string jobId);
        Task<bool> CreateBeforePhotos(IReadOnlyList<string> documentIds, string jobId);
        Task<bool> CreateAfterPhotos(IReadOnlyList<string> documentIds, string jobId);
        Task<bool> CreateJobHazards(IReadOnlyList<string> hazardIds, string jobId);
        Task<bool> CreateJobLines(IReadOnlyList<JobLine> jobLines);
        Task<bool> UpdateBeforePhotos(IReadOnlyList<string> documentIds, string jobId);
        Task<bool> UpdateAfterPhotos(IReadOnlyList<string> documentIds, string jobId);
        Task<bool> UpdateJobHazards(IReadOnlyList<string> hazardIds, string jobId);
        Task<bool> UpdateJobLines(IReadOnlyList<JobLine> jobLines, string log);
        Task<bool> DeleteJobLines(IReadOnlyList<JobLine> jobLines, string log);
        Task<bool> CreateJobLog(JobLog jobLog);
    }
}