using System;
using System.Text.Json;

namespace HotelPlanner
{
    class Program
    {
        class Guest
        {
            public int id;
            public string firstName;
            public string lastName;
            public int roomNumber;
            public int startTimestamp;
            public int endTimestamp;
        }

        class Companies
        {
            public int id;
            public string company;
            public string city;
            public string timezone;
        }

        class Message
        {
            public string message;
        }

        static void Main(string[] args)
        {
            //parsing json files from a local folder
            string companiesJson = System.IO.File.ReadAllText(@"./Companies.json");
			string guestsJson = System.IO.File.ReadAllText(@"./Guests.json");
			string messageJson = System.IO.File.ReadAllText(@"./Message_Template.json");


			JsonDocument guestsJsonDoc = JsonDocument.Parse(guestsJson);
			JsonElement guests = guestsJsonDoc.RootElement;

            // Save guest info from json into the array of Guest object for later use
            Guest[] guestList = new Guest[guests.GetArrayLength()];
			for (int i = 0; i < guests.GetArrayLength(); i++)
			{
                // instantiate the object
                guestList[i] = new Guest();
                guestList[i].id = guests[i].GetProperty("id").GetInt32();
                guestList[i].firstName = guests[i].GetProperty("firstName").GetString();
                guestList[i].lastName = guests[i].GetProperty("lastName").GetString();
                
				var reservation = guests[i].GetProperty("reservation");
                guestList[i].roomNumber = reservation.GetProperty("roomNumber").GetInt32();
                guestList[i].startTimestamp = reservation.GetProperty("startTimestamp").GetInt32();
                guestList[i].endTimestamp = reservation.GetProperty("endTimestamp").GetInt32();
			}

			JsonDocument companiesJsonDoc = JsonDocument.Parse(companiesJson);
			JsonElement companies = companiesJsonDoc.RootElement;

            //Save company info - similar to above
            Companies[] companyList = new Companies[companies.GetArrayLength()];
			for (int i = 0; i < companies.GetArrayLength(); i++)
			{
                // instantiate the object
                companyList[i] = new Companies();
                companyList[i].id = companies[i].GetProperty("id").GetInt32();
                companyList[i].company = companies[i].GetProperty("company").GetString();
                companyList[i].city = companies[i].GetProperty("city").GetString();
                companyList[i].timezone = companies[i].GetProperty("timezone").GetString();
			}

			JsonDocument messageJsonDoc = JsonDocument.Parse(messageJson);
			JsonElement message = messageJsonDoc.RootElement;
            
            Message[] messageList = new Message[message.GetArrayLength()];
            for (int i = 0; i < message.GetArrayLength(); i++)
            {
                //instantiate the object
                messageList[i] = new Message();
                messageList[i].message = message[i].GetProperty("message").GetString();
            }

			Console.Write("Please pick a Guest ID from 1-6:");
			int userChoice = Convert.ToInt32(Console.ReadLine());
            Guest userChoiceGuest = new Guest();
            for(int i = 0; i < guestList.Length; i++)
            {
                // find guest id that match userChoice
                if(guestList[i].id == userChoice)
                {
                    // this is the guest that we want info from
                    userChoiceGuest = guestList[i];

                    // got the info, break out from the loop
                    break;
                }
            }

			
            Console.Write("Please pick a Company ID from 1-5:");
			userChoice = Convert.ToInt32(Console.ReadLine());
            Companies userChoiceCompany = new Companies();
            for(int i = 0; i < companyList.Length; i++)
            {

                if(companyList[i].id == userChoice)
                {
                    userChoiceCompany = companyList[i];

                    break;
                }
            }

            //grab the current time here
            string timeOfDay;
            DateTime time = DateTime.Now;

            Console.WriteLine(time);

            if (time.Hour >=0 && time.Hour < 12 ){
                timeOfDay = "morning";
            }

            else if (time.Hour >= 12 && time.Hour < 18){
                timeOfDay = "afternoon";
            }

            else if (time.Hour >= 18){
                timeOfDay = "evening";
            }
            else {
                timeOfDay = "day";
            }
            
            //after picking the IDs for guest and companies, it'll replace the placeholders here and out what template the user wants
            Console.Write("[Message Templates] \n");
            for(int i = 0; i < messageList.Length; i++)
            {
                Console.WriteLine((i+1) + "." + messageList[i].message.Replace("$timeOfDay", timeOfDay).Replace("$firstName", userChoiceGuest.firstName).Replace("$company", userChoiceCompany.company).Replace("$roomNumber", userChoiceGuest.roomNumber.ToString()));
            }

            Console.Write("Please pick a template between 1-3:");
            userChoice = Convert.ToInt32(Console.ReadLine());
            Message userChoiceMessage = new Message();
            for(int i = 0; i < messageList.Length; i++)
            {
                if(i == (userChoice - 1))
                {
                    userChoiceMessage = messageList[i];

                    break;
                }
            }

            //and finally... the output
            Console.WriteLine(userChoiceMessage.message.Replace("$timeOfDay", timeOfDay).Replace("$firstName", userChoiceGuest.firstName).Replace("$company", userChoiceCompany.company).Replace("$roomNumber", userChoiceGuest.roomNumber.ToString()));

        }
    }
}
