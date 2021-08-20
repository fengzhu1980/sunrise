namespace Core.DataModels.Models
{
    public class BasePagingFilterModel
    {
        private const int MaxPageSize = 50;
        private string _keyword;
        public string Keyword
        {
            get => _keyword;
            set => _keyword = value.ToLower();
        }
        public int PageNo { get; set; } = 1;
        private int _pageSize = 50;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}