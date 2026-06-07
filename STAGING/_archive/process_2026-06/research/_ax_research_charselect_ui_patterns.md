# Research Task — How do polished games solve these 4 character-select UI problems? (web research)

You are researching for RIMA (dark-fantasy iso pixel roguelite, cyan #00FFCC rift accent + void-purple + warm-orange braziers). Our CharacterSelect has 4 concrete problems reported by the user. Research how shipped games solve each, then give concrete recommendations. USE WEB SEARCH — cite specific games/screens, not generic advice.

## P1 — Side panels unreadable ("hiçbir şey okunmuyor; siyah ama saydam olsun; çok mavi olmuş")
Our left (character identity+stats) and right (skills) panels: text drowns, everything is cyan-on-dark-blue.
Research: Hades (boon/weapon screens), Darkest Dungeon (hero panel), Dead Cells, Children of Morta, Hollow Knight (charms) — how do they treat panel BACKGROUNDS (opacity %, solid vs translucent black, blur?) and TEXT hierarchy (which elements get accent color vs neutral white/grey; size contrast)? What ratio of accent color do premium dark UIs use (e.g. 60-30-10)? Where do warm accents go vs cool?

## P2 — Primary buttons look stale ("GERİ ve KİLİDİ AÇ/SEÇ çok bayat")
Bottom bar: GERİ (back) + SEÇ/KİLİDİ AÇ (select/unlock) buttons look like flat web buttons.
Research: how do Hades ("BEGIN ESCAPE"), Darkest Dungeon ("EMBARK"), Slay the Spire, Monster Train style their PRIMARY action button vs secondary (back)? Shape language (wide banner? angular plate? rune-etched?), hover/pressed states, label typography. Concrete pattern we can copy with 9-slice + TMP.

## P3 — Selection feedback: too much flash ("seçilince karakter çok fazla flash oluyor")
We currently flash the character on click. Research: what NON-flash selection feedback do character/loadout selects use? (Hades weapon aspects, Hades II, Brotato char select, Vampire Survivors, Children of Morta, Cult of the Lamb follower select). Patterns: ground rune fade-in, others-dim, frame/banner lights up, short scale-settle, sfx-only? Which feel "premium subtle"?

## P4 — Color discipline ("renkler doğru yapılsın, çok mavi")
Our screen: cyan title + cyan panels + cyan stats + cyan ring + cyan cracks = monochrome blue soup.
Research/synthesize: accent-color discipline rules from game UI design (and any GDC/article sources): how many UI elements should carry the accent? What should fall back to neutral (off-white/parchment/grey)? Role of a SECOND accent (our warm-orange #E89020) — which elements should be warm (selected? currency? danger?) vs cool?

## OUTPUT (bullets, concrete, cite games)
For each P1-P4: 2-4 real-game patterns (name the game + what exactly they do) + ONE recommended recipe for RIMA (specific: opacity numbers, color assignments, which elements lose cyan, button shape spec, selection feedback spec). Short — no fluff.