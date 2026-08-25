param(
	[Parameter(Mandatory = $true)]
	[hashtable]$Roots,

	[Parameter(Mandatory = $true)]
	[string]$RoslynDirectory
)

Add-Type -Path (Join-Path $RoslynDirectory 'Microsoft.CodeAnalysis.dll')
Add-Type -Path (Join-Path $RoslynDirectory 'Microsoft.CodeAnalysis.CSharp.dll')

function Count-Matches([string]$Text, [string]$Pattern)
{
	[regex]::Matches($Text, $Pattern).Count
}

$rows = foreach ($entry in $Roots.GetEnumerator())
{
	$files = @(Get-ChildItem -LiteralPath $entry.Value -Recurse -Filter *.cs |
		Where-Object { $_.FullName -notmatch '\\obj\\' })
	$result = [ordered]@{
		Version = $entry.Key
		CsFiles = $files.Count
		ConstructorArtifacts = 0
		BackingFields = 0
		UnknownResult = 0
		CompilerGenerated = 0
		RefPatterns = 0
		SyntaxErrorFiles = 0
		SyntaxErrors = 0
		EmptyCollectionExpressions = 0
		CompilerHelperFiles = 0
	}

	foreach ($file in $files)
	{
		$text = [IO.File]::ReadAllText($file.FullName)
		$result.ConstructorArtifacts += Count-Matches $text '_002Ector|\.ctor\s*\('
		$result.BackingFields += Count-Matches $text '<[^>\r\n]+>k__BackingField'
		$result.UnknownResult += Count-Matches $text 'Unknown result type'
		$result.CompilerGenerated += Count-Matches $text '\[CompilerGenerated\]'
		$result.RefPatterns += Count-Matches $text '\)\(ref\s+'
		$result.EmptyCollectionExpressions += Count-Matches $text '(?:=>|=|\breturn)\s*\[\]'

		if ($file.Name -match '^(?:--f__AnonymousDelegate\d+|--y__InlineArray\d+|--z__ReadOnly(?:SingleElement)?List|-PrivateImplementationDetails-|__VsMefMetadataView_).*\.cs$')
		{
			$result.CompilerHelperFiles++
		}

		$tree = [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText($text)
		$errors = @($tree.GetDiagnostics() |
			Where-Object { $_.Severity -eq [Microsoft.CodeAnalysis.DiagnosticSeverity]::Error })
		if ($errors.Count)
		{
			$result.SyntaxErrorFiles++
			$result.SyntaxErrors += $errors.Count
		}
	}

	[pscustomobject]$result
}

$rows | Format-Table -AutoSize
$rows | ConvertTo-Json -Depth 3

