stormpath-csharp-sample
=======================

This sample application shows how to make calls to [Stormpath](https://stormpath.com) from C#, using the [RestSharp](http://restsharp.org/) library.

Some comments for the people that will use this project:

1. It is a C# project for Visual Studio.
1. The <code>Main</code> method is in the <code>Program</code> class.
1. It uses two external libraries: RestSharp (REST/HTTP library) and Json.NET (Json serializer).
1. The CSV file, <code>accounts_to_import.csv</code>, that contains the accounts to be imported is configured to always be copied to the output directory of this project.
1. It assumes that the <code>apiKey.properties</code> file is in: <code>$HOME/Stormpath/apiKey.properties</code>
1. If the <code>apiKey.properties</code> file is downloaded in Windows, it will need a new line right before the *apiKey.secret* section. Looks like the new line doesn't work automatically for Windows. It will look something like: 
   <code>apiKey.id=THEAPIKEYIDVALUEapiKey.secret=THEAPIKEYSECRETVALUE</code>
1. If tou want to remove from Stormpath what the application creates, uncomment the last few lines of the <code>Main</code> method.

But it should look like:
```
apiKey.id=THEAPIKEYIDVALUE
apiKey.secret=THEAPIKEYSECRETVALUE
```
