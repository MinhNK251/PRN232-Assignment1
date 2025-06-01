using RepositoriesLayer;
using ServiceLayer;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5153") // FE application URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// -- SystemAccount
builder.Services.AddSingleton<ISystemAccountRepo, SystemAccountRepo>();
builder.Services.AddSingleton<ISystemAccountService, SystemAccountService>();

// -- NewsArticle
builder.Services.AddSingleton<INewsArticleRepo, NewsArticleRepo>();

// -- Category
builder.Services.AddSingleton<ICategoryRepo, CategoryRepo>();
builder.Services.AddSingleton<ICategoryService, CategoryService>();

builder.Services.AddSingleton<INewsArticleRepo, NewsArticleRepo>();
builder.Services.AddSingleton<INewsArticleService, NewsArticleService>();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();


app.MapControllers();

app.Run();
