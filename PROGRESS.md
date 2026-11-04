# PROGRESS — NoWoodWaterDamage

## Goal
- Verify the mod against the installed latest Valheim
- Improve patch cost (load, lag, FPS) without changing gameplay

## Acceptance criteria
- WearNTear.IsWet / wood materials still match Valheim 1.0.21
- Default config skips vanilla underwater queries for protected wood
- Release 1.1.2 builds and Thunderstore zip is in `/dist`

## Status
- **Completed** — API matches 1.0.21; 1.1.2 Release build succeeded; dist zip created

## Files changed
- `src/Plugin.cs` — 1.1.2; Harmony Prefix skip + cached ship check
- `Package/manifest.json`, `dist/manifest.json` — 1.1.2
- `CHANGELOG.md`, `Package/CHANGELOG.md` — 1.1.2 notes

## Commands
- Cecil dump of installed `assembly_valheim.dll`: `Version.CurrentVersion` = 1.0.21, `c_networkVersion` = 41
- Game + project libs `assembly_valheim.dll` same size/date (10/28/2026)
- `dotnet build ... -c Release` — succeeded (0 warnings, 0 errors)

## Output
- DLL: `D:\AI-Agent-Projects\cjaycraft\Cjayride.NoWoodWaterDamage\bin\Release\net481\Cjayride.NoWoodWaterDamage.dll`
- Package DLL: `D:\AI-Agent-Projects\cjaycraft\Cjayride.NoWoodWaterDamage\Package\Cjayride.NoWoodWaterDamage.dll`
- ZIP: `D:\AI-Agent-Projects\cjaycraft\Cjayride.NoWoodWaterDamage\dist\cjayride-NoWoodWaterDamage-1.1.2.zip`

## Next steps
1. Install 1.1.2 into a Gale test profile if in-game confirmation is wanted
2. Git commit / tag only if requested
