# ADVERSARIAL CRITIC — ChatGPT playtest PixelLab/Egg/UI kararı (blind, 2026-06-16)

Rol: KARAR'i çürütmeye çalıştım (taraf değilim). Sentez gerekçesini görmeden taze baktım; iddiaları KODA karşı doğruladım.

## VERDICT: **NEEDS-FIX** (çekirdek yön SAĞLAM, ama 1 yanlış-teşhis label + 2 eksik guardrail)

Karar'ın stratejik omurgası doğru ve kanıtlı: (a) "warblade anim = demo'nun TEK PixelLab işi" → council 4/4 + golden-path filtresi tutarlı; (b) "reward-beat = KOD, asset değil" → **KOD'la doğrulandı** (aşağı); (c) 4-cardinal DROP / V3-only / PRO-DROP / Egg post-demo → canon-uyumlu, doğru etiketli. Çürütemedim. ANCAK aşağıdaki maddeler bağlayıcı düzeltme istiyor.

## DOĞRULANAN iddialar (çürütme BAŞARISIZ = karar haklı)
- **"U1 yeni art DEĞİL, KOD fix"** → DOĞRU ve güçlü kanıtlı. Reward kartı `SkillOfferUI.BuildSkillCard` ile **tamamen kodda** inşa ediliyor (her `sizeDelta` programatik; Instantiate/prefab YOK; footer/sinerji-chip de kodda — `chipRt.sizeDelta = (CardWidth-20f, 22f)` satır ~363). Yeni sprite collapse'i çözmez. Karar'ın "asset bunu çözmez" sonucu sağlam.
- **"REWARD-02 fix = RewardPickup.cs, asset-bağımsız"** → DOĞRU. G yolu `Update`+`playerInRange` (satır 62-76), salt input/trigger; `OnTriggerStay2D`+`Awake OverlapCircle` fix hiçbir sprite'a dokunmaz. Asset-çakışması guardrail'i (prefab collider/G-input'a dokunma) yerinde.
- **8-yön LOCKED / V3-ucuz-PRO-pahalı / Egg post-demo** → kanon + PIXELLAB_REFERENCE ile uyumlu (create_character pro ~20-40 gen = pahalı; V3 1 gen/yön). Doğru.

## BINDING FIX'ler (uygulanacak)
1. **[YANLIŞ-TEŞHİS / HIGH] "combo-box" = uydurma + 3 ayrı bug'ın birleştirilmesi.** "combo-box" terimi ne pakette (`04_MASTER_BUG_REGISTER`) ne kodda geçer; COUNCIL_opus serbest-isimlendirmesinden ("Combo box...") aynen taşınmış. Gerçekte 3 AYRI iş birleştirilmiş: **UI-01** = reward **kartı** footer genişlik çöküşü (SS-04, P1, layout); **U1 backlog** = **TooltipSystem** dikey-şerit (`TooltipSystem`, ayrı dosya/ayrı widget); **DATA-01** = "eşleşir/KOMBO AÇAR" **kopya/string** işi (sinerji-chip metni, layout değil). Karar'da "U1 combo-box/footer min-genişlik (TMP/ContentSizeFitter)" → ÜÇE AYIR; implementer'ı yanlış yönlendirir.
2. **[KANITSIZ DETAY / MED] "TMP/ContentSizeFitter çatışması" RIMA koduna uymuyor.** Bu, paketin SPEKÜLATİF nedeni; gerçek kartta **ContentSizeFitter YOK**, sabit `sizeDelta` kullanılıyor (SkillOfferUI). Fix yönünü "ContentSizeFitter çakışması" diye yazma → gerçek olası-neden = sabit-genişlik footer/desc RectTransform'unun (`drt.sizeDelta`/chip) TR/EN uzun metinde dar kalması. "Önce repro, sonra root-cause" notu ekle (NO speculative-fix).
3. **[EKSİK GUARDRAIL / MED] Reward beat'i için iki paralel reward sistemi (RoomRunDirector merkez-spawn / RuntimeRoomManager öne-spawn) demo-kritik; karar bunu sadece "post-demo konsolidasyon" diye geçiyor.** REWARD-02 fix'i `RewardPickup` robustness'la iki yolu da kapsıyor (doğru) AMA U1 footer fix'i SkillOfferUI'da → **iki fix farklı dosya, ikisi de demo-kritik beat'te**; karar U1'i "opsiyonel/zaman kalırsa" tonuna kaydırma riski taşıyor. U1'i REWARD-02 ile eşit demo-kritik say (golden-path videosunda kart footer GÖRÜNÜR).
4. **[OVER-REACH değil ama ZAYIF / LOW] "warblade-only" çağrısı doğru-katı.** ax_pro/cx'in "reward-card/chrome demo'ya alınabilir" itirazı, karar tarafından KOD'a yönlendirilerek doğru çürütüldü (yeni art yerine layout fix). Fazla katı DEĞİL. TEK ekleme: "opsiyonel ucuz dokunuş" (RewardPickup 96px shell sprite) maddesi REWARD-02 fix'iyle ÇAKIŞABİLİR (sprite atama Awake fallback'ini etkiler, satır 36-51) → bu opsiyoneli "REWARD-02 audit-PASS sonrası, ayrı commit, collider/sprite-null-path'e dokunma" diye guardrail'le.
5. **[İYİLEŞTİRME / LOW] "35° → 70-80°" düzeltmesi doğru ama nüansı koru.** COUNCIL_opus "35° horizon'dan değil zemin-eğimi terminolojisi olabilir" diyor; karar bunu literal-yanlış varsayıp düzeltiyor — sonuç (prompt'a "high top-down 3/4 ~70-80°" yaz) aynı, sorun yok, ama "literal 35° koyma" riski zaten R3'te var → tutarlı.

## Sonuç
Karar yayınlanabilir; yukarıdaki 5 fix (özellikle #1 üçe-ayırma + #2 ContentSizeFitter iddiasını kaldır + #3 U1=eş demo-kritik) işlenmeli. Scope-drift YOK (Egg/UI-pack doğru post-demo); DROP'lar doğru. Canon-flag tablosu doğru.
