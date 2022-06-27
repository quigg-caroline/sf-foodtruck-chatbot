namespace FoodTruckBot.Dialogs
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FoodTruckBot.Utilities;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;

    public class TruckFinderDialog : ComponentDialog
    {
        public TruckFinderDialog()
            : base(nameof(TruckFinderDialog))
        {
            // This array defines how the Waterfall will execute.
            var waterfallSteps = new WaterfallStep[]
            {
                LatitudeStepAsync,
                LongitudeStepAsync,
                LocationConfirmAsync,
                RecommendationStepAsync,
            };

            // Add named dialogs to the DialogSet. These names are saved in the dialog state.
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private static async Task<DialogTurnResult> LatitudeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Please enter your latitude.") }, cancellationToken);
        }

        private static async Task<DialogTurnResult> LongitudeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["latitude"] = Convert.ToDouble(stepContext.Result);

            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Please enter your longitude.") }, cancellationToken);
        }

        private async Task<DialogTurnResult> LocationConfirmAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["longitude"] = Convert.ToDouble(stepContext.Result);

            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Thanks, you entered {stepContext.Values["latitude"]}, {stepContext.Result}."), cancellationToken);

            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = MessageFactory.Text("Is this the correct location?") }, cancellationToken);
        }

        private async Task<DialogTurnResult> RecommendationStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result)
            {
                var originLongitude = Convert.ToDouble(stepContext.Values["longitude"]);
                var originLatitude = Convert.ToDouble(stepContext.Values["latitude"]);
                var foodTruckData = FoodTruckDataHelper.LoadFoodTruckData();
                var results = FoodTruckDataHelper.FindFiveClosetTrucks(foodTruckData, originLatitude, originLongitude);

                var msg = string.Join("\n", results.Select(truck => truck.Address));

                await stepContext.Context.SendActivityAsync(msg, cancellationToken: cancellationToken);
                return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
            }
            else
            {

                await stepContext.Context.SendActivityAsync(MessageFactory.Text("No problem, let's start again."), cancellationToken);

                // User said "no" so we will restart the dialog.
                return await stepContext.ReplaceDialogAsync(nameof(WaterfallDialog));
            }
        }
    }
}
