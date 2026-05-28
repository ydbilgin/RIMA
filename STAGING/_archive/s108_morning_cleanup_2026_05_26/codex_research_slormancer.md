# Codex Research Task — The Slormancer (Steam)

## Görev
Steam ARPG/roguelite **The Slormancer** (https://store.steampowered.com/app/1104280/The_Slormancer/) oyununu **teknik mekanik açıdan** araştır. RIMA projemizle (2D top-down chibi roguelite, 64×64 sprite, Hades-tarzı oda) **karşılaştırılabilir** boyutları çıkar.

## RIMA Context (referans)
- **Kamera:** High top-down ~30-35° (Hades match)
- **Sprite:** 64×64 chibi pixel art, PPU=64
- **Renderer:** URP 2D Renderer + Pixel Perfect Camera + 2D Lights
- **Yön:** 4 cardinal (N/S/E + W=flipX)
- **Anim FPS:** 10-12
- **Class sistem:** 10 sınıf, her birinde LMB/RMB/F + 4 equip slotu, Hades tarzı 3-seçim ödül
- **Skill taksonomi:** 4 aktif (STRIKE/ZONE/REACTIVE/STATE) + 3 pasif (KEYSTONE/MODIFIER/RESONANCE)
- **Cross-class:** 90 kombo + 50 Shadow Echo
- **Combat:** Input buffering, dash i-frame, hitstop, screen shake (game feel bible)
- **Room flow:** Hades-tarzı oda → 3 seçim ödül → boss

## İstenenler
1. **Kamera/perspektif açısı:** Slormancer top-down mu, isometrik mi, açı kaç derece? RIMA'nın 30-35° high top-down'una benzer mi? Screenshot/video referansı temel al.
2. **Sprite çözünürlüğü ve sanat stili:** Pixel art mı, hi-res mi? Karakter sprite boyutu yaklaşık? RIMA'nın 64×64 chibi'sine yakın mı?
3. **Class sistemi:** Kaç sınıf, her sınıfın skill sayısı, skill ağacı tipi (linear/branching/passive tree)?
4. **Skill modifier sistemi:** Slormancer'ın çok övülen "Slorm" (skill modifier) sistemi nasıl çalışır? Skill başına kaç modifier slotu, modifier nereden geliyor (drop/craft/level)?
5. **Loot/itemization:** Item rarity, slot sayısı, set bonus var mı, legendary mantığı?
6. **Roguelite mi yoksa ARPG mi?** Run-based mi, persistant character mi, hybrid mi? Death penalty?
7. **Combat feel:** Hitstop, screen shake, dash, parry var mı? Skill iptal sistemi?
8. **Boss tasarımı:** Faz geçişleri, telegraph, mekanik çeşitliliği?
9. **Endgame:** Mapping, infinite scaling, leaderboard var mı?
10. **RIMA için adapte edilebilirler (Tier S/A/B):** Hangi mekaniklerin RIMA'nın 2D chibi roguelite'ına %1-1 adapte edilebilir, hangileri scope dışı?

## Format
- Madde işaretli, tablo şeklinde rapor
- Her bulguya kaynak referansı (Steam review, wiki, dev blog, YouTube video URL)
- Final tablo: **RIMA-adaptasyon Tier S/A/B** (S = doğrudan al, A = adapte et, B = düşün, X = al-ma)
- Steam URL temel al; gerekirse PC Gamer / RPS / Reddit / Slormancer wiki / dev Discord public posts referans
- Hangi mekanikler **RIMA Karar #74/#100/#144/#145 LIVE LOCK** ile uyumlu, hangi değil belirt

## Output
`STAGING/codex_research_slormancer_DONE.md` — uzunluk hedef 800-1500 kelime, ham bilgi + Codex değerlendirme.
