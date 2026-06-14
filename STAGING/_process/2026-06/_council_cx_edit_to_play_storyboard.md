ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  NB=$(cat .claude/nlm.local 2>/dev/null); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<your question>"
  (NLM_NOTEBOOK_ID gizli/gitignored — ham ID repo'ya konmaz)
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amac
RIMA "Edit-to-Play" demo videosunun saniye-saniye storyboard'i icin FEASIBILITY / REUSE lensiyle gorus ver. ANALIZ ONLY, kod degisikligi YOK. Sonucu CODEX_DONE.md'ye yaz, donus ≤15 satir ozet + dosya yolu.

# Lens (senin acin: CODE / FEASIBILITY / WHAT-ALREADY-EXISTS-IN-RIMA / REUSE)
Sen kod tarafini biliyorsun. Soru: storyboard'da gosterilecek editor god-node'lari GERCEKTE su an calisiyor mu, hangi tetik/hotkey ile, hangi akista demo-sirasinda PATLAR? Hangi sira en az bug riskiyle "edit-to-play" momentini kanitlar?

# Baglam
RIMA sunumunun centerpiece'i = "Edit-to-Play" demo videosu. Tez: RIMA bir oyun DEGIL, bir "environment + ilk vertical slice" (domain-specific reusable tooling). Graphify full-map kaniti: 6925 node, god-node 6/10 = editor araci. Eksen: %20 oyun / %60 mimari / %20 graphify-audit. Video ~2-4 dk, gercek OBS ekran kaydi (overlay UI ekran kaydinda CIKAR). Sunum ~20 Haziran.

Editor god-node adaylari:
- In-game seviye editoru / Build Mode (F2 hotkey, eski IMGUI InPlayMapPaintOverlay)
- IsoRoomBuilder / Room Browser (oda = cliff-tile yuzen ada; _Arena = temiz canonical)
- Director Mode = Stat + Spawn + Telemetry + Prop/Light placement
- Skill/build sistemi, reward->kart akisi

Golden-path (videoda kusursuz calismasi sart minimal oyun akisi): oda gir -> dusman -> reward -> kart -> sonraki oda.
BILINEN BUG'LAR (golden-path'i bozabilir, gostermekten kacin): F1 reward room-leak (oda gecisinde onceki odul kaliyor), F2 reward al->kart cikmiyor.

# Cevapla (1-5)
1. Saniye-saniye shot listesi (0:00-...): ne gosterilecek, hangi editor araci, hangi an Play'e gecis, "edit-to-play" momentinin tam nerede oldugu. Kod-gercekligine dayandir (hangi hotkey/tool gercekten var).
2. Hangi 3-4 editor god-node'u vitrine konmali — kod-olgunluguna gore (en stabil + anlatiyi en cok guclendiren).
3. Golden-path videoda nerede gosterilir, F1/F2 bug'larini nasil BYPASS ederiz (hangi oda/akis bug'a girmeden calisir).
4. Riskler/tuzaklar (demo-aninda patlama riski olan tool'lar, "oyunu satmaya calisma" tuzagi, cok teknik olup juriyi kaybetme).
5. Acilis hook'u (ilk 10 saniye) ve kapanis.

Onceki bir audit'i TEKRAR URETME. Kisa, kararci, kod-gercekligine dayali ol.
