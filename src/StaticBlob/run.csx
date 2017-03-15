using System.Net;
using System.Net.Http.Headers;

static string defaultPage = GetEnvironmentVariable("defaultPage") ?? "index.htm";
static string root = GetEnvironmentVariable("Root");

public static HttpResponseMessage Run(HttpRequestMessage req, TraceWriter log)
{
    var path = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "file", true) == 0)
        .Value ?? defaultPage;
    
    var r = new HttpResponseMessage(HttpStatusCode.Redirect);
    r.Headers.Location = new Uri($"{root}{req.RequestUri.AbsolutePath}/{path}");
    return r;
}

private static string GetEnvironmentVariable(string name)
    => System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);