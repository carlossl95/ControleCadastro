namespace ControleCadastro.Front
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configura��es de sess�o para armazenar o token JWT
            builder.Services.AddDistributedMemoryCache(); // Necess�rio para armazenar dados em mem�ria
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Defina o tempo de expira��o da sess�o
                options.Cookie.HttpOnly = true; // Aumenta a seguran�a
                options.Cookie.IsEssential = true; // Permite que a sess�o funcione mesmo com configura��es de GDPR
            });

            builder.Services.AddHttpClient(); // Adiciona servi�os de HTTP client
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

            // Adicionando o middleware de sess�o antes do middleware de autoriza��o
            app.UseSession(); // Este � o middleware que habilita a sess�o

            // Middleware de roteamento
            app.UseRouting();

            // Middleware de autoriza��o
            app.UseAuthorization();

            // Mapeamento da rota padr�o
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Inicia a aplica��o
            app.Run();
        }
    }
}
