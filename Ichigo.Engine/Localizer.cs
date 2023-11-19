using Ichigo.Engine.Resources;
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
  public static class StringLocalization
  {
    public static string Localize(this string localizationKey)
    {
      return Localizer.Instance.TextLocalization[localizationKey];
    }
  }

  public class Localizer
  {
    private static Localizer instance;
    public static Localizer Instance
    {
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

    internal static CultureInfo DEFAULT_CULTURE = new CultureInfo("en-us");
    public CultureInfo Locale { get; private set; }
    internal Dictionary<string, string> TextLocalization
    {
      get
      {
        ResourceController.GetOrLoadResource(Locale.Name, out LocaleData data);
        return data.Localizations;
      }
    }

    private void Initialize()
    {
      Locale = CultureInfo.CurrentCulture;
    }
  }
}
