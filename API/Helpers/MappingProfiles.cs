using API.Dtos;
using AutoMapper;
using Core.Entities;

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
        }
    }
}