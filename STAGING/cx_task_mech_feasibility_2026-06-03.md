ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — ANALYSIS ONLY, write nothing except the report (4) BLOCKED if unclear.
NLM ACCESS: if needed: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>". Direct-read: code / CURRENT_STATUS.md / .claude/PROJECT_RULES.md / STAGING / memory.

# Amaç (Purpose)
Codebase-feasibility analizi: aşağıdaki 8 aday oyun-mekaniğinin RIMA Unity kod tabanına eklenmesi NE KADAR zor? Mevcut hangi sistemler var, neye dokunmak gerekir, hangi dosyalar. SADECE STATİK ANALİZ — kod YAZMA, Unity MCP KULLANMA, play mode'a GİRME, recompile TETİKLEME (başka bir agent şu an Unity'yi kullanıyor). Sadece `Assets/Scripts/**` içinde Grep/Read yap.

# Aday mekanikler (kaynak: 60-mechanics bankası; F:\LaurethStudio\03_IDEAS\MECHANIC_BANK\Youtube_60_Mechanics.md)
1. **#14 Dynamic Wave Spawning** — sonraki dalga önceki %50-70 ölünce tetiklenir (downtime yok).
2. **#2 Action-Based Time/Reward** — EXECUTE/BREAK/parry gibi aksiyonlar bonus/timer/extra-kart verir.
3. **#7 Parry-Reward Projectile** — renk-kodlu projektili parry → düşmana masif hasar.
4. **#33 Dash = Parry** — düşman saldırısına doğru dash = parry + BREAK artışı.
5. **#17 Heal-on-LevelUp/Pickup** — XP/void-shard toplamak can yeniler (aggression ödülü).
6. **#26 Card Weight / Draw Rate** — skill draft kartları "ağırlık"lı; güçlü kartlar daha nadir gelir.
7. **#11 Extraction Roguelite** — ölünce bazı slot/item kalıcı kayıp + "şimdi çık" portal kararı.
8. **#32 Mid-Fight Hacking** — BREAK'teki düşmana skill-check mini-game → EXECUTE hasarı ×2.

# Her mekanik için ver
- **Mevcut sistem kancası:** hangi sınıf/dosya/event bu mekaniğe en yakın (örn. EnemySpawner, RoomClearVictoryTrigger, DraftManager/SkillOfferGenerator, PlayerAttack, Health, Dash, KnockbackReceiver, ChainWindowTracker/Sundered Beat, MapFlowManager). Gerçek dosya yolları + sınıf adları ver (grep ile bul).
- **Dokunulacak dosyalar + kaba değişiklik** (yeni component mi, mevcut metoda hook mu).
- **Feasibility:** TRIVIAL / EASY / MEDIUM / HARD + kısa gerekçe (mevcut altyapı var mı, yok mu).
- **Risk/çakışma:** mevcut bir lock'u/sistemi bozar mı (örn. timeScale, depth-sort, run economy).

Sonunda: feasibility'ye göre sıralı kısa bir "önce-bunları-yap" listesi.

# Output
Markdown rapor (transcript'e). Hiçbir dosya değiştirme, COMMIT yok, Unity'ye dokunma. profile laurethayday (yekta fallback).
