using System;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

// For more information about this template visit http://aka.ms/azurebots-csharp-luis
[Serializable]
public class BasicLuisDialog : LuisDialog<object>
{
    public const string Entity_Flight_Code = "flight_code";
    public const string Entity_Flight_Date = "builtin.datetime.date";
    public const string Entity_Flight_Date_Arrival = "isDateArrival";

    public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(Utils.GetAppSetting("LuisAppId"), Utils.GetAppSetting("LuisAPIKey"))))
    {
    }

    [LuisIntent("None")]
    public async Task NoneIntent(IDialogContext context, LuisResult result)
    {
        await context.PostAsync($"HOPAREY You have reached the none intent. You said: {result.Query}"); //
        context.Wait(MessageReceived);
    }

    // Go to https://luis.ai and create a new intent, then train/publish your luis app.
    // Finally replace "MyIntent" with the name of your newly created intent in the following handler
    [LuisIntent("MyIntent")]
    public async Task MyIntent(IDialogContext context, LuisResult result)
    {
        await context.PostAsync($"You have reached the MyIntent intent. You said: {result.Query}"); //
        context.Wait(MessageReceived);
    }
    [LuisIntent("greeting")]
    public async Task GreetingIntent(IDialogContext context, LuisResult result)
    {
        await context.PostAsync($"Hello! How can I help you?"); //
        context.Wait(MessageReceived);
    }
    [LuisIntent("flightstatus")]
    public async Task FlightStatusIntent(IDialogContext context, LuisResult result)
    {
        string flight_code = "";
        string flight_date="";
        string flightDateArrivalorDeparture = "Departure";
        bool allChecksPassed = true;
        EntityRecommendation title;
        //Find if the customer specified the flight code:
        if (result.TryFindEntity(Entity_Flight_Code, out title))
        {
            flight_code = title.Entity;
            //context.Wait(MessageReceived);
        }
        else
        {
            await context.PostAsync($"You didn't specift a Flight Code!");
            allChecksPassed = false;
           // context.Wait(MessageReceived);
        }

        // Find if the customer specified the date is arrival:
        if (result.TryFindEntity(Entity_Flight_Date_Arrival, out title))
        {
            flightDateArrivalorDeparture = "Arrival";
        }
        else
        {
            flightDateArrivalorDeparture = "noluyoya";
        }
       

        // Find if the customer specified the flight date:
        if (result.TryFindEntity(Entity_Flight_Date, out title))
        {   
            flight_date = title.Entity;
            //context.Wait(MessageReceived);
        }
        else
        {
            await context.PostAsync($"You didn't specift a Flight Date!");
            allChecksPassed = false;
            //context.Wait(MessageReceived);
        }
        if (allChecksPassed)
        {
            await context.PostAsync($"You asked for Flight Status for Flight:" + flight_code+ " " +flightDateArrivalorDeparture+" on "+flight_date); 
        }

        context.Wait(MessageReceived);
    }
    

}