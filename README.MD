#HttpUtil

> .NET HTTP helper classes

[![AppVeyor](https://ci.appveyor.com/api/projects/status/github/ryanelian/HttpUtil)](https://ci.appveyor.com/project/RyanElian/httputil)

[![NuGet HttpUtil](https://img.shields.io/nuget/v/HttpUtil.svg)](https://www.nuget.org/packages/HttpUtil/)

1. HttpResponseExceptionFactory

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

2. JsonContentFactory

	Creates new HttpContent with provided payload, serialized as JSON string and set to "application/json" content type.

	```csharp
	var client = new HttpClient();
	var payload = new { foo = "bar" };
	var content = JsonContentFactory.CreateNew(payload);
	var request = client.PostAsync("https://www.google.com", content);
	```
	