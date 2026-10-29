# 🌧️ No Wood Water Damage

### Protect Your Wooden Builds from the Elements

No Wood Water Damage prevents wood, hardwood, and timberwood building pieces from deteriorating due to rain, weather, seasons, or water exposure.

Keep your docks, waterfront buildings, and wooden structures looking their best without constantly repairing weathered pieces.

- **Global weather protection:** Prevents rain, weather, and seasonal wear on wooden building pieces everywhere.
- **Waterline protection:** Optionally protects pieces at or near the water's surface.
- **Ship hulls unchanged:** Ships retain their vanilla damage behavior.
- **Configurable behavior:** Enable or disable each protection mode independently.

> **Installation requirement:** Install this mod on both the client and dedicated server.

## ⚙️ Configuration

Configuration is stored in:

`BepInEx/config/cjayride.NoWoodWaterDamage.cfg`

The file is generated automatically on first launch.

| Setting | Default | Description |
|---|---|---|
| `PreventAllWeatherDamage` | `true` | Prevents rain, weather, and seasonal wear on all covered wooden pieces, regardless of location. |
| `ProtectAtWaterline` | `true` | Protects pieces at or near the water surface. Useful when global weather protection is disabled. |

### Recommended Settings

**Protect all wooden builds (default)**

```ini
PreventAllWeatherDamage = true
ProtectAtWaterline = true
```

**Protect only pieces at the waterline**

```ini
PreventAllWeatherDamage = false
ProtectAtWaterline = true
```

**Disable the mod's protection**

```ini
PreventAllWeatherDamage = false
ProtectAtWaterline = false
```

## 📥 Installation

Install through your preferred mod manager, or follow the installation instructions in `Package/README.md`.

Remember to install on **both clients and the dedicated server**.

## 💬 Contact

- **X:** [@cjayride](https://x.com/cjayride)
- **Discord:** [discord.gg/cjayride](https://discord.gg/cjayride) — Find `cjayride` near the top of the user list.
- **Twitch:** [twitch.tv/cjayride](https://twitch.tv/cjayride)

## 🤖 AI Disclosure

The code was **not AI-generated**. AI was used to help verify compatibility with the updated version of Valheim.

