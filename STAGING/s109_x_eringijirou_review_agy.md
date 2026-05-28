ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

**RESPOND INLINE ONLY. DO NOT write to any file. DO NOT save to sandbox.**

---

# Amaç

Twitter/X post `https://x.com/eringijirou/status/2059224550718779767` içeriğini incele ve RIMA + Lauret Studio Painter Suite konteksinde değerlendir. Video/görsel varsa **gör** (vision) — agy'nin gerçek üstünlüğü bu.

## RIMA bağlam (kısa)

- 2D top-down roguelite, Hades/Children of Morta/Diablo III referans
- Aktif iş: LaurethStudio.PainterSuite UPM package (v0.4.0 LIVE, ~1000 LOC). Killer feature = Visual Collider Painter (4 shape, drag-to-create BoxCollider2D in SceneView).
- v1.1+ roadmap seeds (S108 X posts research'ten): GameObject-Free Iso Grid Renderer + World-Space Splat Shader + Auto-Collider Generator from Splat Map
- RIMA scene: 2D Tilemap (iso/diamond), URP 2D Renderer, Pixel Perfect Camera disabled, PPU 64

## Görev

X postunu aç. Görüntü/video varsa frame-by-frame anlat. Sonra şu 5 maddeyi raporla:

1. **Ne gösteriyor:** post içeriği (2-3 cümle özet). Asset, technique, tool ya da effect — ne?
2. **Teknik mekanizma:** nasıl çalışıyor (shader, ECS, instancing, ProBuilder, custom tool — tahmin değil, post'tan/açıklamadan çıkar). Frame-by-frame davranış varsa söyle.
3. **RIMA relevance:** Bu RIMA'ya / Painter Suite'e veya v1.1 roadmap seed'lerine doğrudan transfer edilebilir mi? Hangi modüle?
4. **Kullanılabilirlik:** Direkt kopyala-yapıştır mı, ilham mı, yoksa bizim akışımıza uymuyor mu? Effort kestirimi (saat).
5. **VERDICT:** TAKE (hemen seed olarak ekle) / WATCH (v1.2 sonrası) / SKIP (ilgisiz) + tek cümle sebep.

## Çıktı formatı

Markdown bullet, max 400 kelime. Spekülasyon yok — gördüğünü söyle. Web search izinli (video erişim için).
