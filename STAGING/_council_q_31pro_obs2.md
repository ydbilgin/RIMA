# RIMA — Canon obstacle/door/decor spec DOĞRULAMA + imagegen PROMPT taslakları (art-direction lens)

Sen kıdemli 2D izometrik sanat-yönetmenisin. Bir CANON-grounded üretim spec'ini doğrula + her asset için imagegen prompt taslağı yaz.

OKU (PATH ile, inline yapıştırma yok):
- Spec: `STAGING/OBSTACLES_DOORS_DECISION_2026-06-03.md` (PART 2 — NLM CANON bölümü AUTHORITATIVE).
- GERÇEK stil referansları (concept DEĞİL — bunlara birebir uy): `Assets/Sprites/Environment/PixelLabFloor451/floor451_0.png` (zemin), `Assets/Sprites/Environment/CliffKit_RefB_pixelified/cliff_S.png` (cliff taş stili), `Assets/Resources/Characters/Warblade/warblade_idle_SE.png` (karakter ölçek/palet/stil).

Palet kilidi: slate #3A3D42 base / cyan #00FFCC emissive ≤%15 / warm-orange #E89020 (sadece mangal) / void-mor #3A1A4A unlit bg. NÖTR ÇİZİM (Light2D var → sprite'a gölge/glow bake ETME).

## Cevapla
1) **Doğrula:** PART 2 spec'i (kapı=K/B/D rift-threshold, obstacle tablosu, dekor) sanat açısından coherent + complete mi? Eksik/yanlış olan? Boyutlar warblade ölçeğine (idle_SE) göre doğru mu (özellikle kapı 1.5-2× karakter)?
2) **imagegen PROMPT taslağı** — her MUST + dekor asset için (Batch 1+2: 3 kapı eşiği + rün-ikon, pillar, wall-stub, cage, tombstone, mangal, zemin-çatlağı A+B, kenar-erozyon, kemik/death-marker, urn, rubble). Her prompt: 1-2 cümle, ON-BRAND, gerçek-ref stiline kilitli, "true isometric, bottom-center, transparent bg, neutral lighting no baked shadow, limited palette slate+cyan(emissive)" gibi ortak kuyruk. KISA tut.
3) **Kapı modüler mi tek-parça mı:** rift+rün'ü ayrı katman mı üretelim yoksa kapı sprite'ına gömük mü (demo pragmatizmi vs canon modülerliği)? Net öneri.
4) Eksik kalan 1-2 canon öğe (örn. ritüel dairesi, seal-socket, map-fragment) üretim-değer sırası.

Madde-madde, üretime-hazır.
