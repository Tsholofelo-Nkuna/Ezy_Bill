using ClientManagement.UnitTests.DataProviders;

using ClientManagement.Models;
using ClientManagement.Models.DataTransferObjects.Base;
using Core.Presentation.ViewComponents.Components;

namespace ClientManagement.UnitTests
{
    [TestFixture]
    public class TableComponentTests
    {
        [TestCaseSource(typeof(TableComponentTestsDataProvider), nameof(TableComponentTestsDataProvider.CreateTableComponentWorks_DP))]
        public bool CreateTableComponentWorks<TDto>(TableComponentViewModel<TDto> model) where TDto : BaseDto, new()
        {
            var tComponent = new TableComponent<TDto>();
            Assert.That((tComponent.ViewModel?.ViewModelState?.Count() ?? -1) >= 0, Is.True);

            tComponent.ViewModel = model;
            tComponent.ViewModel.ViewModelState =  tComponent.ViewModel.ViewModelState.Append(new TDto());
           
            Assert.That(tComponent.ViewModel.Get<bool>(Guid.Empty, nameof(BaseDto.Archived)), Is.False);
            tComponent.ViewModel.Set(Guid.Empty, nameof(BaseDto.Archived), true);
            return tComponent.ViewModel.Get<bool>(Guid.Empty, nameof(BaseDto.Archived)) ;
        }
    }


}
