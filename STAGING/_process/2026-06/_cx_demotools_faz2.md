ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Capstone SUNUM demosu için Director Mode'a (oyun-içi runtime tool, backtick `) **prop/ışık yerleştirme** ekle + **rift crystal prefab** + **stat validation hook** + demo-güvenlik sertleştirmesi. UnityMCP kullan (Unity açık, edit mode). KILIT karar: `STAGING/DEMO_TOOLS_SCOPE_DECISION_2026-06-12.md`.

# Bağlam (smoke-doğrulanmış)
- `Assets/Scripts/UI/DirectorMode.cs` (RIMA.DirectorMode) overlay, 6 sekme. Spawn sekmesi ZATEN "palette→ghost→tıkla yerleştir→sağ-tık sil" yapıyor (`SpawnSelectedEnemyAtForValidation`, `EraseDirectorEnemyAtForValidation`, `DirectorSpawnedEnemyCountForValidation`, `SelectFirstSpawnEnemyForValidation`, `HasSpawnGhostForValidation` validation hook'ları var). Build/Map sekmeleri `AddEmptyPanel(... "yakinda")` stub.
- Işık component'leri MEVCUT: `Assets/Scripts/Environment/LightFlicker.cs`, `Assets/Scripts/VFX/LightPulse.cs`. Sprite: `Assets/Art/Props/edge_filler_rift_shard.png` (cyan kristal).
- Stat sistemi runtime canlı (`ClassStatProfile→ClassStatRuntime→DamagePacket→DamageCalculator`), `OnStatSliderChanged` private. AMA stat-tune için public `*ForValidation` hook YOK.

# YAPILACAKLAR (surgical, listeli)

## 1. Rift crystal prefab (asset = ışık birlikte)
- Prefab: `Assets/Prefabs/Props/ShatteredKeep_PixelLab/rift_crystal.prefab`
  - Root: SpriteRenderer = `edge_filler_rift_shard.png` (sortingLayer/order gameplay prop seviyesinde, PPU 32 import doğrula)
  - Child "Light": `Light2D` (Point, renk cyan #2BD9D9, intensity ~1.0, radius makul) + `LightFlicker` (yavaş subtle pulse — mevcut component'in alanlarını kullan, yeni alan UYDURMA)
- Not: tek prefab = asset + ışık. Koyunca ışık da gelir.

## 2. Director Prop/Light placement (Build sekmesini gerçekle)
- Build sekmesindeki "yakinda" stub'ı yerine **prop palette** koy — Spawn sekmesi UX'ini AYNALA (palette→ghost preview→tıkla yerleştir→sağ-tık sil). KOD reuse, yeni paradigma yok.
- Prop kaynağı = küçük CURATED liste (en az rift_crystal). En yalın yol: Director'a serialize edilmiş `List<GameObject> directorPlaceableProps` (Inspector'dan atanır) VEYA Resources'tan yükle. Karmaşık PropRegistry/LiveTool portu YAPMA.
- Yerleştirilen prop'lar `directorSpawnedEnemies` gibi ayrı `directorPlacedProps` listesinde tutulsun (transient kabul; kalıcılık demo için şart değil).
- **Validation hook'lar ekle** (Spawn'ınkileri aynala): `SelectFirstPropForValidation()`, `PlaceSelectedPropAtForValidation(Vector2)`, `DirectorPlacedPropCountForValidation()`, `ErasePlacedPropAtForValidation(Vector2)`, `HasPropGhostForValidation()`.

## 3. Stat validation hook
- Public `bool SetStatForValidation(string statKey, float value)` ekle → mevcut `OnStatSliderChanged` / ApplyCurrentPrimaryStatsToPlayer yolunu çağırsın (yeni stat yolu UYDURMA, mevcut slider mantığını public'le). statKey örn: "physPower","abilityPower","maxHP","moveSpeed","attackSpeedMult","debugGlobalDamageMult".

## 4. Demo-güvenlik
- **Spawn cap:** Director spawn aktif düşman sayısı max ~10 (aşınca yeni spawn no-op + log). 
- **Stat slider clamp:** serbest aşırı değer engeli (zaten slider'sa min/max doğrula; yoksa Clamp ekle).
- **Class buton:** sadece implement edilmiş sınıfları göster/aktif et (desteklenmeyen sınıf controller'sız çökmesin). Mevcut destekleneni TESPİT ET, gerisini disable.

## 5. Test GATE
- İlgili EditMode testi ekle (Spawn validation testlerini örnek al): prop place→count, erase→count düşer, spawn cap, SetStatForValidation stat'ı değiştiriyor.
- `run_tests` EditMode → **0 yeni fail**, mevcut 29 yeşil kalmalı.

# Kısıt
- Combat/encounter/roguelite koduna DOKUNMA. Sadece DirectorMode + yeni prefab + (gerekirse) küçük yardımcı.
- `#if DEMO_BUILD || DEVELOPMENT_BUILD || UNITY_EDITOR` guard'ını koru.
- BLOCKED kalırsan CODEX_DONE.md'ye yaz, kör implement etme.
- Sonucu CODEX_DONE.md'ye: değişen dosyalar + test sonucu + eklenen validation hook imzaları (Faz 3 review + Faz 4 harness için).
