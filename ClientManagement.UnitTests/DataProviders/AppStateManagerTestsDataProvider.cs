using ClientManagement.Presentation.Web;
using Core.Presentation.Models.DataTransferObjects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.UnitTests.DataProviders
{
    public static class AppStateManagerTestsDataProvider
    {
        public static IEnumerable<TestCaseData> AppStateManagerWorks_DP()
        {
            yield return new TestCaseData( new AppStateManager<BaseDto>() ).Returns(true);
        }
    }
}
