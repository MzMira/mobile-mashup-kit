# Unity Mono Mod Template

Template for modding Unity Mono games via DLL replacement or BepInEx.

## Structure

```
unity-mono-mod/
├── dll/                  # Reference DLLs (Assembly-CSharp.dll etc.)
├── src/                  # Your mod source code
│   └── ModPlugin.cs
└── build.sh              ## Usage

1. Dump the game with jadx:
   ```bash
   mum dump game.apk -o ./dumps --tool jadx
   ```

2. Identify the Assembly-CSharp.dll in the APK
3. Write your mod against the game's classes
4. Replace or inject the modified DLL

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
```
