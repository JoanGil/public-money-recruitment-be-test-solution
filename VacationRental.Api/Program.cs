using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using VacationRental.Core.Interfaces;
using VacationRental.Core.Services;
using VacationRental.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<VacationRentalContext>();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Vacation rental information", Version = "v1" }));

builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();

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