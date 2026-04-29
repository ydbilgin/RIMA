# RIMA — TAB Overlay UI Tasarımı
*Oluşturulma: 2026-04-12*

---

## GENEL DAVRANIM

- **TAB** basılınca açılır, tekrar basılınca kapanır
- Oyun DURMAZ (Time.timeScale değişmez)
- Yarı saydam arka plan — arkadaki oyun görünür
- Mouse üzerine gelince tooltip açılır (oyun hâlâ çalışıyor)
- Hareket tuşları (WASD), skill tuşları (Q/E/R/F/Z/X) çalışmaya devam eder

---

## LAYOUT

```
┌─────────────────────────────────────────────────────┐
│  ░░░░░░░░░  KARAKTER ŞEMATİĞİ  ░░░░░░░░░           │
│                                                       │
│  ┌──────────────┐  ┌──────────────────────────────┐  │
│  │  STATS       │  │  AKTİF SKİLLER               │  │
│  │              │  │  [Q] Iron Charge  CD: 3.0s   │  │
│  │  HP: 180/180 │  │  [E] Cleave       CD: 5.0s   │  │
│  │  Rage: 45/100│  │  [R] War Stomp    CD: 8.2s   │  │
│  │  Hasar: +12% │  │  [F] Death Blow   HAZIR       │  │
│  │  CD Azalma:0%│  │  ──────────────────────────  │  │
│  │  CC Direnci:0│  │  [Z] KILITLI                  │  │
│  │              │  │  [X] KILITLI                  │  │
│  │  Sınıf:      │  └──────────────────────────────┘  │
│  │  Warblade    │                                     │
│  │  Secondary:  │  ┌──────────────────────────────┐  │
│  │  Yok         │  │  PASİFLER                    │  │
│  └──────────────┘  │  Blood Drinker    ● ● ○  Lv2 │  │
│                    │  Predator's Eye   ● ○ ○  Lv1 │  │
│  ┌──────────────┐  │  Iron Body        ● ● ● MAX   │  │
│  │  LMB EKOL    │  └──────────────────────────────┘  │
│  │              │                                     │
│  │  Fury Strikes│  ┌──────────────────────────────┐  │
│  │  Lv 2        │  │  TRAİTLER                    │  │
│  │  ────────── │  │  Toughened Hide   ×2  (+40HP) │  │
│  │  RMB EKOL   │  │  Iron Will        ×1  (-20%CC)│  │
│  │  Seçilmedi  │  └──────────────────────────────┘  │
│  └──────────────┘                                     │
└─────────────────────────────────────────────────────┘
```

---

## BÖLÜMLER

### 1. STATS Paneli (sol üst)
Gösterilecekler:
- HP: mevcut / max
- Birincil kaynak (Rage/Mana/Energy/Focus): mevcut / max
- İkincil kaynak (varsa): mevcut / max
- Hasar bonusu: toplam (pasif + trait + ekol kaynaklı)
- CD azalma: toplam %
- CC direnci: toplam %
- Hareket hızı: base + bonus
- Primary class ismi + rengi
- Secondary class ismi + rengi (yoksa "Seçilmedi")

### 2. AKTİF SKİLLER Paneli (sağ üst)
Her skill için:
- Tuş etiketi [Q] / [E] / [R] / [F] / [Z] / [X]
- Skill ismi
- CD durumu: "HAZIR" veya "3.2s" (gerçek zamanlı sayaç)
- Kilitli slotlar için "KILITLI" + kilit ikonu
- **Hover:** Skill açıklaması + hasar formülü + kaynak maliyeti

### 3. PASİFLER Paneli (sağ orta)
Her pasif için:
- İsim
- Seviye göstergesi: ● dolu, ○ boş (max 3)
- "MAX" etiketi 3. seviyede
- **Hover:** Her seviyenin tam açıklaması (Lv1 / Lv2 / Lv3 ne veriyor)

### 4. LMB/RMB EKOL Paneli (sol alt)
- Ekol ismi + seviyesi
- Aktif ekolün mevcut bonusu (kısa özet)
- "Seçilmedi" — ekol seçilmemişse
- **Hover:** Tüm 3 seviyenin açıklaması

