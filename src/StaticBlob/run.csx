using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using King.Azure;

static string defaultPage = GetEnvironmentVariable("defaultPage") ?? "index.htm";

public static HttpResponseMessage Run(HttpRequestMessage req, TraceWriter log)
{
    var pathValue = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "file", true) == 0)
        .Value;

    var path = pathValue ?? defaultPage;

    var response = new HttpResponseMessage(HttpStatusCode.OK);//REDIRECT NOT STREAM
    var stream = new FileStream(filePath, FileMode.Open);
    response.Content = new StreamContent(stream);
    response.Content.Headers.ContentType = new MediaTypeHeaderValue(GetMimeType(filePath));
    return response;
}

private static string GetEnvironmentVariable(string name)
    => System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);