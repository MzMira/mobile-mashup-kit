# Mod Any APK

Agent Skill for modding Android games regardless of engine.

## When to Use

Use this skill when you want to mod, extend, or reverse-engineer an Android game APK. Works on Unity IL2CPP, Unity Mono, Cocos2d-x Lua, and native C++ games.

## Workflow

1. **Scan** — Detect the engine type
   ```bash
   mum scan path/to/game.apk
   ```

2. **Dump** — Extract readable code
   ```bash
   mum dump path/to/game.apk -o ./my_dumps
   ```

3. **Analyze** — Read the dumped code to understand game logic

4. **Hook** — Intercept methods at runtime
   ```bash
   mum hook --package com.game.name --class GameClass --method Update
   ```

5. **Extract** — Pull assets for modding
   ```bash
   mum extract path/to/game.apk -o ./my_assets
   ```

## Engine Detection

| Engine | Key Files | Dumper |
|--------|-----------|--------|
| Unity IL2CPP | libil2cpp.so, global-metadata.dat | Il2CppDumper / Il2CppInspector |
| Unity Mono | Assembly-CSharp.dll, libmono.so | jadx |
| Cocos2d-x Lua | libcocos2d.so, lua scripts | unluac |
| Native C++ | libgame.so | Ghidra / IDA |

## Safety Rules

- Only mod offline, single-player games
- No anti-cheat bypass
- No DRM circumvention
- No commercial redistribution of game assets
- Respect the original developer's work

## MCP Tools

When used via MCP, the following tools are available to AI agents:

- `scan_apk` — Detect engine type
- `dump_code` — Extract readable code
- `hook_method` — Hook a running method
- `list_processes` — List running game processes
- `extract_assets` — Pull game assets

## Tips

- Always scan before dumping — the engine type determines which dumper to use
- Use `--json` flag for machine-readable output
- Dump to a git-ignored directory to avoid committing large files
- When hooking, spawn mode is useful for early initialization hooks
