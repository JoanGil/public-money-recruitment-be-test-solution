using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using System.Collections.Generic;

using VacationRental.Infrastructure;
using VacationRental.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<VacationRentalContext>();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Vacation rental information", Version = "v1" }));



builder.Services.AddSingleton<IDictionary<int, Rental>>(new Dictionary<int, Rental>());
builder.Services.AddSingleton<IDictionary<int, Booking>>(new Dictionary<int, Booking>());

var app = builder.Build();

//-----------------------------------------------------

if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseSwagger();
app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1"));

await app.RunAsync();