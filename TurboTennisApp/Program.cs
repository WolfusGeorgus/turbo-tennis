using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using TurboTennisApp.Components;
using TurboTennisApp.Components.Backend;
using TurboTennisApp.Components.Backend.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TournamentContext>(options =>
    options.UseSqlite("Data Source=D:\\dev\\turbo-tennis\\tournamentV2.db"));

// Add MudBlazor services
builder.Services.AddMudServices();

builder.Services.AddScoped<TournamentService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
