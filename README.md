# SF FoodTruck Bot
Chat with the food truck bot to find your favorite SF food trucks. This is my first time working with Azure's Bot Framework, so thought it would be a fun way to learn something new.

### Try it out!
[Add to MS Teams](https://teams.microsoft.com/l/chat/0/0?users=28:7e4930e6-a2be-4280-a21e-668c65482fd1)

[Try in the browser](https://webchat.botframework.com/embed/Food_Truck_Bot-CUS?s=z3TzvEryfz8.89JINP5WciRL3-2tewRL0TKjHkD2qpd17pp1orkEGlg) using WebChat

#### Development Prerequisites & Resources
- [ASP.NET Core Runtime 3.1](https://dotnet.microsoft.com/en-us/download/dotnet/3.1)
- [Bot Framework Emulator](https://github.com/microsoft/BotFramework-Emulator/blob/master/README.md)
- [Azure Bot Framework SDK](https://docs.microsoft.com/en-us/azure/bot-service/index-bf-sdk?view=azure-bot-service-4.0)


#### Features
- Bot welcomes you and prompts you for latitude and longitude
    - Check that the input is valid coordinates, and re-prompt user in the case that they are not
- Determine five closest trucks based on SF food truck permit data
    - Return results to user as a card with clickable buttons, which link to Google Maps directions to the truck from the user's input location


#### Next Steps
The Bot Framework SDK is incredibly robust and would support a lot more functionality for a project like this. For instance: 
- Ask users for some food preferences to refine the list of food trucks provided
- Manage state of users to save previously input data, such as their location and preferences. The bot won't need to ask these questions every time for the same user.
- More efficient way of loading & processing the food truck data, such as querying the public endpoint to get the most recent data and cache it.

#### Local Setup

Download and open the solution and make sure it builds. You will need .NET Core 3.1 and the Bot Framework Emulator downloaded and installed as well. Once you verify everything builds correctly and all your dependencies are installed, time to get the bot up and running!

1. Start the project, which will start up the application and deploy it to localhost, on port 3798 by default. You can now connect to the bot using the emulator.
2. Open the bot emulator and select 'Open Bot', enter the bot's url `http://localhost:3978/api/messages` and hit 'Connect'.
3. The emulator will then open up a message window the the bot and you can now send and receive messages from the bot.

##### Local Debugging
You can set breakpoints in Visual Studio and start up the application in debug mode and interact with your application through the Bot Emulator to debug. The Bot Emulator also has helpful debugging tools for inspecting the traffic being sent to and from the bot framework. You can read more [here.](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-debug-emulator?view=azure-bot-service-4.0&tabs=csharp)


#### Deploy
The bot is just like any other web application when it comes to deploying and hosting it. The framework SDK is available in several frameworks, but since this project uses .Net Core, the quickest way to host was on Azure. You can do so following the instructions [here](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-deploy-az-cli?view=azure-bot-service-4.0&tabs=userassigned%2Cnewgroup%2Ccsharp). 

Another perk of using Azure, is you can easily configure continuous deployment via Github updates. Any changes pushed to this repro will kick off a deployment to the Azure app service for this project.

#### Learnings and Take Aways
Since this was my first time using the Bot Framework and working with chatbots in general, I did spend a lot of time reading and learning about bot patterns. As a side effect, the bot is pretty simple, but this was obviously a trade off to make sure that what is there works well. 

Since it is simple, a major tradeoff that the algorithm for returning food trucks is _very_ simple and not very efficient. Fortunately, the dataset is fairly small so it's not a huge impact on performance, but it definitely can be improved. 

Deploying to Azure was not particularly easy, even with a detailed tutorial on it. It relies on azure templates and running commands from Azure CLI. If I were to do this again, I'd consider a node.js application. There is a version of the Bot Framework with Javascript support, and then I would have been able to deploy and host much easier on Heroku, for example. 

Overall this was a nice project to learn principals of event-driven development and learn more about how chatbots function and best practices.