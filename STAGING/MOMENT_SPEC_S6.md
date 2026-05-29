# RIMA — Moment-to-Moment Master Spec (UI/UX + Oynanış, S114 S6)

> **Kaynak (4, converge):** UX-workflow (`wtjtm3sv5`, 63 beat) + Gameplay-workflow (`wuykgfi3h`, 83 beat) + cx UX-input + agy UX-input. Opus sentez.
> **Headline (acımasız):** *Demo = oynanabilir iskelet, wishlist-değer YOK. Combat skeleton'un çoğu wired ve ÇALIŞIYOR (hitstop/shake/damage-number/rage/combo/dash) AMA görsel ağırlık (slash-arc/particle/flash) sessiz, HUD/SkillBar/ClassManager/RoomTransitionFX/BossHealthBar kodlu ama sahnede YOK, draft equip kırık, ölüm-ekranı görünmez, boss-win'de Wishlist CTA yok.*

## ✅ ZATEN ÇALIŞIYOR (REDO ETME — 4-kaynak doğruladı)
DamageNumberDriver (scene:10663, OnHit abone) · HitPauseDriver (hitstop 0.04s, scene:10736) · ScreenShakeDriver (scene:10714) · RageSystem (vur+1/hasar+5/kill+3, decay 10/s@1.5s, Fury/Bloodrage event) · MeleeChain combo+knockback+startup-defer · dash i-frame/combo-cancel infra · SkillOfferUI slide-in + ShowReplaceMode.

## ★ MASTER SIRALI SPEC (her an: ekranda-olmalı + oynanış-olmalı)
| # | An | Ekranda (UX) + Oynanış (GP) | Mevcut | Efor | Kanıt/iş |
|---|---|---|---|---|---|
| 1 | **Kalıcı combat HUD** | HP bar (renk-tier + <30% pulse) + **Rage bar** (0'da gizli, max'ta pulse) + oda-label + minimap. **Rage Dominance loop görünür** (vur→dolar/kaç→erir = imza his) | missing | small | HUDController.cs kodlu, **sahneye koy** (backlog #7) |
| 2 | **Draft GERÇEK skill sunar** | 3 gerçek kart (gold/heal değil) + "yeni güç" hook + IronCharge oda1 garantili | partial | small | **#1 fix yaptım** (DraftManager self-heal + AddComponent) — play-verify; + PlayerClassManager sahneye |
| 3 | **Hit-confirm üçlüsü (AĞIRLIK)** | her isabet: **slash-arc (LMB) + HitSpark particle + 0.08s beyaz flash**. "Renkli kareler"i bitiren görsel | missing | small | SlashArc field ata (trivial #4) + **VFXRouter.entries doldur** (#3) + HitFlashDriver enemy'e + Health.TakeDamage'dan çağır (#5) |
| 4 | **SkillBar görünür** | seçilen skill Q/E/R/F slotuna oturur + cooldown radial + hazır-glow | missing | small | SkillBarUI.cs kodlu, sahneye koy (#8, rank 1+2'ye bağlı) |
| 5a | **Ölüm ekranı görünür** | slowmo→siyah overlay + "YOU DIED" + Room/Kills + R-restart | partial | trivial | DeathScreenCanvas **zero-scale fix** (#10) |
| 5b | **Boss-win → Wishlist CTA** | DEMO COMPLETE + run-stat + **steam:// Wishlist butonu** + share-build seed + next-class teaser | partial | medium | DemoCompleteOverlay legacy-Text→TMP+CTA (#14) — **demonun TEK dönüşüm noktası** |
| 6 | **Oda geçişi black-fade** | instant-swap değil → fade-out/hold/fade-in + oda-adı banner | missing | small | RoomTransitionFX.cs kodlu, sahneye koy (#9) |
| 7 | **Boss climax** | boss-bar (alt-orta, faz-renk) + intro (slowmo+isim) + **death→DemoComplete** | partial | small-med | BossHealthBar sahneye + **#2 race fix yaptım** + ⚠️**class-select bypass** (boss-death class-select tetikliyor=Victory ile çakışır) |
| 8 | **Player-hit feedback** | kırmızı vignette + player flash + shake (şu an SADECE SFX, görsel yok) | missing | small | VignetteFlashController player-hit eventi dinlemiyor |
| 9 | **Boss telegraph** | her saldırı zemin-marker/windup-tell (şu an sadece sprite-color → kaçılamaz) | partial | medium | PenitentSovereign.Telegraph → EnemyTelegraph.SpawnLine/Circle |

## 🔴 BUG'LAR (4-kaynak)
- **MapFragment namespace çakışması:** Spawner `Core.MapFragment`, RoomLoader `Environment.MapFragment` dinliyor → **fragment gate'i açmıyor** (reward odası kırık).
- **Boss-death → class-select** Victory/DemoComplete ile çakışıyor (detour/softlock) — boss demo'da bypass etmeli.
- **Duplicate "Systems" GO** (scene:10163+20234) → 2. HitPause/Shake footgun + stale Gate_Room0_Exit → sil (#13).

## 🟢 OYNANIŞ DERİNLİK (çekirdek sonrası)
Tag-synergy görünür+hook (W6) · Rift Altar risk-reward (W7) · 2-stance (W5) · tempo eğrisi (5-oda zorluk) · run-varyasyon · combo-cancel/dash-trail tune.

## YÜRÜTME SIRASI (üretim-dışı, otonom)
rank1 HUD → rank2 draft-equip verify → rank3 hit-confirm üçlüsü → rank4 SkillBar → rank9-bug temizlik → rank5a death-fix + rank6 transition → rank7 boss → rank5b Victory-CTA → rank8 player-hit → rank10 derinlik. Her batch: Opus yaz → cx/agy review → play-verify.
