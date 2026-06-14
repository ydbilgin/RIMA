# RIMA — Entegrasyon Backlog (S114 S6, workflow audit çıktısı 2026-05-29)

> **Kaynak:** `rima-demo-integration-audit` workflow (8 agent, 114 audit bulgusu → ROI-sıralı). 7 Explore (Sonnet) subsystem audit + Opus sentez.
> **demoReadiness (verdict):** "Playable skeleton, not wishlist-worthy: draft gold/heal'e düşüyor (PlayerClassManager + Warblade_SkillController sahnede yok), boss-win 1-frame race ile softlock olabilir, combat renkli kareler — slash arc / hit-flash / hit-kill particle YOK."

## ⚡ NON-GATED (otonom yapılabilir — Opus yazar, cx+agy review)
| # | İş | Efor | Kanıt |
|---|---|---|---|
| 1 | **PlayerClassManager + Warblade_SkillController'ı sahneye ekle** → draft gerçek skill sunar (gold/heal değil) | small | DraftManager.Start skillController=null; GenerateOffers boş→fallback (DraftManager.cs:188-197) |
| 2 | Boss-death→DemoComplete 1-frame race fix (WireBossDeathListener) | small | RoomLoader.cs:350 tek-frame yield → child Health kaçabilir → win softlock |
| 3 | **VFXRouter.entries doldur** (HitSpark/DeathBurst prefab, hit/kill tag) | small | entries boş (scene:10682) → BuildPools erken çıkar → 0 particle. Prefab'lar var (60fa4482/5b5efa99) |
| 4 | SlashArcVFX'i Warblade PlayerAttack.slashArcVFX field'ına ata | trivial | field null (Warblade.prefab:273) → her swing'de arc atlanıyor |
| 5 | HitFlashDriver enemy prefab'larına + Health.TakeDamage'dan Flash çağır | small | built ama 0 instance + TakeDamage çağırmıyor. White-flash = en evrensel hit-confirm |
| 6 | LightPulse'u PlayerLight_Auto Light2D'ye ekle | trivial | Emit çağrılıyor ama subscriber yok (scene:2645) |
| 7 | HUDController'ı sahneye koy (HP/resource bar + oda-adı + prompt) | small | absent → SetHudRoomStatus + tüm HUD çağrıları no-op |
| 8 | SkillBarUI'yi sahneye koy (draft skill'leri HUD slot+cooldown) | small | implemented ama hiç instantiate edilmiyor (rank1/7'ye bağlı) |
| 9 | RoomTransitionFX'i sahneye koy (oda-arası fade) | small | missing → instant swap, odalar zıplayarak geliyor |
| 10 | DeathScreenCanvas zero-scale fix (localScale 0,0,0) | trivial | scene:20451 → ölüm ekranı görünmez → soft-stall |
| 11 | IroncladMomentum passive misclassification + isim uyuşmazlığı | small | SkillBase extends ama PassiveBase değil → pick'te hiçbir şey yapmaz (dead pick) |
| 12 | Berserker/Vampiric elite affix'leri BaseMobBehavior/MobAttack_Melee'ye bağla | small | tint var, caller yok → elite'ler base mob gibi oynuyor |
| 13 | Duplicate inactive Systems GO + stale Gate_Room0_Exit sil | trivial | scene:10163 ikinci CameraShake/HitStop footgun; Gate_Room0_Exit frame-0 kapı bloğu |
| 14 | Gerçek victory/DemoComplete ekranı + Wishlist CTA + run-stat (legacy Text yerine) | medium | DemoCompleteOverlay = hardcoded UI.Text, CTA/stat/share yok — wishlist dönüşüm noktası |
| 15 | FractureImp shardPrefab wire + mis-paste ShardScatter'ı ShardWalker/HollowHulk'tan kaldır | small | shardPrefab null → death scatter no-op |

## 🔒 GATED (kullanıcı kararı — PixelLab/$imagegen + yön)
| # | İş | Efor |
|---|---|---|
| 16 | FractureImp gerçek sprite + controller ata + anim clip sprite-ref onar | large |
| 17 | PenitentSovereign boss sprite + Animator + BossHealthBar göster | large |
| 18 | AudioManager prosedürel SFX → gerçek ses klipleri | medium |
| 19 | Kullanılmayan mob çeşitliliği (ShardWalker skirmisher + HollowHulk tank) sahneye | large |

## Yürütme planı
Non-gated 1-15 → Opus yürütür (çoğu **sahne-wiring** + birkaç küçük kod) Unity MCP ile, batch + her batch sonrası play-verify + cx/agy review. Gated 16-19 → kullanıcı mob/audio kararıyla.
**Sıra önerisi (ORİJİNAL — agy ile güncellendi, aşağı bak).**

## agy ÖNCELİK REVIEW FOLDED (2026-05-29)
**⚠️ Juice'u renkli-karelerin üzerine kurma → sprite gelince REWORK (pivot/arc/flash uyumsuzluğu).** Yerine **interleaved milestone flow:**
1. **#1 skill-controller + #16 FractureImp sprite** → kareleri ekrandan ÖNCE temizle
2. Combat juice (#3,4,5,6 slash-arc/flash/particle/light) → **gerçek sprite pivot'una** giydir
3. HUD (#7,8) · 4. Boss climax (#17 sprite + #2 death-race + health bar) · 5. **#14 Victory + Wishlist CTA**

**En büyük 5 sıçrama:** #16 · #1 · #5+#3 · #17 · #14.
**EKLE (agy — eksik ucuz-yüksek-algı; çoğu CombatJuice'ta VAR ama firing değil, VFXRouter gibi wire gerek):** floating damage numbers · hit-stop 0.03-0.05s (kritik/boss-ölüm) · Hades oda-başı cyan monolog.
**🔑 GATED SPRITE KARARI (agy):** **boss = B (PixelLab/RTX-local — yıldız, asset-flip riski)** · **temel mob = A (arşiv-restore 0-gen, hızlı) → sonra B ile yenile.** Steam asset-flip'e hassas; renkli-kare/NRE/softlock demo → **DE-WISHLIST**. Veri: demo→wishlist %10-20.
**Not:** FractureImp arşiv-restore (A) **0-gen → OTONOM yapılabilir** (gated değil!). Sadece boss-sprite (B) kullanıcı/RTX-local kararı.
