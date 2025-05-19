using BepInEx;
using BepInEx.Configuration;
using GameConsole;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ultrafab;
public class SpawnCheat: ICheat
{
    public string LongName => "Spawn Prefab";
    public string Identifier => "ultrafab.spawnprefab";
    public string ButtonEnabledOverride => "Spawning...";
    public string ButtonDisabledOverride => "Spawn prefab";
    public bool DefaultState => false;
    public bool IsActive => false;
    public string Icon => "warning";
    public StatePersistenceMode PersistenceMode => StatePersistenceMode.NotPersistent;

    public void Enable()
    {
        Global.config.Reload();

        Global.prefab = Global.commonBundle.LoadAsset<GameObject>(Global.configPrefab.Value);

        GameObject player = GameObject.Find("Player");

        // Spawn the prefab at the player's position
        if (player != null)
        {
            GameObject.Instantiate(Global.prefab, player.transform.position, Quaternion.identity);
        }

        Disable();
    }

    public void Disable() { }

    public void Update() { }
}

public static class Global {
    public static GameObject prefab;
    public static ConfigEntry<string> configPrefab;
    public static AssetBundle commonBundle;
    public static ConfigFile config;
}

[BepInPlugin("org.awildergoose.plugins.ultrafab", "UltraFab", "1.0.0.0")]
public class Plugin : BaseUnityPlugin
{
    
    public class SpawnPrefab : ICommand
    {
        public string Name => "Spawn Prefab";
        public string Description => "Spawns a prefab using its path";
        public string Command => "spawnprefab";

        public void Execute(GameConsole.Console con, string[] args)
        {
            if (args.Length < 1)
            {
                con.PrintLine("Usage: spawnprefab <prefab path>");
                return;
            }

            string combined = "";
            
            foreach (string s in args)
            {
                combined += " " + s;
            }

            // remove first character since its gonna be a space
            combined = combined.Remove(0, 1);

            con.PrintLine("Spawned " + combined + "!");

            GameObject player = GameObject.Find("Player");

            // Spawn the prefab at the player's position
            if (player != null)
            {
                GameObject.Instantiate(Global.commonBundle.LoadAsset<GameObject>(combined), player.transform.position, Quaternion.identity);
            }
        }
    }
    
    private void Awake()
    {
        Global.config = Config;
        Global.configPrefab = Global.config.Bind("General",
                                         "PrefabName",
                                         "Assets/Prefabs/Levels/Hakita.prefab",
                                         "The prefab that should be spawned");

        SceneManager.activeSceneChanged += SceneChanged;

        // Load the asset bundle
        Global.commonBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/common");
        Global.prefab = Global.commonBundle.LoadAsset<GameObject>(Global.configPrefab.Value);

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void RegisterCheats()
    {
        if (!CheatsManager.Instance) return;
        
        CheatsManager.Instance.RegisterCheat(new SpawnCheat(), "ULTRAKIT");

        if(!GameConsole.Console.Instance) return;

        GameConsole.Console.Instance.RegisterCommand(new SpawnPrefab());
    }

    private void SceneChanged(Scene current, Scene next)
    {
        RegisterCheats();
    }
}
