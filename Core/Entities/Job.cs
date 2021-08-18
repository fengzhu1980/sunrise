using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Job : BaseDataModel
    {
        [MaxLength(36)]
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
        [MaxLength(36)]
        public string AssignedToStaffId { get; set; }
        [MaxLength(36)]
        public string OriginalAssignedToStaffId { get; set; }
        public bool? HasBeenReassigned { get; set; }
        [MaxLength(36)]
        public string CreatedByStaffId { get; set; }
        [MaxLength(36)]
        public string LastModifiedByStaffId { get; set; }
        public bool? IsCompleted { get; set; }
        public bool? IsRescheduled { get; set; }
        public string Notes { get; set; }
        public string RescheduledReason { get; set; }
        public string StageId { get; set; }
        public bool? IsActive { get; set; }
        [MaxLength(36)]
        public string CustomerId { get; set; }
        [MaxLength(36)]
        public string AddressId { get; set; }
        public string FormTitle { get; set; }
        public string FormDescription { get; set; }
        public virtual Staff AssignedToStaff { get; set; }
        public virtual Staff OriginalAssignedToStaff { get; set; }
        public virtual Staff CreatedByStaff { get; set; }
        public virtual Staff LastModifiedByStaff { get; set; }
        public virtual Address CustomerAddress { get; set; }
        public virtual Customer JobCustomer { get; set; }
        public virtual Stage JobStage { get; set; }
        public virtual ICollection<JobBeforePhoto> BeforePhotos { get; set; }
        public virtual ICollection<JobAfterPhoto> AfterPhotos { get; set; }
        public virtual ICollection<JobNote> RelatedNotes { get; set; }
        public virtual ICollection<JobHazard> JobHazards { get; set; }
        public virtual ICollection<JobTask> JobTasks { get; set; }
        public virtual ICollection<JobLog> JobLogs { get; set; }
    }
}