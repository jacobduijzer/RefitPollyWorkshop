<!-- $theme: default -->

[![Build status](https://ci.appveyor.com/api/projects/status/srpvvexgfu6f2ovs/branch/master?svg=true)](https://ci.appveyor.com/project/jacobduijzer/refitpollyworkshop/branch/master) [![Sonar Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=RefitExample&metric=alert_status)](https://sonarcloud.io/dashboard?id=RefitExample) [![Code Coverage](https://sonarcloud.io/api/project_badges/measure?project=RefitExample&metric=coverage)](https://sonarcloud.io/dashboard?id=RefitExample)

[![Windows Build history](https://buildstats.info/appveyor/chart/jacobduijzer/refitpollyworkshop?branch=master&includeBuildsFromPullRequest=false)](https://ci.appveyor.com/project/jacobduijzer/refitpollyworkshop/history?branch=master)

# Workshop Refit & Polly

###### Jacob Duijzer, January 2018

---

# Refit

Refit: The automatic type-safe REST library for .NET Core, Xamarin and .NET

```
public interface IRemoteApi
{
	[Get("/posts")]
	Task<IEnumerable<Post>> GetAllPostsAsync();

	[Post("/posts")]
	[Headers("Authorization: Bearer")]
	Task<Post> AddPostAsync([Body]Post post);

	[Delete("/posts/{id}")]
	[Headers("Authorization: Bearer")]
	Task DeletePostByIdAsync(int id);
}
```

---

# Refit - sample code

```
var remoteApi = RestService
	.For<IRemoteApi>("http://localhost:3000");


var posts = await remoteApi.GetAllPostsAsync();

var singlePost = await remoteApi.GetPostByIdAsync(1);
```

---

# Refit - Scenario 1

## Simple api calls

---

# Refit - Scenario 2

## Logging

---

# Refit - Scenario 3

## Authenticated api calls

---

# Polly

Policies:

- Retry
- CircuitBreaker
- Timeout
- Bulkhead Isolation
- Cache
- Fallback
- PolicyWrap

---

# Polly - sample code

```

// Retry multiple times, calling an action on each retry
// with the current exception, retry count and context
// provided to Execute()
var _retryPolicy = Policy.Handle<SomeExceptionType>()
	.Retry(3, (exception, retryCount, context) =>
	{
    	// do something to prevent the next exception
	});

await _retryPolicy.ExecuteAsync(
	remoteApi.GetAllPostsAsync()
);
```

---

# Polly - Scenario 1

## Retry

---

# Polly - Scenario 2

## Fallback

---

# Polly - Scenario 3

## CircuitBreaker

---

# Polly - Scenario 4

## Retry with fallback

---

# Polly - Scenario 5

## CircuitBreakerWithRetryAndFallBack

---

# HttpClientHandler

- [MSDN Documentation](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclienthandler)
- [Scott Hanselmans post With REFIT](https://www.hanselman.com/blog/UsingASPNETCore21sHttpClientFactoryWithRefitsRESTLibrary.aspx)

---

# Polly - Unit testing

---

# Alternatives - Flurl

    C#!
    // Flurl will use 1 HttpClient instance per host
    var person = await "https://api.com"
        .AppendPathSegment("person")
        .SetQueryParams(new { a = 1, b = 2 })
        .WithOAuthBearerToken("my_oauth_token")
        .PostJsonAsync(new
        {
            first_name = "Claire",
            last_name = "Underwood"
        })
        .ReceiveJson<Person>();

- [Flur.io](https://flurl.io/)
- [Blog post](https://jeremylindsayni.wordpress.com/2019/01/01/using-polly-and-flurl-to-improve-your-website/)

---

# Alteratives - DalSoft.RestClient

    C#!
    var config = new Config().UseFormUrlEncodedHandler();

    dynamic restClient = new RestClient("https://jsonplaceholder.typicode.com", config);
    var user = new User { name="foo", email="foo@bar.com" };

    //POST name=foo&email=foo@bar.com  https://jsonplaceholder.typicode.com/users/1
    result = await client
      .Headers(new { ContentType = "application/x-www-form-urlencoded" })
      .User(1)
      .Post(user);

- [Blogpost](https://code-maze.com/dalsoft-restclient-consume-any-rest-api/)
- [Github](https://github.com/DalSoft/DalSoft.RestClient)

---

# Setup sample project

- Get the source from [GitHub](https://github.com/jacobduijzer/MyBeerInfo)
- in folder src/api: npm install
- in the folder src/api: npm run start
- configure & run the console app

---

# Links

- [Sample repository](https://github.com/jacobduijzer/RefitPollyWorkshop)
- [Refit](https://github.com/reactiveui/refit)
- [Polly](https://github.com/App-vNext/Polly)
- [JSON Server](https://github.com/typicode/json-server)
- Presentation created from markdown with [Marp](https://yhatt.github.io/marp/)

---