### 5. TRAİTLER Paneli (sağ alt)
Her trait için:
- İsim
- Stack sayısı: ×2 gibi
- Toplam etki: parantez içinde
- **Hover:** Trait'in ne yaptığı + max stack nedir

---

## TOOLTIP TASARIMI

Hover başlar → 0.3s sonra tooltip açılır (anında açılma isistemin kirli görünmesine neden olur)

```
┌──────────────────────────────┐
│  IRON CHARGE           Rare  │
│  ──────────────────────────  │
│  Bakış yönüne ani fırla.     │
│  Yoldaki düşmanlara hasar    │
│  ver ve geri it.             │
│                              │
│  Hasar: 35 + %15 Rage bonus  │
│  Rage kazanımı: +15/isabet   │
│  CD: 3.0s                    │
│  Kaynak: —                   │
└──────────────────────────────┘
```

Tooltip konumu: Mouse pozisyonuna göre dinamik.
Ekran kenarına yakınsa karşı tarafa açılır.

---

## GÖRSEL STİL

- **Arka plan:** Siyah, %65 opacity — oyun görünsün
- **Panel arka planı:** Çok koyu gri (#1a1a1a), %85 opacity
- **Primary class rengi:** Warblade = kırmızı-turuncu tonu
- **Secondary class rengi:** Seçilen class rengi (Elem=mavi, Shadow=mor, Ranger=yeşil)
- **Pasif seviye ●:** Dolu = class rengi, Boş = gri
- **HAZIR:** Yeşil | CD'de: Kırmızı/turuncu
- **Font:** TMP, aynı oyunun geri kalanı ile

---

## ANİMASYON

- Açılış: Alpha 0 → 1, 0.15s (hızlı, oyunu engellemesin)
- Kapanış: Alpha 1 → 0, 0.1s
- Slide yok — direkt fade (oyun devam ederken slide distraksiyon yaratır)

---

## PİXELLAB GEREKSİNİMİ

**Faz 1 için:** HAYIR. Renkli daire/kare placeholder yeterli.

**Faz 2+ için (opsiyonel):**
- Her aktif skill için 32×32px ikon → Edit Image PRO ile üretilir
- Her pasif için 32×32px ikon
- Ortalama ~1 gen/ikon, ~30 skill × 1 gen = ~30 gen

**İkon tarzı:** Koyu arka plan üzerine tek renkli sembolik ikon.
Örnek: Iron Charge = kırmızı ok + kılıç silueti. Karmaşık değil.
PixelLab direktifi: "32x32 pixel art icon, dark background, single color symbol, [skill name], roguelite game"

---

## IMPLEMENTASYON NOTLARI

### Gerekli yeni dosyalar
- `CharacterSheetUI.cs` — TAB panel yönetimi
- `TooltipSystem.cs` — hover tooltip (evrensel, tüm UI'da kullanılabilir)

### Mevcut dosyalardan veri çekilecekler
| Veri | Kaynak |
|------|--------|
| HP | `Health.cs` |
| Rage | `RageSystem.cs` |
| Aktif skiller | `Warblade_SkillController.GetAllSlots()` |
| Skill CD | `SkillBase.RemainingCooldown` |
| Pasifler + seviyeleri | `DraftManager.passiveLevels` |
| Traitler | `TraitSystem.cs` (yapılacak) |
| LMB ekol | `LMBUpgradeSystem.cs` (yapılacak) |
| Class bilgisi | `PlayerClassManager.Instance` |
| Hasar bonusu | Tüm multiplier'ların toplamı (toplanacak) |

### TAB tuşu input
`PlayerController.cs`'e eklenir:
```csharp
if (Input.GetKeyDown(KeyCode.Tab))
    CharacterSheetUI.Instance?.Toggle();
```

---

## FAZ PLANI

| Faz | Kapsam |
|-----|--------|
| Faz 1 | Tüm bölümler (placeholder ikonlarla), tooltip sistemi |
| Faz 2 | Gerçek skill ikonları (PixelLab), secondary class verisi |
| Faz 3+ | Relic bölümü eklenir |
