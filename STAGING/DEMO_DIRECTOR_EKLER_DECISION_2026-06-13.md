# DEMO DIRECTOR EKLERİ — COUNCIL KARARI (2026-06-13 gece, Fable sentezi)

Council: cx (feasibility/reuse) + ax Gemini 3.1 Pro (derin tasarım) + ax Gemini 3.5 Flash (lean). Ham çıktılar: `_process/2026-06/_council_*demo_tools*` + CODEX_DONE. Final karar: Fable (orchestrator).

## Sentez — danışman pozisyonları
| Konu | cx | 3.1 Pro | Flash | FABLE KARARI |
|---|---|---|---|---|
| Genel duruş | 3'lü paket öner (god-mode+kill-all+preset spawn) | 3 tool ekle + Force Post-Boss | SIFIR özellik, prova | **Minimal 2 yeni özellik + F1 keşfini kullan** |
| God-mode | %90 hazır, 15-25 satır | — | hayır | ❌ KOD YAZMA — **F1 DemoDebugPanel'de ZATEN VAR** (`DemoDebugPanel.cs:44-69`); sunum planına not |
| Kill All / Room Clear / Next Room | %90 — F1'de zaten var | — | — | ❌ KOD YAZMA — F1'de var; sunum planına not |
| **Dual-class trigger butonu** | (bakmadı) | ⭐⭐ "Force Post-Boss" | — | ✅ **ONAY #1** — en zayıf vaadi (#8) canlı gösterilebilir yapar; bu gece Play-Mode proof'unda kullanılan `TriggerClassSelection()` yolu butona bağlanır |
| **Stat preset butonları** (Tank/Glass Cannon/Default) | — | ⭐ dramatik beat | — | ✅ **ONAY #2** — mevcut stat setter'larını toplu çağırır, Beat 3'ü şovlaştırır |
| Slow-mo slider | timeScale çok-sahipli, riskli | — | — | ❌ RED (timeScale savaşı — bugün zor kapattık) |
| Telemetry CSV | — | önerdi | gereksiz | ❌ RED (pano kopyası yeterli) |
| Camera zoom slider | %65, orta-yüksek risk | — | — | ❌ RED (mouse-wheel zoom zaten var) |
| Enemy HP bar toggle | %55 | — | — | ❌ RED |
| Silah-ele-oturtma (gece kuyruğu) | — | — | KES | ⚠️ KUYRUKTA — özelliklerden SONRA; rollback=attachMode enum'u geri çevirmek |
| Q/E/R/F scaling fix | — | YAPMA | — | ❌ RED (iki danışman + benim görüşüm aynı: hasar matematiğine gece dokunulmaz) |

## Onaylanan paket (gece implementasyonu)
1. **"DUAL-CLASS DRAFT" butonu** (DirectorMode): yalnız secondary class YOKKEN aktif; `RoomRunDirector` gate yolunu (`TriggerClassSelection()`) çağırır → seçim UI'ı canlı açılır. **Kabul kriteri:** Play Mode'da buton → seçim akışı UI'da gerçekten görünüyor + seçim sonrası controller 1→2 (proof'taki assert'ler). UI akışı yoksa/yarımsa → özellik DEMO'dan ÇIKAR (buton commit edilmez), sunumda kanıt-raporu anlatımı kalır.
2. **Stat preset butonları**: TANK / GLASS CANNON / VARSAYILAN — mevcut slider setter'larıyla aynı yolu kullanır (yeni stat yolu YOK); toast'lar tetiklenir.
3. **Sunum planı güncellemesi**: F1 DemoDebugPanel bölümü (god-mode, Kill All, Force Room Clear, Next Room — "yardımcı debug tool'um" anlatısıyla) + yeni butonların beat'leri.

## Gerekçe (Flash'ın "sıfır özellik" duruşuna karşı)
Flash'ın iki "olmazsa olmaz emniyet kemeri" zaten bu gece yapıldı (raw-damage telemetry + Quick Reset wiring). Kalan gece kapasitesi test+prova ağırlıklı kalıyor; onaylanan 2 özellik DirectorMode içinde, mevcut kanıtlı yolların üstünde ve tek tek geri alınabilir. Kullanıcı talimatı: "onaylarsan özellik ekle" — onay verildi.
