using FoodTruckBot.Dialogs;
using Microsoft.Bot.Builder.Testing;
using Microsoft.Bot.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodTruckBotTests
{
    [TestClass]
    public class TruckFinderDialogTests
    {
        [TestMethod]
        public async Task Dialog_Should_Flow_Expected()
        {
            var dialog = new TruckFinderDialog();
            var testClient = new DialogTestClient(Microsoft.Bot.Connector.Channels.Test, dialog);

            var reply = await testClient.SendActivityAsync<IMessageActivity>("hi");
            Assert.AreEqual("Please enter your latitude.", reply.Text);

            reply = await testClient.SendActivityAsync<IMessageActivity>("37.72");
            Assert.AreEqual("Please enter your longitude.",reply.Text);

            reply = await testClient.SendActivityAsync<IMessageActivity>("-122.43");
            Assert.AreEqual("Thanks, you entered 37.72, -122.43.", reply.Text);

            reply = testClient.GetNextReply<IMessageActivity>();
            Assert.AreEqual("Is this the correct location? (1) Yes or (2) No", reply.Text);

            reply = await testClient.SendActivityAsync<IMessageActivity>("1");
            Assert.AreEqual($"4650 MISSION ST\n1500 GENEVA AVE\n1500 GENEVA AVE\n4384 MISSION ST\n400 ALEMANY BLVD", reply.Text);
        }

        [TestMethod]
        public async Task Dialog_Should_Validate_Input()
        {
            var dialog = new TruckFinderDialog();
            var testClient = new DialogTestClient(Microsoft.Bot.Connector.Channels.Test, dialog);

            var reply = await testClient.SendActivityAsync<IMessageActivity>("hi");
            Assert.AreEqual("Please enter your latitude.", reply.Text);

            reply = await testClient.SendActivityAsync<IMessageActivity>("100");
            Assert.AreEqual("The value entered must be between -90 and 90.", reply.Text);

            reply = await testClient.SendActivityAsync<IMessageActivity>("37.7112");
            Assert.AreEqual("Please enter your longitude.", reply.Text);

            reply = await testClient.SendActivityAsync<IMessageActivity>("250");
            Assert.AreEqual("The value entered must be between -180 and 180.", reply.Text);

            reply = await testClient.SendActivityAsync<IMessageActivity>("-122.43");
            Assert.AreEqual("Thanks, you entered 37.7112, -122.43.", reply.Text);
        }
    }
}
