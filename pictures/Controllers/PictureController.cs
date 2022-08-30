using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System.Diagnostics;

namespace pictures.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PictureController : ControllerBase
{
    AmazonDynamoDBConfig clientConfig;
    AmazonDynamoDBClient client;
    DynamoDBContext context;
    PictureTable model;

    public PictureController()
    {
        this.clientConfig = new AmazonDynamoDBConfig();
        this.client = new AmazonDynamoDBClient(clientConfig);
        this.context = new DynamoDBContext(client);
        this.model = new PictureTable();
    }
    [HttpPost]
    public async Task<PictureTable> Create([FromBody]PictureTableRequest req)
    {
        var rekognitionClient = new AmazonRekognitionClient(Amazon.RegionEndpoint.USEast1);
        var responseList = new List<String>();

        DetectLabelsRequest detectlabelsRequest = new DetectLabelsRequest()
        {
            Image = new Image()
            {
                S3Object = new S3Object()
                {
                    Name = req.photo,
                    Bucket = req.bucket
                },
            },
            MaxLabels = 5,
            MinConfidence = 80F
        };
        try
        {
            var detectLabelsResponse = await rekognitionClient.DetectLabelsAsync(detectlabelsRequest);
            foreach (Label label in detectLabelsResponse.Labels)
                responseList.Add(label.Name);
            if (responseList.Count() > 0)
            {
                var guid = Guid.NewGuid().ToString();
                model = new PictureTable{
                    id = guid,
                    ImageName = $"{req.bucket}/{req.photo}",
                    ImageTags = responseList.ToArray(),
                    User = req.user
                };
                await context.SaveAsync(model);
            }
            return model;
        }
        catch(Exception)
        {
            return model;
            throw;
        }    
    }
    [HttpGet("{id}")]
    public async Task<PictureTable> Read(string id)
    {
        model = await context.LoadAsync<PictureTable>(id);
        return model;
    }

    [HttpDelete("{id}")]
    public async Task<bool> Delete(string id)
    {
        model = await context.LoadAsync<PictureTable>(id);
        if(model != null)
        {
            await context.DeleteAsync<PictureTable>(model);
            return true;
        }
        return false;
    }
}
