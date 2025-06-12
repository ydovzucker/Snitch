using MySql.Data.MySqlClient;
using System;
using System.Text.RegularExpressions;

class Manager
{
    public void ShowMenuForManager()
    {
        
        Console.WriteLine("Choose an option:");
        Console.WriteLine("1. List all potential agents");
        Console.WriteLine("2. List all dangerous targets");
        Console.WriteLine("3. List all active alerts");

        string choice = Console.ReadLine();
        Dal dal = new Dal();
        switch (choice)
        {
            case "1":
                dal.ListAllPotentialAgents();
                break;
            case "2":
                dal.ListDangerousTargets();
                break;
            case "3":
                dal.ListActiveAlerts();
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }

    public void Run()

    {
        try
        {
            string managerPassword = "SnitchMaster!";
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("=======================================");
            Console.WriteLine("         Welcome to MALSHINON          ");
            Console.WriteLine("=======================================");
            Console.WriteLine("     Monitoring and Reporting System    ");
            Console.ResetColor();

            Console.WriteLine(); 
            Console.Write("Are you the manager or a reporter? ");
            string Role = Console.ReadLine().Trim().ToLower();
            if (Role == "manager")
            {
                Console.Write("Please enter the manager password: ");
                string password = Console.ReadLine();
                if (password == managerPassword)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("---------------------------------------");
                    Console.WriteLine("      Welcome back, Manager! 👔        ");
                    Console.WriteLine("  Ready to review the latest reports?  ");
                    Console.WriteLine("---------------------------------------");
                    Console.ResetColor();
                    ShowMenuForManager();
                }

            }
            else if (Role == "reporter")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("----------------------------------------");
                Console.WriteLine(" Thank you for sharing your information!");
                Console.WriteLine("             Let's get started!          ");
                Console.WriteLine("----------------------------------------");
                Console.ResetColor();
                string firstName = Intel.GetFirstName();
                string lastName = Intel.GetLastName();

                Dal dal = new Dal();

                if (dal.PersonExists(firstName, lastName))
                {
                    dal.UpdateTypeForReporter(firstName, lastName);
                }
                else
                {
                    dal.AddPerson(firstName, lastName);
                    dal.AddSecretCode(firstName, lastName);
                    dal.SetAsReporter(firstName, lastName);
                }

                string intel = Intel.AskForIntel();
                if (!string.IsNullOrWhiteSpace(intel))
                {
                    Console.WriteLine($"[{firstName} {lastName}] said: {intel}");

                    dal.IncrementReportCount(firstName, lastName);


                    int? reporterId = dal.GetPersonId(firstName, lastName);

                    var pattern = @"\b[A-Z]+ [A-Z]+\b";
                    var matches = Regex.Matches(intel, pattern);

                    if (matches.Count > 0)
                    {
                        Console.WriteLine("Found the following name(s) in the report:");
                        foreach (Match match in matches)
                        {
                            string fullName = match.Value.Trim();
                            Console.WriteLine(fullName);

                            string[] parts = fullName.Split(' ');
                            if (parts.Length == 2)
                            {
                                string fname = parts[0];
                                string lname = parts[1];

                                if (!dal.PersonExists(fname, lname))
                                {
                                    dal.AddPerson(fname, lname);
                                    dal.AddSecretCode(fname, lname);
                                    dal.SetAsTarget(fname, lname);
                                }
                                else
                                {
                                    dal.UpdateTypeForTarget(fname, lname);
                                }
                                dal.IncrementMentionCount(fname, lname);


                                int? targetId = dal.GetPersonId(fname, lname);
                                dal.InsertIntelReport(reporterId, targetId, intel);
                                dal.CheckAndUpdatePotentialAgent(fname, lname);
                                dal.CheckAndLogPotentialThreatAlert(fname, lname);
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("you must enter manager or reporter");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");

        }

    }

}
       

    