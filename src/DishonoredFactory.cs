using System.Reflection;
using LiveSplit.Dishonored;
using LiveSplit.Model;
using LiveSplit.UI.Components;

[assembly: ComponentFactory(typeof(DishonoredFactory))]

namespace LiveSplit.Dishonored;

public class DishonoredFactory : IComponentFactory
{
    public string ComponentName => "Dishonored";
    public string Description => "Automates splitting and load removal for Dishonored.";
    public ComponentCategory Category => ComponentCategory.Control;

    public string UpdateName => ComponentName;
    public string UpdateURL => "http://fatalis.pw/livesplit/update/";
    public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
    public string XMLURL => UpdateURL + "Components/update.LiveSplit.Dishonored.xml";

    public IComponent Create(LiveSplitState state) => new DishonoredComponent(state);
}
