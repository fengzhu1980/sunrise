using Core.DataModels;

namespace Core.Entities
{
    public class Document : BaseDataModel
    {
        public string Name { get; set; }
        public string RelativePath { get; set; }
        public string Extension { get; set; }
    }
}