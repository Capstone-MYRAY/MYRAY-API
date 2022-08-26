using MYRAY.Business.Helpers;

namespace MYRAY.Api.UnitTest.Support;

public class ConvertVnPhoneNumber
{
    [Theory]
    [InlineData("123456789", "+84123456789")]
    [InlineData("0987654321", "+84987654321")]
    [InlineData("+84987654321", "+84987654321")]
    [InlineData("987654321", "+84987654321")]
    public void TestConvertVNPhoneNumber(string phoneNumber, object expected)
    {

        string actual = phoneNumber.ConvertVNPhoneNumber();
        
        Assert.Equal(actual, expected);
    }
}