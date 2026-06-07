ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
CharacterSelect v4 "ATTUNEMENT CHAMBER" — yürünebilir diegetic seçim odası (kullanıcı-onaylı konsept;
karar = `STAGING/TBH_WALK_SELECT_DECISION_2026-06-05.md` ✅ bölümü). v3.2 tıkla-seç UI'ı TAB-fallback
olarak YAŞAMAYA DEVAM EDER — silme.

# Dosyalar
`Assets/Scripts/UI/CharacterSelectScreen.cs` (ana iş; gerekirse 1-2 küçük yeni dosya `Assets/Scripts/UI/`
altında: örn. ChamberMover.cs, AttunementDummy.cs). KOD-ONLY (.unity YASAK). "◈/elmas glif" YASAK.
Backdrop: `STAGING/imagegen/attunement_chamber_bg_empty.png` → `Assets/Resources/UI/RIMA/CharacterSelect/attunement_chamber_bg.png`
olarak KOPYALA + import (Point, PPU100 default UI, sıkıştırma yok) — Unity import MCP refresh ile.

# MİMARİ KARAR (Opus — uygula, tartışma)
**Canvas-space mover.** Sahne Canvas UI kalır. Oyuncu = UI Image (RectTransform); WASD/ok tuşları ile
anchoredPosition hareketi (hız serialized); 8-yön sprite seti yön'e göre swap (mevcut
`Assets/Art/Characters/<C>/Rotations/*` + varsa walk frame'leri — yoksa idle yön sprite'ı yeter, anim P2).
Fizik YOK — etkileşim = mesafe eşiği (anchor uzaklığı). Yürünebilir alan = serialized elips/poligon clamp
(backdrop'taki iç zemin).

# P1 — YÜRÜME + BÜRÜNME + KAPI
1. **Backdrop swap:** roster-room arka planı yerine attunement_chamber_bg (resilient-load pattern'i koru).
2. **Pedestal anchor'ları:** `[SerializeField] Vector2[] pedestalAnchors` (10, normalized; default'ları
   backdrop görselindeki pedestal merkezlerine GÖZLE yerleştir — kullanıcı Inspector'dan ayarlayacak).
   Her sınıfın echo'su kendi pedestalında: AÇIK = idle frame-0 DONUK + taş-grisi tint (Color ~(0.75,0.78,0.82));
   KİLİTLİ = mevcut opak-siyah silüet. FIT ayak-pivot + uniform boyut AYNEN korunur. İsim etiketi pedestal altı.
3. **Oyuncu:** son-oynanan sınıf (PlayerClassManager.SelectedClass / PlayerPrefs) olarak kapı yakınına
   spawn; canvas-mover ile yürür; depth illüzyonu için y'ye göre hafif scale gerekmiyorsa EKLEME (sabit boyut).
4. **Yaklaşma trigger'ları:** pedestal'a < eşik mesafe → o sınıfın yan panelleri (v3.2 panel build'i REUSE)
   kayarak görünür (dışındayken İKİSİ DE GİZLİ — sahne ferah); üstte `[E] Echo'ya Bürün` prompt'u (TMP).
5. **[E] bürünme (AÇIK sınıf):** yumuşak cyan emilme — FLASH YASAK (alpha-fade + halka parlaması, ≤0.4s);
   oyuncu görseli o sınıfa döner; ESKİ sınıf kendi pedestalına donuk döner; `SelectClass()` çağrılır
   (mevcut class-carry akışı korunur).
6. **Kilitli pedestal:** yaklaşınca tooltip-panel: "{cost} SHATTERED ECHO · veya {orPath}"; bakiye yeterse
   `[E] Kilidi Aç` → EchoWallet.TrySpend → silüet renklenir/donuk-açık olur (mevcut unlock akışı reuse).
7. **Onay = kapı:** kuzey kapı bölgesine yürüyünce (eşik) "Rift'e Gir →" prompt'u + girince mevcut
   OnStartRun/sahne-geçiş akışı tetiklenir. Alt SEÇ butonu kaldırılır (TAB-fallback'te yaşar).
8. **TAB fallback:** TAB → v3.2 klasik tıkla-seç overlay aç/kapa (mevcut ekran build'i yeniden kullanılır;
   orada seçim yapılırsa chamber'daki oyuncu görseli de senkron değişir).

# P2 — DUMMY (vurulabilir, canlı)
9. Merkez-sol boş zemine `AttunementDummy`: görsel = MEVCUT placeholder (öneri: `obstacle_wall_stub` veya
   bones_marker — uygun olanı seç, yeni asset ÜRETME); üstünde mini HP bar (UI). Oyuncu yakındayken
   attack tuşu (LMB/mevcut attack bind) → HitFlash benzeri beyazlama + damage-number popup (mevcut
   pattern'lerden reuse edilebilirse et, edilemezse 15-satırlık lokal versiyon) + HP düşer; 2sn vurulmazsa
   HP yenilenir; ölmez. Amaç: his testi — gerçek combat stack'i BAĞLAMA.

# Doğrulama
dotnet build PASS + read_console 0 + play-observe kanıtları: spawn/yürüme çalışır (pozisyon değişimi ölç),
pedestal yaklaşınca panel görünür + uzaklaşınca gizlenir, E-bürünme sınıf değiştirir + eski sınıf pedestalına
döner, kilitli unlock akışı, kapı-trigger run başlatır, TAB overlay açılır, dummy HP düşer+yenilenir.
CODEX_DONE.md'ye madde madde kanıt.
