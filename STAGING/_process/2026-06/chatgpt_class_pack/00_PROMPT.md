# ChatGPT'ye — RIMA Class Stat Modeli + Dengeleme Tool (HER ŞEY EKTE)

> Bu pakette ihtiyacın olan her şey var: gerçek kod dosyaları (`code/`) + 3 araştırma özeti (kod analizi, Gemini genre-balance, NLM canon). GitHub'a gitmene gerek yok — ekteki dosyaları oku.

## Görev
RIMA = 2D top-down pixel-art roguelite (Unity). Demo'nun amacı: **dengeleme altyapı sistemini** canlı ayarlanabilir göstermek ("altyapı kuruldu, animasyonlar sonradan gelecek"). 10 class var, 5 implemented.

Ekteki 4 belgeyi + kod dosyalarını oku, sonra ŞUNU üret:

### A) Birleşik (ama canon-uyumlu) Class Stat Modeli
**ÇÖZÜLECEK ÇATIŞMA — senin görüşün lazım:**
- **NLM canon:** 2 ayrı hasar statı (Physical Damage + Ability Power). Class'lar Phys/AP olarak bölünmüş. Damage formülü 3-kategori çarpan (Identity×Build cap ×3.0, taşan→Posture; Situational ayrı ×2.0).
- **Gemini:** "attackPower/magicPower ayrımını terk et, tek `damageMult` + ability tag yeter" (modern roguelite normu).
- **NLM:** HP/moveSpeed sabit class-sayısı DEĞİL — ağırlık/hız animasyon frame + dash-cancel penceresinden. UI = 5-bar (Hasar/Dayanıklılık/Hız/Kontrol/Zorluk).

Demo için (animasyon henüz yok) hangisi? Öner: **Phys/AP split'i koru** (canon+RIMA mage class'ları) ama demo-altyapısı için **sayısal placeholder stat seti** ekle (maxHP, physPower, abilityPower, attackSpeed, moveSpeed) — animasyon frame'i sonra gelince tamamlanır. Bu yaklaşımı doğrula/düzelt, gerekçeyle.

### B) 10 Class Sayısal Tablo
Her class → arketip (bruiser/assassin/mage/ranged/berserker/tank) + tablo:
| Class | Arketip | Hasar tipi(Phys/AP) | maxHP | physPower | abilityPower | attackSpeed× | moveSpeed | 5-bar(Hasar/Dayan/Hız/Kontrol/Zorluk) |
Mevcut combo damage'larla TUTARLI (Warblade 95, Elementalist 64, Shadowblade 20, Ranger 18), base=100 referansıyla oransal, dengeli. Gerekçeli.

### C) Ek Dengeleme/Demo Tool Fikirleri
Bizde planlı: god-mode, kill-all, reset, spawn, stat slider'ları (HP/phys/AP/atkSpd/moveSpd), debug HUD, presenter mode, slow-mo, free-cam, screenshot. BUNLARDAN BAŞKA dengeleme demosuna ne eklenir? (DPS meter, damage-number toggle, hitbox görselleştirme, class A/B karşılaştırma, encounter difficulty slider, stat preset kaydet/yükle, posture/break göstergesi). Her biri: ne + Unity'de nasıl + neden dengelemeye yarar.

Türkçe, net, tablo formatında. Kendi çıkarımını da ekle — gözden kaçırdığımız bir şey var mı?

## Ekli dosyalar
- `01_CODE_FINDINGS.md` — mevcut kod sistemi (flat, per-class scaling yok)
- `02_GEMINI_RESEARCH.md` — genre balance normları
- `03_NLM_CANON.md` — RIMA tasarım canon'u (Phys/AP, formül, arketipler)
- `code/` — gerçek stat-ilgili .cs + .asset dosyaları
