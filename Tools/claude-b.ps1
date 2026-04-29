param(
    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$Args
)

$ErrorActionPreference = "Stop"

if ($Args.Count -gt 0 -and $Args[0] -eq "login") {
    & ccs auth create claude-b --share-context --context-group rima
} else {
    & ccs claude-b @Args
}
