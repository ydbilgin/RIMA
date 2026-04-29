---
description: RIMA bilgi tabanında token-verimli arama — heavy dosyaları yüklemeden önce memory'den cevap bul.
---

# /query [soru] — Token-Verimli Bilgi Erişimi

Bu komutu çalıştırırken **önce memory'ye bak, sonra dosyaya git** prensibini uygula.

## Adım 1 — Memory Index Tara

Oku: `C:/Users/ydbil/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/MEMORY.md`

Soruyla ilgili memory entry var mı? Varsa o dosyayı oku. **Yeterliyse burda dur — ağır dosya yükleme.**

## Adım 2 — Eşleşme Yoksa Doküman Haritasına Bak

CLAUDE.md'deki dosya haritasına göre hangi belge ilgili?

| Konu | Dosya |
|---|---|
| Class skill detayı | `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md` |
| Onaylı kararlar | `TASARIM/MASTER_KARAR_BELGESI.md` |
| Mob/combat tasarımı | `TASARIM/COMBAT_ROSTER.md` |
| Boss tasarımı | `TASARIM/BOSS_DESIGN.md` |
| Oda mekaniği | `TASARIM/ROOM_MECHANICS.md` |
| Animasyon | `TASARIM/ANIMATION_REDESIGN.md` |
| Aktif faz scope | `TASARIM/FAZLAR/FAZ[N]_*.md` |
| Genel oyun tasarımı | `TASARIM/GDD.md` |

**Sadece eşleşen dosyayı yükle.** Diğerlerine dokunma.

## Adım 3 — Cevapla ve Dosya Kapat

Cevabı ver. Dosyanın tamamını bağlamda tutma — bilgi çıkarıldıktan sonra devam et.

## Adım 4 — Değerli Cevabı Memory'e Kaydet (opsiyonel)

Eğer bu soru tekrar sorulabilecek türdeyse ve cevap non-obvious ise, uygun memory dosyasına ekle.

---

**Kullanım:** `/query warblade'in hangi skilli stagger veriyor?`
