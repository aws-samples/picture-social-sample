using Amazon.DynamoDBv2.DataModel;
namespace pictures
{
    [DynamoDBTable("pictures")]
    public class PictureTable
    {
        [DynamoDBHashKey]
        public string id { get; set; } = default!;
        [DynamoDBProperty("imagename")]
        public string ImageName { get; set; } = default!;
        [DynamoDBProperty("imagetags")]
        public string[] ImageTags { get; set; } = default!;
        [DynamoDBProperty("user")]
        public string User { get; set; } = default!;
    }

    public class PictureTableRequest
    {
        public string photo { get; set; }= default!;
        public string bucket { get; set; }= default!;
        public string user { get; set; }= default!;
    }
}