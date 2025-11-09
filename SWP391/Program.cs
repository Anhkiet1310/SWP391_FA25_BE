using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repositories;
using Repositories.DBContext;
using Services;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// CORS configuration: read allowed origins from configuration (appsettings.json)
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                     ?? new[] { "http://40.82.145.164:8080" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // If you don't need cookies/auth credentials from browser, remove this and Use AllowAnyOrigin() instead.
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Co_ownershipAndCost_sharingDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySqlConnection"),
        new MySqlServerVersion(new Version(8, 0, 43))));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập 'Bearer {token}'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHttpClient();

//DI Controller
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<CarRepository>();
builder.Services.AddScoped<GroupRepository>();
builder.Services.AddScoped<ContractRepository>();
builder.Services.AddScoped<ScheduleRepository>();
builder.Services.AddScoped<FormRepository>();
builder.Services.AddScoped<VoteRepository>();
builder.Services.AddScoped<CarUserRepository>();
builder.Services.AddScoped<PaymentRepository>();
builder.Services.AddScoped<TransactionRepository>();
builder.Services.AddScoped<PercentOwnershipRepository>();

//DI Service
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CarService>();
builder.Services.AddScoped<GroupService>();
builder.Services.AddScoped<ContractService>();
builder.Services.AddScoped<ScheduleService>();
builder.Services.AddScoped<FormService>();
builder.Services.AddScoped<VoteService>();
builder.Services.AddScoped<CarUserService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<OcrService>();
builder.Services.AddHttpClient<OcrService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<PercentOwnershipService>();

//PayPal
builder.Services.AddScoped<PaymentPayPalRepository>(provider =>
    new PaymentPayPalRepository(
        builder.Configuration["PayPal:ClientId"],
        builder.Configuration["PayPal:Secret"]
    )
);

builder.Services.AddScoped<PaymentPayOSRepository>(provider =>
    new PaymentPayOSRepository(
        builder.Configuration["PayOS:ClientId"],
        builder.Configuration["PayOS:ApiKey"],
        builder.Configuration["PayOS:ChecksumKey"]
    )
);

var app = builder.Build();

// Use CORS policy
app.UseCors("DefaultCorsPolicy");

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

// Authentication + Authorization middlewares
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using Repositories;
//using Repositories.DBContext;
//using Services;
//using System.Text;
//using System.Text.Json.Serialization;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
//        options.JsonSerializerOptions.WriteIndented = true;
//    });
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<Co_ownershipAndCost_sharingDbContext>(options =>
//    options.UseMySql(builder.Configuration.GetConnectionString("MySqlConnection"),
//        new MySqlServerVersion(new Version(8, 0, 43))));


//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
//        };
//    });

//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        Scheme = "bearer",
//        BearerFormat = "JWT",
//        In = ParameterLocation.Header,
//        Description = "Nhập 'Bearer {token}'"
//    });

//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            new string[] {}
//        }
//    });
//});

//builder.Services.AddAuthorization();

//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

////DI Controller
//builder.Services.AddScoped<UserRepository>();
//builder.Services.AddScoped<CarRepository>();
//builder.Services.AddScoped<GroupRepository>();
//builder.Services.AddScoped<ContractRepository>();
//builder.Services.AddScoped<ScheduleRepository>();
//builder.Services.AddScoped<FormRepository>();
//builder.Services.AddScoped<VoteRepository>();
//builder.Services.AddScoped<CarUserRepository>();
//builder.Services.AddScoped<PaymentRepository>();
//builder.Services.AddScoped<TransactionRepository>();
//builder.Services.AddScoped<PercentOwnershipRepository>();

////DI Service
//builder.Services.AddScoped<UserService>();
//builder.Services.AddScoped<CarService>();
//builder.Services.AddScoped<GroupService>();
//builder.Services.AddScoped<ContractService>();
//builder.Services.AddScoped<ScheduleService>();
//builder.Services.AddScoped<FormService>();
//builder.Services.AddScoped<VoteService>();
//builder.Services.AddScoped<CarUserService>();
//builder.Services.AddScoped<PaymentService>();
//builder.Services.AddScoped<OcrService>();
//builder.Services.AddHttpClient<OcrService>();
//builder.Services.AddScoped<TransactionService>();
//builder.Services.AddScoped<PercentOwnershipService>();

////PayPal
//builder.Services.AddScoped<PaymentPayPalRepository>(provider =>
//    new PaymentPayPalRepository(
//        builder.Configuration["PayPal:ClientId"],
//        builder.Configuration["PayPal:Secret"]
//    )
//);

//var app = builder.Build();

//// CORS configuration: read allowed origins from configuration (appsettings.json)
//var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
//                     ?? new[] { "http://40.82.145.164:8080/" };

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("DefaultCorsPolicy", policy =>
//    {
//        policy.WithOrigins(allowedOrigins)
//              .AllowAnyMethod()
//              .AllowAnyHeader();
//    });
//});

////CORS
//app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());




//// Configure the HTTP request pipeline.
////if (app.Environment.IsDevelopment())
////{
//app.UseSwagger();
//    app.UseSwaggerUI();
////}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
