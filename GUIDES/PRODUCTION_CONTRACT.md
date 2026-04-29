# RIMA Production Contract
> Araştırma özeti (2026-04-23) | Her üretim öncesi ve sonrası kontrol et

## Sabit Kurallar (değişmez)
- Canvas: `128×128`
- PPU: `64` — tüm animasyon tiplerinde (idle, run, attack, skill, dash, death)
- Pivot: center `(0.5, 0.5)` — Multiple mode, explicit rect
- 8 gerçek yön — flip ile sahte yön yasak
- Naming: `warblade_idle_SE.png`, `warblade_run_start_N.png` (büyük harf suffix)

## Üretim Sırası
```
1. Battle start base sprite → PixelLab Create Character (128×128)
2. Idle 8 yön → V3 (Keep First Frame ON, 4-6f)
3. Run 3 yön kalite onayı (E/SE/S) → sonra kalan 5 yön
4. run_start + run_stop → idle/run pose farkı büyükse zorunlu
5. Attack/skill → key pose first, sonra V3 interpolate
```

## run_start / run_stop Kuralı
- Pose farkı büyük class → **zorunlu** (Warblade, Ravager, Ronin)
- Pose farkı küçük class → Unity'de snap testi yap, snap yoksa opsiyonel

## Aseprite Cleanup (import öncesi)
- 9 frame çıkınca: ilk frame çöp mü kontrol et, gerekirse sil
- Her frame'de kontrol:
  - Karakter merkezi sabit mi?
  - Ayak tabanı jitter var mı?
  - BBox canvas dışına taşıyor mu?
- Aynı yön idle + run'ı üst üste koy → silhouette + ayak noktası hizası tutuyorsa geç

## Unity Transition Zinciri
```
idle → run_start → run_loop → run_stop → idle
```
- Diagonal input: `S+D=SE`, `S+A=SW`, `W+D=NE`, `W+A=NW`

## Codex QC Otomasyonu (üretim sonrası koştur)
- Missing frame kontrolü (8 yön tam mı?)
- Scale drift: fillPct `%55–65` aralığında mı?
- Pivot drift: ayak tabanı tüm yönlerde aynı Y'de mi?
- Alpha/background hatası: non-transparent piksel var mı?

## Roller
| Kim | Ne yapar |
|-----|---------|
| Kullanıcı | ChatGPT konsept üretimi, PixelLab üretimi, onay kararları |
| Claude | Prompt stratejisi, üretim sözleşmesi, orkestrasyon |
| Codex | Naming, batch export, QC otomasyonu, Unity import wiring |
