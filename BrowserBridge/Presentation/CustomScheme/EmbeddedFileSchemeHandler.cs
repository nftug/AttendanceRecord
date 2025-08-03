using System.IO.Compression;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.FileProviders;

namespace BrowserBridge;

public static class EmbeddedFileSchemeHandler
{
    private static readonly Assembly _assembly = Assembly.GetEntryAssembly()!;

    private static string DetectResourceNamespacePrefix(string rootPath)
    {
        var segment = rootPath.TrimStart('/').Replace('/', '.');

        var match = _assembly.GetManifestResourceNames()
            .FirstOrDefault(name => name.Contains(segment, StringComparison.OrdinalIgnoreCase))
            ?? throw new FileNotFoundException($"Resource segment '{segment}' not found in assembly '{_assembly.FullName}'.");

        var index = match.IndexOf(segment, StringComparison.OrdinalIgnoreCase);
        var prefix = match[..index].TrimEnd('.');
        return prefix;
    }

    public static CustomSchemeHandlerResult GetEmbeddedFile(Uri uri, string rootPath)
    {
        var fileProvider = new EmbeddedFileProvider(_assembly, DetectResourceNamespacePrefix(rootPath));
        var path = Path.Combine(rootPath, uri.LocalPath.TrimStart('/'));

        var fileInfo = fileProvider.GetFileInfo(path);
        if (!fileInfo.Exists)
        {
            var gzPath = path + ".gz";
            var gzFileInfo = fileProvider.GetFileInfo(gzPath);
            if (gzFileInfo.Exists)
            {
                path = gzPath;
                fileInfo = gzFileInfo;
            }
            else
            {
                if (path.Split('/').Last() == "index.html")
                {
                    return new(new MemoryStream(Encoding.UTF8.GetBytes("404 Not Found")), "text/plain");
                }

                var indexUri = new Uri($"{uri.Scheme}://{uri.Host}/index.html");
                return GetEmbeddedFile(indexUri, rootPath);
            }
        }

        var extension = Path.GetExtension(path);
        var actualExtension = extension == ".gz"
            ? Path.GetExtension(Path.GetFileNameWithoutExtension(path))
            : extension;
        var contentType = CustomSchemeUtils.GetContentType(actualExtension);

        var stream = fileInfo.CreateReadStream();
        return new(extension == ".gz" ? new GZipStream(stream, CompressionMode.Decompress) : stream, contentType);
    }
}
