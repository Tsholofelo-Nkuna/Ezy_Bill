using ClientManagement.Presentation.Web;
using ClientManagement.Models.DataTransferObjects.Base;
using Core.Utils.State;
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
