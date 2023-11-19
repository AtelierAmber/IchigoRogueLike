using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ichigo.Engine.Resources
{
  public struct LocaleData
  {
    public Dictionary<string, string> Localizations;
  }

  internal class LocaleLoader : ResourceLoader
  {
    public LocaleLoader() : base(typeof(LocaleData), "Locale", ".lang") { }

    protected override object LoadData(string resourcePath)
    {
      Dictionary<string, string> localizetions = new();

      StreamReader localeStream;
      try
      {
        localeStream = new StreamReader(resourcePath);
      }
      catch (FileNotFoundException e)
      {
        Logger.Error("Locale " + resourcePath + " does not exist. Will default to the default culture", e);

        localeStream = new StreamReader("Locale/" + Localizer.DEFAULT_CULTURE.Name + ".lang");
      }

      string[] lines = localeStream.ReadToEnd().Split('\n');
      foreach (string line in lines)
      {
        string trimmedLine = line.Trim();
        if (trimmedLine.StartsWith('#') || trimmedLine.Length <= 0)
        {
          continue;
        }

        string[] keyvalue = trimmedLine.Split('=');
        if (keyvalue.Length < 2)
        {
          Logger.Info("Locale line " + trimmedLine + " discarded. Impropper formatting");
          continue;
        }
        localizetions.Add(keyvalue[0].Trim(), keyvalue[1].Trim());
      }
      return new LocaleData { Localizations = localizetions };
    }
  }
}
