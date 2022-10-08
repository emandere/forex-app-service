using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Net.Http;
using forex_app_service.Models;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace forex_app_service.Mapper
{
    public class UploadMap
    {
         private readonly IOptions<Settings> _settings;
         public UploadMap(IOptions<Settings> settings)
        {   
            _settings = settings;
        }

        public async Task UploadDailyPrice(IEnumerable<ForexDailyPriceDTO> days)
        {
            foreach(var day in days)
            {
                var dayString= JsonSerializer.Serialize<ForexDailyPriceDTO>(day);
                await UploadFileToS3(
                                    _settings.Value.AWSKeyId,
                                    _settings.Value.AWSKey,"forexdailyprices",
                                    day.Pair+day.Datetime.ToString("yyyyMMdd"),
                                    dayString
                                    );
            }
        }

         private static async Task UploadFileToS3(string awskeyId,string awskey,string bucketname,string key,string info)
        {
            using (var client = string.IsNullOrEmpty(awskeyId) ? new AmazonS3Client(RegionEndpoint.USEast1) : new AmazonS3Client(awskeyId,awskey,RegionEndpoint.USEast1))
            using (var newMemoryStream = new MemoryStream())
            {

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = new MemoryStream(Encoding.UTF8.GetBytes(info)),
                    Key = key,
                    BucketName = bucketname,
                    CannedACL = S3CannedACL.PublicRead
                };

                var fileTransferUtility = new TransferUtility(client);
                await fileTransferUtility.UploadAsync(uploadRequest);
            }
            
        }
    }
}