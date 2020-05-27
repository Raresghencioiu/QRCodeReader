using System.Runtime.Serialization;

namespace QRCodeReader.Models
{
    [DataContract]
    public class QRCodeRequest
    {
        [DataMember(Name = "path")] public string Path { get; set; }
    }
}
