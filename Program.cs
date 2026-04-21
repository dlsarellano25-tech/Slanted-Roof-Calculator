using Microsoft.AspNetCore.Mvc;
using RoofBlockCalculator;

var builder = WebApplication.CreateBuilder(args);

// Listen on the port the host gives us (Docker / Render set $PORT).
var port = Environment.GetEnvironmentVariable("PORT") ?? "6767";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// POST /api/calculate
// Computes the volume of a Pentahedral Slanted-Roof Block.
app.MapPost("/api/calculate", ([FromBody] SolidDimensions d) =>
{
    if (!VolumeCalculator.TryValidate(d, out var error))
    {
        return Results.BadRequest(new { error });
    }

    var result = VolumeCalculator.Calculate(d);
    return Results.Ok(result);
});

app.MapGet("/healthz", () => Results.Ok(new { status = "ok" }));

app.Run();
