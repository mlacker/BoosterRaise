using BepInEx;
using BepInEx.Unity.IL2CPP;

namespace BoosterRaise;

[BepInPlugin(GUID, NAME, VERSION)]
public class BoosterRaisePlugin : BasePlugin
{
    const string GUID = "com.mlacker.plugins.BoosterRaise";
    const string NAME = "BoosterRaise";
    const string VERSION = "0.1.0";
    
    public override void Load()
    {
        // Plugin startup logic
        Log.LogInfo($"Plugin {GUID} is loaded!");
    }
}
