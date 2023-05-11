# Introduction

Switch is a very simple to configure command line parser.

Unlike others, it is not opinionated about how you execute your command, or organise your command structure.

It just parses a command list, and it is intended that is all it will ever do.

# Getting Started!

1.  Install the switch package onto your project:

``` powershell
dotnet add package webefinity-switch
```

2.  Construct an argument builder.

``` c#
var builder = new ArgumentBuilder();
```

3.  Add parameters.

``` c#
builder.Add("param", 'p').AcceptString().MakeRequired().WithDescription("A parameter to love.");
builder.Add("help", '?').AcceptFlag().WithDescription("Get some help.");
```

4.  Build the builder to get back a handler which contains your validated parameters.

``` c#
var handler = builder.Build();
```

5.  Get your parameters value.
``` c#
var paramValue = handler.GetValue<string>("param");
```

6.  Add in some usage warnings.

Before getting any parameter values, include the following to show using using Console.Write:

``` c#
if (optionsHandler.GetValue<bool>("help") || !optionsHandler.IsValid)
{
    optionsHandler.Usage("My Console App", "Version 1.0");
    return;
}
```

# The Full Example

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