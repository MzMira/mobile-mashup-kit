# Cocos2d-x Lua Mod Template

Template for modding Cocos2d-x Lua games via Lua script injection.

## Structure

```
cocos-lua-mod/
├── lua/                  # Your mod scripts
│   └── mod_entry.lua
├── decompiled/           # Decompiled game Lua scripts
└── inject.js             # Frida injection script

## Usage

1. Dump the Lua scripts with unluac:
   ```bash
   mum dump game.apk -o ./dumps --tool unluac
   ```

2. Read the decompiled scripts to understand game logic
3. Write your Lua mod
4. Inject via Frida or replace the scripts in the APK

## Example Frida Script (JavaScript)

```javascript
Java.perform(function() {
    // Hook into the Lua engine
    var LuaEngine = Java.use("org.cocos2dx.lib.Cocos2dxLuaJavaBridge");
    
    // Or inject Lua code directly
    var luaCode = "print('Mod loaded!'); -- your mod here";
    LuaEngine.callLuaFunction(luaCode);
});
```

## Example Lua Mod

```lua
-- mod_entry.lua
local orig_update = Game.update
function Game:update(dt)
    -- Your mod logic here
    orig_update(self, dt)
end
```
