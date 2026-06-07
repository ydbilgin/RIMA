ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
T6 (GENİŞLETİLMİŞ) — Death/Victory mikro-pass + boss momenti + "sunum kırıkları" temizliği. Kaynak kararlar: `STAGING/MASTER_PLAN_FINAL_2026-06-06.md` (T6) + `STAGING/R4_DECISION_2026-06-07.md` (boss-A spec) + `STAGING/UXFLOW_DECISION_2026-06-07.md` (genişletme). Boss kararı KİLİTLİ: Seçenek-A = mevcut boss görseli 1.5-2× runtime scale + rim + telegraph; yeni boss sprite üretimi YOK.

# İşler

## 1. YEŞİL KUTU GLOBAL TEMİZLİĞİ (sunum kırığı #1)
SeamCrawler'ın sprite'ı yok → yeşil kutu render ediyor (boss odası karesinde görüldü). Spawn havuzlarından/banklardan SeamCrawler'ı ÇIKAR (mob sanatı PixelLab seansında üretilecek; o zamana kadar spawn edilmemeli). Başka sprite'sız mob varsa aynısı. FractureImp (sprite'lı) gameplay için KALIR.

## 2. VICTORY/DEATH PANELLERİ (sunum kırığı #2)
- Victory'deki dev SARI panel → RIMA dili: koyu 9-slice panel (`Resources/UI/RIMA/Pack` mevcut card_frame_9slice/panel asset'lerini REUSE et) + cyan/gold accent. "DEMO COMPLETE" başlığı + Echo DÖKÜM paneli: oda sayısı × 3 + kill/5 + first-time bonus satırları ayrı ayrı + toplam (`EchoWallet.ComputeRunAward` mantığını görünür kıl).
- Death ekranı aynı görsel dile çekilir + aynı Echo dökümü.
- "Wishlist on Steam" CTA kalır (EN), daha küçük/premium.

## 3. BOSS MOMENTİ (R4 boss-A spec)
- **Boss intro 1.5s:** boss odasına girişte arena hafif kararır + boss adı ("PENITENT SOVEREIGN") büyük tipografiyle belirir + ritual circle pulse + HP bar slide-in. Skip edilebilir olmasın (1.5s kısa).
- **Boss HP bar:** düz sarı blok → çerçeveli 9-slice bar (mevcut UI pack'ten; yoksa koyu çerçeve + iç dolgu kodla).
- **Boss görseli:** mevcut boss 1.5-2× scale + rim-light efekti (SpriteRenderer üstüne basit rim: ikinci renderer outline tekniği veya mevcut HitFlash malzemesi varyantı — basit tut).
- **Ritual circle + seal fragments wiring:** `Assets/Art/Boss/` (ritual circle = zemin decal boss spawn noktasında; intro seal ring = intro sırasında pulse). 
- **Telegraph wiring:** `Assets/Art/Telegraphs/` (line/circle/cone) → boss'un en az 2 saldırısına telegraph (R4: "2 telegraph"): saldırı öncesi decal belirir (0.6-0.9s) → saldırı iner. Mevcut boss saldırı kodunu OKU, minimal entegre et.
- **Boss-kill payoff:** boss ölümünde son-vuruş slow-mo (mevcut slow-mo 0.3 mekanizmasını REUSE — owner-guard'lı) + seal fragments dağılma + Victory'ye geçiş.

## 4. Doğrulama
- Play probe: boss odasına gir (graph'ta ilerle veya boss template kur+boss spawn) → intro→telegraph→kill→victory zinciri çalışır; screenshot'lar: `STAGING/_process/2026-06/t6_boss_intro.png`, `t6_boss_telegraph.png`, `t6_victory.png`, `t6_death.png`.
- Yeşil kutu hiçbir run odasında spawn olmaz (3-4 oda probe).
- Testler: mevcut EditMode grupları (Encounter/RoomRun/Skill) yeşil kalır; spawn-havuzu değişikliğine test ekle (SeamCrawlerNeverSpawns benzeri).
- Compile 0 error. COMMIT YAPMA.

# KISITLAR
Legacy dosyalara (RoomLoader/Gate/GateBehavior/RuntimeRoomManager) DOKUNMA. UI metinlerinde dil DEĞİŞTİRME (T-LANG ayrı görev — mevcut metinleri koru, sadece panel görselleri). Türkçe metin eklersen TAM Türkçe karakter.

# Çıktı
CODEX_DONE'a: iş 1-4 DONE/BLOCKED + değişen dosya:satır + 4 screenshot + test sonuçları + boss intro süre/his notu.
