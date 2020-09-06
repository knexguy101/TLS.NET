# TLS DotNET
Transfer JA3 tokens with C# using a socket wrappered Go client.
_This project is not official, please feel free to edit as you please_

# Base Project
- https://github.com/zedd3v/mytls#readme (Used the Go base from this project, big shoutout to them)

# Features
 - Transfer Custom JA3 tokens via C# Wrapped Go request client
 - Full proxy support (user-pass and ip auth)
 - Simple and easy to add on to

# Todo
- Automatic cookie management

# Usage
**Step 1**
Create a GoHandler
```cs
var gohandler = new GoHandler(9287); //9287 is the local port
```

**Step 2**
Create a TLSHttpClient
```cs
var client = new TLSHttpClient(gohandler) //pass in a go handler
```

**Step 3**
Create a request using RequestArgs
```cs
var tlsrequestargs = new TLSRequest.RequestArgs()
{
    Url = "url of the request",
    Method = "method",
    Proxy = "proxy (optional)",
    JA3 = "ja3 (optional)",
    Headers = new System.Collections.Generic.Dictionary<string, string>() //optional
}
```

**Step 4**
Send the request
```cs
var res = client.Send(tlsrequestargs);
```

**Step 5**
Handle the response
```cs
 if (res.Error)
{
    Console.WriteLine(res.ErrorMessage.Message);
}
else
{
    Console.WriteLine(res.StatusCode.ToString() + " " + res.Body);
    //res.Headers also exists!
}  
```

**Step 6**
Dispose of the GoHandler
```cs
gohandler.Dispose(); //closes the go process and disposes of all c# resources
```

**Full Usage**
```cs
var gohandler = new GoHandler(9287);
var client = new TLSHttpClient(gohandler);

var res = client.Send(new TLSRequest.RequestArgs()
{
    Url = "https://ja3er.com/json",
    Method = "get",
    //Proxy = "http://localhost:8887",
    JA3 = "771,4865-4866-4867-49195-49199-49196-49200-52393-52392-49171-49172-156-157-47-53,0-23-65281-10-11-35-16-5-13-18-51-45-43-27-21,0-1,1-21",
    Headers = new System.Collections.Generic.Dictionary<string, string>()
    {
        { "content-type", "application/json" },
        { "accept", "*/*" },
        {"user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36" }
        }
    });

if (res.Error)
{
    Console.WriteLine(res.ErrorMessage.Message);
}
else
{
    Console.WriteLine(res.StatusCode.ToString() + " " + res.Body);
} 

Console.ReadKey();

gohandler.Dispose();
```
