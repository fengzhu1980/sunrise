using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API.Dtos;

namespace API.Controllers.Models
{
    public class JobToAddModel
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
        public DateTime? ScheduledStartedOn { get; set; }
        public DateTime? ScheduledEndedOn { get; set; }
        public bool? HasBeenReassigned { get; set; }
        [Required]
        public bool IsCompleted { get; set; }
        public bool? IsRescheduled { get; set; }
        public string Notes { get; set; }
        public string RescheduledReason { get; set; }
        public string StageId { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public string FormTitle { get; set; }
        public string FormDescription { get; set; }
        public string AssignedToStaffId { get; set; }
        public IReadOnlyList<string> BeforePhotoIds { get; set; }
        public IReadOnlyList<string> AfterPhotoIds { get; set; }
        public IReadOnlyList<string> JobHazardIds { get; set; }
        public IReadOnlyList<JobLineReturnDto> JobLines { get; set; }
    }
}