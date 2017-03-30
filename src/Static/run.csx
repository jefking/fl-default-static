using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using MimeTypes;
using King.Azure.Data;

static string defaultPage = GetEnvironmentVariable("DefaultPage") ?? "index.htm";
static string root = GetEnvironmentVariable("Container") ?? "www";
static string storage = GetEnvironmentVariable("Storage");

public async static Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    var filePath = req.GetQueryNameValuePairs()
                        .FirstOrDefault(q => string.Compare(q.Key, "file", true) == 0)
                        .Value;

    filePath = string.IsNullOrWhiteSpace(filePath) ? defaultPage : filePath;
    filePath = filePath.EndsWith("/") ? $"{filePath}{defaultPage}" : filePath;

    var container = new Container(root, storage);
    var exists = await container.Exists(filePath);
    filePath = exists ? filePath : $"{filePath}/{defaultPage}";

    var fileInfo = new FileInfo(filePath);
    var mimeType = MimeTypeMap.GetMimeType(fileInfo.Extension);

    log.Info($"Serving: {filePath}; With MimeType: {mimeType}");

    try
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        var stream = await container.Stream(filePath);
        response.Content = new StreamContent(stream);
        response.Content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
        return response;
    }
    catch
    {
        return new HttpResponseMessage(HttpStatusCode.NotFound);
    }
}

private static string GetEnvironmentVariable(string name)
    => System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);