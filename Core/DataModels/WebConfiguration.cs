namespace Core.DataModels.Models
{
    public class WebConfiguration
    {
        public string DocumentFolderName { get; set; }
        public string ParentFolderOfDocumentFolder { get; set; }
        public int UploadFileSizeLimit { get; set; }
    }
}