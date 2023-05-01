using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;

namespace pictures.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PictureController : ControllerBase
{
    [HttpGet("{photo}")]
    public async Task<IEnumerable<Labels>> DetectLabels(string photo, string? bucket = "picturesocialbucket")
    {
        var rekognitionClient = new AmazonRekognitionClient(Amazon.RegionEndpoint.USEast1);
        var responseList = new List<Labels>();

        DetectLabelsRequest detectlabelsRequest = new DetectLabelsRequest()
        {
            Image = new Image()
            {
                S3Object = new S3Object()
                {
                    Name = photo,
                    Bucket = bucket
                },
            },
            MaxLabels = 30,
            MinConfidence = 60F
        };
        try
        {
            var detectLabelsResponse = await rekognitionClient.DetectLabelsAsync(detectlabelsRequest);
            foreach (Label label in detectLabelsResponse.Labels)
                responseList.Add(new Labels{
                    Name = label.Name,
                    Probability = label.Confidence
                });
            if (responseList.Count() > 0)
            {
                return responseList;
            }
            return null;
        }
        catch(Exception e)
        {
            return null;
            throw;
        }    
    }
    [HttpGet("text/{photo}")]
    public async Task<DetectTextResponse> DetectText(string photo, string? bucket = "picturesocialbucket")
    {
        var rekognitionClient = new AmazonRekognitionClient(Amazon.RegionEndpoint.USEast1);
        var responseList = new List<Labels>();

        var detectTextRequest = new DetectTextRequest()
        {
            Image = new Image()
            {
                S3Object = new S3Object()
                {
                    Name = photo,
                    Bucket = bucket,
                },
            },
        };
        try
        {
            DetectTextResponse detectTextResponse = await rekognitionClient.DetectTextAsync(detectTextRequest);
            if(detectTextResponse != null)
            {
                return detectTextResponse;
            }
            return null;
        }
        catch (System.Exception)
        {
            return null;
            throw;
        }
    }
}
