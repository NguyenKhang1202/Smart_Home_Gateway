using Gateway.Core.Settings;
using Gateway.Web.Host.Helpers;
using Gateway.Web.Host.Protos.Authentications;
using Gateway.Web.Host.Protos.Devices;
using Gateway.Web.Host.Protos.Homes;
using Gateway.Web.Host.Protos.Notifications;
using Gateway.Web.Host.Protos.Rooms;
using Gateway.Web.Host.Protos.Users;
using Gateway.Web.Host.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to DI container.
IServiceCollection services = builder.Services;
{

    services.AddControllers();
    services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
    });

    // add settings in appsettings.json
    services.Configure<MqttSettings>(configuration.GetSection("MqttSettings"));

    // configure strongly typed settings object
    services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    services.AddTransient<JwtMiddleware>();
    services.AddScoped<IMqttService, MqttService>();
    services.AddSingleton<IFirebaseService, FirebaseService>();
    services.AddSingleton<IAppSession, AppSession>();

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
    services.AddGrpcClient<RoomGrpc.RoomGrpcClient>(o =>
    {
        o.Address = new Uri(configuration["Services:RoomServiceUrl"]);
    });
    services.AddGrpcClient<NotificationGrpc.NotificationGrpcClient>(o =>
    {
        o.Address = new Uri(configuration["Services:NotificationServiceUrl"]);
    });

    // add AutoMapper
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
}
/*// subscribe and handle message 
var firebaseService = services.BuildServiceProvider().GetService<IFirebaseService>();
var deviceGrpcClient = services.BuildServiceProvider().GetService<DeviceGrpc.DeviceGrpcClient>();
var homeGrpcClient = services.BuildServiceProvider().GetService<HomeGrpc.HomeGrpcClient>();
var roomGrpcClient = services.BuildServiceProvider().GetService<RoomGrpc.RoomGrpcClient>();
var notificationGrpcClient = services.BuildServiceProvider().GetService<NotificationGrpc.NotificationGrpcClient>();
var userGrpcClient = services.BuildServiceProvider().GetService<UserGrpc.UserGrpcClient>();
MqttUtils mqttUtils = new(configuration, userGrpcClient, firebaseService, deviceGrpcClient, homeGrpcClient, roomGrpcClient, notificationGrpcClient);
mqttUtils.SubscribeAndHandleMessage();*/

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
