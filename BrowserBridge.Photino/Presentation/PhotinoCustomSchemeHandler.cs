namespace BrowserBridge.Photino;

public static class PhotinoCustomSchemeHandler
{
    public static Stream HandleEmbeddedFile(object sender, string scheme, string urlString, out string contentType)
    {
        var uri = new Uri(urlString);

        var result = EmbeddedFileSchemeHandler.GetEmbeddedFile(uri, "wwwroot");
        contentType = result.ContentType;

        return result.Stream;
    }
}
