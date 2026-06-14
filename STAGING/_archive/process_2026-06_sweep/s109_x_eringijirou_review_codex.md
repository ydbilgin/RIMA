ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

**Output dosyası:** `CODEX_DONE_eringijirou_review.md` — kısa rapor, max 500 kelime.

---

# Amaç

Twitter/X post `https://x.com/eringijirou/status/2059224550718779767` içeriğini **teknik/implementation açısından** incele. agy paralelde vision review yapıyor — sen kod/teknik tarafında ikinci görüş ver.

## RIMA bağlam (kısa)

- Unity 2022.3+, URP 2D Renderer, 2D Tilemap (iso/diamond ve rectangular karışık)
- Aktif paket: `Packages/com.laureth.painter-suite/` (UPM, v0.4.0, ~1000 LOC C#)
- Killer feature: Visual Collider Painter (SceneView drag → BoxCollider2D, 4 shape, resize handles, template SO)
- v1.1+ seeds: GameObject-Free Iso Grid Renderer (mesh/instancing) + World-Space Splat Shader (R/G/B channel terrain) + Auto-Collider from Splat Map

## Görev

Web search ile X postunu/embed videosunu aç. Eğer mümkünse linked artikel/blog/source code'a ulaş.

Şu 5 maddeyi raporla:

1. **Konu (1 satır):** post ne gösteriyor (asset, technique, tool, effect)?
2. **Teknik derinlik:** kullanılan teknik (shader graph / compute shader / ECS / GameObject pooling / mesh combine / instancing / Tilemap extension / 3rd party asset — net bir tahmin). Açık source var mı, hangi library?
3. **Unity API ilişkisi:** RIMA'nın stack'inde (URP 2D, Tilemap, 2D Lights, SpriteRenderer, custom shader) implement edilebilir mi? Hangi API?
4. **Painter Suite entegrasyonu:** Bu Painter Suite'in v1.0/v1.1/v1.2 hangi modülüne uyar? Yoksa RIMA gameplay tarafına mı? Effort (S/M/L).
5. **VERDICT:** ADOPT / PROTOTYPE / SKIP + tek cümle sebep. Eğer ADOPT/PROTOTYPE: 3 satır implementation outline.

## Çıktı

`CODEX_DONE_eringijirou_review.md` — yukarıdaki 5 madde + verdict. Spekülasyon yok, web bulgularına dayan. Kod blocku göstermek istersen max 20 satır.
