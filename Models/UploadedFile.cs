namespace AwsAssignmentDemo.Models
{
    public class UploadedFile
    {
        public int Id { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string S3Key { get; set; } = string.Empty;

        public DateTime UploadDate { get; set; }
    }
}