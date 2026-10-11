# Mobile Mashup Kit

AI-assisted game mashup toolkit for mobile (Unity IL2CPP, Unity Mono, Cocos2d-x Lua).

## Quick Start

```bash
# Install
pip install -e .

# Scan an APK
mum scan path/to/game.apk

# Dump readable code
mum dump path/to/game.apk --tool jadx

# Start MCP server for AI agents
mum-mcp
```

## What It Does

- **Engine detection** — identifies Unity IL2CPP, Unity Mono, Cocos2d-x Lua
- **Code dumping** — converts binary game code to readable C#/Java/Lua
- **Runtime hooking** — Frida-based method interception
- **Asset extraction** — pulls textures, audio, meshes from APKs
- **MCP server** — exposes tools to AI coding agents

## Project Structure

```
mobile-mashup-kit/
├── src/mobile_mashup_kit/
│   ├── cli.py              # mum command-line interface
│   ├── scanner.py           # APK engine detection
│   ├── dumper.py            # Code extraction orchestration
│   ├── hooker.py            # Frida runtime hooking
│   ├── extractor.py         # Asset extraction
│   └── mcp_server.py        # MCP server for AI agents
├── templates/
│   ├── unity-il2cpp/        # IL2CPP mod template
│   ├── unity-mono/          # Mono mod template
│   └── cocos-lua/           # Lua mod template
├── skills/
│   └── mod-any-apk/         # Agent Skill documentation
├── knowledge/               # Field notes database
└── tests/
```

## Supported Engines

| Engine | Detection | Dumping | Hooking | Assets |
|--------|-----------|---------|---------|--------|
| Unity IL2CPP | Yes | Il2CppDumper | frida-il2cpp-bridge | UnityPy |
| Unity Mono | Yes | jadx | Frida Java API | UnityPy |
| Cocos2d-x Lua | Yes | unluac | Lua injection | cocos parser |

## Safety Rules

- Offline, single-player games only
- No anti-cheat bypass
- No DRM circumvention
- No commercial redistribution of game assets
