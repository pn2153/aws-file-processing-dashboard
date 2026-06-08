using AwsAssignmentDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace AwsAssignmentDemo.Controllers
{
    public class UploadController : Controller
    {
        private readonly S3Service _s3Service;
        private readonly DatabaseService _databaseService;

        public UploadController(
            S3Service s3Service,
            DatabaseService databaseService)
        {
            _s3Service = s3Service;
            _databaseService = databaseService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Please select a file.";
                return View();
            }

            try
            {
                var s3Key = await _s3Service.UploadFileAsync(file);

                await _databaseService.SaveUploadAsync(
                    file.FileName,
                    s3Key);

                ViewBag.Message =
                    $"File '{file.FileName}' uploaded successfully to S3.";
            }
            catch (Exception ex)
            {
                ViewBag.Message =
                    $"Upload failed: {ex.Message}";
            }

            return View();
        }
    }
}