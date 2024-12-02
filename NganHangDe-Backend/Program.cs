using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NganHangDe_Backend.Data.IRepositories;
using NganHangDe_Backend.Data.Repositories;
using NganHangDe_Backend.ServerSettings;
using NganHangDe_Backend.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ExamDbSetting>(builder.Configuration.GetSection("ExamDb"));
builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddSingleton<IQuestionRepository, QuestionRepository>();
builder.Services.AddSingleton<ISubjectRepository, SubjectRepository>();
builder.Services.AddSingleton<IQuestionGroupRepository, QuestionGroupRepository>();
builder.Services.AddSingleton<IExamRepository, ExamRepository>();
builder.Services.AddSingleton<IGeminiChatService, GeminiChatService>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

// cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {

    });


builder.Services.AddSpaStaticFiles(options =>
{
    options.RootPath = "wwwroot";
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \" Authorization : Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    //c.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference
    //            {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = "Bearer"
    //            }
    //        },
    //        new string[] { }
    //    }
    //});
    c.OperationFilter<AddResponseHeadersFilter>();
});

// Don't need cors

// Don't need https redirection

// add authentication jwt
builder.Services
.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    // options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    // options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
.AddJwtBearer(options =>
{
    var jwtSetting = builder.Configuration.GetSection("Jwt").Get<JwtSetting>();
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSetting.Issuer,
        ValidAudience = jwtSetting.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Secret))
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();

app.UseSpaStaticFiles();

app.UseSpa(options =>
{
    options.Options.SourcePath = "wwwroot";
    if (app.Environment.IsDevelopment())
    {
        options.UseProxyToSpaDevelopmentServer("http://localhost:5173");
    }
});





app.Run();

