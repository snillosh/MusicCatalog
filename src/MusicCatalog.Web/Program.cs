using MusicCatalog.Web;
using MusicCatalog.Web.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddWeb()
    .AddApiClients(builder.Configuration)
    .AddApplicationServices()
    .AddExternalServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
