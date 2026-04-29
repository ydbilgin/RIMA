# Perspective Templates Research — RIMA 64×64 High Top-Down

**Tarih:** 2026-04-27 · **Kaynak:** Gemini 3.1 Pro Preview research · **Durum:** ⚠️ URL'ler doğrulanmadı, indirme öncesi verify

---

## 1. Master template kaynakları (Concept Image slot)

| Kaynak | URL | Kullanım | Verify |
|---|---|---|---|
| MortMort "Very Simple Base" | mortmort.itch.io | 64×64 grid + karakter oran şablonu, AI'ya scale ref | ❓ |
| Bluematt Autotile Templates | bluematt.itch.io/autotile-templates | 47-tile blob şablonu, zindan duvarları | ❓ |
| FlakDeau 64×64 Pixel Tileset | flakdeau.itch.io/64x64-pixel-tileset | High Top-Down hazır taslak | ❓ |

**ACTION:** İndirmeden önce her URL'in 200 dönüp gerçekten 64×64 + High Top-Down içerik olduğunu manuel doğrula. Gemini bu URL'leri grounding'siz halüsine etmiş olabilir.

---

## 2. Geometri kilitleme — 3 kural

- **Shadow Locking:** Duvar+zemin birleşimine 1 px siyah Contact Shadow çizgili şablon → Concept Image
- **Cubic Reference:** Nesne (fıçı, kutu) için gri tonlu 64×64 küp ref + prompt: "Use the gray cube as geometry reference for the [OBJECT_NAME]"
- **Wall Height:** RIMA standardı 2 tile (toplam 128 px / ön yüzey 64 px) şablonda explicit

---

## 3. Prompt anahtar terimleri (PixelLab AI tetikleyici)

| Terim | Etki |
|---|---|
| `Visible top surfaces` | Üst yüzeyi çizmeye zorlar |
| `3x3 bitmasking logic` | Tileset kenarlarını birbirine geçirir |
| `Orthographic bias` | Kaçış noktasını yok eder |
| `60-degree overhead angle` | RIMA High Top-Down açısı (kilit #54) |

---

## 4. RIMA üretim akışı (recipe)

1. `_STAGING/templates/64x64_blob.png` → Concept Image
2. Warblade → Style Image (palet için)
3. Prompt: `Visible top surfaces, dungeon stone texture, high top-down`
4. AI sadece taşları çizer, geometriyi şablondan kopyalar

---

## 5. Verify checklist (üretim öncesi)

- [ ] mortmort.itch.io URL gerçek mi, içerik 64×64 mi
- [ ] bluematt autotile templates 47-blob var mı
- [ ] flakdeau 64×64 tileset gerçek mi, açı uyuyor mu
- [ ] "3x3 bitmasking logic" prompt etkisini test et (Gemini speculation olabilir)
- [ ] "Orthographic bias" prompt etkisini test et
- [ ] Cubic reference workflow Warblade pilot'ta test et

**Eğer URL'ler ölü çıkarsa:** opengameart.org + craftpix.net + itch.io free assets manuel arama.
