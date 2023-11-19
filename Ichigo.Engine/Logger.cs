using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ichigo.Engine
{
  public class Logger
  {
    private static string LOG_DIRECTORY = "Logs/";
    private static string LOG_FILE_PATH = LOG_DIRECTORY + "Log.log";
    private static string ERROR_FILE_PATH = LOG_DIRECTORY + "Error.log";

    private static StreamWriter LOG_FILE = null;
    private static string LOG_BUFFER = "";
    private static StreamWriter ERROR_FILE = null;
    private static string ERROR_BUFFER = "";

    private static int SAVED_FILE_COUNT = 3;
    private static int BUFFER_LIMIT = 100;

    private static bool INITIALIZED = false;

    private static void InitFiles()
    {
      if (INITIALIZED) return;
      Directory.CreateDirectory(LOG_DIRECTORY);
      string[] files = Directory.GetFiles(LOG_DIRECTORY, "Log*");
      if (files.Length >= SAVED_FILE_COUNT)
      {
        for (int i = files.Length - 1; i >= SAVED_FILE_COUNT && i >= 0; --i)
        {
          File.Delete(files[i]);
        }
      }
      for (int i = Math.Min(SAVED_FILE_COUNT, files.Length) - 1; i >= 0; --i)
      {
        File.Move(files[i], LOG_DIRECTORY + "Log_" + (i + 1) + ".log");
      }

      File.Create(LOG_FILE_PATH).Dispose();
      LOG_FILE = new StreamWriter(LOG_FILE_PATH, false, Encoding.Default);
      LOG_FILE.WriteLine("Logging started at " + DateTime.Now);
      LOG_FILE.Flush();

      files = Directory.GetFiles(LOG_DIRECTORY, "Error*");
      if (files.Length >= SAVED_FILE_COUNT)
      {
        for (int i = files.Length - 1; i >= SAVED_FILE_COUNT && i >= 0; --i)
        {
          File.Delete(files[i]);
        }
      }

      for (int i = Math.Min(SAVED_FILE_COUNT, files.Length) - 1; i >= 0; --i)
      {
        File.Move(files[i], LOG_DIRECTORY + "Error_" + (i + 1) + ".log");
      }
      File.Create(ERROR_FILE_PATH).Dispose();
      ERROR_FILE = new StreamWriter(ERROR_FILE_PATH, false, Encoding.Default);
      ERROR_FILE.WriteLine("Error logging started at " + DateTime.Now);
      ERROR_FILE.Flush();

      INITIALIZED = true;
    }

    // Appears in log
    public static void Info(string message)
    {
      if (!INITIALIZED) InitFiles();

      LOG_BUFFER += "[LOG " + DateTime.Now + "] " + message;

      if (LOG_BUFFER.Length > BUFFER_LIMIT)
      {
        LOG_FILE.Write(LOG_BUFFER);
        LOG_FILE.Flush();
      }
    }

    // Appears in both log and error
    public static void Warning(string message)
    {
      if (!INITIALIZED) InitFiles();

      LOG_BUFFER += "[WARN " + DateTime.Now + "] " + message + "\n";

      if (LOG_BUFFER.Length > BUFFER_LIMIT)
      {
        LOG_FILE.Write(LOG_BUFFER);
        LOG_FILE.Flush();
      }

      ERROR_BUFFER += "[WARN " + DateTime.Now + "] " + message + "\n";

      if (ERROR_BUFFER.Length > BUFFER_LIMIT)
      {
        ERROR_FILE.Write(ERROR_BUFFER);
        ERROR_FILE.Flush();
      }
    }

    // Appears in error
    public static void Error(string message, Exception exception = null)
    {
      if (!INITIALIZED) InitFiles();

      ERROR_BUFFER += "[ERROR " + DateTime.Now + "] " + message + "\n";
      if (exception != null)
      {
        ERROR_BUFFER += " TRACE " + exception.Message + ": \n" + exception.StackTrace + "\nEND TRACE\n";
      }

      if (ERROR_BUFFER.Length > BUFFER_LIMIT)
      {
        ERROR_FILE.Write(ERROR_BUFFER);
        ERROR_FILE.Flush();
      }
    }
  }
}
