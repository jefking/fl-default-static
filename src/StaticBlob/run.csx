using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using King.Azure;

const string staticFilesFolder = "www";
static string defaultPage = GetEnvironmentVariable("defaultPage") ??  "index.htm";

public static HttpResponseMessage Run(HttpRequestMessage req, TraceWriter log)
{
    try
    {
        var filePath = GetFilePath(req, log);

        var response = new HttpResponseMessage(HttpStatusCode.OK);//REDIRECT NOT STREAM
        var stream = new FileStream(filePath, FileMode.Open);
        response.Content = new StreamContent(stream);
        response.Content.Headers.ContentType = new MediaTypeHeaderValue(GetMimeType(filePath));
        return response;
    }
    catch
    {
        return new HttpResponseMessage(HttpStatusCode.NotFound);
    }
}

private static string GetEnvironmentVariable(string name)
    => System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);