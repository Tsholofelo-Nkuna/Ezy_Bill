namespace Core.Presentation.Models
{
    public interface IFilterable<TFilterSource, TData>
    {
        /// <summary>
        /// Filter source object
        /// </summary>
        TFilterSource Filter { get; set; }
        Task<IEnumerable<TData>> GetData(TFilterSource filters);
    }
}
