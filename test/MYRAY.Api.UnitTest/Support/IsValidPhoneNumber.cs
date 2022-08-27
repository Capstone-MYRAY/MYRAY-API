using MYRAY.Business.Helpers;

namespace MYRAY.Api.UnitTest.Support;

public class IsValidPhoneNumber
{
    [Theory]
    [InlineData("0123",null, null,false)]
    [InlineData("0987654321",10, 11, true)]
    [InlineData("021939asad2",null, null, false)]
    [InlineData("",null, null, false)]
    [InlineData("987654321",null, null, false)]
    public void TestIsValidPhoneNumber(string phoneNumber,int min, int max, object expected)
    {

        bool actual = phoneNumber.IsValidPhoneNumber(min, max);
        
        Assert.Equal(actual, expected);
    }
}