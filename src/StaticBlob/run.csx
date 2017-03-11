using System.Net;
using System.Net.Http.Headers;
using King.Azure;

static string defaultPage = GetEnvironmentVariable("defaultPage") ?? "index.htm";

public static HttpResponseMessage Run(HttpRequestMessage req, TraceWriter log)
{
    var path = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "file", true) == 0)
        .Value ?? defaultPage;

    return new HttpResponseMessage(HttpStatusCode.Redirect)
    {
        Content.Headers.Location = new Uri(path)
    };
}

private static string GetEnvironmentVariable(string name)
    => System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);