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
                .ForMember(d => d.BeforePhotos, o => o.MapFrom(s => s.BeforePhotos));

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