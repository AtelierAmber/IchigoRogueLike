using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Ichigo.Engine
{
  public class Localizer
  {
    private static Localizer instance;
    public static Localizer Instance {
      get
      {
        if (instance == null)
        {
          instance = new Localizer();
          instance.Initialize();
        }

        return instance;
      }
    }

    private static CultureInfo DEFAULT_CULTURE = new CultureInfo("en-us");
    public CultureInfo Locale { get; private set; }
    private Dictionary<string, string> textLocalization;

    private void Initialize()
    {
      Locale = CultureInfo.CurrentCulture;
      LoadLocaleData();
    }

    private void LoadLocaleData()
    {
      textLocalization = new Dictionary<string, string>();

      StreamReader localeStream;
      try
      {
        localeStream = new StreamReader("Locale/" + Locale.Name + ".lang");
      }
      catch(FileNotFoundException e)
      {
        Logger.Error("Locale " + Locale.Name + " does not exist. Will default to the default culture", e);

        localeStream = new StreamReader("Locale/" + DEFAULT_CULTURE.Name + ".lang");
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
        textLocalization.Add(keyvalue[0].Trim(), keyvalue[1].Trim());
      }
    }

    public static string Localized(string localizationKey)
    {
      return Instance.textLocalization[localizationKey];
    }
  }
}
