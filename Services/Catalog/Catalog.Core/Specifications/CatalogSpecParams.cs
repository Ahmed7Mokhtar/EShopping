namespace Catalog.Core.Specs
{
    public class CatalogSpecParams
    {
        private const int MAX_PAGE_SIZE = 70;
        
        private int _page_size;

        public int PageIndex { get; set; } = 1;
        public int PageSize 
        {
            get => _page_size;
            set => _page_size = (value > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value;
        }
        public string? BrandId { get; set; }
        public string? TypeId { get; set; }
        public string? Sort { get; set; }
        public string? Search { get; set; }
    }
}
