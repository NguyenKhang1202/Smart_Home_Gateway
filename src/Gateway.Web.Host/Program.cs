using Gateway.Web.Host.Helpers;
using Gateway.Web.Host.Protos.Authentications;
using Gateway.Web.Host.Protos.Users;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

// Add services to DI container.
{
    IServiceCollection services = builder.Services;

    services.AddControllers();
    services.AddSwaggerGen();

    // configure strongly typed settings object
    services.AddTransient<JwtMiddleware>();

    // add gRPC client
    services.AddGrpcClient<AuthenticationGrpc.AuthenticationGrpcClient>(o =>
    {
        o.Address = new Uri(configuration["Services:AuthenticationServiceUrl"]);
    });
    services.AddGrpcClient<UserGrpc.UserGrpcClient>(o =>
    {
        o.Address = new Uri(configuration["Services:UserServiceUrl"]);
    });

    // add AutoMapper
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
}

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // global cors policy
    app.UseCors();

    // custom jwt auth middleware
    app.UseMiddleware<JwtMiddleware>();

    app.MapControllers();
}

app.Run("http://localhost:4000");
