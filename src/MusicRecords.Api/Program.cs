using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MusicRecords.Api.Auth;
using MusicRecords.Api.Repositories;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    options.OperationFilter<BearerOperationFilter>();
});
var auth = builder.Configuration.GetSection("Auth").Get<AuthSettings>() ?? new AuthSettings();
var signingKey = auth.GetSigningKey();
builder.Services.AddSingleton(auth);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = AuthSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = AuthSettings.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        NameClaimType = "sub",
        RoleClaimType = "role"
    };
});
builder.Services.AddAuthorization();
var sqlServer = builder.Configuration.GetConnectionString("MusicRecords");
builder.Services.AddDbContext<MusicRecordsDbContext>(options =>
{
    if (string.IsNullOrWhiteSpace(sqlServer))
        options.UseSqlite(builder.Configuration.GetConnectionString("LocalMusicRecords") ?? "Data Source=musicrecords.db");
    else
        options.UseSqlServer(sqlServer, sql => sql.MigrationsHistoryTable("__MusicRecordsMigrations"));
});
builder.Services.AddScoped<IMusicRecordRepository, MusicRecordRepository>();
var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? ["http://localhost:5500", "http://127.0.0.1:5500"];
builder.Services.AddCors(options => options.AddPolicy("Frontend", policy =>
    policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod()));
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<MusicRecordsDbContext>().Database.Migrate();
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
