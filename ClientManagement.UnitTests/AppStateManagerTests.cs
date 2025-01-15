using ClientManagement.Presentation.Web.Interfaces;
using ClientManagement.UnitTests.DataProviders;
using Core.Presentation.Models.DataTransferObjects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.UnitTests
{
    [TestFixture]
    public class AppStateManagerTests
    {
        [TestCaseSource(typeof(AppStateManagerTestsDataProvider), nameof(AppStateManagerTestsDataProvider.AppStateManagerWorks_DP))]
        public bool AppStateManagerWorks(IAppStateManager<BaseDto> appStateManager)
        {
            Assert.That(appStateManager.State.Archived, Is.False);
            return appStateManager.Get<Guid>(nameof(BaseDto.Id)) == Guid.Empty && appStateManager.Set(nameof(BaseDto.Archived), true).Archived;
        }
    }
}
