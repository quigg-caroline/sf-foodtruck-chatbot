# SF FoodTruck Bot
Chat with the food truck bot to find your favorite SF food trucks. This is my first time working with Azure's Bot Framework, so thought it would be a fun way to learn something new.

#### Development Prerequisites & Resources
- [ASP.NET Core Runtime 3.1](https://dotnet.microsoft.com/en-us/download/dotnet/3.1)
- [Bot Framework Emulator](https://github.com/microsoft/BotFramework-Emulator/blob/master/README.md)
- [Azure Bot Framework SDK](https://docs.microsoft.com/en-us/azure/bot-service/index-bf-sdk?view=azure-bot-service-4.0)


#### Local Setup

Download and open the solution and make sure it builds. You will need .NET Core 3.1 and the Bot Framework Emulator downloaded and installed as well. Once you verify everything builds correctly and all your dependencies are installed, time to get the bot up and running!

1. Start the project _without debugging_ this will start up the application and deploy it to localhost, on port 3798 by default. You can now connect to the bot using the emulator.
2. Open the bot emulator and select 'Open Bot', enter the bot's url `http://localhost:3978/api/messages` and hit 'Connect'.
3. The emulator will then open up a message window the the bot and you can now send and receive messages from the bot.

