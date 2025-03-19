using ControleCadastro.Front.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace ControleCadastro.Front.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _linkApi;

        public HomeController(HttpClient httpClient, IConfiguration configuration)
        {
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            _httpClient = new HttpClient(handler);
            _linkApi = configuration["LinkApi"];
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var token = HttpContext.Session.GetString("AuthToken");
                var id = HttpContext.Session.GetString("AuthToken");

                if (string.IsNullOrEmpty(token))
                    return RedirectToAction("Login", "Login");

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"{_linkApi}api/Cliente/perfil");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var clienteResponse = JsonSerializer.Deserialize<Cliente>(json);

                    return View(clienteResponse);
                }

                ViewBag.ErrorMessage = "Erro ao carregar perfil.";
                return View(new Cliente());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erro inesperado. Tente novamente mais tarde.";
                return View(new Cliente());
            }
        }
    }
}
