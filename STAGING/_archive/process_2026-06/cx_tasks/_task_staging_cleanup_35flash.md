# GÖREV: STAGING süreç-artifact arşivleme (MEKANİK — sadece dosya taşıma)

Proje kökü: `F:\Antigravity Projeler\2d roguelite\RIMA`. PowerShell ile çalış.

## MUTLAK KURALLAR
1. **SİLME YOK.** Sadece `Move-Item`. Tek hedef: `STAGING\_archive\process_2026-06\<kategori>\`.
2. **Sadece STAGING üst-seviyesindeki DOSYALARI** taşı — alt klasörlere girme, klasör taşıma YOK.
3. **Whitelist-to-move:** SADECE aşağıdaki desenleri taşı. Desene uymayan HER ŞEY yerinde kalır. Emin değilsen TAŞIMA.
4. Bittiğinde manifest yaz (aşağıda).

## TAŞINACAK DESENLER (üst-seviye STAGING\*.* — kategori klasörüne)
| Desen | Hedef alt klasör |
|---|---|
| `_council_*.md` | `council\` |
| `cx_task_*.md` | `cx_tasks\` |
| `TASK_*.md` — **İSTİSNA: `TASK_portalpack_production_2026-06-07.md` KALIR** | `tasks_done\` |
| `_done_*.md` | `done\` |
| `_review_*.md` | `reviews\` |
| `_research_*.md`, `RESEARCH_*.md`, `_ax_*.md`, `_nlm_*.md`, `_nlm_*.json` | `research\` |
| `QUEUE_*.md` | `queues\` |
| `_task_*.md` (bu görev dosyası dahil, EN SON taşı) | `cx_tasks\` |
| `codex_*.md`, `CODEX_*.md` (üst-seviye eski codex artifact'leri) | `codex_misc\` |
| `cliff_template*.*`, `cliff_*.png` gibi cliff deney artifact'leri (sadece üst-seviye) | `cliff_experiments\` |
| `AGY_*.md`, `agy_*.md` | `research\` |

## KESİNLİKLE DOKUNULMAYACAKLAR (yerinde kalır)
- `*DECISION*.md` (40 karar dokümanı — canonical)
- `*_PLAN*.md`, `*MASTER*.md`, `*_SPEC*.md`, `*_LOCK*.md`, `*BIBLE*.md`, `*AUDIT*.md`
- `ASSET_USAGE_AUDIT_2026-06-07.md`, `_asset_include_list.txt`
- `TASK_portalpack_production_2026-06-07.md` (aktif kuyruk)
- `RIMA_Silah_Uretim_Paketi_2026-06-07.zip`
- TÜM klasörler (`report/`, `imagegen/`, `mockups/`, `rooms_json/`, `chatgpt_weapon_pack/`, `_archive/`, `_incoming/`, `screenshots_auto/`, `report_screenshots/` vb.)
- Desene uymayan her dosya

## MANİFEST
`STAGING\_archive\process_2026-06\MANIFEST.md` oluştur:
- Tarih + neden (süreç artifact'leri arşivlendi; One LIVE Version kuralı)
- Kategori başına taşınan dosya SAYISI + tam dosya listesi
- "Geri alma: dosyalar git-tracked; `git log --follow` ile geçmiş korunur" notu

## ÇIKTI (stdout)
Kategori başına sayı tablosu + toplam taşınan + STAGING üst-seviye kalan dosya sayısı. Hata olduysa hangi dosyada.
