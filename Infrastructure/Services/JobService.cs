using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class JobService : IJobService
    {
        private readonly IUnitOfWork _unitOfWork;
        public JobService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateNewJob(Job model)
        {
            _unitOfWork.Repository<Job>().Add(model);
            var result = await _unitOfWork.Complete();

            return result > 0;
        }

        public async Task<bool> UpdateJob(Job model, string staffId, string log)
        {
            // TODO: Save job log
            var jobInDB = await _unitOfWork.Repository<Job>().GetByIdAsync(model.Id);
            if (jobInDB.Address != model.Address)
            {
                jobInDB.Address = model.Address;
            }
            if (jobInDB.Title != model.Title)
            {
                jobInDB.Title = model.Title;
            }
            if (jobInDB.FirstName != model.FirstName)
            {
                jobInDB.FirstName = model.FirstName;
            }
            if (jobInDB.MiddleName != model.MiddleName)
            {
                jobInDB.MiddleName = model.MiddleName;
            }
            if (jobInDB.LastName != model.LastName)
            {
                jobInDB.LastName = model.LastName;
            }
            if (jobInDB.Email != model.Email)
            {
                jobInDB.Email = model.Email;
            }
            if (jobInDB.Mobile != model.Mobile)
            {
                jobInDB.Mobile = model.Mobile;
            }
            if (jobInDB.Phone != model.Phone)
            {
                jobInDB.Phone = model.Phone;
            }
            if (jobInDB.ScheduledStartedOnUtc != model.ScheduledStartedOnUtc)
            {
                jobInDB.ScheduledStartedOnUtc = model.ScheduledStartedOnUtc;
                jobInDB.IsRescheduled = true;
            }
            if (jobInDB.ScheduledEndedOnUtc != model.ScheduledEndedOnUtc)
            {
                jobInDB.ScheduledEndedOnUtc = model.ScheduledEndedOnUtc;
                jobInDB.IsRescheduled = true;
            }
            if (jobInDB.AssignedToStaffId != model.AssignedToStaffId)
            {
                jobInDB.AssignedToStaffId = model.AssignedToStaffId;
                // Check reassign to
                jobInDB.HasBeenReassigned = true;
            }
            if (jobInDB.IsCompleted != model.IsCompleted)
            {
                jobInDB.IsCompleted = model.IsCompleted;
            }
            if (jobInDB.Notes != model.Notes)
            {
                jobInDB.Notes = model.Notes;
            }
            if (jobInDB.StageId != model.StageId)
            {
                jobInDB.StageId = model.StageId;
            }
            if (jobInDB.IsActive != model.IsActive)
            {
                jobInDB.IsActive = model.IsActive;
            }
            if (jobInDB.FormTitle != model.FormTitle)
            {
                jobInDB.FormTitle = model.FormTitle;
            }
            if (jobInDB.FormDescription != model.FormDescription)
            {
                jobInDB.FormDescription = model.FormDescription;
            }

            jobInDB.LastModifiedByStaffId = staffId;
            jobInDB.LastModifiedOnUtc = DateTime.UtcNow;

            _unitOfWork.Repository<Job>().Update(jobInDB);
            var result = await _unitOfWork.Complete();

            return result > 0;
        }

        public async Task<bool> CreateNewJobStage(Stage model)
        {
            _unitOfWork.Repository<Stage>().Add(model);
            var result = await _unitOfWork.Complete();

            return result > 0;
        }

        public async Task<bool> UpdateJobStage(Stage model)
        {
            var stageInDB = await _unitOfWork.Repository<Stage>().GetByIdAsync(model.Id);
            if (stageInDB.Name != model.Name)
            {
                stageInDB.Name = model.Name;
            }
            if (stageInDB.Notes != model.Notes)
            {
                stageInDB.Notes = model.Notes;
            }
            if (stageInDB.Priority != model.Priority)
            {
                stageInDB.Priority = model.Priority;
            }
            if (stageInDB.IsActive != model.IsActive)
            {
                stageInDB.IsActive = model.IsActive;
            }

            _unitOfWork.Repository<Stage>().Update(stageInDB);
            var result = await _unitOfWork.Complete();

            return result > 0;
        }

        public async Task<bool> CreateNewJobNote(Note note, string jobId)
        {
            // 1. Create note and save
            _unitOfWork.Repository<Note>().Add(note);
            // 2. Create jobnote and save
            var jobNote = new JobNote
            {
                NoteId = note.Id,
                JobId = jobId
            };
            _unitOfWork.Repository<JobNote>().Add(jobNote);
            var result = await _unitOfWork.Complete();

            return result > 0;
        }

        public async Task<bool> CreateBeforePhotos(IReadOnlyList<string> documentIds, string jobId)
        {
            var tempBeforePhotos = new List<JobBeforePhoto>();
            foreach (var documentId in documentIds)
            {
                var tempBeforePhoto = new JobBeforePhoto
                {
                    DocumentId = documentId,
                    JobId = jobId
                };
                tempBeforePhotos.Add(tempBeforePhoto);
            }
            _unitOfWork.Repository<JobBeforePhoto>().AddList(tempBeforePhotos);
            var result = await _unitOfWork.Complete();

            return result > 0;
        }

        public async Task<bool> CreateAfterPhotos(IReadOnlyList<string> documentIds, string jobId)
        {
            var tempAfterPhotos = new List<JobAfterPhoto>();
            foreach (var documentId in documentIds)
            {
                var tempAfterPhoto = new JobAfterPhoto
                {
                    DocumentId = documentId,
                    JobId = jobId
                };
                tempAfterPhotos.Add(tempAfterPhoto);
            }
            _unitOfWork.Repository<JobAfterPhoto>().AddList(tempAfterPhotos);
            var result = await _unitOfWork.Complete();

            return result > 0;
        }

        public async Task<bool> CreateJobHazards(IReadOnlyList<string> hazardIds, string jobId)
        {
            var tempJobHazards = new List<JobHazard>();
            foreach (var hazardId in hazardIds)
            {
                var tempJobHazard = new JobHazard
                {
                    HazardId = hazardId,
                    JobId = jobId
                };
                tempJobHazards.Add(tempJobHazard);
            }
            _unitOfWork.Repository<JobHazard>().AddList(tempJobHazards);
            var result = await _unitOfWork.Complete();

            return result > 0;
        }

        public async Task<bool> CreateJobLines(IReadOnlyList<JobLine> jobLines)
        {
            _unitOfWork.Repository<JobLine>().AddList(jobLines);
            var result = await _unitOfWork.Complete();

            return result > 0;
        }

        public async Task<bool> CreateJobLog(JobLog jobLog)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> UpdateBeforePhotos(IReadOnlyList<string> documentIds, string jobId)
        {
            var spec = new BaseSpecification<JobBeforePhoto>(x => x.JobId == jobId);
            var beforePhotosInDB = await _unitOfWork.Repository<JobBeforePhoto>().ListAsync(spec);

            // 1. No documentIds input
            if (documentIds == null)
            {
                var result = true;
                // 1.1 Has documentids in DB
                if (beforePhotosInDB != null && beforePhotosInDB.Count > 0)
                {
                    _unitOfWork.Repository<JobBeforePhoto>().DeleteList(beforePhotosInDB);
                    var count = await _unitOfWork.Complete();
                    result = count > 0;
                }
                return result;
            }
            // 2. Has documentids input
            else
            {
                // 2.1 No data in DB
                if (beforePhotosInDB == null)
                {
                    var result = await CreateBeforePhotos(documentIds, jobId);
                    return result;
                }
                // 2.2 Has data in DB
                else
                {
                    var tempAllIdsInDB = beforePhotosInDB.Select(h => h.DocumentId).ToList();
                    // 2.2.1 Get data to be added
                    var tempIdsToBeAdded = documentIds.Except(tempAllIdsInDB).ToList();
                    // 2.2.2 Get data to be deleted
                    var tempIdsToBeDeleted = tempAllIdsInDB.Except(documentIds).ToList();
                    // 2.2.3 Save and return
                    if (tempIdsToBeDeleted != null && tempIdsToBeDeleted.Count > 0)
                    {
                        var tempEntitiesToBeDeleted = beforePhotosInDB.Where(h => tempIdsToBeDeleted.Contains(h.DocumentId)).ToList();
                        _unitOfWork.Repository<JobBeforePhoto>().DeleteList(tempEntitiesToBeDeleted);
                        
                        var count = await _unitOfWork.Complete();
                        if (count <= 0) return false;
                    }
                    if (tempIdsToBeAdded != null && tempIdsToBeAdded.Count > 0)
                    {
                        var result = await CreateBeforePhotos(tempIdsToBeAdded, jobId);
                        if (!result) return result;
                    }
                    return true;
                }
            }
        }

        public async Task<bool> UpdateAfterPhotos(IReadOnlyList<string> documentIds, string jobId)
        {
            var spec = new BaseSpecification<JobAfterPhoto>(x => x.JobId == jobId);
            var afterPhotosInDB = await _unitOfWork.Repository<JobAfterPhoto>().ListAsync(spec);

            // 1. No documentIds input
            if (documentIds == null)
            {
                var result = true;
                // 1.1 Has documentids in DB
                if (afterPhotosInDB != null && afterPhotosInDB.Count > 0)
                {
                    _unitOfWork.Repository<JobAfterPhoto>().DeleteList(afterPhotosInDB);
                    var count = await _unitOfWork.Complete();
                    result = count > 0;
                }
                return result;
            }
            // 2. Has documentids input
            else
            {
                // 2.1 No data in DB
                if (afterPhotosInDB == null)
                {
                    var result = await CreateAfterPhotos(documentIds, jobId);
                    return result;
                }
                // 2.2 Has data in DB
                else
                {
                    var tempAllIdsInDB = afterPhotosInDB.Select(h => h.DocumentId).ToList();
                    // 2.2.1 Get data to be added
                    var tempIdsToBeAdded = documentIds.Except(tempAllIdsInDB).ToList();
                    // 2.2.2 Get data to be deleted
                    var tempIdsToBeDeleted = tempAllIdsInDB.Except(documentIds).ToList();
                    // 2.2.3 Save and return
                    if (tempIdsToBeDeleted != null && tempIdsToBeDeleted.Count > 0)
                    {
                        var tempEntitiesToBeDeleted = afterPhotosInDB.Where(h => tempIdsToBeDeleted.Contains(h.DocumentId)).ToList();
                        _unitOfWork.Repository<JobAfterPhoto>().DeleteList(tempEntitiesToBeDeleted);
                        
                        var count = await _unitOfWork.Complete();
                        if (count <= 0) return false;
                    }
                    if (tempIdsToBeAdded != null && tempIdsToBeAdded.Count > 0)
                    {
                        var result = await CreateAfterPhotos(tempIdsToBeAdded, jobId);
                        if (!result) return result;
                    }
                    return true;
                }
            }
        }

        public async Task<bool> UpdateJobHazards(IReadOnlyList<string> hazardIds, string jobId)
        {
            var spec = new BaseSpecification<JobHazard>(x => x.JobId == jobId);
            var jobHazardsInDB = await _unitOfWork.Repository<JobHazard>().ListAsync(spec);

            // 1. No hazardIds input
            if (hazardIds == null)
            {
                var result = true;
                // 1.1 Has hazardIds in DB
                if (jobHazardsInDB != null && jobHazardsInDB.Count > 0)
                {
                    _unitOfWork.Repository<JobHazard>().DeleteList(jobHazardsInDB);
                    var count = await _unitOfWork.Complete();
                    result = count > 0;
                }
                return result;
            }
            // 2. Has hazardIds input
            else
            {
                // 2.1 No data in DB
                if (jobHazardsInDB == null)
                {
                    var result = await CreateJobHazards(hazardIds, jobId);
                    return result;
                }
                // 2.2 Has data in DB
                else
                {
                    var tempAllIdsInDB = jobHazardsInDB.Select(h => h.HazardId).ToList();
                    // 2.2.1 Get data to be added
                    var tempIdsToBeAdded = hazardIds.Except(tempAllIdsInDB).ToList();
                    // 2.2.2 Get data to be deleted
                    var tempIdsToBeDeleted = tempAllIdsInDB.Except(hazardIds).ToList();
                    // 2.2.3 Save and return
                    if (tempIdsToBeDeleted != null && tempIdsToBeDeleted.Count > 0)
                    {
                        var tempEntitiesToBeDeleted = jobHazardsInDB.Where(h => tempIdsToBeDeleted.Contains(h.HazardId)).ToList();
                        _unitOfWork.Repository<JobHazard>().DeleteList(tempEntitiesToBeDeleted);
                        
                        var count = await _unitOfWork.Complete();
                        if (count <= 0) return false;
                    }
                    if (tempIdsToBeAdded != null && tempIdsToBeAdded.Count > 0)
                    {
                        var result = await CreateJobHazards(tempIdsToBeAdded, jobId);
                        if (!result) return result;
                    }
                    return true;
                }
            }
        }
        public async Task<bool> UpdateJobLines(IReadOnlyList<JobLine> jobLines, string log)
        {
            foreach (var jobLine in jobLines)
            {
                _unitOfWork.Repository<JobLine>().Update(jobLine);
            }
            var count = await _unitOfWork.Complete();
            return count > 0;
        }

        // public async Task<bool> UpdateJobLines(IReadOnlyList<JobLine> jobLines, string log)
        // {
        //     // 1. Get joblines by job id
        //     var tempJobId = jobLines.FirstOrDefault().JobId;
        //     var spec = new BaseSpecification<JobLine>(x => x.JobId == tempJobId);
        //     var tempUnitOfWork = _unitOfWork.Repository<JobLine>();
        //     var jobLinesInDB = await tempUnitOfWork.ListAsync(spec);
        //     var jobLineIdsInput = jobLines.Select(j => j.Id).ToList();

        //     var tempAllIdsInDB = jobLinesInDB.Select(h => h.Id).ToList();
        //     // 2. Get ids to be added
        //     var tempIdsToBeAdded = jobLineIdsInput.Except(tempAllIdsInDB).ToList();
        //     // 3. Get ids to be deleted
        //     var tempIdsToBeDeleted = tempAllIdsInDB.Except(jobLineIdsInput).ToList();
        //     // 4. Get ids to be updated
        //     var tempIdsToBeUpdated = tempAllIdsInDB.Intersect(jobLineIdsInput).ToList();
        //     // 5. Delete
        //     if (tempIdsToBeDeleted != null && tempIdsToBeDeleted.Count > 0)
        //     {
        //         var tempEntitiesToBeDeleted = jobLinesInDB.Where(h => tempIdsToBeDeleted.Contains(h.Id)).ToList();
        //         tempUnitOfWork.DeleteList(tempEntitiesToBeDeleted);
        //     }
        //     // 6. Add new
        //     if (tempIdsToBeAdded != null && tempIdsToBeAdded.Count > 0)
        //     {
        //         var tempEntitiesToBeAdded = jobLines.Where(j => tempIdsToBeAdded.Contains(j.Id)).ToList();
        //         tempUnitOfWork.AddList(tempEntitiesToBeAdded);
        //     }
        //     // 7. Update
        //     if (tempIdsToBeUpdated != null && tempIdsToBeUpdated.Count > 0)
        //     {
        //         var tempEntitiesToBeUpdated = jobLines.Where(j => tempIdsToBeUpdated.Contains(j.Id)).ToList();
        //         // var tempEntities = new List<JobLine>();
        //         // foreach (var entityInput in tempEntitiesToBeUpdated)
        //         // {
        //         //     var tempEntityInDB = jobLinesInDB.Where(j => j.Id == entityInput.Id).FirstOrDefault();
        //         //     tempEntityInDB = entityInput;
        //         //     tempUnitOfWork.Update(tempEntityInDB);
        //         // }
        //         tempUnitOfWork.UpdateList(tempEntitiesToBeUpdated);
        //     }
        //     // 8. Save
        //     var count = await _unitOfWork.Complete();
        //     return count >= 0;
        //     // TODO: save log
        // }

        public async Task<bool> DeleteJobLines(IReadOnlyList<JobLine> jobLines, string log)
        {
            // TODO: save log
            _unitOfWork.Repository<JobLine>().DeleteList(jobLines);
            var count = await _unitOfWork.Complete();
            return count > 0;
        }
    }
}