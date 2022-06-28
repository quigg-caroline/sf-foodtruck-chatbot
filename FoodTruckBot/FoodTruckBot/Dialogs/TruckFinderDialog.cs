namespace FoodTruckBot.Dialogs
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FoodTruckBot.Utilities;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Schema;

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
            AddDialog(new TextPrompt(nameof(LongitudeStepAsync), LongitudeValidatorAsync));
            AddDialog(new TextPrompt(nameof(LatitudeStepAsync), LatitudeValidatorAsync));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private static async Task<DialogTurnResult> LatitudeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Please enter your latitude."),
                RetryPrompt = MessageFactory.Text("The value entered must be between -90 and 90."),
            };
            return await stepContext.PromptAsync(nameof(LatitudeStepAsync), promptOptions, cancellationToken);
        }

        private static async Task<DialogTurnResult> LongitudeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {           
            stepContext.Values["latitude"] = stepContext.Result;
            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Please enter your longitude."),
                RetryPrompt = MessageFactory.Text("The value entered must be between -180 and 180."),
            };

            return await stepContext.PromptAsync(nameof(LongitudeStepAsync), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> LocationConfirmAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["longitude"] = stepContext.Result;

            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Thanks, you entered {stepContext.Values["latitude"]}, {stepContext.Result}."), cancellationToken);

            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = MessageFactory.Text("Is this the correct location?") }, cancellationToken);
        }

        private static Task<bool> LatitudeValidatorAsync(PromptValidatorContext<string> promptContext, CancellationToken cancellationToken)
        {
            // This condition is our validation rule. You can also change the value at this point.
            return Task.FromResult(promptContext.Recognized.Succeeded && 
                Convert.ToDouble(promptContext.Recognized.Value) > -90 && 
                Convert.ToDouble(promptContext.Recognized.Value) < 90);
        }

        private static Task<bool> LongitudeValidatorAsync(PromptValidatorContext<string> promptContext, CancellationToken cancellationToken)
        {
            // This condition is our validation rule. You can also change the value at this point.
            return Task.FromResult(promptContext.Recognized.Succeeded &&
                Convert.ToDouble(promptContext.Recognized.Value) > -180 &&
                Convert.ToDouble(promptContext.Recognized.Value) < 180);
        }

        private async Task<DialogTurnResult> RecommendationStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result)
            {
                var originLongitude = Convert.ToDouble(stepContext.Values["longitude"]);
                var originLatitude = Convert.ToDouble(stepContext.Values["latitude"]);
                var foodTruckData = FoodTruckDataHelper.LoadFoodTruckData();
                var results = FoodTruckDataHelper.FindFiveClosetTrucks(foodTruckData, originLatitude, originLongitude);

                var attachment = FoodTruckDataHelper.GetHeroCard(results, stepContext.Values["latitude"].ToString(), stepContext.Values["longitude"].ToString()).ToAttachment();

                var msg = MessageFactory.Attachment(attachment);

                await stepContext.Context.SendActivityAsync(msg, cancellationToken: cancellationToken);

                msg = MessageFactory.Text("If you want to get recommendations again, please send another message");
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
