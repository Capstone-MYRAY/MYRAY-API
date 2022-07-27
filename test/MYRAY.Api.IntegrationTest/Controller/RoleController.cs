using MYRAY.Api.IntegrationTest.Constants;
using MYRAY.Api.IntegrationTest.TestData;
using MYRAY.Api.IntegrationTest.Utils;
using MYRAY.Business.DTOs.Role;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MYRAY.Api.IntegrationTest.Controller;

public class RoleController: IClassFixture<ServerFixture<Program>>
{
    private readonly ServerFixture<Program> _factory;
    private readonly JObject _testData;
    private readonly HttpClient _client;
    private readonly string _baseRequestUrl;

    public RoleController(ServerFixture<Program> factory)
    {
        _factory = factory;
        _client = _factory.ApiClient;
        _testData = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(TestDataPath.RoleController))!;
        _baseRequestUrl = _testData[Support.TestData.BaseUrlConst]!.ToString();

    }
    
    public class Version1
    {
        public class Get : RoleController
        {
            public Get(ServerFixture<Program> factory) : base(factory)
            {
            }

            [Fact]
            public async Task ShouldReturn200OK_WhenRequestedWithNoParams()
            {
                JToken httpMethod = _testData[GetType().Name]!;
                JToken function = httpMethod[UtilText.CurrentMethod()]!;
                // Arrange
                var data = new Support.TestData(_baseRequestUrl, function);
                
                // Act
                var responseObj = await _client.GetObjectFromApiAsync<IList<GetRoleDetail>>(data.RequestUrl);
                string responseString = await responseObj.ResponseMessage.Content.ReadAsStringAsync();
                var actualResponseBody = JToken.Parse(responseString);
                
                // Assert
                Assert.Equal(data.ExpectedResponseCode, (int)responseObj.ResponseMessage.StatusCode);
                Assert.NotNull(responseObj.Object);
                
                //--compare total records of return response
                Assert.Equal((int)NumberOfItems.Role, (responseObj.Object as IList<GetRoleDetail>)!.Count);
                
                //--compare total attributes of return response
                Assert.Contains(actualResponseBody.First, data.ExpectedResponseBody);

            }
        }
    }
}