using Newtonsoft.Json.Linq;

namespace MYRAY.Api.IntegrationTest.Support;

public class TestData
{
    public const string BaseUrlConst = "BaseRequestUrl";
    public const string PathConst = "Path";
    public const string RequestParamsConst = "RequestParams";
    public const string RequestPathParamConst = "RequestPathParam";
    private const string RequestBodyConst = "RequestBody";
    private const string ResponseCodeConst = "ResponseCode";
    private const string ResponseBodyConst = "ResponseBody";
    private const string SqlTearDownCommandConst = "SqlTearDownCommand";
    

    public TestData(string baseRequestUrl, JToken testData)
    {
        RequestBody = testData[RequestBodyConst] ?? null;
        ExpectedResponseCode = (int)testData[ResponseCodeConst];
        ExpectedResponseBody = testData[ResponseBodyConst];
        SqlTearDownCommand = testData[SqlTearDownCommandConst]?.ToString();
        RequestUrl = baseRequestUrl + testData[RequestPathParamConst] + "?";
        JToken parameters = testData[RequestParamsConst];
        if (parameters != null)
        {
            foreach (JProperty param in parameters)
            {
                
                RequestUrl += param.Name + "=" + param.Value + "&";
            }
        }
    }
    
    public string RequestUrl { get; }
    public JToken Headers { get; }
    public JToken? RequestBody { get; }
    public JToken ExpectedResponseBody { get; }
    public int ExpectedResponseCode { get; }
    public string SqlTearDownCommand { get; }

}