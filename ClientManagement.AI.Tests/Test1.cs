using ClientManagement.AI.StateMachines.UserAssistant;
using ClientManagement.AI.StateMachines.UserAssistant.Triggers;
using ClientManagement.Models.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using Moq;

namespace ClientManagement.AI.Tests;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    [DynamicData(nameof(UserInstructions), typeof(Test1))]
    public void TestMethod1(string userInstructions, bool expected)
    {
        var moq = new Mock<IChatClient>();
        var opt = new Mock<IOptions<AgentOptions>>();
        opt.Setup(x => x.Value).Returns(new AgentOptions());
        var x = new UserAssistantContext(moq.Object, opt.Object);
        x.SwitchOn();
        x.IncomingMessageSelector?.Receptor(x, new(ChatRole.User, "Hello agent"));

       // x.IncomingUserMessageSignal?.
    }

    public static IEnumerable<(string input, bool expected)> UserInstructions => [("Hello World!", true)];

    //public void 
     
}
