using Microsoft.Data.Sqlite;
using System.Data;
using System.Globalization;
using System.Runtime.InteropServices;

internal class Program
{
    static string connectionString = @"Data Source=D:\CSharpProjs\Habit-TrackerVS2022\Habit-Tracker\Habit-Tracker\Database\Habit-Tracker.db";

    public class DrinkingWater
    {
        public int id { get; set; }
        public DateTime? Date { get; set; }
        public int quantity { get; set; }
    }

    private static void Main(string[] args)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS drinking_water (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Quantity INTEGER
                )";
            tableCmd.ExecuteNonQuery();

            connection.Close();
        }

        static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("MAIN MENU");
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("Press 0 to Close the Application");
                Console.WriteLine("Press 1 to View all the records");
                Console.WriteLine("Press 2 to Insert a record");
                Console.WriteLine("Press 3 to Delete a record");
                Console.WriteLine("Press 4 to Update a record");
                Console.WriteLine("-----------------------------------");

                string? Input = Console.ReadLine();

                switch (Input)
                {
                    case "0":
                        Console.WriteLine("Have a nice day, Adios!");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        GetAllRecords();
                        break;
                    case "2":
                        InsertRecord();
                        break;
                    case "3":
                        DeleteRecord();
                        break;
                    case "4":
                        UpdateRecord();
                        break;
                    default: Console.WriteLine("Please select a number between 0 to 4"); break;

                }
            }

        }

        static void GetAllRecords()
        {
            Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var textcmd = connection.CreateCommand();
                textcmd.CommandText = @"SELECT * FROM drinking_water";

                List<DrinkingWater> tabledata = new();

                SqliteDataReader sqliteDataReader = textcmd.ExecuteReader();
                if(sqliteDataReader.HasRows)
                {
                    while(sqliteDataReader.Read())
                    {
                        tabledata.Add(
                            new DrinkingWater()
                            {
                                id = sqliteDataReader.GetInt32(0),
                                Date = DateTime.ParseExact(sqliteDataReader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                                quantity = sqliteDataReader.GetInt32(2)
                            });
                    }
                }
                else
                {
                    Console.WriteLine("No Rows found.");
                }

                connection.Close();

                Console.WriteLine("------------------------------------\n");
                foreach(var dw in tabledata)
                {
                    Console.WriteLine($"{dw.id} | {dw.Date} | Quantity: {dw.quantity}");
                }
                Console.WriteLine("------------------------------------\n");

            }
        }

        static void InsertRecord()
        {
            Console.Clear();
            string? Date = GetDateInput();
            int? quantity = GetNumberInput("Please enter the no of glases. (no decimals allowed)");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"INSERT INTO drinking_water(date,quantity) VALUES('{Date}',{quantity})";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        static int? GetNumberInput(string msg)
        {
            Console.WriteLine(msg);
            string? number = Console.ReadLine();
            if (number == "0") GetUserInput();

            while (!Int32.TryParse(number, out _) || Convert.ToInt32(number) < 0)
            {
                Console.WriteLine("\n\nInvalid number. Try again.\n\n");
                number = Console.ReadLine();
            }

            int finalInput = Convert.ToInt32(number);

            return finalInput;
        }

        static void DeleteRecord()
        {
            Console.Clear();
            int? no = GetNumberInput("Please type the Id you want to delete.");

            using(var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DELETE FROM drinking_water where Id = {no}";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        static void UpdateRecord()
        {
            int? no = GetNumberInput("Please type the Record Id you want to update.");
            string? Date = GetDateInput();
            int? quantity = GetNumberInput("Please enter the no of glases. (no decimals allowed)");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE drinking_water SET date = {Date}, quantity = {quantity} WHERE Id = {no} ";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }

        }

        static string? GetDateInput()
        {
            Console.WriteLine("Please insert the date: (Format dd-mm-yy). Type 0 to return to main menu.");

            string? dateInput = Console.ReadLine();
            if (dateInput == "0") GetUserInput();

            //while(!DateTime.TryParseExact(dateInput,"dd-MM-yy",new CultureInfo("en-US"), DateTimeStyles.None, out var date))
            //{
            //    Console.WriteLine("Invalid Date, Please enter in this format dd-MM-yy. Press 0 to try again or enter main menu.");
            //    dateInput = Console.ReadLine();
            //}

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to main manu or try again:\n\n");
                dateInput = Console.ReadLine();
            }
            return dateInput;
        }

        

        GetUserInput();
    }
}