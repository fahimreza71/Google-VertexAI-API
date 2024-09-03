using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Google.Cloud.AIPlatform.V1;
using System;
using System.Threading.Tasks;

namespace VertexAI_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VertexAPIController : ControllerBase
    {
        private readonly ILogger<VertexAPIController> _logger;

        public VertexAPIController(ILogger<VertexAPIController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{query}")]
        public async Task<string> GetResponse(
            string query,
            string projectId = "productfit",
            string location = "us-central1",
            string publisher = "google",
            string model = "gemini-1.5-flash-001")
        {

            var predictionServiceClient = new PredictionServiceClientBuilder
            {
                Endpoint = $"{location}-aiplatform.googleapis.com"
            }.Build();
            string prompt = query;

            var generateContentRequest = new GenerateContentRequest
            {
                Model = $"projects/{projectId}/locations/{location}/publishers/{publisher}/models/{model}",
                Contents =
                {
                    new Content
                    {
                        Role = "USER",
                        Parts =
                        {
                            new Part { Text = prompt }
                        }
                    }
                }
            };

            GenerateContentResponse response = await predictionServiceClient.GenerateContentAsync(generateContentRequest);

            string responseText = response.Candidates[0].Content.Parts[0].Text;
            Console.WriteLine(responseText);

            return responseText;
        }
    }
}
