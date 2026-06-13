# Playtest Screenshot Şablonu — Demo Tools (2026-06-13)

**Amaç:** Her model (Opus, Codex, Gemini 3.1 Pro, Gemini 3.5 Flash) demo sunum recipe'sini Play-mode'da çalıştırır, **Unity game-view** screenshot'larını kendi klasörüne koyar. Kullanıcı gözle kontrol eder.

## ⚠️ Screenshot kuralı (KİLİT)
- **Unity MCP `manage_camera` action=`screenshot`, `capture_source=game_view`, KAMERA BELİRTME** → ScreenCapture API tüm katmanları (ScreenSpaceOverlay Director UI dahil) yakalar.
- **OS/ekran screenshot DEĞİL.** `output_folder` = modelin kendi klasörü.
- Kamera belirtilirse overlay UI görünmez (memory: ScreenSpaceOverlay screenshot'ta çıkmaz) → o yüzden kamera YOK.
- `max_resolution` yüksek tut (≥1280) ki okunabilsin.

## Klasör yapısı
```
STAGING/playtest_caps_2026-06-13/
├── _TEMPLATE.md        ← bu dosya
├── opus/               ← Opus'un çekimleri + _NOTES.md
├── codex/              ← cx'in çekimleri + _NOTES.md
├── gemini-31pro/       ← Gemini 3.1 Pro + _NOTES.md
└── gemini-35flash/     ← Gemini 3.5 Flash + _NOTES.md
```

## Çekilecek screenshot'lar (her model AYNI seti, AYNI isimlerle)
| Dosya adı | Ne göstermeli |
|---|---|
| `01_director_open.png` | `` ` `` ile Director overlay AÇIK; sol-rail sekmeler (Spawn/Class/Stats/Telemetry...) görünür |
| `02_spawn_enemies.png` | Spawn sekmesi; sahada 3-5 düşman spawn edilmiş (ghost→tıkla) |
| `03_stats_baseline.png` | Stats sekmesi; başlangıç slider değerleri okunur (physPower/abilityPower/maxHP/damageMult) |
| `04_stats_tuned.png` | physPower veya debugGlobalDamageMult YÜKSELTİLMİŞ (slider gözle değişmiş) |
| `05_combat_before.png` | Test mode'a dönülmüş; dövüş başlıyor, düşman(lar) full HP |
| `06_telemetry_dps_ttk.png` | Telemetry sekmesi; DPS + TTK sayıları DOLU (vuruş sonrası) |
| `07_telemetry_csv.png` | CSV export tetiklenmiş (log/clipboard kanıtı veya export satırı) |
| `08_class_warblade.png` | Class&Skill; Warblade seçili (sprite/stat değişimi) |
| `09_class_elementalist.png` | Class&Skill; Elementalist seçili |
| `10_gameplay_normal.png` | Director KAPALI; normal combat görünümü (oynanabilirlik/animasyon) |

## Her model klasörüne `_NOTES.md`
İçerik: her shot için PASS/FAIL + gözlem · bulunan bug'lar · "şu beklenen ama şu çıktı" · genel izlenim. Türkçe.

## Recipe mayınları (her model dikkat etsin)
- Damage slider SADECE temel saldırı/packetized skill'de kanıtlar (legacy DealDamage baypas eder).
- timeScale=0 duraklatır → stat/telemetry etkisini Test mode'a dönünce göster.
- A4 raycast: EventSystem yoksa UI tık dünyaya da spawn eder → not düş.
- 5 class buton güvenli, 10 değil.
