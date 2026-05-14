# Warblade Run — Custom V3 Narrative Prompt (S69)

**Mod:** Web UI Custom V3 (enhance with AI **KAPALI**, sadece bu metin)
**Frames:** 8 (bracket: 6-8f = 2 gen)
**Yön:** Tek yön deneme — south önce, başarılıysa 8 yön
**Cost:** 2 gen × 8 yön = 16 gen total (başarılı olursa)

---

## PROMPT (kopyala-yapıştır)

Chibi warrior sprints forward at a steady jog. Shoulders roll with each stride; the torso leans slightly into the motion while the head stays level and focused ahead. Legs cycle through a firm running gait — knees lift in a tight arc and the body bobs gently up and down with the gallop. The greatsword stays carried tight alongside the body throughout the entire run, blade angled slightly back and swaying with momentum, the weapon clearly visible and silhouetted in every pose. The motion feels weighted but agile — a soldier closing distance with purpose.

---

## NEDEN BU FORMAT

- **Motion-only:** Sadece hareket fiilleri (sprints / roll / leans / cycle / lift / bobs / sway). Karakter kimliği yok (armor renk, cape, hand position yok).
- **Weapon visibility positive-only (Karar #71):** "stays carried tight", "clearly visible and silhouetted in every pose" — "doesn't drop" gibi negatif yok.
- **Hand mention SİLİNDİ (Karar #99 düzeltme):** Sağ/sol/which-hand yok — drift riski sıfır. Sprite zaten greatsword'u sağ elde gösteriyor, model devam ettirir.
- **Frame / pixel / loop talimatı YOK:** PixelLab idle prompt style kuralı (S68).
- **Magnitude words:** slightly, tight, firm, gently — model abartmaz.

## TEST PROTOKOLÜ

1. Önce **south** yönünde dene (1 yön × 2 gen = 2 gen)
2. Sonuç: greatsword her frame'de görünüyor mu? Sprint hissi tatmin edici mi?
3. **PASS** → 7 diğer yön dispatch (8 yön × 2 gen = 16 gen total)
4. **FAIL** → prompt revize: "blade angled slightly back" yerine "blade clearly drawn alongside the hip, never breaking silhouette" gibi varyasyon

## YASAK

- ❌ "enhance with AI" — AI serbest yorumlar, sword düşürür
- ❌ "left/right hand" mention
- ❌ "8 frames", "loop seamlessly", "60px height"
- ❌ "dark warrior", "in armor", "with cape" — kimlik mentions
