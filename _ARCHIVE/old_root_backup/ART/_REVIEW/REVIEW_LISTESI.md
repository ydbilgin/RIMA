# RIMA — Review Listesi
> South yönü referans görselleri. Beğenilen → Pro mode ile final üretim.
> İndirmek için: `python ART/_REVIEW/download_reviews.py`

---

## Player Classes (96px Pro)

| Karakter | ID | Durum |
|----------|----|-------|
| Warblade | — | bekliyor |
| Elementalist | — | bekliyor |
| Shadowblade | — | bekliyor |
| Ranger | — | bekliyor |
| Ravager | — | bekliyor |
| Hexer | — | bekliyor |
| Summoner | — | bekliyor |
| Templar | — | bekliyor |
| Hemomancer | — | bekliyor |

## Mobs (boyut moba göre)

| Mob | Boyut | ID | Durum |
|-----|-------|----|-------|
| Shard Walker | 64px | — | bekliyor |
| Void Thrall | 96px | — | bekliyor |
| Seam Crawler | 48px | — | bekliyor |
| Echo Hound | 80px | — | bekliyor |

## Bosses (boyut bossa göre)

| Boss | Boyut | ID | Durum |
|------|-------|----|-------|
| Iron Warden | 128px | — | bekliyor |
| Fractured King | 160px | — | bekliyor |
| Hollow Sovereign | 144px | — | bekliyor |
| Nexus Core | 160px | — | bekliyor |

---

## Onay Süreci

1. Referans üretildi → south.png indir → gözle incele
2. Beğenildi → Pro mode ile 8 yön final + animasyonlar
3. Beğenilmedi → prompt düzelt, yeniden üret

## Boyut Mantığı (Hades Tarzı)

| Tip | Boyut | Notlar |
|-----|-------|--------|
| Player | 96px | Baseline |
| Küçük/çevik mob | 48px | Sürü, hızlı |
| Standart mob | 64px | Çoğu grunt |
| Orta mob | 80px | Belirgin tehdit |
| Ağır mob | 96px | Oyuncuyla boy ölçüşür |
| MiniBoss | 128px | Oyuncudan büyük |
| Boss | 144-160px | Ekranı doldurur |
