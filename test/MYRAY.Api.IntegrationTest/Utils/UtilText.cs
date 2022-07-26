using System.Runtime.CompilerServices;

namespace MYRAY.Api.IntegrationTest.Utils;

public class UtilText
{
    public static string CurrentMethod([CallerMemberName] string methodName = null) => methodName;

}