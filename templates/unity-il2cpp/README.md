# Unity IL2CPP Mod Template

Template for modding Unity IL2CPP games via Il2CppDumper output + BepInEx or Melly.

## Structure

```
unity-il2cpp-mod/
├── dll/                  # Reference DLLs from Il2CppDumper output
├── src/                  # Your mod source code
│   └── ModPlugin.cs
├── CMakeLists.txt        # If building native components
└── build.sh              ## Usage

1. Dump the game with Il2CppDumper:
   ```bash
   mum dump game.apk -o ./dumps --tool il2cppdumper
   ```

2. Copy reference DLLs from `dumps/il2cpp/`
3. Write your mod against the dumped classes
4. Build and inject

## Example Hook (C#)

```csharp
using BepInEx;
using HarmonyLib;

[BepInPlugin("com.yourname.gamemod", "Game Mod", "1.0.0")]
public class ModPlugin : BaseUnityPlugin
{
    void Awake()
    {
        Harmony.CreateAndPatchAll(typeof(ModPlugin));
        Logger.LogInfo("Mod loaded!");
    }
}

[HarmonyPatch(typeof(PlayerClass), "Update")]
public class PlayerUpdatePatch
{
    static void Postfix(PlayerClass __instance)
    {
        // Your mod logic here
    }
}
```
