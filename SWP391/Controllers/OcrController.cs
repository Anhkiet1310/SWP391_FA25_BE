using Microsoft.AspNetCore.Mvc;
using Services;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OcrController : ControllerBase
    {
        private readonly OcrService _ocrService;

        public OcrController(OcrService ocrService)
        {
            _ocrService = ocrService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("Không có file");

            // (Optional) Bạn có thể lưu tạm file, resize, rotate, enhance trước khi gửi
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.Position = 0;

            var root = await _ocrService.CallFptOcr(ms, file.FileName);
            if (root == null) return StatusCode(500, "OCR provider lỗi");

            var mapped = _ocrService.MapFptResult(root.Value);
            return Ok(mapped);
        }
    }
}
