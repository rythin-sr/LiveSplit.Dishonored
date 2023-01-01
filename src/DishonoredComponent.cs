using System.Runtime.CompilerServices;
using System.Xml;
using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;

namespace LiveSplit.Dishonored;

class DishonoredComponent : LogicComponent
{
    public override string ComponentName => "Dishonored";

    public DishonoredSettings Settings { get; set; }

    private readonly TimerModel _timer;
    private readonly GameMemory _gameMemory;
    private readonly Timer _updateTimer;

    public DishonoredComponent(LiveSplitState state)
    {
#if DEBUG
        Debug.Listeners.Clear();
        Debug.Listeners.Add(TimedTraceListener.Instance);
#endif

        Settings = new();

        _timer = new() { CurrentState = state };
        _timer.CurrentState.OnStart += timer_OnStart;

        _updateTimer = new() { Interval = 15, Enabled = true };
        _updateTimer.Tick += updateTimer_Tick;

        _gameMemory = new();
        _gameMemory.OnFirstLevelLoading += gameMemory_OnFirstLevelLoading;
        _gameMemory.OnPlayerGainedControl += gameMemory_OnPlayerGainedControl;
        _gameMemory.OnLoadStarted += gameMemory_OnLoadStarted;
        _gameMemory.OnLoadFinished += gameMemory_OnLoadFinished;
        _gameMemory.OnPlayerLostControl += gameMemory_OnPlayerLostControl;
        _gameMemory.OnAreaCompleted += gameMemory_OnAreaCompleted;
    }

    public override void Dispose()
    {
        _timer.CurrentState.OnStart -= timer_OnStart;
        _updateTimer?.Dispose();
    }

    void updateTimer_Tick(object sender, EventArgs eventArgs)
    {
        try
        {
            _gameMemory.Update();
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.ToString());
        }
    }

    void timer_OnStart(object sender, EventArgs e)
    {
        _timer.InitializeGameTime();
    }

    public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
    {

    }

    void gameMemory_OnFirstLevelLoading(object sender, EventArgs e)
    {
        if (Settings.AutoStartEnd)
            _timer.Reset();
    }

    void gameMemory_OnPlayerGainedControl(object sender, EventArgs e)
    {
        if (Settings.AutoStartEnd)
            _timer.Start();
    }

    void gameMemory_OnLoadStarted(object sender, EventArgs e)
    {
        _timer.CurrentState.IsGameTimePaused = true;
    }

    void gameMemory_OnLoadFinished(object sender, EventArgs e)
    {
        _timer.CurrentState.IsGameTimePaused = false;
    }

    void gameMemory_OnPlayerLostControl(object sender, EventArgs e)
    {
        if (Settings.AutoStartEnd)
            _timer.Split();
    }

    void gameMemory_OnAreaCompleted(object sender, GameMemory.AreaCompletionType type)
    {
        if ((type == GameMemory.AreaCompletionType.IntroEnd && Settings.AutoSplitIntroEnd)
            || (type == GameMemory.AreaCompletionType.MissionEnd && Settings.AutoSplitMissionEnd)
            || (type == GameMemory.AreaCompletionType.PrisonEscape && Settings.AutoSplitPrisonEscape)
            || (type == GameMemory.AreaCompletionType.OutsidersDream && Settings.AutoSplitOutsidersDream)
            || (type == GameMemory.AreaCompletionType.Weepers && Settings.AutoSplitWeepers))
        {
            _timer.Split();
        }
    }

    public override XmlNode GetSettings(XmlDocument document)
    {
        return Settings.GetSettings(document);
    }

    public override Control GetSettingsControl(LayoutMode mode)
    {
        return Settings;
    }

    public override void SetSettings(XmlNode settings)
    {
        Settings.SetSettings(settings);
    }
}

public class TimedTraceListener : DefaultTraceListener
{
    private static TimedTraceListener _instance;
    public static TimedTraceListener Instance => _instance ??= new();

    private TimedTraceListener() { }

    public int UpdateCount
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set;
    }

    public override void WriteLine(string message)
    {
        base.WriteLine("Dishonored: " + UpdateCount + " " + message);
    }
}
