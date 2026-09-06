
using ClientManagement.Models;
using ClientManagement.Models.DataTransferObjects;
using ClientManagement.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.UnitTests.DataProviders
{
    public static class TableComponentTestsDataProvider
    {
        public static IEnumerable<TestCaseData> CreateTableComponentWorks_DP()
        {
            yield return new TestCaseData(
                 new ClientsViewModel().TableConfig
                )
            { TypeArgs = new[] { typeof(ClientDto) } }.Returns(true);
        }
    }
}
