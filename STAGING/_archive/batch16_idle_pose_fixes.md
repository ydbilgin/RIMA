# Batch16 Idle Pose QC — Fix Notları
**2026-05-12 | batch16_final_v3.md çıktı sonrası**

## Genel Durum
- 16/16 sprite çıktı: PASS (sayı doğru)
- Pose tipi: tüm karakter south-facing idle (tek frame)
- Ana sorun: 2 karakter idle pose düzeltmesi gerekiyor

---

## HATALAR / Düzeltme Gerekli

### 1. Ravager — YANLIŞ SİLAH
**Sorun:** Tek büyük balta (single two-handed axe)
**Doğru:** İki elde ayrı birer kısa balta (dual short compact axes)
**Prompt düzeltmesi:**
- Description'a ekle: `"holding two short axes, one gripped in each hand, axes hang loosely at sides"`
- Negative prompt'a ekle: `"single axe, two-handed axe, one weapon, greataxe"`
- **LOCK kaynağı:** S61 — CLASS_SILHOUETTE_BIBLE Ravager dual short compact axes LOCKED

### 2. Elementalist — EKSİK POSE DETAYI
**Sorun:** İki el normal yan duruş (neutral hands-at-sides)
**Doğru:** Casting ready idle pose — bir el göğüs/omuz hizasında kaldırılmış, avuç öne bakıyor (büyü hazır pozisyonu)
**Prompt düzeltmesi:**
- Description'a ekle: `"idle casting pose, right hand raised to chest height with palm facing outward, slight magical energy visible in raised hand, left hand relaxed at side"`
- NOT: Sol el aşağıda, sağ el havada — simetrik duruş istenmiyor, karaktere kişilik katıyor

---

## Diğer Karakterler — Genel İzlenim

Aşağıdakiler görselden okundu, büyük sorun yok ama bilgi için:

| # | Görünen Karakter | Pose Durumu |
|---|---|---|
| R1-1 | Sword warrior (Warblade?) | Sword at side — OK |
| R1-2 | Bow user (Ranger?) | Bow held — OK |
| R1-3 | Ninja/Shadow | Arms crossed or at side — OK |
| R1-4 | Blonde mage (Summoner?) | Robe stance — OK |
| R2-1 | **Ravager** | ❌ TEK BALTA — FIX GEREKLİ |
| R2-2 | Sword female (Hexer/Ronin?) | Sword idle — OK |
| R2-3 | Dual pistols | Guns holstered or held — OK |
| R2-4 | Brawler/Monk | Fists up stance — OK |
| R3-1 | Staff robed figure | Staff in hand — OK |
| R3-2 | Dark hood figure | Robe stance — OK |
| R3-3 | Blue/teal alien-looking | Arms out — possible enemy/boss |
| R3-4 | Heavy armored dark | Big stance — possible enemy/boss |
| R4-1 | Purple winged (Imp?) | Wings spread — possible enemy |
| R4-2 | **Elementalist?** | ❌ EL HAREKETİ EKSİK — FIX GEREKLİ |
| R4-3 | Dark glowing hands | Hands out — similar to Elementalist? |
| R4-4 | Grey golem/giant | Arms at side — possible enemy/boss |

---

## Sonraki Adım
1. Ravager ve Elementalist için prompt düzeltmeli **2 ayrı re-gen** yap (Create Image Pro, tek karakter)
2. Diğer 14: PASS kabul et → Create Character'a geçilebilir
3. Re-gen PASS olursa → batch16 tamamlanmış sayılır → Create Character → Custom Animation v3

---

## Idle Pose Referans (Tüm Sınıflar İçin)
Her sınıfın idle pose tanımı — prompt yazarken referans al:

| Sınıf | Idle Pose Tanımı |
|-------|-----------------|
| Warblade | Sword gripped in both hands, tip angled slightly toward ground, combat-ready stance, slight forward lean |
| Ravager | Two short axes, one in each hand, arms hanging loosely at sides, chest out, aggressive neutral |
| Ronin | One hand on sword hilt (at hip, sheathed), other hand open at side, calm composed stance |
| Hexer | One hand holds grimoire open at waist, other hand raised with two fingers extended (hex gesture) |
| Elementalist | Right hand raised to chest height, palm outward (casting ready), left hand relaxed at side, slight magical glow on raised hand |
| Summoner | Staff/totem planted in ground, both hands resting on top of staff, slightly hunched, mysterious |
| Ranger | Bow held loosely in off-hand at side, arrow hand relaxed, alert upright stance |
| [Other melee] | Weapon at ready position, slight combat stance |
