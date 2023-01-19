using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductApi.Dto;
using ProductApi.Entities;
using RabbitMQ.Client;
using System.Text;

namespace ProductApi.Services;

public class ProductsService
{
    private readonly IMongoCollection<Product> _productsCollection;

    public ProductsService()
    {
        MongoClient _mongoClient = new MongoClient("mongodb://localhost:27017");
        IMongoDatabase _database = _mongoClient.GetDatabase("Product_db");
        _productsCollection = _database.GetCollection<Product>("Products");
    }

    public async Task<List<Product>> GetAsync() =>
        await _productsCollection.Find(_ => true).ToListAsync();

    public async Task<Product?> GetAsync(string id) =>
        await _productsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<Product> CreateAsync(ProductDto newProduct)
    {
        var product = newProduct.Adapt<Product>();
        product.Id = ObjectId.GenerateNewId(DateTime.Now).ToString();
        await _productsCollection.InsertOneAsync(product);
        SendMessage(product);
        return product;
    }

    public async Task UpdateAsync(string id, ProductDto updatedProduct) =>
        await _productsCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct.Adapt<Product>());

    public async Task RemoveAsync(string id) =>
        await _productsCollection.DeleteOneAsync(x => x.Id == id);


    public void SendMessage(Product product)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "username",
            Password = "password",
            Port = 5672
        };

        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare("product_ad", false, false, false, null);

        var productJson = Newtonsoft.Json.JsonConvert.SerializeObject(product);
        var productJsonByte = Encoding.UTF8.GetBytes(productJson);

        channel.BasicPublish("", "product_ad", null, productJsonByte);

        channel.Close();
        connection.Close();
    }
}
