# PROGRESS — NoWoodWaterDamage

## Goal
- Verify build against Valheim 1.0.21
- BepInExPack `denikson-BepInExPack_Valheim-5.4.2360`
- Release 1.1.1: changelog, Thunderstore `/dist` zip, git commit, push, tag

## Status
- **Completed** — Release build succeeded; dist zip created; git push/tag pending verification

## Files changed
- `src/Plugin.cs` — version 1.1.1
- `Package/manifest.json` — version + BepInEx dependency
- `CHANGELOG.md`, `Package/CHANGELOG.md` — 1.1.1 notes
- `dist/` — Thunderstore staging + zip

## Commands
- `dotnet build ... -c Release` — succeeded (0 warnings, 0 errors)

## Output
- DLL: `D:\AI-Agent-Projects\cjaycraft\Cjayride.NoWoodWaterDamage\bin\Release\net481\Cjayride.NoWoodWaterDamage.dll`
- Package DLL: `D:\AI-Agent-Projects\cjaycraft\Cjayride.NoWoodWaterDamage\Package\Cjayride.NoWoodWaterDamage.dll`
