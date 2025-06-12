using MySql.Data.MySqlClient;
using System;

class Dal
{
    private readonly Loging logger = new Loging(); 
    private MySqlConnection ConnectToDataBase()
    {
        try
        {
            string connStr = "server=localhost;username=root;password=;database=snitch_form";
            logger.WriteLog("connectedto data base");
            return new MySqlConnection(connStr);
            
        }
        catch(Exception ex)
        {
            logger.WriteLog($"Error: {ex.Message}");
            return null;
        }
    }


    public bool PersonExists(string firstName, string lastName)
    {
        try
        {


            using (MySqlConnection conn = ConnectToDataBase())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM people WHERE first_name = @firstName AND last_name = @lastName";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@firstName", firstName.Trim());
                cmd.Parameters.AddWithValue("@lastName", lastName.Trim());
                

                return Convert.ToInt64(cmd.ExecuteScalar()) > 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            logger.WriteLog($"couldn't check if person exits because of Error: {ex.Message} ");
            return false;
        }
    
    }
    public void AddPerson(string firstName, string lastName)
    {
        try
        {
            using (MySqlConnection conn = ConnectToDataBase())
            {
                conn.Open();
                string query = "INSERT INTO people (first_name, last_name) VALUES (@firstName, @lastName)";
                MySqlCommand command = new MySqlCommand(query, conn);

                command.Parameters.AddWithValue("@firstName", firstName.Trim());
                command.Parameters.AddWithValue("@lastName", lastName.Trim());
                command.ExecuteNonQuery();
                ;
            }
            logger.WriteLog($"added {firstName} {lastName} to the people tabel");
        }
        
        catch (Exception ex)
        {
            logger.WriteLog($"coulden't add person {firstName} {lastName} Error: {ex.Message}");
            
        }
    }
    

    public void AddSecretCode(string firstName, string lastName)
    {
        try
        {
            string secretCode = Guid.NewGuid().ToString();
            using (MySqlConnection conn = ConnectToDataBase())
            {
                conn.Open();
                string query = @"
        UPDATE people
        SET secret_code = @secretCode
        WHERE first_name = @firstName AND last_name = @lastName";

                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@secretCode", secretCode);
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);

                int rowsAffected = command.ExecuteNonQuery();
                logger.WriteLog($"added secret code for {firstName} {lastName} to the people tabel");
            }
        }
        catch (Exception ex)
        {
            logger.WriteLog($"coulden't add secret code {firstName} {lastName} Error: {ex.Message}");
   
        }
    }
    public void AddType(string type)
    {
        try
        {
            using (MySqlConnection conn = ConnectToDataBase())
            {
                conn.Open();
                string query = "INSERT INTO people (type) VALUES (@type)";
                MySqlCommand command = new MySqlCommand(query, conn);

                command.Parameters.AddWithValue("@type", type);
                command.ExecuteNonQuery();
                
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");

        }
    }
    public string CheckType(string firstName, string lastName)
    {
        try
        {
            using (MySqlConnection conn = ConnectToDataBase())
            {
                conn.Open();
                string query = "SELECT type FROM people WHERE first_name = @firstName AND last_name = @lastName";
                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@firstName", firstName.Trim());
                command.Parameters.AddWithValue("@lastName", lastName.Trim());
                object result = command.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    return null;
                }
                
                return result.ToString();
                
            }
            
        }
        catch (Exception ex)
        {
            logger.WriteLog($"couldent check type for {firstName} {lastName}Error: {ex.Message}");

            return null;

        }

    }

    public void SetAsReporter(string firstName, string lastName)
    {
        try
        {
            using (MySqlConnection conn = ConnectToDataBase())
            {
                conn.Open();
                string query = "UPDATE people SET type = 'reporter' WHERE first_name = @firstName AND last_name = @lastName";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@firstName", firstName.Trim());
                cmd.Parameters.AddWithValue("@lastName", lastName.Trim());
                cmd.ExecuteNonQuery();
            }
            logger.WriteLog($"set type for reporter {firstName} {lastName}");
        }
        catch (Exception ex)
        {
            logger.WriteLog($" couldent set type for reporter {firstName} {lastName}Error: {ex.Message}");

        }
    }
    public void SetAsTarget(string firstName, string lastName)
    {
        try
        {
            using (MySqlConnection conn = ConnectToDataBase())
            {
                conn.Open();
                string query = "UPDATE people SET type = 'target' WHERE first_name = @firstName AND last_name = @lastName";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@firstName", firstName.Trim());
                cmd.Parameters.AddWithValue("@lastName", lastName.Trim());
                cmd.ExecuteNonQuery();
            }
            logger.WriteLog($"set type for target {firstName} {lastName}");
        }
        catch (Exception ex)
        {
            logger.WriteLog($" couldent set type for target {firstName} {lastName}Error: {ex.Message}");

        }
    }
    public void UpdateTypeForReporter(string firstName, string lastName)
    {
        try
        {
            string currentType = CheckType(firstName, lastName);
            string newType = null;

            if (currentType == null)
                newType = "reporter";
            else if (currentType == "target")
                newType = "both";

            if (newType != null)
            {
                using (MySqlConnection conn = ConnectToDataBase())
                {
                    conn.Open();
                    string updateQuery = "UPDATE people SET type = @newType WHERE first_name = @firstName AND last_name = @lastName";
                    MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@newType", newType);
                    cmd.Parameters.AddWithValue("@firstName", firstName.Trim());
                    cmd.Parameters.AddWithValue("@lastName", lastName.Trim());
                    cmd.ExecuteNonQuery();
                    logger.WriteLog($"set type for reporter {firstName} {lastName}");
                }
            }
            logger.WriteLog($"set type for reporter {firstName} {lastName}");
        }
        catch (Exception ex)
        {
            logger.WriteLog($" couldn't set type reporter for {firstName} {lastName} Error: {ex.Message}");

        }
    }
    public void UpdateTypeForTarget(string firstName, string lastName)
    {
        try
        {
            string currentType = CheckType(firstName, lastName);
            string newType = null;

            if (currentType == null)
                newType = "target";
            else if (currentType == "reporter")
                newType = "both";

            if (newType != null)
            {
                using (MySqlConnection conn = ConnectToDataBase())
                {
                    conn.Open();
                    string updateQuery = "UPDATE people SET type = @newType WHERE first_name = @firstName AND last_name = @lastName";
                    MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@newType", newType);
                    cmd.Parameters.AddWithValue("@firstName", firstName.Trim());
                    cmd.Parameters.AddWithValue("@lastName", lastName.Trim());
                    cmd.ExecuteNonQuery();
                }
            }
            logger.WriteLog($"set type for target {firstName} {lastName}");
        }
        catch (Exception ex)
        {
            logger.WriteLog($" couldn't set type reporter for {firstName} {lastName} Error: {ex.Message}");

        }
    }
    public void IncrementReportCount(string firstName, string lastName)
    {
        try
        {
            using (MySqlConnection conn = ConnectToDataBase())
            {
                conn.Open();
                string query = @"
            UPDATE people 
            SET num_reports = IFNULL(num_reports, 0) + 1 
            WHERE first_name = @firstName AND last_name = @lastName";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@lastName", lastName);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");

        }
    }
    public void IncrementMentionCount(string firstName, string lastName)
    {
        try
        {
            using (MySqlConnection conn = ConnectToDataBase())
            {
                conn.Open();
                string query = @"
            UPDATE people 
            SET num_mentions = IFNULL(num_mentions, 0) + 1
            WHERE first_name = @firstName AND last_name = @lastName";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@firstName", firstName.Trim());
                cmd.Parameters.AddWithValue("@lastName", lastName.Trim());
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");

        }
    }
    public void InsertIntelReport(int? reporterId, int? targetId, string text)
    {
        try
        {
            if (reporterId == null)
            {
                Console.WriteLine("Error: Reporter ID cannot be null.");
                return;
            }

            if (targetId == null)
            {
                Console.WriteLine("Error: Target ID cannot be null.");
                return;
            }

            using (var conn = ConnectToDataBase())
            {
                conn.Open();
                string query = @"
            INSERT INTO IntelReports (reporter_id, target_id, text, timestamp)
            VALUES (@reporterId, @targetId, @text, NOW())";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@reporterId", reporterId.Value);
                    cmd.Parameters.AddWithValue("@targetId", targetId.Value);
                    cmd.Parameters.AddWithValue("@text", text);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Intel report successfully recorded:\nReporter ID: {reporterId}\nTarget ID: {targetId}\nText: {text}");
                    }
                    else
                    {
                        Console.WriteLine("Failed to insert intel report into the database.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");

        }
    }
    public int? GetPersonId(string firstName, string lastName)
    {
        try
        {
            using (var conn = ConnectToDataBase())
            {
                conn.Open();
                string query = "SELECT id FROM people WHERE first_name = @firstName AND last_name = @lastName";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@firstName", firstName.Trim());
                    cmd.Parameters.AddWithValue("@lastName", lastName.Trim());
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        return Convert.ToInt32(result);
                    else
                        return null;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;

        }
    }
    public void CheckAndUpdatePotentialAgent(string firstName, string lastName)
    {
        try
        {
            string currentType = CheckType(firstName, lastName);
            if (currentType != "potential_agent")
            {

                using (var conn = ConnectToDataBase())
                {
                    conn.Open();

                    string countQuery = @"
            SELECT COUNT(*), AVG(CHAR_LENGTH(text))
            FROM IntelReports
            JOIN people ON IntelReports.reporter_id = people.id
            WHERE people.first_name = @firstName AND people.last_name = @lastName";

                    using (var cmd = new MySqlCommand(countQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@firstName", firstName.Trim());
                        cmd.Parameters.AddWithValue("@lastName", lastName.Trim());

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int IndexOfReportCount = 0;
                                int IndexOfAverageTextLength = 1;

                                bool isReportCountNull = reader.IsDBNull(IndexOfReportCount);
                                int reportCount = isReportCountNull ? 0 : reader.GetInt32(IndexOfReportCount);

                                bool isAvgLengthNull = reader.IsDBNull(IndexOfAverageTextLength);
                                double avgTextLength = isAvgLengthNull ? 0 : reader.GetDouble(IndexOfAverageTextLength);

                                if (reportCount >= 10 && avgTextLength >= 100)
                                {
                                    logger.WriteLog($"{firstName} {lastName} upgraded to potential_agent (Reports: {reportCount}, Avg Length: {avgTextLength})");

                                    reader.Close();

                                    string updateQuery = @"
                            UPDATE people
                            SET type = 'potential_agent'
                            WHERE first_name = @firstName AND last_name = @lastName";

                                    using (var updateCmd = new MySqlCommand(updateQuery, conn))
                                    {
                                        updateCmd.Parameters.AddWithValue("@firstName", firstName.Trim());
                                        updateCmd.Parameters.AddWithValue("@lastName", lastName.Trim());
                                        updateCmd.ExecuteNonQuery();
                                        logger.WriteLog($"set type potential reporter for {firstName} {lastName}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.WriteLog($"couldn't update type potential agent for {firstName} {lastName} Error: {ex.Message}");

        }
    }
    public void CheckAndLogPotentialThreatAlert(string firstName, string lastName)
    {
        try
        {
            using (var conn = ConnectToDataBase())
            {
                conn.Open();

                string query = @"
        SELECT num_mentions
        FROM people
        WHERE first_name = @firstName AND last_name = @lastName";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@firstName", firstName.Trim());
                    cmd.Parameters.AddWithValue("@lastName", lastName.Trim());

                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        int mentionCount = Convert.ToInt32(result);
                        if (mentionCount >= 20)
                        {
                            logger.WriteLog($"⚠️ ALERT: {firstName} {lastName} is a potential threat (mentions: {mentionCount})");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.WriteLog($"couldent update type potential threat for {firstName} {lastName} Error: {ex.Message}");

        }
    }
    public void CheckRecentReports(string firstName, string lastName)
    {
        try
        {
            using (var conn = ConnectToDataBase())
            {
                conn.Open();


                int? reporterId = GetPersonId(firstName, lastName);
                if (reporterId == null)
                {
                    Console.WriteLine("Reporter not found.");
                    return;
                }

                string query = @"
            SELECT COUNT(*) 
            FROM IntelReports
            WHERE reporter_id = @reporterId
              AND timestamp >= NOW() - INTERVAL 15 MINUTE";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@reporterId", reporterId.Value);

                    var result = cmd.ExecuteScalar();
                    int count = Convert.ToInt32(result);

                    if (count >= 3)
                    {
                        logger.WriteLog($"⚠️ ALERT: {firstName} {lastName} has submitted {count} reports in the last 15 minutes!");
                    }
                }
            }
        }

        catch (Exception ex)
        {
            logger.WriteLog($"couldn't crate ALERT for {firstName} {lastName} he has submitted over 3 reports in the last 15 minutes!\" Error: {ex.Message}");

        }
    }
    public void ListAllPotentialAgents()
    {
        try
        {
            using (var conn = ConnectToDataBase())
            {
                conn.Open();

                string query = @"
            SELECT p.first_name, p.last_name, p.num_reports, AVG(CHAR_LENGTH(r.text)) AS avg_length
            FROM people p
            JOIN IntelReports r ON p.id = r.reporter_id
            WHERE p.type = 'potential_agent'
            GROUP BY p.id, p.first_name, p.last_name, p.num_reports";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("📋 Potential Agents:\n");

                    while (reader.Read())
                    {
                        string firstName = reader.GetString("first_name");
                        string lastName = reader.GetString("last_name");
                        int reportCount = reader.GetInt32("num_reports");
                        double avgLength = reader.GetDouble("avg_length");

                        Console.WriteLine($"- {firstName} {lastName}: reports = {reportCount}, avg. length = {avgLength:F2} chars");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");

        }
    }
    public void ListDangerousTargets()
    {
        try
        {
            using (var conn = ConnectToDataBase())
            {
                conn.Open();
                string query = @"
            SELECT first_name, last_name, num_mentions
            FROM people
            WHERE type IN ('target', 'both')
              AND num_mentions >= 20";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("🚨 Dangerous Targets:\n");

                    while (reader.Read())
                    {
                        string firstName = reader.GetString("first_name");
                        string lastName = reader.GetString("last_name");
                        int mentionCount = reader.GetInt32("num_mentions");

                        Console.WriteLine($"- {firstName} {lastName}: mentions = {mentionCount}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");

        }
    }


    public void ListActiveAlerts()
    {
        try {
            using (var conn = ConnectToDataBase())
            {
                conn.Open();

                string query = @"
            SELECT r.reporter_id, p.first_name, p.last_name, COUNT(*) AS report_count
            FROM reports r
            JOIN people p ON r.reporter_id = p.id
            WHERE r.timestamp >= NOW() - INTERVAL 15 MINUTE
            GROUP BY r.reporter_id
            HAVING report_count >= 3";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("🔔 Active Alerts (last 15 minutes):\n");

                    while (reader.Read())
                    {
                        string firstName = reader.GetString("first_name");
                        string lastName = reader.GetString("last_name");
                        int reportCount = reader.GetInt32("report_count");

                        Console.WriteLine($"⚠️ {firstName} {lastName} reported {reportCount} times in the last 15 minutes.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");

        }
    }
        
}
       















