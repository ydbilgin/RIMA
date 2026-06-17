# RIMA — ChatGPT Full Review Pack (2026-06-17)

> **ChatGPT'ye:** Bu repo'yu (github.com/ydbilgin/RIMA, `master`) doğrudan okuyabiliyorsun. Aşağıdaki bağlam + sorularla **demo-bitirme** kararımızı ve özellikle yeni keşfedilen **combat sorununu** değerlendir. Ekran görüntüleri repo'da: `STAGING/_process/2026-06/demo_screenshots/capture_v3/` (35 taze PNG).

## 1. RIMA nedir (tez)
2D top-down roguelite ARPG, **akademik bitirme projesi**. Sunum tezi: "**sadece oyun değil — bir environment + vertical slice + yeniden-kullanılabilir oyun-içi TOOLING**". Centerpiece = **Edit-to-Play** (F2 Build Mode'da oda kur → çık → aynı odayı oyna) + **Director Mode** (runtime stat/spawn/telemetry aracı). Değerlendirme ekseni ~%20 oyun / %60 mimari / %20 graphify-audit. Demo tarihi **~19 Haziran**. Act1 "Shattered Keep" görsel kanonu: slate #3A3D42 + void-mor #3A1A4A + ember #E89020, cyan ≤%15, grimdark DEĞİL, chibi top-down 3/4.

## 2. Bu oturumda ne yapıldı
- **Ultracode council** (23 agent, graphify+22-ekran+kod toprakli) → kilitli bitirme kararı: `STAGING/DEMO_BITIRME_DECISION_2026-06-17.md`.
- **35-state taze capture** (capture-truth doğrulamalı, 0 dup SHA): `capture_v3/`.
- **A1 PixelLab prop import** → F2 Build Mode catalog (AllProps 9→19).
- **Director Mode IDE-dock skin** (eski en-zayıf UI → düzgün dock layout).
- **🔴 Combat'ın BOZUK olduğu keşfedildi + kısmen düzeltildi** (aşağıda — EN ÖNEMLİ).
- **Telegraph VFX spec** (boss saldırı belirtileri): `STAGING/ENEMY_TELEGRAPH_VFX_SPEC_2026-06-17.md`.

## 3. 🔴 EN KRİTİK: Combat bug keşfi + kısmi fix (senin asıl değerlendirmen)
**Keşif:** capture'da "combat" etiketli kareler aslında **draft/death ekranı** çıktı (KILLS 0 / SÜRE 00:00) → data-driven verify → **COMBAT BROKEN**:
- **Engagement:** düşmanlar IDLE kalıyordu — `BaseMobBehavior.detectionRange=8` < spawn mesafesi (9.12); HalfThrall prefab'ı detectionRange=6. → düşman hiç engage etmiyor (140sn boş).
- **Lethality:** kümede 3-4sn'de ölüm; Penitent-mob combo 20/25/40 (=85) × eşzamanlı = 170 vs 100 HP.

**Uygulanan fix (ax Opus 4.6):** base detectionRange **8→12** + tüm prefab'lar güncellendi + Update'te Player re-acquire eklendi. `Assets/Scripts/Enemies/BaseMobBehavior.cs`. **Sentetik testte doğrulandı** (11u'daki mob bile chase→attack).

**⚠️ AÇIK RİSKLER (senin görüşünü istiyoruz):**
1. **Gerçek full-flow combat test YAPILMADI** — sadece sentetik (elle instantiate). "yeşil-assert ≠ çalışıyor" riski.
2. **`DemoPlayer` Untagged** (_Arena sahnesinde) — gerçek oyuncu runtime'da "Player" tag'i alıyor mu? Almıyorsa detectionRange fix işe yaramaz. **Kod: `BaseMobBehavior.Start()` FindGameObjectWithTag("Player").**
3. **Penitent-combo lethality** (85×2=170) pilot wave'de (`Assets/Resources/Encounters/Act1_Wave_Pilot.asset`, enemyType 3) hâlâ var — token gating (maxMelee=2) damage'ı düşürmüyor.

## 4. Ekran görüntüleri (`capture_v3/`, 35 PNG)
Kilit kareler: `11/12/13_combat*` (aslında death/draft = bug kanıtı) · `32-35_boss*` (Penitent boss, health-bar, slash VFX) · `16/20_*draft` (skill kartları, iyi) · `21_runmap` (8 node-art branching) · `24-28_buildmode*` (F2 araçları) · `17-23_director*` (IDE-dock tab'lar) · `30_merchant` · `31_elite`.

## 5. SANA SORULAR (öncelik sırasıyla)
1. **Combat fix yeterli mi?** detectionRange 8→12 + Player re-acquire doğru yaklaşım mı? `DemoPlayer` untagged sorunu gerçek bir demo-killer mı, nasıl kesin doğrularız (full-flow)?
2. **Penitent lethality** pilot wave'de demo-risk mi? Token gating yeterli mi yoksa opening wave'den Penitent-mob'u çıkarmalı / damage'ı mı düşürmeli?
3. **Bitirme planı** (CombatJuice→_Arena, SeloutOutline enemy-outline, telegraph boss P2/P3, prop döşeme, capture-truth) ~2 gün için doğru öncelik mi? Eksik/yanlış olan?
4. **19 Haziran öncesi demo-killer riskler** neler (gözünden kaçırdığımız)?
5. **Sunum tezi** (environment+tooling) repo kanıtıyla iyi destekleniyor mu? Hangi kanıt zayıf?
6. **Telegraph yaklaşımı** (mevcut `EnemyTelegraph.cs`'i reuse, boss P2/P3 + SpawnDelayedRing/FlashImpact) sağlam mı?

## 6. Mevcut durum (dürüst)
**DONE+test:** GATE bootstrap, Boss presentation, HUD, reward-bleed, Build Mode functional, Director skin, A1 props.
**Kısmi:** combat engagement fix (sentetik-verified, full-flow değil).
**Pending:** CombatJuice, SeloutOutline, telegraph, döşeme, capture-truth, music bed (CC0 hazır).
**Kısıt:** PixelLab balance=0 (yeni sprite yok), demo ~19 Haziran.
