using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(
		   Path.Combine(builder.Environment.ContentRootPath, "blobs")),
	RequestPath = "/blobs",
	ContentTypeProvider = new FileExtensionContentTypeProvider(
			new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
			{
				{ ".bz2", "application/x-bzip" },
			})
});

app.MapGet("/", () => "Simple ASP.NET Server");
app.Run();
