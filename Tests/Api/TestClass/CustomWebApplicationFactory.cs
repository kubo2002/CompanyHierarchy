using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Tests.Api.TestClass
{
    /// <summary>
    /// Testovacia factory trieda pre integracne testy
    /// </summary>
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
    }
}
