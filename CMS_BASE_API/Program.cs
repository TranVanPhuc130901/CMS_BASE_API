using CMS_BASE_API.Authorization;
using CMS_BL;
using CMS_BL.ArticleBL;
using CMS_BL.ProductBL;
using CMS_Common;
using CMS_Common.Database;
using CMS_Common.Mappe;
using CMS_Common.Model;
using CMS_DL;
using CMS_DL.ProductDL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMvc();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PermissionManager", policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser();
        policyBuilder.Requirements.Add(new PermissionRequirement());
    });

    options.AddPolicy("UserPermission", policyBl =>
    {
        policyBl.RequireAuthenticatedUser();
        policyBl.RequireRole("admin");
        policyBl.Requirements.Add(new PermissionRequirement());
    });
});
builder.Services.AddDbContext<MyDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("MyDB"),
    b => b.MigrationsAssembly("CMS_BASE_API"));
    
});
builder.Services.AddCors(p => p.AddPolicy("MyCors", build =>
{
    build.WithOrigins("http://localhost:8080");
    build.AllowAnyHeader();
    build.AllowAnyMethod();
    build.AllowAnyOrigin();
}));

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }

    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(options =>
{
    // định nghĩa 1 bản mật cho swagger vơi tên là oauth2 với kiểu là apikey dùng JWT bearer để bảo mật
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization using the Bearer scheme (\"bearer {token}\")",
        // định nghĩa rằng mã thông báo JWT sẽ được gửi trong Header của yêu cầu API
        In = ParameterLocation.Header,
        // chỉ định tên của header
        Name = "Authorization",
        // chỉ định mật khẩu là kiẻu apiKey
        Type = SecuritySchemeType.ApiKey
    });
    // áp dụng các bộ lọc bảo mật cho các hoạt động Api của Swagger
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
// thêm dịch vụ xác thực và cung cấp phương thức xác thực là JwtBearerDefaults
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // TokenValidationParameters là 1 đối tượng chứa thông tin cần thiết để xác thực, với kiểu là TokenValidationParameter
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // ValidateIssuerSigningKey là giá trị xác định xem khoá xác thực có hợp lệ không,
            // giá trị true cho phép xác thực với khoá xác thực để tạo Jwt
            ValidateIssuerSigningKey = true,
            // IssuerSigningKey là 1 khoá xác thực được tạo bằng cách sử dụng giá trị chuỗi được lấy từ appSetting
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("AppSettings:Token").Value)),
            // kiểm tra tên có hợp lệ không, nêý là false thì xác thực với bất kì tên nào
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });


// Add services to the container.



builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));
//builder.Services.AddScoped(typeof(IBaseBL<,>), typeof(BaseBL<,>));
builder.Services.AddScoped(typeof(IBaseBL<ProductImage, ProductImageModel>), typeof(BaseBL<ProductImage, ProductImageModel>));
builder.Services.AddScoped(typeof(IBaseBL<ProductContent, ProductContentModel>), typeof(BaseBL<ProductContent, ProductContentModel>));
builder.Services.AddScoped(typeof(IBaseBL<Product, ProductModel>), typeof(BaseBL<Product, ProductModel>));
builder.Services.AddScoped(typeof(IBaseBL<ProductMetaData, ProductMetaDataModel>), typeof(BaseBL<ProductMetaData, ProductMetaDataModel>));
builder.Services.AddScoped(typeof(IBaseBL<ProductPrice, ProductPriceModel>), typeof(BaseBL<ProductPrice, ProductPriceModel>));
builder.Services.AddScoped(typeof(IBaseBL<Category, CategoryModel>), typeof(BaseBL<Category, CategoryModel>));
builder.Services.AddScoped(typeof(IBaseBL<Article, ArticleModel>), typeof(BaseBL<Article, ArticleModel>));

builder.Services.AddScoped<IUserAuth, UserAuth>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

builder.Services.AddScoped<IProductDL, ProductDL>();
builder.Services.AddScoped<IProductBL<Product, ProductModel>, ProductBL>();

builder.Services.AddScoped<IArticleBL<Article, ArticleModel>, ArticleBL>();
builder.Services.AddScoped(typeof(IBaseBL<Article, ArticleModel>), typeof(BaseBL<Article, ArticleModel>));
//builder.Services.AddScoped<ICategoryDL, CategoryDL>();
//builder.Services.AddScoped<IProductBL<Product, ProductModel>, CategoryBL>();

var app = builder.Build();

app.UseCors("MyCors");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
