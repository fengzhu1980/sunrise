using System.Linq;
using API.Controllers.Models;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Job, JobToReturnDto>()
                .ForMember(d => d.CreatedByStaff, o => o.MapFrom(s => s.CreatedByStaff.FirstName))
                .ForMember(d => d.BeforePhotos, o => o.MapFrom(s => s.BeforePhotos.Select(bp => bp.Document)))
                .ForMember(d => d.AfterPhotos, o => o.MapFrom(s => s.AfterPhotos.Select(ap => ap.Document)))
                .ForMember(d => d.AssignedToStaff, o => o.MapFrom(s => s.AssignedToStaff.FirstName))
                .ForMember(d => d.OriginalAssignedToStaff, o => o.MapFrom(s => s.OriginalAssignedToStaff.FirstName))
                .ForMember(d => d.CreatedByStaff, o => o.MapFrom(s => s.CreatedByStaff.FirstName))
                .ForMember(d => d.LastModifiedByStaff, o => o.MapFrom(s => s.LastModifiedByStaff.FirstName))
                .ForMember(d => d.JobStage, o => o.MapFrom(s => s.JobStage.Name))
                .ForMember(d => d.JobHazardIds, o => o.MapFrom(s => s.JobHazards.Select(jh => jh.HazardId)))
                .ForMember(d => d.JobLines, o => o.MapFrom(s => s.JobLines.ToList()))
                .ForMember(d => d.RelatedNotes, o => o.MapFrom(s => s.RelatedNotes.Select(rn => rn.Note)))
                .ForMember(d => d.BeforePhotoIds, o => o.MapFrom(s => s.BeforePhotos.Select(bp => bp.DocumentId)))
                .ForMember(d => d.AfterPhotoIds, o => o.MapFrom(s => s.AfterPhotos.Select(ap => ap.DocumentId)))
                .ReverseMap();

            CreateMap<JobLine, JobLineReturnDto>()
                .ReverseMap();
            CreateMap<Note, NoteToReturnDto>()
                .ReverseMap();

            CreateMap<Job, JobToAddModel>()
                .ReverseMap();

            CreateMap<Document, DocumentToReturnDto>()
                .ForMember(d => d.UploadedByStaff, o => o.MapFrom(s => s.UploadedByStaff.FirstName))
                .ForMember(d => d.DocumentUrl, o => o.MapFrom<DocumentUrlResolver>());

            CreateMap<Staff, StaffToReturnDto>()
                .ForMember(s => s.Roles, o => o.MapFrom(s => s.StaffRoles.Select(sr => sr.Role.Name).ToList()))
                .ForMember(s => s.RoleIds, o => o.MapFrom(s => s.StaffRoles.Select(sr => sr.RoleId).ToList()))
                .ForMember(s => s.Image, o => o.MapFrom(s => s.Document.RelativeFilePath))
                .ReverseMap();
            // CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<Staff, StaffUpdateModel>()
                .ForMember(s => s.RoleIds, o => o.MapFrom(s => s.StaffRoles.Select(sr => sr.RoleId).ToList()))
                .ReverseMap();
            CreateMap<Hazard, HazardToReturnDto>()
                .ForMember(h => h.CreatedByStaff, o => o.MapFrom(s => s.CreatedByStaff.FirstName))
                .ForMember(h => h.LastUpdatedByStaff, o => o.MapFrom(s => s.LastUpdatedByStaff.FirstName))
                .ForMember(h => h.SafeWorkMethodStatementIds, o => o.MapFrom(s => s.SafeWorkMethodStatements.Select(swms => swms.SafeWorkMethodStatementId).ToList()))
                .ReverseMap();

            CreateMap<Role, RoleToReturnDto>()
                .ForMember(r => r.CreatedByStaff, o => o.MapFrom(s => s.CreatedByStaff.FirstName))
                .ForMember(r => r.LastUpdatedByStaff, o => o.MapFrom(s => s.LastUpdatedByStaff.FirstName))
                .ReverseMap();
        }
    }
}