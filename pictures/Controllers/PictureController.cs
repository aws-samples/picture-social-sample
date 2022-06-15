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
        public async Task<IEnumerable<Labels>> DetectLabels(string photo, string? bucket = "")
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
                MaxLabels = 10,
                MinConfidence = 80F
            };
                
            var detectLabelsResponse = await rekognitionClient.DetectLabelsAsync(detectlabelsRequest);
            foreach (Label label in detectLabelsResponse.Labels)
                responseList.Add(new Labels{
                    Name = label.Name,
                    Probability = label.Confidence
                });
            return responseList;
        }
}
