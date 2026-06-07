ACTIVE RULES: (1) think before answering (2) concrete, example-backed, no speculation (3) actionable (4) say UNSURE if you don't know.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

RESPOND INLINE (do NOT write to a file; the dispatcher captures your stdout).

# CONTEXT — RIMA (the game)
2D top-down (HIGH top-down 3/4, ~70-80 deg, Hades / Children of Morta / Diablo look), pixel-art ARPG roguelite.
- Aesthetic: "Hades Elysium V1" — wall-less floating arena over an abyss, cliff edges, cyan #00FFCC rune/rift accents, warm braziers. Theme = "yarik/rift" (cracks in reality), corruption.
- Demo scope (Phase 1): ONE class (Warblade, melee greatsword) + 5-room loop (3 combat -> 1 reward/skill-draft -> boss) + ~4 mob types (FractureImp, ShardWalker, HollowHulk + boss PenitentSovereign) + skill draft (pick 1 of 3 after each room, Hades-style) + map-fragment gate. Target: ~10 min playable loop -> Steam demo.
- Combat: weaponless body + HandAnchor weapon sprite + 8-dir, procedural swing, VFX-first juice (hit pause, screen shake, cyan hitspark). 17 Warblade skills already coded.
- Status: combat works, rooms link, player stable. Cliff visuals + skill-screen wiring + audio still rough. No music/SFX yet.

# AMAC — INDUSTRY + PLAYER research (this is YOUR job: research + ideas; Opus will decide)
Answer concisely, bulleted, with concrete game examples:

1. **2024-2026 roguelite/ARPG trends.** Hangi mekanikler/loop'lar su an one cikiyor? (Hades II, Vampire Survivors, Brotato, Dead Cells, Children of Morta, Enter the Gungeon, Cult of the Lamb, Balatro vb.) Hangileri RIMA'nin top-down ARPG roguelite formatina UYUYOR?

2. **Retention / "bir run daha" hook'u.** Oyunculari roguelite'ta ne BAGLIYOR? Meta-progression, build cesitliligi, run varyasyonu, sinerji/combo sistemleri, risk-reward (Hades heat / Dead Cells boss cells). Somut, demo-olcekli ornekler.

3. **RIMA scope kritigi.** Mevcut tek-sinif demo loop'una bakinca: NE EKLENMELI (yuksek-ROI, demo-olcekli — orn. skill sinerjisi, oda-modifier, mini-elite, para/ekonomi)? NE CIKARILMALI / ERTELENMELI (over-scope riski — orn. cross-class, 10 sinif, live-editor)?

4. **Narrative entegrasyonu.** Bir roguelite DEMO'sunda hikaye nasil ANLAMLI ve UCUZ entegre edilir? Hades'in run-ici diyalog + olum-dongusu modeli, environmental storytelling, boss-intro. RIMA'nin rift/cyan/PenitentSovereign temasiyla nasil baglanir? 2-3 somut, dusuk-maliyetli fikir.

5. **Steam-demo cilasi.** 10 dk'lik bu loop'u "wishlist cektiren" bir demoya ceviren 3-5 SOMUT sey (oyun-hissi, ilk-5-dk, hook, juice).

Kisa tut, madde madde, uygulanabilir. Bilmiyorsan UNSURE yaz.
