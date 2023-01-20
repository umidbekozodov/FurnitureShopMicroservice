using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Entities;
using System.Text;
using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using ProductApi.Services;
using ProductApi.Dto;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/controller")]
public class ProductsController : ControllerBase
{
    private readonly ProductsService _productsService;

    public ProductsController(ProductsService productsService) =>
        _productsService = productsService;

    [HttpGet]
    public async Task<List<Product>> Get() =>
        await _productsService.GetAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> Get(string id)
    {
        var product = await _productsService.GetAsync(id);

        return product ?? (ActionResult<Product>)NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Post(ProductDto newProduct)
    {
        var createdProduct = await _productsService.CreateAsync(newProduct);

        return CreatedAtAction(nameof(Get), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, ProductDto updatedProduct)
    {
        await _productsService.UpdateAsync(id, updatedProduct);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var product= await _productsService.GetAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        await _productsService.RemoveAsync(id);

        return NoContent();
    }
}
