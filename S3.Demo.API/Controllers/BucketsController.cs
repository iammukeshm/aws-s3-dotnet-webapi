using Amazon.S3;
using Microsoft.AspNetCore.Mvc;

namespace S3.Demo.API.Controllers;

[Route("api/buckets")]
[ApiController]
public class BucketsController : ControllerBase
{
    private readonly IAmazonS3 _s3Client;
    public BucketsController(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateBucketAsync(string bucketName)
    {
        var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
        if (bucketExists) return BadRequest($"Bucket {bucketName} already exists.");
        await _s3Client.PutBucketAsync(bucketName);
        return Ok($"Bucket {bucketName} created.");
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllBucketAsync()
    {
        var data = await _s3Client.ListBucketsAsync();
        var buckets = data.Buckets.Select(b => { return b.BucketName;});
        return Ok(buckets);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteBucketAsync(string bucketName)
    {
        await _s3Client.DeleteBucketAsync(bucketName);
        return NoContent();
    }

}
