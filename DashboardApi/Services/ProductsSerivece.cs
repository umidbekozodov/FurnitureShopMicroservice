﻿using DashboardApi.Data;
using DashboardApi.Models;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace DashboardApi.Services;

public class ProductsService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private AppDbContext _context;
    private IConnection _connection;
    private IModel _channel;

    public ProductsService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    private void HandleQueue()
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (sender, args) =>
        {
            var productJson = Encoding.UTF8.GetString(args.Body.ToArray());
            var product = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductQueue>(productJson);
            SaveProduct(product);
        };

        _channel.BasicConsume(consumer, "product_ad", false);
    }

    private void SaveProduct(ProductQueue productQueue)
    {
        _context.Productlar!.Add(new Product()
        {
            ProductId = productQueue.Id,
            ProductCount = productQueue.Count,
            ProductName = productQueue.Name,
        });

        _context.SaveChanges();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "username",
            Password = "password",
            Port = 5672
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare("product_ad", false, false, false, null);

        HandleQueue();
    }
}
