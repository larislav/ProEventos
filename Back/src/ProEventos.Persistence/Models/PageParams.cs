namespace ProEventos.Persistence.Models
{
    public class PageParams
    {
        public const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        public int _PageSize = 10;

        public int PageSize 
        { 
            get {return _PageSize;}
            set { _PageSize = (value > MaxPageSize) ? MaxPageSize : value; }
            
        }

        public string Term { get; set; } = string.Empty;
    }
}