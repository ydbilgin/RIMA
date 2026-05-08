$ProjectRoot = Split-Path -Parent $PSScriptRoot
Set-Location -LiteralPath $ProjectRoot

function Slice-Tiles {
    param(
        [string]$source,
        [string]$output,
        [int]$cols,
        [int]$rows,
        [int]$width,
        [int]$height,
        [string]$prefix
    )

    if (-not (Test-Path -LiteralPath $source -PathType Leaf)) {
        Write-Host "SKIP: $source not found"
        return
    }

    python STAGING/process_tiles.py --source $source --output $output --cols $cols --rows $rows --width $width --height $height --prefix $prefix
}

Slice-Tiles "STAGING/tiles_raw/w1_sheet_v2.png" "Assets/Art/Tiles/Act1/W1" 4 4 64 96 "w1_"
Slice-Tiles "STAGING/tiles_raw/w2_sheet_v2.png" "Assets/Art/Tiles/Act1/W2" 4 4 64 96 "w2_"
Slice-Tiles "STAGING/tiles_raw/obw_sheet.png" "Assets/Art/Tiles/Act1/WB" 4 3 64 128 "wb_"
Slice-Tiles "STAGING/tiles_raw/f3_sheet_v2.png" "Assets/Art/Tiles/Act1/F3" 4 4 64 64 "f3_"
Slice-Tiles "STAGING/tiles_raw/trans_f1f2_sheet.png" "Assets/Art/Tiles/Act1/Trans_F1F2" 4 2 64 64 "tf12_"
Slice-Tiles "STAGING/tiles_raw/trans_f2f3_sheet.png" "Assets/Art/Tiles/Act1/Trans_F2F3" 4 2 64 64 "tf23_"

Write-Host "batch_tiles: all done."
