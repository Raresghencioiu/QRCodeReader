using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeReader.Services
{
    interface IService<QRCodeRequest>
    {
        Task GetQRResult(QRCodeRequest request);
    }
}
