param(
    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$CodexArgs
)

$ErrorActionPreference = "Stop"

$npmRoot = Join-Path $env:APPDATA "npm"
$localNode = Join-Path $npmRoot "node.exe"
$codexBin = Join-Path $npmRoot "node_modules\@openai\codex\bin\codex.js"

if (-not (Test-Path -LiteralPath $codexBin)) {
    throw "Codex CLI script not found: $codexBin"
}

$node = if (Test-Path -LiteralPath $localNode) { $localNode } else { "node.exe" }

& $node $codexBin @CodexArgs
exit $LASTEXITCODE
