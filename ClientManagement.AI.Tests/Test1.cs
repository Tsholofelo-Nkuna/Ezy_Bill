namespace ClientManagement.AI.Tests;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    [DynamicData(nameof(UserInstructions), typeof(Test1))]
    public void TestMethod1(string userInstructions)
    {
        var stack = new Stack<char>();
        char openToken = '(';
        char closeToken = ')';
        foreach(var c in userInstructions)
        {
            if (c.Equals(openToken))
            {
                stack.Push(c);
            }
            else
            {
               if(stack.Count() == 0)
               {
                    Assert.IsTrue(true);
               }
               else
               {
                    var closingTag = stack.Pop();
                    if (closingTag.Equals(closeToken)) { 
                        Assert.IsTrue(true);    
                    }

               }
            }
        }

    }

    public static IEnumerable<(string input, bool expected)> UserInstructions => [("\"[a-zA-Z]{1,}\"", true)];

    
     
}
