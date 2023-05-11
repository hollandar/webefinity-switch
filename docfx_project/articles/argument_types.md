# Introduction

Assume for the following we have a builder:
``` c#
var builder = new ArgumentBuilder();
```
## Adding options
Add options by calling the Add method:

``` c#
    ArgumentOption Add(string longVersion, char? shortVersion = null, bool isDefault = false);
```

The long version is the flag passed with "--".

The short version if the flag passed with "-" and a single character.  It is optional.

## Option Adjusters

You can adjust an argument option once it is added by marking it to Accept types, or the following:

### MakeRequired

Marks an option as required to have been passed, if this is set an option must be passed, any default value you provide will not be enough.

``` c#
builder.Add("required", 'r').AcceptString().MakeRequired();
```

### WithDescription

Marks an option as being described in the usage documentation.  If you do not call WithDescription the usage will show "No description provided."

``` c#
builder.Add("word", 'w').AcceptString().WithDescription("The word you are searching for in the output");
```

# Integer

Accepts an integer argument.

``` c#
builder.Add("integer", 'i').AcceptInteger()
builder.Build();
long val = builder.GetValue<long>("integer");
```

# Decimal

Accepts a decimal argument.

``` c#
builder.Add("decimal", 'd').AcceptDecimal()
builder.Build();
decimal val = builder.GetValue<decimal>("decimal");
```

# String

Accepts a string argument.

``` c#
builder.Add("string", 's').AcceptString()
builder.Build();
string val = builder.GetValue<string>("string");
```

# Enum

Accepts an enumeration as an argument, by the string values of its entries.

``` c#
enum Positions { first, second, third }
builder.Add("enum", 's').AcceptEnum<Positions>()
builder.Build();
Position val = builder.GetValue<Positions>("enum");
```

# Flag (Boolean)

Accepts a boolean flag.

``` c#
builder.Add("flag", 'd').AcceptFlag()
builder.Build();
bool val = builder.GetValue<bool>("flag");
```

A flag need not be followed by the value true or false, it may be followed by a new argument, as in --flag --another.

The existence of the flag is enough to make it true.
