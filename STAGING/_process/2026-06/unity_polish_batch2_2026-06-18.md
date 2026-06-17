# GÖREV: Unity Demo Polish Batch 2 (3 cerrahi fix) — TEK Unity ajanı

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

UNITY ERROR CHECK: iş bitince read_console (Error+Warning); kendi hatanı ÇÖZ, ilgisiz/önceden-var hatayı BİLDİR (silme), raporda console durumunu yaz (0-sürpriz).

NLM ACCESS: RIMA tasarım bağlamı gerekirse:
  NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>"

## Bağlam
Demo YARIN (~19 Haz). Sınıflar: Warblade + Elementalist. Unity ŞU AN açık + 0 error (temiz başlangıç). Dev-test sahnesi `_Arena`. **Kullanıcı Unity'de aktif olabilir — recompile onu kesebilir, raporda belirt.**

⛔ DOKUNMA: GATE / Boss-akış / reward-bleed / Build-çekirdek / weaponless-anim / branching-seed. **Prop collider Phase B-2 alanlarını (colliderShape/ratio) REFACTOR ETME** — onlar ölü kod, bırak; sadece aşağıdaki cerrahi DATA değişiklikleri yap.

## FIX A — HUD HP rengi cyan→crimson (KOD-S)
**Sorun:** HUD'da HP barı CYAN görünüyor; olması gereken CRIMSON. Kök neden: runtime class-tint logic HP fill'i de sınıf rengiyle boyuyor.
**Hedef dosya:** `HUDController.cs` (Grep: `class-tint`, `classColor`, `hpFill`, `Color`). Önce class-tint mantığını OKU.
**Fix:** HP bar **her zaman crimson `#C01020`** olmalı (sınıf rengi HP'yi override ETMESİN). Resource/mana barı sınıf rengi / cyan `#10A0C0` kalabilir. Düşük-HP titreme efekti KORUNUR.
**Doğrulama:** Play `_Arena` → execute_code ile HUD HP fill `Image.color`'ı oku → crimson civarı (R yüksek, G/B düşük) olduğunu data-proof et. Screenshot opsiyonel.

## FIX B — Prop collider taban-merkez (KOD-S + DATA) — #2
**Sorun:** `PropColliderAutoBuilder` offset = `size*0.5` → collider tabandan yukarı kayık; sandık/fıçı içinden yürünüyor.
**Hedef:** `PropColliderAutoBuilder.cs` (Grep ile bul). Offset'i **taban-merkeze** düzelt (collider sprite'ın alt-tabanına otursun, üst-merkeze değil) + per-prop `blocksWalkable` + `footprintSize` data desteği (`PropDefinitionSO` zaten alan içeriyorsa onu kullan, yoksa MİNİMUM ekle).
**KISIT:** B-2 (colliderShape/colliderRatio) sistemine DOKUNMA — ölü, bırak. Sadece offset matematiği + footprint.
**Doğrulama:** `_Arena`'da bir sandık prefab'ı spawn et / mevcut props üzerinde execute_code ile collider bounds vs sprite bounds taban hizasını data-proof et.

## FIX C — Reward drop hitbox + yarık (DATA-S) — #5b + #4
**#5b:** `RewardPickup` prefab'ının trigger collider radius'u çok büyük (grid-kare gibi) → daralt (elmas/item görselinin gerçek boyutuna). Prefab: Grep `RewardPickup`.
**#4 yarık (chasm):** Yer-yarığı yürünür ama orta-blok olmalı: `blocksWalkable=false` (üstünden geçilebilir) + prefab'a KÜÇÜK merkez collider (tam ortasına düşülmesin) + **Decals** sorting layer (yer-decal). #1 prop-Ysort fix'iyle tutarlı.
**Doğrulama:** execute_code ile RewardPickup trigger radius değerini + chasm collider varlığını/layer'ını data-proof et.

## ÇIKTI (E1 — DOSYAYA yaz, dönüşte ≤10 satır + dosya yolu)
Sonucu yaz: `STAGING/_process/2026-06/BUILDER_POLISH_BATCH2_DONE.md`
İçerik: her fix için (dosya, değişiklik, data-proof sonucu) + son `read_console` durumu (0 error mi?) + DOKUNULMAYAN B-2 notu + kullanıcı-Unity-aktif uyarısı varsa.
