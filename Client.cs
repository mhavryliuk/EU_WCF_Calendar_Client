using System;
using System.ServiceModel;

/**<remark>
 * Create a service for remembering of events.
 * It takes a date and returns a list of events for the next week (The next seven days to the current date).
</remark> */

namespace _20180417_WCF_HistoricalEventsServiceClient
{
    internal class Client
    {
        private static void Main()
        {
            Console.Title = "Historical Events (Client)";

            var client = new RemoteHistoricalEvents.HistoricalEventsClient();

            try
            {
                // Connection test.
                Console.Write("Checking connections to the service... ");
                if (!string.Equals(client.TestConnection(), "OK", StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new Exception("Connection check failed!");
                }

                Console.WriteLine(client.TestConnection());
                Console.WriteLine();


                // Getting of initial date from the user.
                DateTime date;
                Console.WriteLine("Calendar of events for 2018.");
                do
                {
                    Console.Write("Please, input initial date (for example, 2018.05.01): ");
                } while (!DateTime.TryParse(Console.ReadLine(), out date));


                // We send initial date to the server and we receive result.
                var eventsForTheWeek = client.InfoAboutEvents(date);

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\nThis week, the following events occurred:");
                Console.ResetColor();

                if (eventsForTheWeek.Length == 0)
                    Console.WriteLine("There are no events in this period. Try setting a new date.");
                else
                    foreach (var someEvent in eventsForTheWeek)
                    {
                        Console.WriteLine(someEvent);
                    }

                try
                {
                    client.Close();
                }
                catch (CommunicationException ex)
                {
                    client.Abort();
                    Console.WriteLine($"Error: {ex.Message}");
                }
                catch (TimeoutException ex)
                {
                    client.Abort();
                    Console.WriteLine($"Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    client.Abort();
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadKey();
        }
    }
}