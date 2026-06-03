# RIMA concept batch — isometric vs top-down 3/4 comparison (10 images)
# Mechanism: ax laurethayday (switch) -> ax dispatch --no-swap -> agy/Imagen (1024^2 opaque)
# Refs: Elementalist SE idle + floor451_0 granite tile
$ErrorActionPreference = 'Continue'
$rima = 'F:\Antigravity Projeler\2d roguelite\RIMA'
$dir  = Join-Path $rima 'STAGING\imagegen'
New-Item -ItemType Directory -Force -Path $dir | Out-Null
$log  = Join-Path $dir 'batch_log.txt'
"BATCH START $(Get-Date -Format o)" | Set-Content $log -Encoding UTF8
$ref1 = Join-Path $rima 'Assets\Resources\Characters\Elementalist\elementalist_idle_SE.png'
$ref2 = Join-Path $rima 'Assets\Sprites\Environment\PixelLabFloor451\floor451_0.png'

# User-requested billing account for image gen
& ax laurethayday *>> $log 2>&1

$style = 'Flat-painterly fantasy concept art, NOT photorealistic, NO pixel-art look, NO text, NO UI elements, NO HUD. RIMA palette: cold slate-grey and dark iron granite stone, deep void-purple fading to black surroundings, accented SPARINGLY with glowing cyan (hex 00FFCC) seal-energy in cracks, runes and magic. Moody cinematic lighting. A single floating stone island suspended in an endless dark void. Use the provided granite floor texture as the ground stone and the provided robed elemental mage as the hero character.'

$iso = 'ISOMETRIC view, true 30-degree isometric diamond projection, camera looking down onto a floating stone island. '
$td  = 'HIGH TOP-DOWN three-quarter view, camera about 70 to 80 degrees from the horizon looking down and slightly behind the hero (Hades / Children of Morta / Diablo style), close heroic hero scale. '

$jobs = @(
  @{ name='concept01_hero_room_ISO';      prompt=($iso + 'A lone robed elemental mage stands at the centre of a small square granite room. Quiet exploration moment, the floor faintly veined with cyan. ' + $style) },
  @{ name='concept02_hero_room_TD';       prompt=($td  + 'A lone robed elemental mage stands at the centre of a small square granite room. Quiet exploration moment, the floor faintly veined with cyan. ' + $style) },
  @{ name='concept03_sundered_beat_ISO';  prompt=($iso + 'The mage unleashes the signature Sundered Beat: a violent cyan shockwave cracks the granite floor outward in a ring, an armoured enemy is staggered and breaking apart at the impact. High-energy combat. ' + $style) },
  @{ name='concept04_sundered_beat_TD';   prompt=($td  + 'The mage unleashes the signature Sundered Beat: a violent cyan shockwave cracks the granite floor outward in a ring, an armoured enemy is staggered and breaking apart at the impact. High-energy combat. ' + $style) },
  @{ name='concept05_portal_chest_ISO';   prompt=($iso + 'A calm reward room: a swirling cyan rune-portal glows on one side and an ornate treasure chest sits nearby on the granite floor, the mage approaching. ' + $style) },
  @{ name='concept06_portal_chest_TD';    prompt=($td  + 'A calm reward room: a swirling cyan rune-portal glows on one side and an ornate treasure chest sits nearby on the granite floor, the mage approaching. ' + $style) },
  @{ name='concept07_boss_arena_ISO';     prompt=($iso + 'A large circular granite arena. A huge looming boss silhouette wreathed in cyan seal-energy faces the small lone mage across the floor. Epic set-piece tension. ' + $style) },
  @{ name='concept08_boss_arena_TD';      prompt=($td  + 'A large circular granite arena. A huge looming boss silhouette wreathed in cyan seal-energy faces the small lone mage across the floor. Epic set-piece tension. ' + $style) },
  @{ name='concept09_void_map_AGY';       prompt=('A bird-eye overview of MANY small floating granite islands connected by faint glowing cyan energy threads, branching like a node map scattered across the dark void, Slay-the-Spire style room map. Isometric lean. ' + $style) },
  @{ name='concept10_sanctuary_AGY';      prompt=('A peaceful sanctuary island: a small shrine glowing with soft cyan seal-energy, the robed mage resting beside it, calm safe-room mood. Interpret the RIMA slate-iron and cyan void aesthetic freely. ' + $style) }
)

$i = 0
foreach ($j in $jobs) {
  $i++
  $name = $j.name
  $target = Join-Path $dir ($name + '.png')
  $tf = Join-Path $env:TEMP ('ax_genimg_' + [guid]::NewGuid().ToString() + '.md')
  $task = "TASK: Generate ONE image with the generate_image tool, then copy it to an exact path.`n`n" +
          "1. Call generate_image with ImageName `"$name`", ImagePaths [`"$ref1`", `"$ref2`"], and this Prompt:`n" + $j.prompt + "`n`n" +
          "2. Copy the resulting PNG to this exact absolute path (create the folder if missing):`n$target`n`n" +
          "3. Report ONLY two lines: the final absolute path, and the pixel width x height."
  $task | Set-Content $tf -Encoding UTF8
  Add-Content $log "`n==== [$i/10] $name  $(Get-Date -Format HH:mm:ss) ===="
  & ax dispatch --task-file $tf --no-swap --print-timeout 220 *>> $log 2>&1
  Remove-Item $tf -ErrorAction SilentlyContinue
  if (Test-Path $target) { Add-Content $log "OK -> $target" } else { Add-Content $log "MISSING -> $target" }
}
Add-Content $log "`nBATCH DONE $(Get-Date -Format o)"
