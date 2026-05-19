ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE_yasinderyabilgin.md AS THE VERY LAST STEP.

# Codex Görev: Painted Background vs Hybrid — Will it Really Work for RIMA?

**ACTIVE RULES:** (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.

**NLM ACCESS:** uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

## User Direktif Soru (2026-05-19 S95)

> "bu şekilde painted olarak yapılınca doğru çalışıyor mu oyun gerçekten güzel olacak mı?"

User'ın endişesi: Önceki Codex dispatch "Path A (painted background per room)" önerdi. User şüpheci — oyun gerçekten çalışır mı, güzel olur mu? Pure painted yola gitsek RIMA roguelite mimarisine uyacak mı?

Önceki dispatch verdict link: `STAGING/CODEX_DONE_yasinderyabilgin.md` (henüz daha önce yazıldı, son version oradadır).

## Bağlam Recap

**RIMA mimarisi (LIVE):**
- Roguelite (Karar #149: 3-5 sub-room sequence per encounter, ~20-30 sub-room template hedef)
- Karar #150: Fake-iso + dungeon-inside, 32×22 default sub-room
- Karar #100: 35° tilt baked art (8-dir character, sprite-based)
- Karar #144: Weaponless body + WeaponSR child
- Roadmap (Karar #150): 3 Act × ~110 PNG envanter target

**V4 reference image:** Codex image_gen gpt-image-1 backend ile üretildi. PAINTED whole-room composition (Path A formula).

**Mevcut envanter (Act 1):** 119 PNG
- Walls (4 unified)
- Pillars (3), arches (2), statues (3), ritual (5)
- Mounting decoration (15, pure-attachment no-wall LOCK applied)
- Mobs (16), props (13)
- Floor tiles 35, decals 16, patches 3, rift 3

**Önceki Codex verdict (özet):**
- Path A (pure painted) önerildi 1-oda demo için
- Path C (hybrid: painted floor + sprite walls) production için
- Path B (modular+shader) reddedildi

## Codex'e Asıl Soru

User'ın endişesini doğrudan cevapla:

**Sor 1 (CRITICAL):** Painted background yaklaşımı (Path A) RIMA için tam çalışır mı? Spesifik olarak:
- a) Visual: v4 hedefine ulaşır mı gerçekten?
- b) Gameplay: roguelite mekaniği (3-5 sub-room sequence, procedural variation Karar #149) ile UYUMLU mu?
- c) Iteration cost: visual tweak'ler için her seferinde full image re-gen pratik mi yoksa style drift olur mu?
- d) Production cost: 30+ sub-room template için painted yol scalable mı?

**Sor 2 (CRITICAL):** Hybrid (Path C: large painted floor texture + sprite walls/props) RIMA için daha mı doğru? Hades-Pyre-Dead Cells gibi indie roguelite'lerin gerçek yöntemi bu mu?

**Sor 3:** Real-world örnek karşılaştırma:
- Octopath Traveler (HD-2D, painted) — RIMA modeli olabilir mi?
- Hades (semi-painted + sprite) — RIMA modeli olabilir mi?
- Dead Cells (tile + heavy painterly) — RIMA modeli olabilir mi?
- Hangisinin yöntemi RIMA'ya en uygun, neden?

**Sor 4:** "Test demo 1 saat" yaklaşımı: 1 oda painted test ile feel proof yapıp, sonra production yöntemi karar vermek mantıklı mı? Risk-managed mı yoksa zaman israfı mı?

**Sor 5 (HONEST):** User'a vereceğin TEK NET CÜMLE: "Painted yol seç" veya "Hybrid yol seç" veya "Test demo ile karar ver". Argümanlı.

**Sor 6:** Eğer hybrid önerirsen, "büyük painted tile" implementasyonu net:
- Tile size kaç (256? 512?)
- Kaç adet farklı tile (4 material × 4 var = 16?)
- Tilemap component vs single large SpriteRenderer
- PixelLab / Codex image_gen ile prompt template
- Walls/props sprite overlay scale + composition rules

## Çıktı Format

```
## Path A (Pure Painted) — Gerçek Çalışır mı?
[a/b/c/d cevap, kısa actionable]

## Path C (Hybrid) — RIMA'ya Uyar mı?
[gerçek değer + Hades/Pyre comparison]

## Real-World Comparison
Octopath: [RIMA'ya uyar mı]
Hades: [...]
Dead Cells: [...]
Recommended for RIMA: [seçim + neden]

## Test Demo Strategy
[1 saat 1 oda test mantıklı mı]

## TEK NET VERDICT
[Painted / Hybrid / Test ile karar — argümanlı 2-3 cümle]

## Hybrid Implementation (eğer Hybrid seçilirse)
- Tile size: ...
- Variant count: ...
- Tilemap vs SpriteRenderer: ...
- Image_gen prompt template: ...
- Sprite overlay rules: ...
```

Cevap max 2000 token. NLM gerekirse query yap. Code önerme (planlama only).


---
ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE_yasinderyabilgin.md AS THE VERY LAST STEP.