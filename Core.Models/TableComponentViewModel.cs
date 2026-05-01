using Core.Presentation.Models.DataTransferObjects;
using Core.Presentation.Models.DataTransferObjects.Base;
using Core.Presentation.Models.Base;
using System.Globalization;

namespace Core.Presentation.Models
{
    public class TableComponentViewModel<TRecordType> : GenericListViewModel<TRecordType>
        where TRecordType : BaseDto, new()
    {
       public TableComponentViewModel(): this(Enumerable.Empty<TRecordType>()){}
       public TableComponentViewModel(IEnumerable<TRecordType> modelState): base(modelState) { }
       public List<TableComponentColumnConfig<TRecordType>> ColumnConfigs = new List<TableComponentColumnConfig<TRecordType>>();
       public bool ShowCreateNewButton { get; set; }
       public string DeleteAction = string.Empty;
       public string DeleteController = string.Empty;
       public string ArchiveAction = string.Empty;
       public string ArchiveController = string.Empty;
       public string SaveOrUpdateController = string.Empty;
       public string SaveOrUpdateAction = string.Empty;
       public float InnerWidth { get; set; }
       public float InnerHeight { get; set; }
       public bool ShowDeleteButton { get; set; } = true;
       public bool ShowArchiveButton { get; set; } = true;
       public bool ShowViewButton { get; set; } = true;
       public string ViewAction {  get; set; } = string.Empty;
       public string ViewController { get; set; } = string.Empty;
       public string GetDataAction { get; set; } = string.Empty;
       public string GetDataController { get; set;} = string.Empty;
       public PageRequestDto<TRecordType> PageRequest { get; set; } = new PageRequestDto<TRecordType>();
       public PageResponseDto<TRecordType>? PageResponse { get; set; } = new PageResponseDto<TRecordType>();
     
       public int PageCount
        {
            get
            {
                if (PageRequest.GetAllPages)
                {
                    return PageResponse.TotalRecords > 0 ? (PageResponse.Items.Count() / PageResponse?.TotalRecords ?? 0) : 0;
                }
                else
                {
                    return (PageResponse?.PageSize ?? 0) > 0 ? (int)Math.Ceiling((PageResponse?.TotalRecords ?? 0) / (float)(PageResponse?.PageSize ?? 0)) : 0;
                }
               
            }
        }
      
    }

    public class TableComponentColumnConfig<TRecord> where TRecord: BaseDto, new()
    {
        public string Index { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public InputFieldViewModel<TRecord>? EditInputFieldViewModel { get; set; }
       
    }

    public class TableComponentColumnContext<TRecord> where TRecord : BaseDto, new() 
    {
        public TableComponentColumnContext(int rowIndex, TableComponentColumnConfig<TRecord> columnConfiguration)
        {
            RowIndex = rowIndex;
            ColumnConfiguration = columnConfiguration;
        }

        public int RowIndex { get; set; }
        public TableComponentColumnConfig<TRecord> ColumnConfiguration {  get; set; }
    }

   
}
