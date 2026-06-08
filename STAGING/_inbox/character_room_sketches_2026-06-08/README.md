# RIMA — Character Select + Room Layout + Interaction Test Pack

Bu paket, Character Select / Chamber görünümünü ve interaction prompt bug'larını birlikte ele alır.

## İçerik

- `01_character_select_ideal.png`
- `02_current_screen_diagnosis.png`
- `03_chamber_layout.png`
- `04_combat_room_layout.png`
- `05_portal_choice_layout.png`
- `06_boss_room_layout.png`
- `CLAUDE_PROMPT.md`
- `INTERACTION_PROMPT_TEST_AUTOMATION.md`
- `UI_PROMPT_STANDARD.md`
- `ACCEPTANCE_CHECKLIST.md`

## Ana karar

Character Select stat ekranı değildir. RIMA için doğru yaklaşım:

```text
Attunement Chamber
10 farklı Echo pedestal
her pedestal farklı class kimliği
combat HUD kapalı
seçili class için alt strip
tek prompt standardı
```

Interaction prompt standardı:

```text
[KEY] Aksiyon
```

Örnek:

```text
[G] Bürün: Warblade
[G] Rift'e Gir
[G] Ödülü Al
[RMB] İnfaz
[M] Harita
[TAB] Karakter
```

Localization stringleri tuş içermez. Key ekleme işi tek formatter sistemine ait olur.
