using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace chatbot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebhookController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public WebhookController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://localhost:3000/api/"); // URL do servidor WAHA
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveMessage([FromBody] dynamic body)
        {
            try
            {
                string chatId = body?.from;
                string text = body?.text;

                if (!string.IsNullOrEmpty(chatId) && !string.IsNullOrEmpty(text))
                {
                    Console.WriteLine($"Mensagem recebida de {chatId}: {text}");

                    // Exemplo de resposta automática
                    await _httpClient.PostAsJsonAsync("sendMessage", new
                    {
                        to = chatId,
                        message = $"Bem vindo a Devar"
                    });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}