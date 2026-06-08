# Interaction Prompt Test Otomasyonu

## Problem

Bazı UI promptlarında input key iki kez görünebiliyor:

```text
[G] [G] Rift'e Gir
G G Bürün
[RMB] [RMB] İnfaz
```

Bu küçük görünür ama demo kalitesini direkt düşürür.

## Ana karar

Input key'in sahibi tek sistem olacak.

Tercih edilen standart:

```text
Localization string = sadece aksiyon metni
Prompt formatter = key + aksiyon metni
```

Doğru örnek:

```text
Loc.T("chamber.enter_rift") => "Rift'e Gir"
Format("G", "Rift'e Gir") => "[G] Rift'e Gir"
```

Yanlış örnek:

```text
Loc.T("chamber.enter_rift") => "[G] Rift'e Gir"
Format("G", "[G] Rift'e Gir") => "[G] [G] Rift'e Gir"
```

## Yeni sınıf önerisi

Path:

```text
Assets/Scripts/UI/InteractionPromptFormatter.cs
```

API:

```csharp
public static class InteractionPromptFormatter
{
    public static string Format(string keyToken, string localizedActionText);
    public static bool ContainsHardcodedKeyToken(string text);
    public static int CountKeyTokens(string text);
}
```

Davranış:

```text
Format("G", "Rift'e Gir") => "[G] Rift'e Gir"
Format("[G]", "Rift'e Gir") => "[G] Rift'e Gir"
Format("G", "[G] Rift'e Gir") => "[G] Rift'e Gir" + warning
```

Yani duplicate üretmeyecek. Ama warning basacak, çünkü localization'a key gömülmüşse bu veri hatasıdır.

## EditMode testleri

Path:

```text
Assets/Tests/EditMode/UI/InteractionPromptFormatterTests.cs
```

Testler:

```text
Format_AddsSingleKey_WhenActionTextHasNoKey
Format_NormalizesRawKeyToken
Format_DoesNotDuplicateKey_WhenLocalizedTextAlreadyContainsSameKey
Format_WarnsOrStrips_WhenLocalizedTextContainsBracketedKey
Format_PreservesActionText
CountKeyTokens_ReturnsCorrectCount
```

## Localization lint testi

Path:

```text
Assets/Tests/EditMode/UI/UITextLintTests.cs
```

Yasak patternler:

```text
[G]
[E]
[RMB]
[LMB]
[TAB]
[SPACE]
G G
E E
[G] [G]
[E] [E]
[RMB] [RMB]
```

Testler:

```text
AllInteractLocalizationStrings_DoNotContainHardcodedKeyTokens
AllGeneratedInteractPrompts_ContainExactlyOneKeyToken
NoPromptTextContainsDuplicateKeyTokens
NoTurkishPromptUsesBrokenPhrase_RiftGir
```

Özellikle şunu yakala:

```text
Rift Gir
```

Doğrusu:

```text
Rift'e Gir
```

## Chamber PlayMode / integration testleri

Path:

```text
Assets/Tests/PlayMode/UI/ChamberPromptTests.cs
```

Testler:

```text
WarbladePedestalPrompt_ContainsExactlyOneG
RiftExitPrompt_ContainsExactlyOneG
ChamberPrompt_DoesNotContainDuplicateKey
ChamberPrompt_LocalizationTR_DoesNotDuplicateKey
ChamberPrompt_LocalizationEN_DoesNotDuplicateKey
```

Beklenen promptlar:

```text
[G] Bürün: Warblade
[G] Rift'e Gir
```

## Acceptance criteria

- `[G] [G]` hiçbir yerde yok.
- `G G` hiçbir yerde yok.
- Localization stringleri input token taşımaz.
- Chamber pedestal prompt tek `[G]` içerir.
- Rift exit prompt tek `[G]` içerir.
- Reward prompt tek `[G]` içerir.
- TR/EN geçişi duplicate üretmez.
- Testler CI/EditMode/PlayMode içinde çalışır.
