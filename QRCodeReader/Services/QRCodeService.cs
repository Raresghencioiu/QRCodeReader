
using Microsoft.AspNetCore.Http;
using Microsoft.Web.Administration;
using Newtonsoft.Json;
using QRCodeReader.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace QRCodeReader.Services
{
    public class QRCodeService : IService<QRCodeRequest>
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string readerEndpoint = ConfigurationManager.AppSettings["QREndpoint"];
        public HttpContext Context { get; set; }


        public QRCodeService(IHttpClientFactory clientFactory, IHttpContextAccessor context)
        {
            _clientFactory = clientFactory;
            this.Context = context.HttpContext;
        }

        public async Task GetQRResult(QRCodeRequest qrRequest)
        {
            try
            {
                using (var client = _clientFactory.CreateClient())
                {
                    var image = new Bitmap(qrRequest.Path);

                    var request = new HttpRequestMessage(HttpMethod.Post, readerEndpoint);

                    using (var content = new MultipartFormDataContent())
                    {
                        var byteArray = ImageToByteArray(image);
                        content.Add(new ByteArrayContent(byteArray), "file", "file.jpg");
                        request.Content = content;

                        var response = await client.SendAsync(request);
                        response.EnsureSuccessStatusCode();

                        var message = await response.Content.ReadAsStringAsync();

                        var responseObj = JsonConvert.DeserializeObject<QrapiResponseModel[]>(message);
                        var qrText = responseObj.FirstOrDefault().Symbol.FirstOrDefault().Data;

                        await Context.Response.WriteAsync(qrText);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in handling QR request : {ex.Message}");
                throw;
            }
        }

        public byte[] ImageToByteArray(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }
    }
}
