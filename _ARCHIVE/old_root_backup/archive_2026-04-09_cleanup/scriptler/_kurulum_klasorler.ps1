# Unity Klasor Iskeleti Kurulum Scripti
# Kullanim: Unity projesi acildiktan sonra calistir
# Komut: powershell -ExecutionPolicy Bypass -File "_scriptler/_kurulum_klasorler.ps1"
#
# Bu script UnityProject klasoru icinde Assets/ altinda tum klasorleri olusturur.
# Mevcut klasorlere dokunmaz.

param(
    [string]$ProjectRoot = "F:\Antigravity Projeler\2d roguelite\TheConfluence"
)

$AssetsRoot = Join-Path $ProjectRoot "Assets"

if (-not (Test-Path $AssetsRoot)) {
    Write-Error "Assets klasoru bulunamadi: $AssetsRoot"
    Write-Error "Once Unity Hub'dan projeyi olustur, sonra bu scripti calistir."
    exit 1
}

$folders = @(
    "_Project/Settings",
    "_Project/Rendering",
    "_Project/Fonts",

    "Scripts/Core",
    "Scripts/Player",
    "Scripts/Enemies",
    "Scripts/Skills/Base",
    "Scripts/Skills/Warblade",
    "Scripts/Skills/Elementalist",
    "Scripts/Skills/Shadowblade",
    "Scripts/Skills/Ranger",
    "Scripts/Skills/Hexer",
    "Scripts/Skills/Summoner",
    "Scripts/Skills/Ravager",
    "Scripts/Skills/Brawler",
    "Scripts/Skills/Shared",
    "Scripts/Systems/SkillDraft",
    "Scripts/Systems/Resources",
    "Scripts/Systems/Map",
    "Scripts/Systems/Loot",
    "Scripts/Systems/Meta",
    "Scripts/Systems/StatusEffects",
    "Scripts/Bosses/BossBase",
    "Scripts/Bosses/IronWarden",
    "Scripts/Bosses/VoidWarden",
    "Scripts/Bosses/ChainWarden",
    "Scripts/Bosses/FracturedKing",
    "Scripts/Bosses/HollowSovereign",
    "Scripts/Bosses/NexusCore",
    "Scripts/UI/HUD",
    "Scripts/UI/Menus",
    "Scripts/UI/Draft",
    "Scripts/UI/Hub",
    "Scripts/Hub",
    "Scripts/Localization",
    "Scripts/Utils",

    "ScriptableObjects/Skills/Warblade",
    "ScriptableObjects/Skills/Elementalist",
    "ScriptableObjects/Skills/Shadowblade",
    "ScriptableObjects/Skills/Ranger",
    "ScriptableObjects/Skills/Hexer",
    "ScriptableObjects/Skills/Summoner",
    "ScriptableObjects/Skills/Ravager",
    "ScriptableObjects/Skills/Brawler",
    "ScriptableObjects/Skills/Shared",
    "ScriptableObjects/Classes",
    "ScriptableObjects/Enemies",
    "ScriptableObjects/Bosses",
    "ScriptableObjects/Rooms",

    "Prefabs/Player",
    "Prefabs/Enemies",
    "Prefabs/Skills",
    "Prefabs/Bosses",
    "Prefabs/UI",
    "Prefabs/Lighting",
    "Prefabs/Particles",
    "Prefabs/Rooms",

    "Sprites/Characters/Warblade",
    "Sprites/Characters/Elementalist",
    "Sprites/Characters/Shadowblade",
    "Sprites/Characters/Ranger",
    "Sprites/Characters/Hexer",
    "Sprites/Characters/Summoner",
    "Sprites/Characters/Ravager",
    "Sprites/Characters/Brawler",
    "Sprites/Characters/Enemies",
    "Sprites/Environment/Act1_Ruins",
    "Sprites/Environment/Act2_Wastes",
    "Sprites/Environment/Act3_Core",
    "Sprites/Environment/Hub_Threshold",
    "Sprites/UI/HUD",
    "Sprites/UI/Icons",
    "Sprites/UI/Map",
    "Sprites/VFX",

    "Animations/Player",
    "Animations/Enemies",
    "Animations/UI",

    "Audio/Music",
    "Audio/SFX/Player",
    "Audio/SFX/Enemies",
    "Audio/SFX/UI",

    "Materials/Lit",
    "Materials/Unlit",
    "Materials/PostProcess",

    "Shaders",

    "Scenes",

    "Localization/Tables",
    "Localization/Settings"
)

$created = 0
$skipped = 0

foreach ($folder in $folders) {
    $fullPath = Join-Path $AssetsRoot $folder
    if (-not (Test-Path $fullPath)) {
        New-Item -ItemType Directory -Path $fullPath -Force | Out-Null
        # Unity icin .meta olmayan klasorler gorunmez — bos klasoru koru
        $keepFile = Join-Path $fullPath ".gitkeep"
        "" | Set-Content $keepFile
        Write-Host "  + $folder" -ForegroundColor Green
        $created++
    } else {
        $skipped++
    }
}

Write-Host ""
Write-Host "Tamamlandi: $created klasor olusturuldu, $skipped zaten vardı." -ForegroundColor Cyan
Write-Host "Unity Editor'u yeniden baslat (Assets > Refresh) veya Ctrl+R yaparak .meta dosyalarinin olusmasini bekle."
