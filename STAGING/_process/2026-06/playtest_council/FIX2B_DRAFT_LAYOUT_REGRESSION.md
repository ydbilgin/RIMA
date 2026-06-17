# FIX-2B: SkillOfferUI draft kart layout REGRESSION (FIX-2 yan-etkisi)

ACTIVE RULES: (1) think (2) min code (3) surgical — sadece SkillOfferUI canvas/scaler + gerekirse UIManager modal-sort, başka DOKUNMA (4) BLOCKED yaz.
UNITY ERROR CHECK: bitince read_console (Error); kendi hatanı çöz, raporda console durumu.

## REGRESSION (senin FIX-2 SkillOfferUI değişikliğin yaptı)
Açılış/ödül draft'ında 3 kart ESKİDEN temiz ORTALANMIŞ SIRA idi; FIX-2 sonrası **dağınık** (GRAVITY CLEAVE üst-sol, IRON CHARGE alt-orta, EARTHSPLITTER alt-sağ, alt kenardan taşıyor). Golden-path KRİTİK.

## KESİN KÖK NEDEN (runtime data-proof, ben pinledim)
`SkillOfferUI.cs` `BuildPanel`'da artık KOŞULSUZ `canvasGo.AddComponent<Canvas>() + AddComponent<CanvasScaler>()` yapıyorsun. Bu yeni CanvasScaler **default = ConstantPixelSize ref=(800,600)**. Runtime'da `[SkillOfferPanel]` artık kendi ROOT canvas'ı (parent=SkillOfferUI_Auto, canvas'sız) → kartlar 800x600 ConstantPixelSize'da yanlış konumlanıyor. Eskiden (orijinal kod) parent canvas VARSA kendi canvas'ını EKLEMİYORDU → parent'ın scaling'ini miras alıyordu → kartlar doğru.
Runtime ek kanıt: `[SkillOfferPanel]` canvas `overrideSorting=False order=1200` (sen `true`/1050 set etmiştin → UIManager modal-stack mutasyonuyla çakışıyor).

## HEDEF
Kartlar TEKRAR temiz ortalanmış sıraya dönsün (FIX-2 öncesi gibi) **VE** draft scrim'in ÜSTÜNde render etsin (FIX-2 scrim kazanımı korunsun). Diğer 3 ekranda (Settings/Pause/Director scrim) bleed-fix BOZULMASIN.

## ÖNERİLEN ÇÖZÜM (en temizini SEN seç + uygula)
- **A (tercih):** SkillOfferPanel'e kendi root-CanvasScaler'ı EKLEME — parent canvas'ı miras alsın (nested Canvas: sadece `Canvas`+`overrideSorting=true`+yüksek `sortingOrder`, CanvasScaler YOK → nested canvas zaten parent scaling'i miras alır). Böylece kart layout parent'tan gelir (doğru), sorting override ile scrim üstünde kalır.
- **B:** Eğer panel gerçekten root olmak zorundaysa, CanvasScaler'ı `ScaleWithScreenSize` + diğer modallerle (UI_Scrim ref 1920x1080 match 0.5) UYUMLU yap — ama kartlar hangi ref'e göre authored ise ona göre; kart RectTransform anchor/pivot'larını da kontrol et.
- UIManager modal-stack'in draft canvas'ını override-etme/order çakışmasını çöz (overrideSorting=False'a düşürmesin; draft order > scrim order kalsın).

## VERIFY
- Compile 0-error.
- Runtime: play (`pss=_Arena`, sonunda MainMenu restore), açılış draft'ında `[SkillOfferPanel]` altındaki 3 kart RectTransform'unu logla → x konumları ARTAN, y'ler EŞİT (sıra) + ekran içinde. execute_code metod-gövdesi (üst-using YOK, tam-nitelik).
- DÖNÜŞ ≤10 satır: hangi çözüm (A/B), değişen satırlar, kart-pozisyon assert (sıra mı), scrim hâlâ çalışıyor mu, console durumu.
