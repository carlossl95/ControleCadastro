using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using ControleCadastro.Front.Models;
using System.Net.Http.Headers;
using System.Reflection;

namespace ControleCadastro.Front.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public LoginController(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;

            // Criando um handler customizado para permitir certificados auto-assinados
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            // Inicializando o HttpClient com o handler customizado
            _httpClient = new HttpClient(handler);
        }

        // Exibir página de login (sem layout)
        public IActionResult Login()
        {
            return View(); // Retorna a view Login sem Layout
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AuthToken");  // Limpar o token JWT ou qualquer outra informação de sessão
            return RedirectToAction("Login", "Login");  // Redirecionar para a página de login
        }

        // Enviar a requisição de login para a API
        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Obtenção do ClientId e ClientSecret a partir da configuração
                    var clientId = _configuration["AppSettings:ClientId"];
                    var clientSecret = _configuration["AppSettings:ClientSecret"];
                    var linkApi = _configuration["LinkApi"];

                    string authUrl = $"{linkApi}api/Token/cadastro/{clientId}/{clientSecret}";

                    // Requisição para obter o token de autenticação
                    var authResponse = await _httpClient.GetAsync(authUrl);

                    

                    if (authResponse.IsSuccessStatusCode)
                    {
                        // Se a autenticação foi bem-sucedida, obter o token
                        var authToken = await authResponse.Content.ReadAsStringAsync();

                        // Agora, você pode passar o token e o restante dos dados para o login
                        var loginData = new
                        {
                            Email = model.Email,
                            Senha = model.Senha,
                            ClientId = clientId,
                            ClientSecret = clientSecret
                        };

                        var loginJson = JsonSerializer.Serialize(loginData);
                        var loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");

                        // Adicionar o token de autenticação ao cabeçalho da solicitação
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

                        // Fazer a requisição de login
                        var loginResponse = await _httpClient.PostAsync($"{linkApi}api/Token/geral", loginContent);
                        if (loginResponse.IsSuccessStatusCode)
                        {
                            // Sucesso! Armazenar o token JWT
                            var token = await loginResponse.Content.ReadAsStringAsync();
                            HttpContext.Session.SetString("AuthToken", token);

                            return RedirectToAction("Index", "Home"); // Redireciona para a página principal
                        }
                        else
                        {
                            var jsonResponse = await loginResponse.Content.ReadAsStringAsync();
                            var apiResponse = JsonSerializer.Deserialize<ApiResposta>(jsonResponse);

                            TempData["Message"] = apiResponse.Mensagem;
                            TempData["Codigo"] = apiResponse.Codigo;
                            return RedirectToAction("Login", "Login");
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Ocorreu um erro inesperado. Tente novamente mais tarde.";
                    }
                }

                // Em caso de erro, renderiza a mesma view de login
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro inesperado. Tente novamente mais tarde.";
                return View(model);
            }
        }
        public IActionResult Cadastro()
        {
            return View(new Cadastro());
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(Cadastro cadastro)
        {
            try
            {
                var clientId = _configuration["AppSettings:ClientId"];
                var clientSecret = _configuration["AppSettings:ClientSecret"];
                var linkApi = _configuration["LinkApi"];

                string authUrl = $"{linkApi}api/Token/cadastro/{clientId}/{clientSecret}";

                var authResponse = await _httpClient.GetAsync(authUrl);

                if (authResponse.IsSuccessStatusCode)
                {
                    var authToken = await authResponse.Content.ReadAsStringAsync();

                    var user = new
                    {
                        Nome = cadastro.Nome,
                        Email = cadastro.Email,
                        SenhaHash = cadastro.SenhaHash
                    };

                    var Json = JsonSerializer.Serialize(cadastro);
                    var cadastroContent = new StringContent(Json, Encoding.UTF8, "application/json");

                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

                    var loginResponse = await _httpClient.PostAsync($"{linkApi}api/Cliente/inserir", cadastroContent);
                    var jsonResponse = await loginResponse.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResposta>(jsonResponse);

                    if (loginResponse.IsSuccessStatusCode)
                    {
                        TempData["Message"] = apiResponse.Mensagem;
                        TempData["Codigo"] = apiResponse.Codigo;
                        return RedirectToAction("Login", "Login");
                    }
                    else
                    {
                        TempData["Message"] = apiResponse.Mensagem;
                        TempData["Codigo"] = apiResponse.Codigo;
                    }
                }
                return View();

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro inesperado. Tente novamente mais tarde.";
                return RedirectToAction("Login", "Login");
            }
        }       
        
    }
}