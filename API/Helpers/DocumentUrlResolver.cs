using API.Dtos;
using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public class DocumentUrlResolver : IValueResolver<Document, DocumentToReturnDto, string>
    {
        private readonly IConfiguration _config;
        public DocumentUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Document source, DocumentToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.RelativeFilePath))
            {
                return _config["ApiUrl"] + source.RelativeFilePath;
            }

            return null;
        }
    }
}