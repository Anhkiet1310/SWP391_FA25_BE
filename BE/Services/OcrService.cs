using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Services
{
    public class OcrService
    {
        private readonly HttpClient _httpClient;
        private readonly string _fptApiKey;

        public OcrService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _fptApiKey = configuration["FptAi:ApiKey"];
        }

        public async Task<JsonElement?> CallFptOcr(Stream imageStream, string filename)
        {
            using var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(imageStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            content.Add(streamContent, "image", filename);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("api-key", _fptApiKey);

            var url = "https://api.fpt.ai/vision/idr/vnm"; // kiểm tra docs nếu khác
            var resp = await _httpClient.PostAsync(url, content);
            resp.EnsureSuccessStatusCode();

            using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
            return doc.RootElement.Clone();
        }

        public object MapFptResult(JsonElement root)
        {
            // Xử lý tuỳ theo response
            // FPT.AI có thể trả dạng { data: { ... }, success: true, message: ... }
            if (root.TryGetProperty("data", out var data))
            {
                // Trả về một object đơn giản
                var info = data[0];
                var mapped = new Dictionary<string, string?>();
                if (info.TryGetProperty("id", out var id)) mapped["id"] = id.GetString();
                if (info.TryGetProperty("name", out var name)) mapped["name"] = name.GetString();
                if (info.TryGetProperty("dob", out var dob)) mapped["dob"] = dob.GetString();
                if (info.TryGetProperty("sex", out var sex)) mapped["sex"] = sex.GetString();
                if (info.TryGetProperty("address", out var addr)) mapped["address"] = addr.GetString();
                return mapped;
            }
            // fallback: trả toàn bộ json nếu format khác
            return root;
        }
    }
}
