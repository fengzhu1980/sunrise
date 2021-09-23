using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class JobToReturnDto
    {
        public string Id { get; set; }
        public string JobCode { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? LastModifiedOnUtc { get; set; }
        public DateTime? ScheduledStartedOnUtc { get; set; }
        public DateTime? ActualStartedOnUtc { get; set; }
        public DateTime? ScheduledEndedOnUtc { get; set; }
        public DateTime? ActualEndedOnUtc { get; set; }
        public bool? HasBeenReassigned { get; set; }
        public bool? IsCompleted { get; set; }
        public bool? IsRescheduled { get; set; }
        public string Notes { get; set; }
        public string RescheduledReason { get; set; }
        public string StageId { get; set; }
        public bool? IsActive { get; set; }
        public string FormTitle { get; set; }
        public string FormDescription { get; set; }
        public string AssignedToStaff { get; set; }
        public string AssignedToStaffId { get; set; }
        public string OriginalAssignedToStaff { get; set; }
        public string CreatedByStaff { get; set; }
        public string LastModifiedByStaff { get; set; }
        public string JobStage { get; set; }
        public List<DocumentToReturnDto> BeforePhotos { get; set; }
        public List<DocumentToReturnDto> AfterPhotos { get; set; }
        public List<NoteToReturnDto> RelatedNotes { get; set; }
        public List<string> BeforePhotoIds { get; set; }
        public List<string> AfterPhotoIds { get; set; }
        public List<string> JobHazardIds { get; set; }
        public List<JobLineReturnDto> JobLines { get; set; }
        // public List<JobHazard> JobHazards { get; set; }
        // public List<JobTask> JobTasks { get; set; }
        // public List<JobLog> JobLogs { get; set; }
    }
}