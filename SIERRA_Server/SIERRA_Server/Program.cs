using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Infra;
using SIERRA_Server.Models.Interfaces;
using SIERRA_Server.Models.Repository.EFRepository;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(Options => {
    Options.UseSqlServer(builder.Configuration.GetConnectionString("Sierra"));
    });

builder.Services.AddControllers();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var MyAllowSpecificOrigins = "AllowAny";
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowSpecificOrigins,
        policy => policy.WithOrigins("*").WithHeaders("*").WithMethods("*"));
});
//DI注入
builder.Services.AddScoped<IMemberCouponRepository,MemberCouponEFRepository>();
builder.Services.AddScoped<MemberEFRepository>();
builder.Services.AddScoped<HashUtility>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();

// Allow CORS
app.UseCors("AllowOrigin");
app.UseHttpsRedirection();

// 驗證(登入所需加的)
app.UseCookiePolicy();
app.UseAuthentication();
// 授權(原本就有)
app.UseAuthorization();

app.MapControllers();

app.Run();
