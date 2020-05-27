using Newtonsoft.Json;

namespace QRCodeReader.Models
{
    public partial class QrapiResponseModel
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("symbol")]
        public Symbol[] Symbol { get; set; }
    }

    public partial class Symbol
    {
        [JsonProperty("seq")]
        public long Seq { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("error")]
        public object Error { get; set; }
    }
}
