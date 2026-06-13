ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
"Inversion Day" mekaniğinin (uzayın katı↔boş durumunu runtime tersine çevirme) RIMA'nın MEVCUT sistemlerinde teknik feasibility'sini ve reuse-vs-build maliyetini ölç. ANALYSIS ONLY, kod değişikliği YOK.

# Mekanik (soyut)
Oyuncu "inversion" ile bir alanın KATI/BOŞ ikili durumunu tersine çevirir: boş alan → üzerinde durulabilir katı zemin/platform; katı duvar → geçilebilir boşluk/uçurum. Side-scroll'da platform/puzzle için kullanılıyor. RIMA top-down ARPG olduğu için karşılığı: **duvar↔yürünebilir zemin↔uçurum** runtime toggle.

# Sorular (feasibility / reuse lens — RIMA KODUNA bak)
1. **Tilemap/collision mimarisi:** RIMA'da zemin/duvar nasıl temsil ediliyor? (Tilemap + TilemapCollider2D mı, ayrı collision layer mı, NavMesh/grid mi?) Runtime'da bir hücreyi katı↔boş arası toggle etmek MEVCUT sistemle mümkün mü, yoksa yeni altyapı mı gerekir? İlgili dosya/path'leri ver.
2. **Düşman pathfinding/AI:** Düşmanlar zemine/engele nasıl tepki veriyor? Runtime'da zemin değişirse (duvar açılır / zemin uçuruma döner) pathfinding bunu handle eder mi, yoksa kırılır mı? (Act1 prop/door/placement sistemi `STAGING/PROPS_DOORS_PLACEMENT_PLAN` ile ilişkili olabilir.)
3. **Reuse:** RIMA'da zaten kısmen var olan ve bu mekaniğe DAYANAK olabilecek sistem var mı? (prop placement %80 hazır deniyor, BridsonPoisson+validator+CompositionRole; door sistemi; herhangi bir destructible/togglable tile?) Sıfırdan mı, yoksa mevcut üstüne mi?
4. **Combat entegrasyonu maliyeti:** Bir SKILL olarak (tek sınıfa özel signature: küçük alanda duvar yarat/zemin-uçuruma-çevir) implement etmek vs TÜM odaları invertible yapan bir SİSTEM — iki uçtaki kabaca iş yükü/risk farkı nedir?
5. **Roguelite kırılma riski:** Procedural oda layout'unu oyuncunun runtime değiştirmesi RIMA'nın encounter/balance/validator sistemini bozar mı?

ANALYSIS ONLY. Sonucu CODEX_DONE.md'ye yaz. Önceki audit'leri tekrar üretme.
