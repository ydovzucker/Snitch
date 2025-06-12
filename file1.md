# Snitch

Snitch is an intelligence reporting and monitoring system designed for security analysts and intelligence officers. The application tracks persons of interest, identifies potential agents, and detects dangerous targets by analyzing report frequency and content. It provides real-time alerts for suspicious behavior and supports updating statuses based on report data.

## Features

- Retrieve person IDs by name for quick reference
- Automatically upgrade a person's status to **potential_agent** based on report count and average report length
- Log alerts when a person is mentioned frequently or submits many reports in a short time
- List all potential agents with their report statistics
- List dangerous targets based on mention counts and status
- Display active alerts for high reporting activity in the last 15 minutes

## Installation

1. Make sure you have .NET Framework installed (version 4.7.2 or later recommended)
2. Clone or download this repository
3. Set up a MySQL database with the appropriate schema (tables: `people`, `IntelReports`, `reports`)
4. Update the database connection string in the code to match your environment
5. Build the project using Visual Studio or the command line:

    ```bash
    dotnet build
    ```

6. Run the application:

    ```bash
    dotnet run
    ```

## Usage

- The application connects to the database to retrieve and update information about persons of interest.
- Use the provided methods to check report counts, potential threat alerts, and status updates.
- The application logs alerts and status changes for easy monitoring.
- The console outputs lists of potential agents, dangerous targets, and active alerts for quick review.

## Database Schema (Overview)

- `people`: Stores person information (`id`, `first_name`, `last_name`, `type`, `num_reports`, `num_mentions`, etc.)
- `IntelReports`: Stores intelligence reports linked to reporters (`reporter_id`, `text`, `timestamp`)
- `reports`: Stores report records with timestamps and reporter references

Make sure your database tables match the expected schema for the app to work correctly.

## Logging

- Alerts and important updates are logged via the `logger` object in the code.
- Make sure to implement or configure the logger to capture logs to file or console as needed.

## Contributing

Contributions and improvements are welcome!  
Feel free to open issues or submit pull requests with enhancements or bug fixes.

## License

This project is licensed under the MIT License — see the `LICENSE` file for details.
