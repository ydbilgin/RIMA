# COUNCIL — ax 3.5 Flash design-judgment (Yalın Lens) 2026-06-16

## 1. YALIN VERDICT (Demo ≤4 Gün, Golden-Path Filtresi)
- **MUTLAK MİNİMUM ÜRETİM:** Sadece `warblade` animasyonları (Idle, Run, LMB strike - 5 yön + 3 mirror). Videoda combat hissini veren tek centerpiece budur.
- **POST-DEMO:** (A) Modüler UI pack, (B) Rift-Forged Egg ve (C) UI/UX polish spec.
- **KOD FIX:** HUD font/scale readability sorunu (kod ile çözülecek, asset yok). REWARD-02 kritik bug fix (RewardPickup.cs).

## 2. CRITICAL CRITIQUE (Over-Engineering Değerlendirmesi)
- **~50-Parça UI Asset Pack:** **NET OVER-ENGINEERING.** 4 günde 50 parçayı PixelLab'dan üretmek, temizlemek, 9-slice ve atlas kurmak intihardır. Demo için mevcut UI yeterli.
- **Rift-Forged Egg:** **GEREKSİZ.** Yeni ekonomi getirmeyen, sadece mevcut reward'ın diegetic kaplaması olan bir prop için 4 gün feda edilemez. Mevcut 3-kart modalı demo için yeterlidir.
- **Tek Seçim (Şimdi Yap):** **Warblade Animasyonları.** Karaktersiz veya animasyonsuz bir video golden-path olamaz. UI/Egg olmadan video kurtarılır.

## 3. ACI MASIZ KES-LİSTESİ (DROP/POST-DEMO)
1. **[DROP]** 4-cardinal / flip-yok yön modeli (Canon ihlali, RIMA 8-yön standardı korunsun).
2. **[DROP]** Egg incubation / pet companion sistem tasarımı.
3. **[POST-DEMO]** Tüm UI asset atlas inşası ve 9-slice paneller/butonlar.
4. **[POST-DEMO]** Rift-Forged Egg prop ve animasyon üretimi.
5. **[POST-DEMO]** Bespoke Q/E/R/F skill animasyonları (LMB'yi reuse et + engine juice).

## 4. CANON & RİSK UYARILARI
- **Kamera Açısı:** Paket "35° top-down" diyor; RIMA canon "70-80° high top-down". Promptlarda "35-degree" silinmeli, aksi takdirde yanlış açılı sprite üretilir.
- **Yön Standardı:** Paketin "4 cardinal" önerisi reddedilmeli, RIMA'nın 8-yön (5 üret + 3 mirror) canon'u korunmalı.
- **Bütçe Riski:** PixelLab'ı UI/Egg için açmak, REWARD-02 fix'ini geciktirerek demoyu riske atar.

## 5. EKSTRA DEĞERLİ MADDE (Benimsenecek)
1. **"Polish-first / hedef ekrandan parça çıkarma"** prensibi UI üretiminde kesin kural olmalı.
2. **Reward lifecycle** (kapı açılmadan ödül alınması zorunluluğu) UX kuralı kod seviyesinde uygulanmalı.
