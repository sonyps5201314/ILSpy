# ILSpy modern C# decompilation validation material

This directory contains sanitized reproduction material for the ILSpy pull request
`Improve primary-constructor decompilation, collection expressions, and project export`.

No Microsoft binaries or complete decompiled source trees are included. Paths from the
original ILSpy configuration were replaced with placeholders.

## Revisions

- Previous private baseline: `fe70c814858488a4f7b34cfbd9747d5c5a66e780`
- Benchmark upstream baseline: `24ec3327e7fa9fcc56159bbbd7cfafdfc3e5ee89`
- Current upstream PR base: `e930120ade794c5714e12cc4b9d09e5c8f2f9d98`
- PR head at upload time: `fa6f1ee3f649a7cf82476f69778dbec649e8a1b8`

The upstream commits between the benchmark baseline and the current PR base modify bundle,
resource and LightJson validation. The PR branch was rebased onto the current base without
conflicts.

## Settings

Relevant settings are stored in `ILSpy.Copilot.sanitized.xml`. Replace placeholders such as
`{VS_18_INSTALL_DIR}`, `{VS_2022_INSTALL_DIR}`, `{WINDOWS_DIR}`, `{DOTNET_ROOT}` and
`{USER_PROFILE}` before use.

Important options:

- language version: latest
- primary-constructor syntax for non-record types: enabled
- collection expressions: enabled
- inline arrays: enabled
- field keyword: enabled
- SDK-style project format: enabled
- dead-code/dead-store removal: disabled

## Dataset A: `vs2026`

| Metric | Private | Upstream | PR | PR vs upstream |
|---|---:|---:|---:|---:|
| Generated C# files | 3,402 | 3,399 | 3,398 | -1 |
| `.ctor` / `_002Ector` artifacts | 24 | 20 | **0** | **-20** |
| Raw backing fields | 50 | 0 | **0** | 0 |
| `Unknown result type` comments | 248 | 263 | 265 | +2 |
| `[CompilerGenerated]` markers | 396 | 1,247 | **51** | **-1,196** |
| Detected synthesized record methods | 79 | 29 | **0** | **-29** |
| Raw `)(ref ...)` patterns | 29 | 24 | **2** | **-22** |
| Files with syntax errors | 257 | 9 | **1** | **-8** |
| Total syntax errors | 29,241 | 24 | **2** | **-22** |
| Recovered empty collection expressions | 0 | 0 | **727** | +727 |
| Compiler-helper files | 22 | 19 | 19 | 0 |

The two remaining `)(ref ...)` occurrences are valid delegate invocations. The two remaining
syntax diagnostics are both in `Document.cs` and also occur in the upstream output.

## Dataset B: `Copilot.Conversations.Service_vs2026`

| Metric | Private | Upstream | PR | PR vs upstream |
|---|---:|---:|---:|---:|
| Generated C# files | 5,174 | 5,174 | 5,171 | -3 |
| `.ctor` / `_002Ector` artifacts | 36 | 30 | **0** | **-30** |
| Raw backing fields | 26 | 0 | **0** | 0 |
| `Unknown result type` comments | 261 | 116 | **115** | -1 |
| `[CompilerGenerated]` markers | 420 | 1,308 | **40** | **-1,268** |
| Detected synthesized record members | 179 | 108 | **0** | **-108** |
| Raw `)(ref ...)` patterns | 105 | 35 | **2** | **-33** |
| Files with syntax errors | 322 | 8 | **0** | **-8** |
| Total syntax errors | 46,690 | 33 | **0** | **-33** |
| Recovered empty collection expressions | 0 | 0 | **918** | +918 |
| Compiler-helper files | 19 | 19 | **16** | -3 |

## Reproduction

1. Build the upstream revision and PR branch in `Release|Any CPU`.
2. Replace placeholders in the sanitized XML and use the same target DLLs for both builds.
3. Create a flat reference directory from the existing entries of the selected assembly list.
4. Export every selected assembly as an SDK-style project with nested namespace directories.
5. Run `scan-decompilation.ps1` against the private, upstream and PR output roots.
6. Parse generated files with Roslyn Preview and compare the diagnostics.

## Tests

- Full Release solution build: 16 projects succeeded, 0 failed.
- Complete unit-test run: 5,706 total, 5,684 passed, 22 pre-existing skipped, 0 failed.
- Mono.Cecil round-trip inner suite: 218/218 passed.


