namespace ControleCadastro.Front
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configurações de sessão para armazenar o token JWT
            builder.Services.AddDistributedMemoryCache(); // Necessário para armazenar dados em memória
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Defina o tempo de expiração da sessão
                options.Cookie.HttpOnly = true; // Aumenta a segurança
                options.Cookie.IsEssential = true; // Permite que a sessão funcione mesmo com configurações de GDPR
            });

            builder.Services.AddHttpClient(); // Adiciona serviços de HTTP client
            builder.Services.AddControllersWithViews(); // Adiciona suporte para controllers e views                      

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Usando o redirecionamento seguro e outros middlewares essenciais
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Adicionando o middleware de sessão antes do middleware de autorização
            app.UseSession(); // Este é o middleware que habilita a sessão

            // Middleware de roteamento
            app.UseRouting();

            // Middleware de autorização
            app.UseAuthorization();

            // Mapeamento da rota padrão
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Inicia a aplicação
            app.Run();
        }
    }
}
