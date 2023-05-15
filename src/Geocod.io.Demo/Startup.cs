using Geocod.io.Demo.Clients;
using RestSharp;

namespace Geocod.io.Demo;

public class Startup
{
    public Startup(IWebHostEnvironment env)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
            .AddEnvironmentVariables();
        Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // // Add framework services.
        services.AddMvc();

        services.AddControllers();

        services.AddSwaggerDocument();

        services.AddScoped<IGeocodIoClient, GeocodIoClient>();
        services.AddScoped<IRestClient>(_ =>
        {
            // TODO: Add URL to the appsettings.json file
            var client =  new RestClient("https://api.geocod.io/v1.7/");

            // TODO: Add the API Key to the appsettings.json file
            client.DefaultParameters.AddParameter(new QueryParameter("api_key","00fe16f0646546060e81540514406e1214f1818"));

            return client;
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();
        else
            app.UseExceptionHandler("/Home/Error");

        var options = new DefaultFilesOptions();
        options.DefaultFileNames.Clear();
        options.DefaultFileNames.Add("index.html");

        app.UseDefaultFiles(options);

        app.UseStaticFiles();

        app.UseOpenApi();
        app.UseSwaggerUi3();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute("api", "{controller=Home}/{action=Index}/{id?}");
        });
    }
}
