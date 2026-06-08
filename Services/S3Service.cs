    using Amazon.S3;
    using Amazon.S3.Model;

    namespace AwsAssignmentDemo.Services
    {
        public class S3Service
        {
            private readonly IConfiguration _configuration;

            public S3Service(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task<string> UploadFileAsync(IFormFile file)
            {
                var region = Amazon.RegionEndpoint.APSouth1;

                using var client = new AmazonS3Client(region);

                using var stream = file.OpenReadStream();

                var s3Key = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{file.FileName}";

                var request = new PutObjectRequest
                {
                    BucketName = _configuration["AWS:BucketName"],
                    Key = s3Key,
                    InputStream = stream
                };

                await client.PutObjectAsync(request);
                return s3Key;
            }
        }
    }