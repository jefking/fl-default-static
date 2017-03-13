using System.Net;
using System.Net.Http.Headers;
using King.Azure.Data;

static string defaultPage = GetEnvironmentVariable("defaultPage") ?? "index.htm";
static string container = GetEnvironmentVariable("Container") ?? "www";
static string connection = GetEnvironmentVariable("DataStore");

public static HttpResponseMessage Run(HttpRequestMessage req, TraceWriter log)
{
    var path = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "file", true) == 0)
        .Value ?? defaultPage;

    var c = new Container(container, connection);

    var r = new HttpResponseMessage(HttpStatusCode.Redirect)
    {
        Status = "301 Moved Permanently",
    };
   // r.Content.Headers.Location = new Uri(path);
    r.AddHeader("Location", "http://www.google.com");
    return r;
}

private static string GetEnvironmentVariable(string name)
    => System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);