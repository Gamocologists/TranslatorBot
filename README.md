# Translation Bot

Derived from GamocologistsBot's Translation module, 
this discord bot provides translation using the DeepL 
translation engine.

This bot is used to translate text from one language to another.
It has two general purpose commands:

The bot uses slash commands.

-> **translate** - Translates text from an automatically detected language to another specified language.


-> **translate-from** - Translate text from a specified language to another specified language.
The syntax is:

The bot is impervious to API outages. 
If the API is down, the bot will not break, but will instead return an informative message.
A reconnection attempt can be made by using the reconnect-to-deepl command.

You can deploy this bot yourself by following the instructions below.

## Requirements
A DeepL API key is required to use this bot.
A discord bot token is also required.

## Installation
1. Clone this repository.
2. Install .NET 8.0 or higher.
3. Inside of the appsettings.json file insert your discord bot token in the 'token' field.
4. Insert your DeepL API key in the 'deeplApiKey' field in the Modules/Translation/Data/translator_data.da file.
5. Move to the directory with the Core.csproj file. 
7. Perform the command `dotnet build --configuration Release Core.csproj`
8. Move to the directory bin/Release/net8.0 (or bin/Release/net9.0 if you are using .NET 9)
9. Perform the command `dotnet Core.dll`


