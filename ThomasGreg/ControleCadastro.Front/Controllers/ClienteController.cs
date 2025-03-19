namespace ControleCadastro.Front.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using ControleCadastro.Front.Models;
    using System.IO;
    using System.Reflection;
    using System.Text;

    public class ClienteController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _linkApi;

        public ClienteController(HttpClient httpClient, IConfiguration configuration)
        {
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            _httpClient = new HttpClient(handler);
            _linkApi = configuration["LinkApi"];
        }

       

        // Exibir dados do cliente
        public async Task<IActionResult> ClienteIndex()
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

        // Atualizar imagem de perfil
        public async Task<IActionResult> AtualizarImagem(IFormFile imagem)
        {
            try
            {
                if (imagem != null && imagem.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagem.CopyToAsync(memoryStream);

                        var imagemBase64 = Convert.ToBase64String(memoryStream.ToArray());

                        var dadosImagem = new
                        {
                            Logotipo = imagemBase64
                        };

                        var json = JsonSerializer.Serialize(dadosImagem);

                        var token = HttpContext.Session.GetString("AuthToken");

                        if (string.IsNullOrEmpty(token))
                            return RedirectToAction("Login", "Login");

                        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        var response = await _httpClient.PutAsync($"{_linkApi}api/Cliente/atualizarDados", content);

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonResponse = await response.Content.ReadAsStringAsync();
                            var apiResponse = JsonSerializer.Deserialize<ApiResposta>(jsonResponse);

                            TempData["Message"] = apiResponse.Mensagem;
                            TempData["Codigo"] = apiResponse.Codigo;

                            return RedirectToAction("ClienteIndex", "Cliente"); 
                        }
                        else
                        {

                            TempData["Message"] = "Nenhuma imagem foi fornecida.";
                            TempData["Codigo"] = 400;

                            return RedirectToAction("ClienteIndex", "Cliente"); 

                        }
                    }
                }
                else
                {
                    TempData["Message"] = "Nenhuma imagem foi fornecida.";
                    TempData["Codigo"] = 400;
                    return RedirectToAction("ClienteIndex", "Cliente"); 

                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Ocorreu um erro: {ex.Message}";
                TempData["Codigo"] = 500;  
                return View();
            }
        }


        [HttpGet]
        public IActionResult AtualizarSenha()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarSenha(string novaSenha, string confirmarSenha)
        {
            try
            {
                if (novaSenha != confirmarSenha)
                {
                    TempData["Message"] = "As senhas não coincidem.";
                    return RedirectToAction("Perfil");
                }

                var token = HttpContext.Session.GetString("AuthToken");

                if (string.IsNullOrEmpty(token))
                    return RedirectToAction("Login", "Login");

                var loginData = new
                {
                    SenhaHash = novaSenha
                };

                var loginJson = JsonSerializer.Serialize(loginData);
                var content = new StringContent(loginJson, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PutAsync($"{_linkApi}api/Cliente/atualizarDados", content);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResposta>(jsonResponse);

                if (response.IsSuccessStatusCode && apiResponse.Codigo == 200)
                {

                    TempData["Message"] = apiResponse.Mensagem;
                    TempData["Codigo"] = apiResponse.Codigo;
                    return RedirectToAction("ClienteIndex");
                }
                else
                {

                    TempData["Message"] = apiResponse.Mensagem;
                    TempData["Codigo"] = apiResponse.Codigo;
                    return RedirectToAction("ClienteIndex");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Erro ao atualizar senha.";
                return RedirectToAction("ClienteIndex");
            }
        }

        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<IActionResult> GetLogo()
        {
            byte[] logotipo;

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "icon", "user.png");

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

                    logotipo = clienteResponse?.Logotipo;

                    if (logotipo == null && logotipo.Length < 0)
                        logotipo = System.IO.File.ReadAllBytes(imagePath);
                }
                else
                {
                    logotipo = System.IO.File.ReadAllBytes(imagePath);
                }
            }
            catch (Exception ex)
            {
                logotipo = System.IO.File.ReadAllBytes(imagePath);
            }

            return File(logotipo, "image/jpeg");

        }
    }
}


