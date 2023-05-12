using System;
using System.Collections.Generic;
using System.IO;

class CheckForExistingTimestamps
{
    static void Main(string[] args)
    {
        string directoryPath = "E:\\OneDrive - Elutions Inc\\Documents\\Currently Working On\\NLNG\\NLGN Imprortable 2\\NLGN Imprortable 2\\MAESTRO_IMPORTABLE";

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

        // Prompt user for optional input for the first field ID
        Console.Write("Enter an optional ID to filter by (leave blank for no filter): ");
        string optionalIdString = Console.ReadLine();
        int optionalId;
        bool filterById = int.TryParse(optionalIdString, out optionalId);

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
        string logFilePath = Path.Combine("D:\\Documents\\SVN\\Projects\\NLGN\\CheckForExistingTimestamps\\CheckForExistingTimestamps", "search_log.log");
        StreamWriter logWriter = new StreamWriter(logFilePath);

        // Loop through each CSV file and read each row
        foreach (string csvFile in csvFiles)
        {
            // Create a set to keep track of unique identifiers we've already found
            HashSet<string> foundIds = new HashSet<string>();
            Console.WriteLine("Timestamp not found in file: " + Path.GetFileName(csvFile));
            using (StreamReader reader = new StreamReader(csvFile))
            {
                string line;
                bool foundTimestamp = false;
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

                    // Check if we've already found this unique identifier
                    if (foundIds.Contains(parts[0])) //Need to fix this, should add it for the individual file, not just in general
                    {
                        continue;
                    }

                    // Check if we should filter by the first field ID
                    if (filterById && int.Parse(parts[0]) != optionalId)
                    {
                        continue;
                    }

                    // Write the unique identifier and filename to the log file
                    string logLine = " Object ID: " + parts[0] + " found in " + Path.GetFileName(csvFile);
                    logWriter.WriteLine(logLine);
                    foundTimestamp = true;
                    // Print the unique identifier and filename to the console
                    Console.WriteLine(logLine);

                    // Add the unique identifier to the set of found IDs
                    foundIds.Add(parts[0]);
                }
                if (foundTimestamp == false)
                {
                    string logLine = "Timestamp/object ID not found in file: " + Path.GetFileName(csvFile);
                    logWriter.WriteLine(logLine);
                    Console.WriteLine(logLine);
                }
            }
        }

        // Close the log file
        logWriter.Close();
    }
}