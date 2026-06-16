# COUNCIL — Opus design-judgment verdict (PixelLab asset/Egg/UI önceliklendirme) 2026-06-16

Lens: cross-system, golden-path-first, scope-lock HOLD. Demo tezi = "%20 oyun / %60 mimari / %20 graphify", centerpiece = **Edit-to-Play video**. Filtre: bir asset videodaki AKIŞI/ALGIYI güçlendirmiyorsa → DEMO DEĞİL. Asset üretimi REWARD-02 fix'inin ÖNÜNE GEÇMEZ (tek demo-kritik = o fix).

## VERDICT

### DEMO-ÜRET (≤4 gün, öncelikli — yalnızca videoyu güçlendiren)
1. **warblade idle/run/LMB-strike anim** (V3, armed-anchor greatsword) — golden-path karakteri ekranda; stat→damage beat'i SADECE LMB'de görünür → bu anim demo'nun kalbi. (Zaten council-onaylı pipeline; paket-dışı ama #1.) ⚠️ tutarsız-kılıç riski varsa anchor'dan redo.
2. **HUD greybox→art YOK; sadece readability fix** (font/scale, asset üretmeden). Paket "72px mikroskobik" uyarısı DOĞRU ama bu KOD/scaler işi, PixelLab asset değil → U3/U-batch'e bağla, PixelLab harcama.

Net: PixelLab'dan demo için ÜRETİLECEK TEK ŞEY = warblade anim. Geri kalan UI/Egg = "video'yu daha güzel gösterir" ama 4 günde PNG-mezarlığı riski + golden-path'i bozmaz → POST-DEMO.

### POST-DEMO (paketin değerli ama scope-dışı kısmı)
- (A) Modüler UI pack 9-slice (panel/button/skill-slot/reward-card) — polish-first kuralıyla, hedef-ekran onayından SONRA. Demo'da mevcut UI yeterli.
- (C) UI/UX polish spec uygulaması (HUD/Pause/Settings/Codex art-pass).
- (B) Rift-Forged Egg — paketin KENDİSİ "bug'lar çözülmeden yapma, incubation post-demo" diyor; doğru. Egg = mevcut reward'ın diegetic skin'i (yeni ekonomi DEĞİL) → değerli ama demo-akışı zaten 3-kart modal ile çalışıyor.
- Elementalist idle/run/cast (caster prep, council'de zaten post-demo).

### DROP
- Egg "incubation/pet companion" ileri-faz (modüler-disiplin: "hak ediyor mu?" = HAYIR, solo-dev savaşı).
- Tam UI atlas inşası (8 atlas) demo için — over-engineer.
- Paketin 4-cardinal/flip-yok KARAKTER yön modeli (canon ihlali, aşağı).

## CANON-FLAG TABLOSU
| Paket maddesi | Çatışma | Öneri |
|---|---|---|
| Karakter "4-cardinal S/E/N/W, flip YOK" | ✅ ÇATIŞMA (Karar #114: 8-yön = 5+3 mirror) | REDDET; karakter/anim'de uygulama. UI flat/tek-yön prop'a uygulanmaz. |
| "35° top-down ARPG" | ⚠️ KISMİ | RIMA = HIGH top-down ~70-80°/horizon. "35°"=horizon'dan değil zemin-eğimi terminolojisi olabilir; Egg/prop prompt'undaki "35-degree" SİL → "high top-down 3/4 (~70° from horizon)" yaz. |
| Stil (slate/iron/cyan≤%15/amber/no-purple-overload) | ✅ YOK | Canon-uyumlu, aynen kullan. |
| UI scale 1080p/1440p + safe-area | ✅ YOK | Doğru; readability fix demo'ya alınabilir (kod). |
| Atlas stratejisi (Chrome≠FX ayrı) | ✅ YOK (teknik doğru) | Post-demo; demo'da gerek yok. |
| AIM-02 "tek AimService, mouse-aim" | ⚠️ TASARIM | RIMA koreografi LMB-merkezli; "skill aim=cursor" mevcut facing/snapshot modelini değiştirir → POST-DEMO mekanik, asset değil. Brief kapsamı-dışı not. |

## EKSTRA DEĞERLİ (gözden kaçan, benimse)
1. **"Polish-first / hedef-ekran onayından sonra parça çıkar" altın kuralı** — paketin en değerli mimari disiplini; RIMA modüler-felsefesiyle birebir. PixelLab'a dokunmadan ÖNCE bunu kural-yap (PNG-mezarlığı sigortası).
2. **Reward lifecycle atomikliği: "kapı reward tamamlanmadan açılmaz VEYA açık Reddet/Atla butonu"** — bu UX kuralı REWARD-03'ün doğru tasarım-cevabı; demo golden-path'i sertleştirir (asset değil, ucuz kod/UX kararı).
3. **"Combo box sabit min-genişlik + 'KOMBO AÇAR' trigger/outcome"** — U1 tooltip dikey-şerit bug'ının kalıcı çözüm yönü (canonical skill-data'dan doldur).

## ASSET SPEC DOĞRULAMASI (demo'da üretilecek tek asset)
- **warblade anim:** mode=V3 (`animate_character`, 1 gen/yön, frame 4-16); PRO KULLANMA. 5 yön (S,SE,E,NE,N)+3 mirror. Canvas 120×120, PPU 64. Transparent zorunlu + alfa-doğrula. Prompt RIMA-stil (no dark-fantasy). Armed-anchor greatsword'dan (tutarlılık).
- (Post-demo asset spec'leri paket-içinde doğru; ANCAK Egg prompt'undaki "35-degree top-down" → "high top-down 3/4" düzelt, "96x96 / transparent PNG / no purple overload" KORUNUR, shell≠glow ayrılığı doğru. UI source-boyutları makul başlangıç; final border Sprite Editor'da.)

## RİSKLER
- **R1 (yüksek):** PixelLab'ı şimdi açmak REWARD-02 fix'ini geciktirir → demo-kritik kaymalar. warblade anim DIŞINDA hiçbir şey 4 günde başlatma.
- **R2:** 4-cardinal modelini yanlışlıkla anim'e taşımak = 8-yön canon kırılır (paket bunu açıkça öneriyor → tuzak).
- **R3:** "35°" terminolojisini prompt'a literal koymak → yanlış kamera açılı sprite (mevcut sahneyle uyumsuz).
- **R4:** Egg'i "yeni sistem" sanıp WorldRewardChoiceSet'i demo'ya sokmak = scope patlaması; reward zaten 3-kart modal ile çalışıyor.
