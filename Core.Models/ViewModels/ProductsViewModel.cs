using Core.Presentation.Models.DataTransferObjects;
using Core.Presentation.Models.DataTransferObjects.Base;
using Core.Presentation.Models.Base;
using Core.Presentation.Models.Validation;
using Core.Presentation.Models.Validation.Base;


namespace Core.Presentation.Models.ViewModels
{
    public class ProductsViewModel : GenericListViewModel<ProductDto>
    {
        public ProductsViewModel(): this(Enumerable.Empty<ProductDto>().Append(new ProductDto())) { }
        public ProductsViewModel(IEnumerable<ProductDto> state) : base(state)
        {
        }

        public FormComponentViewModel<ProductDto> ProductSearchViewModel { get; set; } =
        new FormComponentViewModel<ProductDto>(Enumerable.Empty<ProductDto>().Append(new ProductDto()), "Product-Search-Form")
        {
          
            Title = "Filter",
            ActionName = "Get",
            ControllerName = "Products",
            Fields = new List<InputFieldViewModel<ProductDto>>
            {
                new InputFieldViewModel<ProductDto>(nameof(ProductDto.Name), "Name")
                {
                    ControlType = ControlType.Text,
                 
                }//,
                // new InputFieldViewModel<ProductDto>(nameof(ProductDto.Archived), "Show Archived Only")
                //{
                //    ControlType = ControlType.Checkbox,  
                //}
            },
            ColClass = "col-4",
            SubmitButtonText = "Filter"
        };

        public TableComponentViewModel<ProductDto> ProductTableViewModel { get; set;  } =
            new TableComponentViewModel<ProductDto>
            {
                DeleteAction = "Delete",
                DeleteController = "Products",
                ArchiveController = "Products",
                ArchiveAction = "Archive",
                ColumnConfigs = new List<TableComponentColumnConfig<ProductDto>>
                {
                    new TableComponentColumnConfig<ProductDto>
                    {
                        Name = "Name",
                        Index = nameof(ProductDto.Name),
                        EditInputFieldViewModel = new InputFieldViewModel<ProductDto>(
                            nameof(ProductDto.Name),
                            "Name")
                    },
                    new TableComponentColumnConfig<ProductDto>
                    {
                        Name = "Description",
                        Index = nameof(ProductDto.Description),
                         EditInputFieldViewModel = new InputFieldViewModel<ProductDto>(
                            nameof(ProductDto.Description),
                            "Description")
                    },
                     new TableComponentColumnConfig<ProductDto>
                    {
                        Name = "Price",
                        Index = nameof(ProductDto.Price),
                         EditInputFieldViewModel = new InputFieldViewModel<ProductDto>(
                            nameof(ProductDto.Price),
                            "Price",
                            new ValidatorBase<ProductDto>(new []{ Validators.Number()}, nameof(ProductDto.Price))
                            )
                    }
                },
                ShowCreateNewButton = true,
                ShowArchiveButton = false,
                ShowViewButton = false,
                SaveOrUpdateController = "Products",
                GetDataController="Products",
                GetDataAction="Get"
            };

        public FormComponentViewModel<ProductDto> NewProductFormViewModel { get; set; } =
            new FormComponentViewModel<ProductDto>(Enumerable.Empty<ProductDto>().Append(new ProductDto()), "New-Product-Form")
            {
                Fields = new List<InputFieldViewModel<ProductDto>>
                {
                    new InputFieldViewModel<ProductDto>(
                        nameof(ProductDto.Name), 
                        "Name", 
                        new ValidatorBase<ProductDto>(new []{ Validators.Required()}, nameof(ProductDto.Name)))
                    {
                        ControlType = ControlType.Text,
                    },
                     new InputFieldViewModel<ProductDto>(nameof(ProductDto.Description), "Description")
                    {
                        ControlType = ControlType.Text,
                    },
                      new InputFieldViewModel<ProductDto>(
                          nameof(ProductDto.Price), 
                          "Price",
                          new ValidatorBase<ProductDto>(new []{Validators.Required(), Validators.Number()}, 
                              nameof(ProductDto.Price))
                          )
                    {
                        ControlType = ControlType.Text,
                       
                    }
                },
                ColClass = "col-12",
                
            };
        public ModalViewModel<BaseDto> NewProductModalViewModel { get; set; } =
            new ModalViewModel<BaseDto>()
            {
                Title = "New Product/Service"
            };
        
    }
}
