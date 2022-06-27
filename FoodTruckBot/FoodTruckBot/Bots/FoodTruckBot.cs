// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//

using FoodTruckBot.Utilities;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodTruckBot.Bots
{
    public class FoodTruckBot : ActivityHandler
    {
        // Messages sent to the user.
        private const string WelcomeMessage = "Hello there! Welcome to FoodTruck Bot."+
                                              "Let me help you find some tasty places to eat near you. ";

        protected readonly Dialog Dialog;
        protected readonly BotState ConversationState;
        protected readonly BotState UserState;

        // Initializes a new instance of the FoodTruckBot class.
        public FoodTruckBot(UserState userState, ConversationState conversationState, Dialog dialog)
        {
            UserState = userState;
            ConversationState = conversationState;
            Dialog = dialog;
        }

        // Greet when users are added to the conversation.
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(WelcomeMessage, cancellationToken: cancellationToken);

                    // Run the Dialog with the new message Activity.
                    await Dialog.RunAsync(turnContext, ConversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);
                }
            }
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            // Save any state changes that might have occurred during the turn.
            await ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await UserState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            // Run the Dialog with the new message Activity.
            await Dialog.RunAsync(turnContext, ConversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);
        }
    }
}
