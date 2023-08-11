using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(
		   Path.Combine(builder.Environment.ContentRootPath, "blobs")),
	RequestPath = "/blobs"
});


app.Run();
