using Newtonsoft.Json;

namespace MYRAY.Business.DTOs.Momo;

public class MomoRequest
{
    [JsonProperty(PropertyName = "partnerCode")]
    public string PartnerCode { get; set; }

    [JsonProperty(PropertyName = "orderId")]
    public string OrderId { get; set; }

    [JsonProperty(PropertyName = "requestId")]
    public string RequestId { get; set; }

    [JsonProperty(PropertyName = "amount")]
    public float Amount { get; set; }

    [JsonProperty(PropertyName = "orderInfo")]
    public string OrderInfo { get; set; }

    [JsonProperty(PropertyName = "orderType")]
    public string OrderType { get; set; }

    [JsonProperty(PropertyName = "transId")]
    public string TransId { get; set; }

    [JsonProperty(PropertyName = "resultCode")]
    public int ResultCode { get; set; }

    [JsonProperty(PropertyName = "message")]
    public string Message { get; set; }

    [JsonProperty(PropertyName = "payType")]
    public string PayType { get; set; }

    [JsonProperty(PropertyName = "responseTime")]
    public long ResponseTime { get; set; }

    [JsonProperty(PropertyName = "extraData")]
    public string ExtraData { get; set; }

    [JsonProperty(PropertyName = "signature")]
    public string Signature { get; set; }
}