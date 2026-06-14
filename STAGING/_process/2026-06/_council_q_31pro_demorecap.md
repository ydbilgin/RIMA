# ax Gemini 3.1 Pro (High) — DEMO NARRATIVE / stage-mapping lens

ANALYSIS ONLY. Read-only. Dosya değiştirme, Unity açma.

Repo: F:\Antigravity Projeler\2d roguelite\RIMA
GÖREV: 2026-06-14 oturumunda yapılan 8 fix'i, hocaya yapılacak CANLI DEMO'nun HANGİ AŞAMASINDA hissedildiğine bağlayan, anlatıcı (narrator) tarzında bir demo-yürüyüşü üret. Kullanıcı bunu sunum/anlama için isteyecek → akıcı, hikâye gibi, AMA teknik olarak doğru.

OKU (demo akışını + vaadi doğru çıkarmak için):
- STAGING/DEMO_SUNUM_PLANI_2026-06-13.md (demo sunum planı/akış)
- CURRENT_STATUS.md (güncel durum + bu oturumun özeti)
- Gerekirse NLM sorgula: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "demo akışı aşamaları ve 9 sistem vaadi"

BU OTURUMDA YAPILAN 8 FIX (özet):
1. P1 — Sandık ödülü: sandıktan çıkan 2. yetenek artık skill bar'a doğru geliyor (placeholder/off-class filtrelendi, draft sessiz-iptal düzeltildi).
2. P2 — Tooltip: LMB/RMB temel saldırı kutularına hover'da ipucu çıkıyor; hasar değerleri behaviorType'a göre doğru (Ranger RMB 0→18 fix).
3. P3 — Açılış yeteneği: araştırıldı, hata yok (Warblade+Elementalist'in Q opening-kit'i çalışıyor) — no-fix doğrulandı.
4. P4 — 4-slot dolu: yetenek değiştirme (replace) modu band-doğru + slot-0 ezme bug'ı + cross-band sızıntısı düzeltildi.
5. P5 — Mavi yay: Warblade vuruşundaki çift arc (mavi+turuncu) → tek EMBER yay; element-agnostik tint (Void mor kalıyor).
6. P6 — Silah: ölü placeholder temizlendi + kılıç sapı artık tam ELDE (pixel-perfect).
7. P7.5 — GÖRÜNMEZ KARAKTER (demo-kritik): Elementalist gövdesi + bazı düşmanlar runtime'da görünmüyordu (bozuk animasyon clip'leri) → persistent sprite-keeper ile görünür.
8. P7.5c — Düşman combat görünürlüğü: combat'ta düşman gövdesi kırmızı kareydi → gerçek görseli geri kondu (40/40 frame doğrulandı).

İSTENEN ÇIKTI (markdown, zengin): 
- Demo akışını AŞAMA AŞAMA bir tablo/akış olarak ver (MainMenu → CharacterSelect → Arena → açılış draft → combat → reward/draft → ilerleme/replace), ve HER aşamaya o aşamada DEVREYE GİREN fix'leri eşle.
- Her fix için: "demoda ne zaman görünür / hoca neyi görecek / düzeltilmeseydi ne olurdu" üçlüsü.
- Genel AMAÇ paragrafı (neden bu işler demo için kritikti — özellikle görünmez-karakter).
- Kısa, anlatıcı, sunulabilir Türkçe. Emoji/tablo serbest.

Bu senin perspektifin; orkestratör (Opus) tüm danışmaları sentezleyip final anlatıyı kullanıcıya verecek.
