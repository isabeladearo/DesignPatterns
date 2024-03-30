using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using src.Application.Decorator;
using src.Application.Presenter;
using src.Application.Repository;
using src.Application.UseCase;
using src.Infra.Collection;
using src.Infra.Database;
using src.Infra.Presenter;
using src.Infra.Repository;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoDBOptions>(options => configuration.GetSection(nameof(MongoDBOptions)).Bind(options));
builder.Services.AddSingleton<MongoDBContext<ContractCollection>>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();

builder.Services.AddScoped<IRequestHandler<Input, object>, GenerateInvoices>();
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggerDecorator<,>));

builder.Services.AddScoped<IPresenter, CsvPresenter>();
builder.Services.AddScoped<IPresenter, JsonPresenter>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
