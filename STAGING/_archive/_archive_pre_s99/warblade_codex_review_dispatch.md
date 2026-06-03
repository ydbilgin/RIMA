Review the following PixelLab prompt for producing a Warblade character sprite.

## Task
Check the prompt against the NLM design spec below.
Output: PASS or FAIL per item, then a final corrected prompt if any FAIL.

## PixelLab Prompt to Review

```
64x64 pixel art chibi character sprite, male.

FACING: True south. Direct front view, symmetric.
Left and right shoulders equal width. NO south-east, NO three-quarter.
High top-down 35 degree camera angle, face fully visible.

Warblade heavy warrior:
Two-handed greatsword, low guard stance, sword tip near ground,
blade horizontal at waist. Sword always in hand, never on back.
Dark leather armor, battle-worn, no ornate decoration.
Brass buckle accents.
Color: #4F3A2C body, #C09455 accents. Muted desaturated palette.
Thick black outline. Hard pixel edges. No anti-aliasing.
85-90 percent canvas fill. Transparent background.
```

## NLM Design Spec (CLASS_SILHOUETTE_BIBLE — Warblade)

- Silüet: Omuz + iki-el kılıç dominant. Gövde geniş, postür ileriye eğik.
- South yönünde: kılıç gövdenin ön-üst yarısını kapatır, baş öne eğik.
- Silah: İki-el greatsword, sprite'ın ~%45'i. Yatay tutuş (horizontal carry), low guard. Sırtta veya kında versiyon YOK.
- Duruş arketipi: Low guard — sword tip yere yakın, omuz ileri, baş hafif öne eğik. Ağırlık merkezi alçak.
- Renk: #4F3A2C (koyu deri zırh) + #C09455 (pirinç detay/tokalar) + #D43F3F danger (sadece VFX, sprite'a yansıtma).
- Zırh estetiği: İşçi estetiği, savaş kıyafeti. Süslü/parlak şövalye zırhı değil.
- Anti-pattern: ince zırh, pastel renk, parlak dekorasyon, kılıç sırtta, hızlı animasyon hissi.
- PixelLab hook (NLM Karar #80): "Into Samomor palette reference"

## Review Criteria
1. Facing/açı talimatı doğru mu?
2. Silah tasarımı spec'e uyuyor mu?
3. Renk paleti doğru mu?
4. Zırh estetiği doğru mu?
5. South yönü silüet davranışı prompt'a yansıtılmış mı? (kılıç ön-üst yarıyı kapatır)
6. "Into Samomor palette reference" eklenmeli mi?
7. Eksik veya fazla olan kritik cümleler var mı?

Output format:
- Her kriter: [PASS] veya [FAIL — düzeltme]
- En sona: düzeltilmiş final prompt (sadece değişen kısımlar)
