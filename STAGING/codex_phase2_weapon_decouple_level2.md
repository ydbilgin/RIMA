# Codex Phase 2 — Karar #123 Yol A Level 2 (per-frame weapon anchor)

**Karar refs:** #71 weapon-ready / #80 Silhouette Bible / #123 Yol A weapon decouple / Karar #99 weapon visibility
**Sebep:** Faz 1 MVP Karar #123 STRICT — body silahsız sprite + Unity WeaponDatabase attach. Strateji A (attack frame'inde silah sprite içine çizdir) REJECT edildi. Level 2 per-frame anchor zorunlu — silah el pozisyonunu attack/run swing sırasında doğru takip etmeli.

**Mevcut altyapı:** `HandAnchorAttach.cs` (commit 3662ec6, Level 1 statik attach). `WeaponDatabaseSO` + 8 class weapon prefab placeholder.

**Tahmini süre:** 3-5 saat
**Background:** evet
**Dotnet build PASS** her commit öncesi

---

## Görev: Per-frame hand anchor sync between PixelLab animation frames and Unity weapon attach

PixelLab MCP'nin **frame metadata'sındaki keypoint data**'sını kullanarak (veya yoksa fallback Unity-side annotation) her animation frame'inde silah el pozisyonunu doğru takip etsin.

---

## 5 DELIVERABLE (sırayla uygula)

### D1. PixelLab keypoint discovery + metadata parse

**Hedef:** PixelLab animasyon ZIP export'undaki metadata.json yapısında **hand anchor keypoint** var mı yok mu net tespit.

Adımlar:
1. Bir test character'a (veya mevcut 072be2d5'e) template animation ekle (örn. `walk-1`, sadece `directions=["south"]` 1 gen)
2. `get_character` ile job tamam mı kontrol
3. ZIP indir (`curl --fail` ile `https://api.pixellab.ai/mcp/characters/<id>/download`)
4. ZIP içindeki `metadata.json`'u parse et, animasyon frame'leri için keypoint structure'ı document et
5. Çıktı: `STAGING/pixellab_keypoint_structure.md` — keypoint field isimleri, koordinat formatı (pixel/normalized), kaç keypoint var (hand_left, hand_right umulan)

**Karar gate D1 sonu:**
- **Hand keypoint VAR** → D2-D5 PixelLab metadata-driven implement
- **Hand keypoint YOK** → D2-D5 Unity-side manual annotation fallback (Animation Curve veya Custom Importer)

### D2. PixelLab → Unity sprite metadata import

**Hand keypoint varsa (case A):**
- `Assets/Editor/PixelLabSpriteImporter.cs` (yeni): PixelLab ZIP veya tek-tek frame import sırasında keypoint pozisyonlarını sprite atlas'ın **pivot offset**'ine veya custom `SpriteHandData` ScriptableObject'ine kaydet
- Sprite asset Inspector'ında "Hand Left Anchor" + "Hand Right Anchor" pixel coord görünür
- Custom ScriptableObject: `Assets/Scripts/Data/SpriteHandData.cs` → fields: `Sprite sprite`, `Vector2 handLeftPx`, `Vector2 handRightPx`

**Hand keypoint yoksa (case B):**
- `Assets/Editor/SpriteHandAnnotatorWindow.cs`: EditorWindow'da sprite üstüne tıklayarak hand anchor pixel coord set (her sprite için manuel)
- Aynı `SpriteHandData` SO

### D3. HandAnchorAttach Level 2 — per-frame update

`Assets/Scripts/Combat/HandAnchorAttach.cs` modifiye:
- Mevcut Level 1 statik attach davranışı KORUNUR (fallback)
- Yeni Level 2 mode: `[SerializeField] SpriteRenderer bodyRenderer` reference. Her LateUpdate'te (veya animation event'le) bodyRenderer.sprite'ı oku → `SpriteHandData` SO'dan o sprite için handLeft/handRight pixel coord al → pixel-to-world convert (PPU 64) → weapon Transform'ını oraya pin'le
- Editor preview: gizmo ile hand anchor pozisyonları visualize

### D4. WeaponDatabase + weapon prefab pivot align

`Assets/Scripts/Combat/WeaponDatabase.cs` + weapon prefab'ler:
- Her weapon prefab'in pivot (grip point) sprite içinde **net belli** olsun (örn. greatsword için kabza ortası)
- HandAnchorAttach weapon Transform.position'ı = bodyHandAnchor world pos + weapon localGripOffset
- 2-hand silahlar için: Level 2 hand_left + hand_right ikisini kullan (weapon Transform iki nokta arasında orient olur — Vector3.Lerp + Quaternion.LookRotation)

### D5. Test scene + verify

