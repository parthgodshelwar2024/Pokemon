using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    //internal class AiOptions
    //{
    //}
    public class AiOptions
    {
        public string Provider { get; set; } = "OpenAI"; // OpenAI | OpenRouter
        public string OpenAiApiKey { get; set; } = string.Empty;
        public string OpenRouterApiKey { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int MaxTokens { get; set; } = 300;
    }
}
