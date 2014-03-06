stormpath-csharp-sample
=======================

This sample application shows how to make calls to [Stormpath](https://stormpath.com) from C#, using the [RestSharp](http://restsharp.org/) library.

Some comments for the people that will use this project:

It is a C# project for Visual Studio.
1. The "main" method is in the "Program" class.
2. It uses two external libraries: RestSharp (REST/HTTP library) and Json.NET (Json serializer).
3. It assumes that the "apiKey.properties" file is in: $HOME/Stormpath/apiKey.properties
4. If the "apiKey.properties" file is downloaded in Windows, it will need a new line right before the 'apiKey.secret' section. Looks like the new line doesn't work automatically for Windows. It will look something like: 
   apiKey.id=THEAPIKEYIDVALUEapiKey.secret=THEAPIKEYSECRETVALUE

But it should look like:
  apiKey.id=THEAPIKEYIDVALUE
  apiKey.secret=THEAPIKEYSECRETVALUE
