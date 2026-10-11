# Plants vs Bloons - Unity Mashup Project

A lane-based tower defense mashup combining Plants vs Zombies gameplay with Bloons TD6 enemies.

## Project Structure

```
mashup-project/
├── Assets/
│   ├── Scripts/
│   │   ├── Core/           # GameManager, GridManager, WaveManager, Projectile, InputHandler, SunPickup
│   │   ├── Plants/         # Plant base + Peashooter, Sunflower, Wallnut, SnowPea, Repeater, CherryBomb
│   │   ├── Bloons/         # Bloon base + Red, Blue, Green, Yellow, Pink, Black, White, Rainbow, Ceramic, MOAB
│   │   └── UI/             # UIManager, PlantButton
│   ├── Scenes/             # Main game scene
│   ├── Prefabs/            # Plant, Bloon, Projectile prefabs
│   └── Sprites/            # Imported game assets
└── ProjectSettings/
```

## Setup

1. Open this folder in Unity 2022.3 LTS
2. Import sprites from `assets/btd6/` and `assets/pvz/`
3. Create prefabs for each plant and bloon type
4. Assign sprites to prefabs
5. Open the main scene and assign references in GameManager
6. Press Play

## Game Design

- **Grid**: 5 lanes x 9 columns (PvZ style)
- **Plants**: Peashooter (100), Sunflower (50), Wallnut (50), SnowPea (175), Repeater (200), CherryBomb (150)
- **Bloons**: Red (1hp), Blue (1hp), Green (1hp), Yellow (1hp), Pink (1hp), Black (1hp), White (1hp), Rainbow (1hp), Ceramic (10hp), MOAB (200hp)
- **Economy**: Start with 150 sun, Sunflower produces 25 sun every 8s
- **Lives**: 20 lives, bloons deal 1 damage each

## Asset Sources

- BTD6 sprites: `assets/btd6/` (Unity IL2CPP)
- PvZ sprites: `assets/pvz/` (Cocos2d-x Lua)

## Build

File > Build Settings > Android > Build
