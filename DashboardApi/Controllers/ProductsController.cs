using DashboardApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace DashboardApi.Controllers;

[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromServices] AppDbContext context)
    {
        return Ok(await context.Productlar!.ToListAsync());
    }
}
