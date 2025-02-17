//using ProductAPI.infrastructure.DependencyInjection;
//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//builder.Services.AddInfrastructureService(builder.Configuration);
//var app = builder.Build();

//app.UseInfrastructurePolicy();
//app.UseSwagger();
//app.UseSwaggerUI();
//app.UseHttpsRedirection();
//app.UseAuthorization();
//app.MapControllers();
//app.Run();


using ProductAPI.infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureService(builder.Configuration); // No changes needed here
builder.Services.AddHealthChecks();
var app = builder.Build();

app.UseInfrastructurePolicy();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductAPI v1");
        c.RoutePrefix = string.Empty; // Swagger UI is at the root
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseHealthChecks("/healthz");
app.Run();


