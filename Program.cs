using System.Text;
using AutoMapper;
using Buy_NET.API.Contracts.Error;
using Buy_NET.API.Data.Contexts;
using Buy_NET.API.Mappers;
using Buy_NET.API.Repositories.Class;
using Buy_NET.API.Repositories.Interfaces.CategoryRepositoryInterface;
using Buy_NET.API.Repositories.Interfaces.OrderItemsRepositoryInterfaces;
using Buy_NET.API.Repositories.Interfaces.OrderRepositoryInterfaces;
using Buy_NET.API.Repositories.Interfaces.ProductRepositoryInterface;
using Buy_NET.API.Repositories.Interfaces.UserRepositoryInterface;
using Buy_NET.API.Services.Class;
using Buy_NET.API.Services.Interfaces.CategoryServiceInterfaces;
using Buy_NET.API.Services.Interfaces.OrderServiceInterfaces;
using Buy_NET.API.Services.Interfaces.ProductServiceInterfaces;
using Buy_NET.API.Services.Interfaces.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

ConfigurarServices(builder);

ConfigurarInjecaoDeDependencia(builder);

var app = builder.Build();

ConfigurarAplicacao(app);

app.Run();

// Metodo que configrua as injeções de dependencia do projeto.
static void ConfigurarInjecaoDeDependencia(WebApplicationBuilder builder)
{
    string? connectionString = builder.Configuration.GetConnectionString("PADRAO");
    builder.Services.AddDbContext<ApplicationContext>(options =>
        options.UseNpgsql(connectionString), ServiceLifetime.Transient, ServiceLifetime.Transient
    );

    var config = new MapperConfiguration(configs => {
        configs.AddProfile<UserProfile>();
        configs.AddProfile<CategoryProfile>();
        configs.AddProfile<ProductProfile>();
        configs.AddProfile<OrderProfile>();
        configs.AddProfile<OrderItemProfile>();
    });

    IMapper mapper = config.CreateMapper();

    builder.Services
    .AddSingleton(builder.Configuration)
    .AddSingleton(builder.Environment)
    .AddSingleton(mapper)
    .AddScoped<TokenService>()
    .AddScoped<IUserService, UserService>()
    .AddScoped<IUserRepository, UserRepository>()
    .AddScoped<ICategoryService, CategoryService>()
    .AddScoped<ICategoryRepository, CategoryRepository>()
    .AddScoped<IProductService,ProductService>()
    .AddScoped<IProductRepository, ProductRepository>()
    .AddScoped<IOrderService, OrderService>()
    .AddScoped<IOrderRepository, OrderRepository>();
}

// Configura o serviços da API.
static void ConfigurarServices(WebApplicationBuilder builder)
{
    builder.Services
    .AddCors()
    .AddControllers().ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    }).AddNewtonsoftJson();

    builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JTW Authorization header using the Beaerer scheme (Example: 'Bearer 12345abcdef')",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
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
                Array.Empty<string>()
            }
        });
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "BuyNET.Api", Version = "v1" });   
    });

    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })

    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["KeySecret"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
        x.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                // Suprimir o comportamento padrão do OnChallenge
                context.HandleResponse();

                // Definir o objeto de erro personalizado
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var errorResponse = new ErrorContract
                {
                    StatusCode = 401,
                    Title = "Unauthorized",
                    Description = "Você precisa estar autenticado para acessar este recurso.",
                    Date = DateTime.Now,
                };

                // Escrever o objeto de erro na resposta
                return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(errorResponse));
            }
        };
    });
}

// Configura os serviços na aplicação.
static void ConfigurarAplicacao(WebApplication app)
{
    // Configura o contexto do postgreSql para usar timestamp sem time zone.
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    app.UseDeveloperExceptionPage()
        .UseRouting();

    app.UseSwagger()
        .UseSwaggerUI(c =>
        {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BuyNet.Api v1");
                c.RoutePrefix = string.Empty;
        });

    app.UseCors(x => x
        .AllowAnyOrigin() // Permite todas as origens
        .AllowAnyMethod() // Permite todos os métodos
        .AllowAnyHeader()) // Permite todos os cabeçalhos
        .UseAuthentication();

    app.UseAuthorization();

    app.UseEndpoints(endpoints => endpoints.MapControllers());

    app.MapControllers();
}