# Task — Mob death decal/ghost (code-only) — ax-Opus-4.6 PILOT görevi

Amaç: Mob ölümü şu an görsel olarak boş — ölümde kısa squash/fade + yerde kalıcı-ish iz (decal) ekle. Kod-only, asset üretimi YOK.

Bağlam (oku): `CODEX_DONE_yasinderyabilgin.md` §2 (taze file:line envanteri):
- Ölüm eventi = `Health.OnDeath` (`Assets/Scripts/Core/Health.cs:53-56`), `DeathVFX.cs:17-33` zaten subscribe ediyor, `BaseMobBehavior.cs:200-224` merkezi ölüm hook'u.
- DİKKAT: hem `BaseMobBehavior` hem `EnemyAnimator` (`:77-95`) root'u destroy edebilir; decal DESTROY EDİLEN mobun child'ı OLMAMALI — bağımsız GO olarak spawn et.
- Özel ölüm davranışları (`VoidThrall_DeathSplit`, `Penitent_AntiHealAura`, `MobAffix_Fractured`) kırılmamalı — onlara dokunma.
- ⚠️ YENİ: Bu session knockdown paketi eklendi/ekleniyor (`KnockdownDriver.cs` vb. `Assets/Scripts/Core/`) — ölüm anında knockdown aktifse çakışma olmasın (ölüm her şeyi iptal eder).

Yap (LEAN):
1. Yeni component `Assets/Scripts/Enemies/MobDeathResidue.cs`: `Health.OnDeath`'e subscribe; ölümde (a) sprite'ı ~0.2s'de Y-squash→0 + alpha fade (coroutine bağımsız runner'da — mob destroy olunca coroutine ölmesin), (b) mobun pozisyonunda bağımsız decal GO spawn (koyu yarı-saydam leke; mevcut bir sprite reuse et — ör. GroundBlobShadow sprite'ı veya basit Sprite üret runtime'da), decal ~6-10s sonra fade-out + destroy.
2. Auto-attach: `BaseMobBehavior.Awake`'te yoksa ekle (EnemyAnimator destroy path'iyle çift-spawn OLMASIN — tek guard flag).
3. Sıralama/sorting: decal zeminin üstünde, entity'lerin altında (mevcut depth-sort düzenine uy — Custom-Axis Y sort, Entities layer).

Kısıtlar: scene dosyası düzenleme YOK · sadece listelenen + yeni dosyalar · compile-clean + console 0 error doğrula (UnityMCP) · mümkünse play-mode'da bir mob öldürüp gözle (UnityMCP execute/screenshot) · COMMIT YAPMA (cx cross-review edecek).

Rapor: `STAGING/_done_T2_deathdecal.md` (dosyalar, doğrulama kanıtı, bilinen sınırlar).
