﻿using Geocod.io.Demo.Clients;
using Geocod.io.Demo.Hubs;
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
        services.AddSignalR(options =>
        {
            options.KeepAliveInterval = TimeSpan.FromSeconds(15);
        });



        services.AddMvc();
        services.AddControllers();

        services.AddSwaggerDocument();

        services.AddScoped<IGeocodIoClient, GeocodIoClient>();

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
            endpoints.MapHub<GeocodeHub>("/hubs/geocode");
        });
    }
}
