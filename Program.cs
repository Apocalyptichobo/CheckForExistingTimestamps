using System;
using System.Collections.Generic;
using System.IO;

class CheckForExistingTimestamps
{
    static void Main(string[] args)
    {
        string directoryPath = "C:\\Users\\epicd\\Desktop\\CSVTest\\Importable Data";

        // Get a list of all CSV files in the specified directory
        string[] csvFiles = Directory.GetFiles(directoryPath, "*.csv");

        // Check if any CSV files were found
        if (csvFiles.Length == 0)
        {
            Console.WriteLine("No CSV files found in directory: " + directoryPath);
            return;
        }

        // Prompt user for start and end dates
        Console.Write("Enter the start date (mm-dd-yyyy or Unix timestamp): ");
        string startDateString = Console.ReadLine();
        Console.Write("Enter the end date (mm-dd-yyyy or Unix timestamp): ");
        string endDateString = Console.ReadLine();
        Console.Write("Enter an object instance to query (press enter none): ");
        string objectId = Console.ReadLine();

        // Parse the start and end dates
        DateTime startDate, endDate;
        long startTimestamp, endTimestamp;

        if (!long.TryParse(startDateString, out startTimestamp))
        {
            if (!DateTime.TryParse(startDateString, out startDate))
            {
                Console.WriteLine("Invalid date format. Please use mm-dd-yyyy or Unix timestamp.");
                return;
            }
            startTimestamp = new DateTimeOffset(startDate).ToUnixTimeMilliseconds();
        }

        if (!long.TryParse(endDateString, out endTimestamp))
        {
            if (!DateTime.TryParse(endDateString, out endDate))
            {
                Console.WriteLine("Invalid date format. Please use mm-dd-yyyy or Unix timestamp.");
                return;
            }
            endTimestamp = new DateTimeOffset(endDate).ToUnixTimeMilliseconds();
        }

        // Create a log file to write the results
        string logFilePath = Path.Combine(directoryPath, "search_log.txt");
        StreamWriter logWriter = new StreamWriter(logFilePath);

        // Loop through each CSV file and read each row
        foreach (string csvFile in csvFiles)
        {
            using (StreamReader reader = new StreamReader(csvFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Split the line into its components
                    string[] parts = line.Split(';');
                    if (parts.Length != 5)
                    {
                        Console.WriteLine("Invalid data format: " + line);
                        continue;
                    }

                    // Parse the timestamp and check if it's within the specified range
                    long timestamp;
                    if (!long.TryParse(parts[3], out timestamp))
                    {
                        Console.WriteLine("Invalid timestamp format: " + line);
                        continue;
                    }

                    if (timestamp < startTimestamp || timestamp > endTimestamp)
                    {
                        continue;
                    }

                    // Write the unique identifier and filename to the log file
                    string logLine = parts[0] + " found in " + csvFile;
                    logWriter.WriteLine(logLine);

                    // Print the unique identifier and filename to the console
                    Console.WriteLine(logLine);
                }
            }
        }

        // Close the log file
        logWriter.Close();
    }
}