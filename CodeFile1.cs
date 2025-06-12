using System;
using System.IO;
public class Loging
{

    private readonly string logFolderPath;
    private readonly string logFileBaseName;
    private readonly long maxFileSizeBytes;

    private int currentLogIndex = 1;
    private string currentLogFilePath;
    public Loging(string baseName = "log", long maxFileSizeBytes = 5 * 1024 * 1024)
    {
        logFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        logFileBaseName = baseName;
        this.maxFileSizeBytes = maxFileSizeBytes;


        if (!Directory.Exists(logFolderPath))
        {
            Directory.CreateDirectory(logFolderPath);
        }
        currentLogFilePath = Path.Combine(logFolderPath, $"{logFileBaseName}{currentLogIndex}.txt");
    }
    public void WriteLog(string message)
    {
        
        if (File.Exists(currentLogFilePath) && new FileInfo(currentLogFilePath).Length >= maxFileSizeBytes)
        {
          
            currentLogIndex++;
            currentLogFilePath = Path.Combine(logFolderPath, $"{logFileBaseName}{currentLogIndex}.txt");
        }

        
        string timestampedMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

        File.AppendAllText(currentLogFilePath, timestampedMessage + Environment.NewLine);
    }
}