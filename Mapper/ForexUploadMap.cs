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
using System.Globalization;

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
 
        public  async Task<DateTime> GetDay(string pair)
        {
            Console.WriteLine($"ID: {_settings.Value.AWSKeyId}");
            Console.WriteLine($"VAL: {_settings.Value.AWSKey}");

            var days = await GetDays(
                 _settings.Value.AWSKeyId,
                _settings.Value.AWSKey,"forexdailyprices"
            );

            var maxDay = days.Where(x=>x.Item1==pair).Max(x=>x.Item2);
            return maxDay;
        }
        

        public static async Task<List<(string,DateTime)>> GetDays(string awskeyId,string awskey, string bucketname)
        {
             var days = new List<(string,DateTime)>();
             using (var client = string.IsNullOrEmpty(awskeyId) ? new AmazonS3Client(RegionEndpoint.USEast1) : new AmazonS3Client(awskeyId,awskey,RegionEndpoint.USEast1))
             {
                 ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = bucketname,
                    MaxKeys = 100
                };


                ListObjectsV2Response response;
                do
                {
                    response = await client.ListObjectsV2Async(request);

                    // Process the response.
                    foreach (S3Object entry in response.S3Objects)
                    {
                        var forexDate = DateTime.ParseExact(entry.Key.Substring(6),"yyyyMMdd",CultureInfo.InvariantCulture); 
                        var forexPair = entry.Key.Substring(0,6);
                        days.Add((forexPair,forexDate));
                         
                    }
                    //Console.WriteLine("Next Continuation Token: {0}", response.NextContinuationToken);
                    request.ContinuationToken = response.NextContinuationToken;
                } while (response.IsTruncated);

                /*ParallelOptions parallelOptions = new()
                {
                    MaxDegreeOfParallelism = 3
                };

                await Parallel.ForEachAsync(dayskey,parallelOptions,async (x,token)  => 
                {
                   var dailyprice = await ReadFileFromS3<ForexDailyPriceDTO>(x,bucketname); 
                   days.Add(dailyprice);
                   Console.WriteLine(x + "Added");
                });*/

                
                return days;

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