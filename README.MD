# HttpUtil

> ASP.NET HTTP helper classes

[![AppVeyor](https://ci.appveyor.com/api/projects/status/github/ryanelian/HttpUtil)](https://ci.appveyor.com/project/RyanElian/httputil)

[![NuGet HttpUtil](https://img.shields.io/nuget/v/HttpUtil.svg)](https://www.nuget.org/packages/HttpUtil/)

## HttpResponseExceptionFactory

Throws new HttpResponseException with provided status code and message.

```csharp
HttpResponseExceptionFactory.ThrowNew(HttpStatusCode.BadRequest, "Your error message here.");
```

When used in ASP.NET Web API controller method, should trigger a response like this:

```
HTTP/1.1 400 Bad Request
Content-Type: application/json; charset=utf-8
Content-Length: 

{
	"Message": "Your error message here."
}
```

## JsonContentFactory

Creates new HttpContent with provided payload, serialized as JSON string and set to "application/json" content type.

```csharp
var payload = new { foo = "bar" };
var content = JsonContentFactory.CreateNew(payload);

var client = new HttpClient();
var request = client.PostAsync("https://www.google.com", content);
```

## UnhandledExceptionLogger

Grabs an exception that triggers in an ASP.NET Web API controller, ignores HttpResponseException.

Compiles exception message and stack trace into a string. If exception is an AggregateException, all inner exceptions are flattened and then also compiled.

Compiles request context into string, which includes method, URI, header and body.

After that, passes the compiled exception and request string to the log handler.

```csharp
//at WebApiConfig.Register(HttpConfiguration config)

var logHandler = new Action<string, string>((exception, request) =>
{
	using (var db = new MyDbContext())
	{
		db.LogException(exception, request);
	}
});

config.Services.Replace(typeof(IExceptionLogger), new UnhandledExceptionLogger(logHandler));
```
