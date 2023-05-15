# CSV Searcher
The CSV Searcher is a command-line tool for searching CSV files in a directory for rows that match a given date range and/or unique identifier.

# Requirements
- .NET 5.0 runtime or SDK
- A directory containing one or more CSV files

# Usage
To use the CSV Searcher, navigate to the directory containing the CSV files in a terminal or command prompt and run the following command:

> dotnet csvsearcher.dll

The program will prompt you for the following inputs:

> The start date (in mm-dd-yyyy or Unix timestamp format)
> The end date (in mm-dd-yyyy or Unix timestamp format)
> (Optional) A unique identifier to filter by (as an integer)

After entering these inputs, the program will search through all CSV files in the directory for rows that match the specified criteria. It will output the unique identifier and the name of the CSV file where the match was found to the console and to a log file called search_log.txt in the same directory as the CSV files.

If no CSV files are found in the directory, the program will display an error message and exit.

# Example
Suppose we have a directory called csv_files that contains the following CSV files:

> orders.csv
> sales.csv
> customers.csv

To search for all rows between January 1, 2021 and January 31, 2021 in the orders.csv file where the unique identifier is 123, we would run the following command:

> dotnet csvsearcher.dll
> Enter the directory path containing the CSV files: csv_files
> Enter the start date (mm-dd-yyyy or Unix timestamp): 01-01-2021
> Enter the end date (mm-dd-yyyy or Unix timestamp): 01-31-2021
> Enter a unique identifier to filter by (optional): 123

The program will output any matching rows to the console and to the search_log.txt file in the csv_files directory.
