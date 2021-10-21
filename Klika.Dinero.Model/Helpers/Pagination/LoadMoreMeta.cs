namespace Klika.Dinero.Model.Helpers.Pagination
{
    public class LoadMoreMeta
    {
        public int CurrentOffset { get; set; }
        public int CurrentLimit { get; set; }
        public int TotalItems { get; set; }
        public bool HasNext { get; set; }
    }
}
