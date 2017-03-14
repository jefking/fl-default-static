using System.Net;
using System.Net.Http.Headers;
using King.Azure.Data;

static string defaultPage = GetEnvironmentVariable("defaultPage") ?? "index.htm";
static string container = GetEnvironmentVariable("Container") ?? "www";
static string rootUrl = "https://todaystestisrvqvzbftfzw.blob.core.windows.net/";

static string connection = GetEnvironmentVariable("DataStore");

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    var path = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "file", true) == 0)
        .Value ?? defaultPage;

        log.Info(path);

    var c = new Container(container, connection);
    var exists = await c.Exists(req.RequestUri.AbsolutePath);

    var l = exists ? rootUrl + container + req.RequestUri.AbsolutePath
                    : rootUrl + container + req.RequestUri.AbsolutePath + '/' + defaultPage;

    var r = new HttpResponseMessage(HttpStatusCode.Redirect);

    r.Headers.Location = new Uri(l);

    return r;
}

private static string GetEnvironmentVariable(string name)
    => System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);