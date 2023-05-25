using Gateway.Web.Host.Helpers;
using Gateway.Web.Host.Protos.Authentications;
using Gateway.Web.Host.Protos.Users;
using Gateway.Web.Host.Protos.Homes;
using Gateway.Web.Host.Protos.Devices;
using Gateway.Core.Settings;
using Gateway.Web.Host.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to DI container.
{
    IServiceCollection services = builder.Services;

    services.AddControllers();
    services.AddSwaggerGen();

    // add settings in appsettings.json
    services.Configure<MqttSettings>(configuration.GetSection("MqttSettings"));

    // configure strongly typed settings object
    services.AddTransient<JwtMiddleware>();
    services.AddScoped<IMqttService, MqttService>();

    // add gRPC client
    services.AddGrpcClient<AuthenticationGrpc.AuthenticationGrpcClient>(o =>
    {
        o.Address = new Uri(configuration["Services:AuthenticationServiceUrl"]);
    });
    services.AddGrpcClient<UserGrpc.UserGrpcClient>(o =>
    {
        o.Address = new Uri(configuration["Services:UserServiceUrl"]);
    });
    services.AddGrpcClient<HomeGrpc.HomeGrpcClient>(o =>
    {
        o.Address = new Uri(configuration["Services:HomeServiceUrl"]);
    });
    services.AddGrpcClient<DeviceGrpc.DeviceGrpcClient>(o =>
    {
        o.Address = new Uri(configuration["Services:DeviceServiceUrl"]);
    });

    // add AutoMapper
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
}
// subscribe and handle message 
MqttUtils mqttUtils = new(configuration);
mqttUtils.SubscribeAndHandleMessage();

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

app.Run("http://0.0.0.0:12000");
