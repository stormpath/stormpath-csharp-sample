stormpath-csharp-sample
=======================

This sample application shows how to make calls to [Stormpath](https://stormpath.com) from C#, using the [RestSharp](http://restsharp.org/) library.

Some comments for the people that will use this project:

It is a C# project for Visual Studio.
1. The <code>Main<code> method is in the <code>Program<code> class.
2. It uses two external libraries: RestSharp (REST/HTTP library) and Json.NET (Json serializer).
3. It assumes that the <code>apiKey.properties<code> file is in: <code>$HOME/Stormpath/apiKey.properties<code>
4. If the <code>apiKey.properties<code> file is downloaded in Windows, it will need a new line right before the *apiKey.secret* section. Looks like the new line doesn't work automatically for Windows. It will look something like: 
   <code>apiKey.id=THEAPIKEYIDVALUEapiKey.secret=THEAPIKEYSECRETVALUE<code>

But it should look like:
  <code>apiKey.id=THEAPIKEYIDVALUE
  apiKey.secret=THEAPIKEYSECRETVALUE<code>
