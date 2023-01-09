using System.Reflection;
using System.Xml;

namespace LiveSplit.Dishonored;

public partial class DishonoredSettings : UserControl
{
    public bool AutoStartEnd { get; set; }
    public bool AutoSplitIntroEnd { get; set; }
    public bool AutoSplitMissionEnd { get; set; }
    public bool AutoSplitPrisonEscape { get; set; }
    public bool AutoSplitOutsidersDream { get; set; }
    public bool AutoSplitWeepers { get; set; }
	public bool AutoSplitDLC06IntroEnd { get; set; }

	public DishonoredSettings()
    {
        InitializeComponent();

        chkAutoStartEnd.DataBindings.Add("Checked", this, "AutoStartEnd", false, DataSourceUpdateMode.OnPropertyChanged);
        chkAutoSplitIntroEnd.DataBindings.Add("Checked", this, "AutoSplitIntroEnd", false, DataSourceUpdateMode.OnPropertyChanged);
        chkAutoSplitMissionEnd.DataBindings.Add("Checked", this, "AutoSplitMissionEnd", false, DataSourceUpdateMode.OnPropertyChanged);
        chkAutoSplitPrisonEscape.DataBindings.Add("Checked", this, "AutoSplitPrisonEscape", false, DataSourceUpdateMode.OnPropertyChanged);
        chkAutoSplitOutsidersDream.DataBindings.Add("Checked", this, "AutoSplitOutsidersDream", false, DataSourceUpdateMode.OnPropertyChanged);
        chkAutoSplitWeepers.DataBindings.Add("Checked", this, "AutoSplitWeepers", false, DataSourceUpdateMode.OnPropertyChanged);
		chkAutoSplitDLC06IntroEnd.DataBindings.Add("Checked", this, "AutoSplitDLC06IntroEnd", false, DataSourceUpdateMode.OnPropertyChanged);

		// defaults
		AutoStartEnd = true;
    }

    public XmlNode GetSettings(XmlDocument doc)
    {
        XmlElement settingsNode = doc.CreateElement("Settings");

        settingsNode.AppendChild(ToElement(doc, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));

        settingsNode.AppendChild(ToElement(doc, "AutoStartEnd", AutoStartEnd));
        settingsNode.AppendChild(ToElement(doc, "AutoSplitIntroEnd", AutoSplitIntroEnd));
        settingsNode.AppendChild(ToElement(doc, "AutoSplitMissionEnd", AutoSplitMissionEnd));
        settingsNode.AppendChild(ToElement(doc, "AutoSplitPrisonEscape", AutoSplitPrisonEscape));
        settingsNode.AppendChild(ToElement(doc, "AutoSplitOutsidersDream", AutoSplitOutsidersDream));
        settingsNode.AppendChild(ToElement(doc, "AutoSplitWeepers", AutoSplitWeepers));
		settingsNode.AppendChild(ToElement(doc, "AutoSplitDLC06IntroEnd", AutoSplitDLC06IntroEnd));

		return settingsNode;
    }

    public void SetSettings(XmlNode settings)
    {
        AutoStartEnd = ParseBool(settings, "AutoStartEnd", true);
        AutoSplitIntroEnd = ParseBool(settings, "AutoSplitIntroEnd");
        AutoSplitMissionEnd = ParseBool(settings, "AutoSplitMissionEnd");
        AutoSplitPrisonEscape = ParseBool(settings, "AutoSplitPrisonEscape");
        AutoSplitOutsidersDream = ParseBool(settings, "AutoSplitOutsidersDream");
        AutoSplitWeepers = ParseBool(settings, "AutoSplitWeepers");
		AutoSplitDLC06IntroEnd = ParseBool(settings, "AutoSplitDLC06IntroEnd");
	}

    static bool ParseBool(XmlNode settings, string setting, bool @default = false)
    {
        return bool.TryParse(settings[setting]?.InnerText, out bool val) ? val : @default;
    }

    static XmlElement ToElement<T>(XmlDocument document, string name, T value)
    {
        XmlElement str = document.CreateElement(name);
        str.InnerText = value.ToString();
        return str;
    }
}
