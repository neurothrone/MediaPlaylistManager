using MediaPlaylistManager.DAL.EFCore.Shared.Interfaces;
using MediaPlaylistManager.WebApi.Data;
using MediaPlaylistManager.WebApi.Endpoints;
using MediaPlaylistManager.WebApi.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApiDbContext>(options => options.UseSqlite(
    builder.Configuration.GetConnectionString(nameof(ApiDbContext)) ??
    throw new InvalidOperationException($"Connection string '{nameof(ApiDbContext)}' not found.")));

builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddScoped<IMediaItemRepository, MediaItemRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// !: Apply migrations and create the database at runtime if it doesn't exist
// Source:
// https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#apply-migrations-at-runtime
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApiDbContext>();
    context.Database.Migrate();
}

app.MapPlaylistEndpoints();
app.MapMediaItemEndpoints();

app.Run();