# Webefinity Switch

Switch is a simple command line argument validator and handler for .NET.

It is not opinionated about how you kick off your app. You can use it before or after you set up any dependency injection, so it is easy to inject the settings into your dependency pipeline if you need to do that.

We want it to be simple and quick.  It takes only a few lines to set it up.  And a couple extra lines to handle usage help.
And you are good to go.

``` c#
// Define our arguments
var builder = new ArgumentBuilder();
builder.Add("param", 'p').AcceptString().MakeRequired().WithDescription("A parameter to love.");
builder.Add("help", '?').AcceptFlag().WithDescription("Get some help.");

// Parse arguments from Environment.GetCommandLineArgs()
var handler = builder.Build();

// If args were not valid, or help was flagged
if (optionsHandler.GetValue<bool>("help") || !optionsHandler.IsValid)
{
    optionsHandler.Usage("My Console App", "Version 1.0");
    return;
}

// Otherwise use the arguments
var paramValue = handler.GetValue<string>("param");
Console.Writeline(paramValue);

```

[Getting Started](articles/intro.md) [GitHub](https://github.com/hollandar/webefinity-switch)