- `Assets/Scenes/Phase2_WeaponAttach_Test.unity` (yeni)
- Test character: 072be2d5 (veya kullanıcının ürettiği herhangi bir character) body sprite + Animator (walk + attack template anim)
- Test weapon: Warblade greatsword prefab (placeholder OK)
- Play mode'da: idle → walk → attack chain, silah el pozisyonunu **her frame'de** doğru takip ediyor mu (manuel gözle + screenshot 3 farklı frame)
- Acceptance:
  - Idle: silah elinde sabit
  - Walk: silah hafifçe oscillate (el ile birlikte)
  - Attack swing: silah el yörüngesini takip eder, "elinden ayrılma" yok
- Bug: silah orientation yanlışsa weapon prefab pivot adjustment

---

## Karar gate D1 (önemli)

D1 sonu kararı orchestrator'a (cx_dispatch.py'de Codex tek dispatch zinciri olduğu için Codex kendi karar verebilir):

```
IF pixellab metadata.json animations frames contain "keypoints" field with "hand_left"/"hand_right" or equivalent:
  Continue D2-D5 with PixelLab metadata-driven path
ELIF metadata contains generic keypoints (head/center/feet only) but no hand:
  Continue D2-D5 with Unity-side manual annotation path (case B)
  Document this finding in STAGING/pixellab_keypoint_structure.md
ELSE (no keypoints at all):
  Continue with case B + flag this as Phase 2.5 follow-up
```

Codex hangi path'i seçtiğini commit message'da belirt (`[S79][D2-B] Manual annotation fallback`).

---

## File Scope

**ALLOWED:**
- `Assets/Scripts/Combat/HandAnchorAttach.cs` (extend, don't break Level 1)
- `Assets/Scripts/Combat/WeaponDatabase.cs` (small fields if needed)
- `Assets/Scripts/Data/SpriteHandData.cs` (yeni SO)
- `Assets/Editor/PixelLabSpriteImporter.cs` (yeni, case A)
- `Assets/Editor/SpriteHandAnnotatorWindow.cs` (yeni, case B)
- `Assets/Scenes/Phase2_WeaponAttach_Test.unity` (yeni)
- `Assets/Data/SpriteHandData_*.asset` (örnek SO'lar)
- `STAGING/pixellab_keypoint_structure.md` (D1 çıktısı, yeni)

**FORBIDDEN:**
- `Assets/Scripts/MapDesigner/**` (S78 Codex Phase 1 dokunma)
- `Assets/Scripts/Character/**` core class scripts (sadece HandAnchorAttach extend)
- `Assets/Scripts/Animation/**` mevcut (animation system Faz 1 stub)
- Karar #131 CornerWangPainter (dokunma)

---

## Workflow (Codex execute)

1. **D1:** Test animation ekle (1 credit), ZIP indir, metadata.json incele, structure document, gate kararı
2. **D2:** Path A veya Path B implement → dotnet build PASS → commit `[S79][D2-A]` veya `[S79][D2-B]`
3. **D3:** HandAnchorAttach Level 2 → commit
4. **D4:** WeaponDatabase + prefab pivot → commit
5. **D5:** Test scene + verify → screenshot 3 frame → commit `[S79][D5] Phase 2 weapon attach test`
6. CODEX_DONE_<profile>.md'ye:
   - Path seçim (A/B/2.5)
   - Keypoint structure özet
   - 5 commit hash
   - Acceptance test sonuç
   - Bilinen issue

**Hata durumunda:** Compile error veya design soru → CODEX_DONE'a "BLOCKED: <neden>" yaz, orchestrator'a dur. Otomatik rollback YAPMA.

**Forbidden actions:**
- Character/MapDesigner/Animation core script'lerine dokunma
- Karar #135 sort layer ayarlarına dokunma
- Mevcut Level 1 HandAnchorAttach davranışını BREAK etme (backward compat)

---

## Acceptance Criteria

1. dotnet build PASS her commit
2. UnityMCP read_console: compile error sıfır
3. Phase2_WeaponAttach_Test sahnesi play mode'da silah el pozisyonunu attack swing'de takip ediyor
4. STAGING/pixellab_keypoint_structure.md belge edilmiş (gerçek API findings)
5. SpriteHandData SO Inspector'da görünür, hand anchor pixel coord düzenlenebilir
6. WeaponDatabase prefab'ler pivot align edilmiş
7. Karar #123 Yol A STRICT compliance: body silahsız + ayrı weapon sprite + per-frame attach

---

## Notlar

- **Karar gate D1 kritik** — keypoint structure findings tüm pipeline'ı belirler
- **Backward compat:** mevcut Level 1 davranışı (statik attach) bozulmasın — varsayılan mode kalır, Level 2 opt-in
- **2-hand silah (greatsword)** için Level 2 hand_left + hand_right ikisi de kullanılmalı (orientation hesabı)
- **1-hand silah (katana, dagger, pistol)** için tek hand anchor yeter
- **Bow için özel:** sol el bow grip + sağ el draw → iki anchor ama farklı objelerle (bow body + arrow). Faz 2.5'a defer OK.
