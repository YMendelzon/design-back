using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

public class S3Service
{
    private readonly string _bucketName;
    private readonly IAmazonS3 _s3Client;
    private readonly string _region;

    public S3Service(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
        _bucketName = Environment.GetEnvironmentVariable("S3_BUCKET_NAME");
        _region = Environment.GetEnvironmentVariable("S3_REGION");

        if (string.IsNullOrEmpty(_bucketName) || string.IsNullOrEmpty(_region))
        {
            throw new Exception("Missing AWS S3 configuration in environment variables");
        }
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var key = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        using (var stream = file.OpenReadStream())
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = stream,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.PublicRead // או ACL אחר על פי הצורך
            };
            await _s3Client.PutObjectAsync(request);
        }
        return $"https://{_bucketName}.s3.{_region}.amazonaws.com/{key}";
    }

    public async Task<bool> DeleteFileAsync(string fileUrl)
    {
        try
        {
            var fileKey = GetFileKeyFromUrl(fileUrl);
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = fileKey
            };

            // Debug: Print the file key
            Console.WriteLine("Deleting file key: " + fileKey);

            var response = await _s3Client.DeleteObjectAsync(deleteObjectRequest);
            return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            // Debug: Print the exception message
            Console.WriteLine("Exception in DeleteFileAsync: " + ex.Message);
            throw new Exception("שגיאה במחיקת תמונה מ-S3: " + ex.Message);
        }
    }

    private string GetFileKeyFromUrl(string fileUrl)
    {
        // Extract the file key from the URL
        var uri = new Uri(fileUrl);
        return uri.AbsolutePath.TrimStart('/');
    }
}


