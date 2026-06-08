# RIMA UI Prompt Standardı

## Temel format

```text
[KEY] Aksiyon
```

## Onaylı promptlar

```text
[G] Bürün: Warblade
[G] Rift'e Gir
[G] Ödülü Al
[RMB] İnfaz
[M] Harita
[TAB] Karakter
[ESC] Duraklat
```

## Localization stringleri

Localization stringlerinde input key olmayacak.

Doğru:

```text
Bürün: {0}
Rift'e Gir
Ödülü Al
İnfaz
Harita
Karakter
Duraklat
```

Yanlış:

```text
[G] Bürün: {0}
[G] Rift'e Gir
[RMB] İnfaz
```

## Tek formatter kuralı

Tüm promptlar aynı formatter'dan geçecek:

```text
InteractionPromptFormatter.Format("G", Loc.T("chamber.enter_rift"))
```

Her ekran kendi string concat'ini yapmayacak. Yoksa yine `[G] [G]` çıkar.
