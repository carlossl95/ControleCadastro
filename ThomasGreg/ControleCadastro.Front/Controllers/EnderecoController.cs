namespace ControleCadastro.Front.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Net.Http;
    using System.Text.Json;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ControleCadastro.Front.Models; 
    using System.Web;

    public class EnderecoController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _linkApi;

        public EnderecoController(HttpClient httpClient, IConfiguration configuration)
        {
            // Criando um handler customizado para permitir certificados auto-assinados
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            // Inicializando o HttpClient com o handler customizado
            _httpClient = new HttpClient(handler);
            _linkApi = configuration["LinkApi"]; // Garantir que o LinkApi esteja configurado no appsettings.json
        }

        // Exibe todos os endereços do usuário
        public async Task<IActionResult> Index()
        {
            try
            {
                var token = HttpContext.Session.GetString("AuthToken");

                if (string.IsNullOrEmpty(token))
                    return RedirectToAction("Login", "Login");

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"{_linkApi}api/Endereco/listarTodos");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var enderecoResponse = JsonSerializer.Deserialize<List<Endereco>>(json);

                    if (enderecoResponse != null && enderecoResponse.Any())
                    {
                        return View(enderecoResponse);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Nenhum endereço encontrado.";
                        return View(new List<Endereco>());
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Erro ao carregar endereços.";
                    return View(new List<Endereco>());
                }
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro inesperado. Tente novamente mais tarde.";
                return View(new List<Endereco>());
            }
        }

        public async Task<IActionResult> Inserir()
        {
            var token = HttpContext.Session.GetString("AuthToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Login");


            return View(new Endereco());

        }
        [HttpPost]
        public async Task<IActionResult> Inserir(Endereco endereco, string action)
        {
            try
            {
                var token = HttpContext.Session.GetString("AuthToken");

                if (string.IsNullOrEmpty(token))
                    return RedirectToAction("Login", "Login");

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var json = JsonSerializer.Serialize(endereco);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_linkApi}api/Endereco/inserir", content);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResposta>(jsonResponse);

                if (response.IsSuccessStatusCode)
                {
                    if (apiResponse != null && apiResponse.Codigo == 200)
                    {
                        TempData["Message"] = apiResponse.Mensagem;
                        TempData["Codigo"] = apiResponse.Codigo;
                        return RedirectToAction("Index"); // Redirect after success
                    }
                    else
                    {
                        TempData["Message"] = apiResponse.Mensagem;
                        TempData["Codigo"] = apiResponse.Codigo;
                        return View(endereco);
                    }
                }

                TempData["Message"] = response.ReasonPhrase;
                TempData["Codigo"] = response.StatusCode;
                return View(endereco);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["Codigo"] = 500;
                return View(new Endereco());
            }

        }
        // Ação de Editar (GET)
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var token = HttpContext.Session.GetString("AuthToken");

                if (string.IsNullOrEmpty(token))
                    return RedirectToAction("Login", "Login");

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"{_linkApi}api/Endereco/consultarPorId{id}");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResposta>(jsonResponse);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var enderecoResponse = JsonSerializer.Deserialize<Endereco>(json);

                    if (enderecoResponse == null)
                    {
                        ViewBag.ErrorMessage = "Erro ao carregar o endereço.";
                        return RedirectToAction("Index");
                    }

                    return View(enderecoResponse);
                }

                TempData["Message"] = response.ReasonPhrase;
                TempData["Codigo"] = response.StatusCode;
                return View(new Endereco());
            }
            catch (Exception ex)
            {
                // Em caso de exceção inesperada
                ViewBag.ErrorMessage = "Ocorreu um erro inesperado. Tente novamente mais tarde.";
                return View(new Endereco());
            }
        }

        // Ação POST para salvar o endereço editado
        [HttpPost]
        public async Task<IActionResult> Edit(Endereco endereco, string action)
        {
            try
            {
                var token = HttpContext.Session.GetString("AuthToken");

                if (string.IsNullOrEmpty(token))
                    return RedirectToAction("Login", "Login");

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var json = JsonSerializer.Serialize(endereco);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_linkApi}api/Endereco/editar", content);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResposta>(jsonResponse);

                if (response.IsSuccessStatusCode)
                {

                    if (apiResponse != null && apiResponse.Codigo == 200)
                    {
                        TempData["Message"] = apiResponse.Mensagem;
                        TempData["Codigo"] = apiResponse.Codigo;
                        return RedirectToAction("Index"); // Redirect after success
                    }
                    else
                    {
                        TempData["Message"] = apiResponse.Mensagem;
                        TempData["Codigo"] = apiResponse.Codigo;
                        return View(endereco);
                    }
                }

                TempData["Message"] = response.ReasonPhrase;
                TempData["Codigo"] = response.StatusCode;
                return View(endereco);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["Codigo"] = 500;
                return View(new Endereco());
            }
        }


        // Ação de Excluir
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var token = HttpContext.Session.GetString("AuthToken");

                if (string.IsNullOrEmpty(token))
                    return RedirectToAction("Login", "Login");

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"{_linkApi}api/Endereco/{id}");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResposta>(jsonResponse);

                if (response.IsSuccessStatusCode)
                {
                    if (apiResponse != null && apiResponse.Codigo == 200)
                    {
                        TempData["Message"] = apiResponse.Mensagem;
                        TempData["Codigo"] = apiResponse.Codigo;
                        return RedirectToAction("Index"); // Redirect after success
                    }
                    else
                    {
                        TempData["Message"] = apiResponse.Mensagem;
                        TempData["Codigo"] = apiResponse.Codigo;
                        return RedirectToAction("Index"); // Redirect after success
                    }
                }

                TempData["Message"] = response.ReasonPhrase;
                TempData["Codigo"] = response.StatusCode;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["Codigo"] = 500;
                return RedirectToAction("Index"); // Redirect after success
            }
            
        }

        public class EnderecoApiResponse
        {
            public List<Endereco> Value { get; set; }
        }
    }
}
