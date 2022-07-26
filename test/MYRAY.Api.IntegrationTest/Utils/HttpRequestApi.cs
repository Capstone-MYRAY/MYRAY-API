using System.Net;
using System.Text;
using MYRAY.Api.IntegrationTest.Support;
using Newtonsoft.Json;

namespace MYRAY.Api.IntegrationTest.Utils;

public static class HttpRequestApi
{
    public static async Task<ResponseObject> GetObjectFromApiAsync<T>(this HttpClient client, string apiUrl)
    {
        var responseObj = new ResponseObject()
        {
            ResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        };

        try
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                responseObj.Object = JsonConvert.DeserializeObject<T>(data)!;
            }

            responseObj.ResponseMessage = response;
        }
        catch (Exception e)
        {
            responseObj.ResponseMessage.Content = new StringContent(e.Message, Encoding.UTF8, "application/json");
           
        }

        return responseObj;
    }
}