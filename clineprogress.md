# Cline Progress - NoWoodWaterDamage Mod Fix

## Current Goal
Fix the mod so wood pieces never take weather/rain damage (not just at waterline). Add config settings.

## Last Completed Step
- Read .clinerules (updated rules: Git Bash shell, 1 edit/turn max, progress in project folder)
- Read and analyzed src/Plugin.cs, .csproj, README.md
- Identified root cause: IsAtWaterline() only protects wood near water; dry land rain passes through

## Next Planned Step
1. [DONE] Add BepInEx config + modify Postfix to block ALL weather damage on wood
2. [DONE] Bump version, update README/CHANGELOG
3. [IN PROGRESS] Attempt build with dotnet build

## Open Problems / Blockers
- Cannot build on this machine: Valheim is not installed (needs game managed DLLs as references)
- User must build locally where Valheim + BepInEx are installed, or create Environment.props pointing to game path

## Files Touched This Session
- Z:\.clinerules (read)
- Z:\cjaycraft\Cjayride.NoWoodWaterDamage\src\Plugin.cs (read)
- Z:\cjaycraft\Cjayride.NoWoodWaterDamage\Cjayride.NoWoodWaterDamage.csproj (read)
- Z:\cjaycraft\Cjayride.NoWoodWaterDamage\README.md (read)