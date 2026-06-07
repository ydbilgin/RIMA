ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Iki mekanik referans dosyasini oku, RIMA'ya HANGI mekanikler uygulanabilir feasibility/reuse acisindan degerlendir. SADECE ANALIZ — kod yazma/degistirme YOK. Opus senin + 2 Gemini'nin gorusunu sentezleyecek.

# OKUNACAK KAYNAKLAR (READ THESE)
1. STAGING/mechanic_refs/Youtube_Mina_The_Hollower_Full_Transcript.md  (Mina the Hollower — Zelda-like action game; tasarim/mekanik transkript)
2. STAGING/mechanic_refs/Youtube_61_Mechanics_Detailed.md  (61 roguelite/action mekanik detayli liste)

# RIMA baglam (kisa)
RIMA = 2D iso-gorunumlu top-down roguelite ARPG (Unity URP 2D, PPU64). Setting "Shattered Keep"; cyan seal-energy (#00FFCC) + void-purple. 10 sinif, 8-yon sprite, Hades/Diablo 3/4 gorus. Imza mekanik = "Sundered Beat" (BREAK -> EXECUTE). Su an: data-driven oda sistemi (RoomTemplateSO/RoomBankSO mevcut, oda tipleri Combat/Elite/Reward/Boss), 3-kart Hades reward draft, run-basina rastgele oda. Mevcut mekanik bankasi = F:\LaurethStudio\03_IDEAS\MECHANIC_BANK ; onceki sentez = STAGING/MECHANIC_ADDITIONS_SYNTHESIS_2026-06-03.md (varsa goz at, TEKRARLAMA).

# SORULAR (rapor halinde)
1. Her dosyadan RIMA'ya UYGULANABILIR mekanikleri cikar. Her biri icin: 1-cumle ozet + RIMA'da nereye oturur (combat/oda/reward/movement/meta) + KOD-feasibility (TRIVIAL/EASY/MED/HARD + hangi mevcut dosya/sisteme dokunur).
2. RIMA'da ZATEN VAR olanlari ele (cifte yapma): mevcut kodda benzeri var mi? (Explore/grep). Varsa "zaten var, su dosyada".
3. Mina the Hollower'a OZGU (Zelda-like) hangi fikirler RIMA'nin iso-roguelite yapisina UYMAZ — ele ve nedenini yaz.
4. En yuksek deger/dusuk efor 5 mekanik (quick wins) + 2 buyuk-fikir (yatirim degeri) ayir.
5. Imza "Sundered Beat" (BREAK->EXECUTE) ile sinerji yaratan mekanikler hangileri?

# Cikti
Markdown rapor -> CODEX_DONE.md. Madde madde, feasibility etiketli. KOD YAZMA. Onceki audit'leri TEKRAR URETME.
