# RIMA — Mekanik / Sistem Opsiyonları (2026-06-15)

> Her madde kendi içinde tam anlatımlı. **💭 görüş = bağlayıcı değil. ⚙️ teknik dikkat = olgu/risk.** Demo'ya ne girer kararı senin.
> Kaynak: studio bankası (ID'ler) + HoH2 araştırması (işaretli). On-brand kontrolü RIMA canon'una göre yapıldı (skill taksonomisi STRIKE/ZONE/REACTIVE/STATE + KEYSTONE/MODIFIER/RESONANCE · kaynak-imza · tap-to-aim · Attack Token · x3.0 cap→Posture · additive · signature/boss/Echo bespoke).

---

## GRUP A — Combat-feel / Warblade dikey-dilimi güçlendirenler
*(💭 genel sezgim: demonun "savaş iyi hissettiriyor" işini bunlar yapar — ama sıralama/seçim senin.)*

### A1. Flowstep — combo ödülü hasar değil, AKICILIK
- **Ne:** Hasarsız ardışık STRIKE/dash-cancel-kill zinciri kısa bir STATE açar; bu modda LMB-chain'de micro-step, RMB-cleave'de hareket-kaybı azalır, dash-cancel penceresi biraz genişler. *(V0041/M59/V0308/V0113 · HoH2#6)*
- **Hangi boşluk:** combat var ama combo ödülü sadece hasar/puan; bu "akış" hissi katar.
- **On-brand:** taksonomide net **STATE**; ActionCommitProfile'ı kırmaz (commit iptal yok); hold-charge yasağını delmez; kaynak-imza (Rage/tempo) okur.
- **⚙️ Teknik dikkat:** **hareket-HIZI artışı OLMASIN** → sadece micro-step + dash-cancel genişleme (yoksa oyuncu odayı dolaşır, mob'lar vuramaz = Attack Token bozulur). Afterimage eklenirse pool + hard-cap (~8). Tetik sadece *başarılı* zincirle dolsun (spam'le bedava mobilite olmasın).
- **💭 görüş:** demoyu en ucuz şekilde "ayırt-edici" yapan şey bu olabilir.

### A2. Posture-Crack Exploit + "SHATTERED!" glyph
- **Ne:** Düşmanın Posture'ı dolunca 1.2-1.8s cyan çatlak-telegraph açılır; o pencerede STRIKE bonus-stagger, ZONE cleave-echo yapar. Kırılma anında 1-kare beyaz-freeze + ekranda büyük pixel-glyph ("SHATTERED!"/"BREAK!", 3-5 varyant). *(V0025/M211)*
- **Hangi boşluk:** zaten var olan "x3.0 cap → taşan hasar Posture'a" mekaniğini görünür/okunur bir taktik-ana çevirir; HP-sponge yerine sequencing.
- **On-brand:** Attack Token + telegraph adaleti; chibi'de cyan-crack/white-flash okunur; no-dark-fantasy uyumlu. Penitent Sovereign + Fracture Imp elite için ideal.
- **⚙️ Teknik dikkat:** crack VFX + hasar# pool+cap (Canvas-rebuild fırtınası = FPS katili). Düşman-türüne göre dolum-anahtarı farklılaşsın (her hedefte aynıysa spam). Glyph = A2'nin feedback-katmanı, ayrı mekanik değil.
- **💭 görüş:** boss anını "izlenir" yapan en yüksek etki/efor; düşük maliyetli glyph sunum etkisini katlıyor.

### A3. Aggression-Gated Heal — gri-can (anti-turtle)
- **Ne:** İyileşme vial'i hasar VEREREK şarj olur / alınan hasar gri-can kalır, vurunca geri akar (Bloodborne rally). *(M204)*
- **Hangi boşluk:** combat ritmi; defansif köşede-bekleme cezalansın.
- **On-brand:** jenerik stamina yok, kaynak-imza (Rage/Combo) ile uyumlu; Attack Token adaletini bozmaz.
- **⚙️ Teknik dikkat:** heal-oran iyi dengelenmezse "ölümsüzlük" hissi. Heal#/VFX pool, gri-can bar shader (zero-alloc).
- **💭 görüş:** "ölüyordum, geri döndüm" anı jüride güçlü; A1/A2 ile sinerjik (riskli oyna→iyileş).

### A4. Sweet-Spot Damage (pozisyonel)
- **Ne:** Cursor-aim max-mesafede "sweet-spot ring" bonus hasar → doğru dash-mesafesini ödüllendirir. *(M207)*
- **On-brand:** tap-to-aim imzasını pekiştirir; pozisyonlamayı ödüllendirir.
- **⚙️ Teknik dikkat:** bonus **max +%20 raw, Posture katkısı YOK** (yoksa A2 ile üst üste binip kapanmayan agresyon-döngüsü olur). Perf-nötr (tek float karşılaştırma).
- **💭 görüş:** güzel ama A1+A2 zaten spacing ödüllendiriyor; üst-üste-binme riskine karşı "nice-to-have".

### A5. Rolling / Deferred Health bar
- **Ne:** Tek-hata cezasını yumuşatan gecikmeli-can çubuğu; hit-stop'la eşleşir. *(M205)*
- **On-brand:** additive HUD; mevcut can-bar üstüne. Perf-nötr.
- **💭 görüş:** A3 gri-can ile kavramsal akraba — ikisinden birini seç, ikisi birden gerekmeyebilir.

### A6. Accessibility Contract — yeni-oyuncu güvenlik ağı
- **Ne:** İlk zorlanma sonrası açılan gizli, opt-in assist-modifier menüsü (hasar-azalt, ekstra-iframe). *(M212; Celeste/Hades-kanıtlı)*
- **Hangi boşluk:** jüri/yeni-oyuncu 10 dk'da bunalmasın AMA core sulanmasın.
- **⚙️ Teknik dikkat:** **demoda tetik = "ölüm-sonrası" YERİNE** "3× yarı-can" veya "ilk oda 60sn+" olsun (jüri 1. denemede ulaşsın, 2. deneme başlamadan). Basit SO flag + UI panel.
- **💭 görüş:** demo bir jüriye gösteriliyorsa güvenlik ağı değerli; ama opsiyonel.

---

## GRUP B — Derinlik / meta / oda-çeşitliliği
*(💭 genel sezgim: bunlar daha çok yatırım; bazıları kendi canon'unla (Karar#25) çakışıyor. Ama kodun elverdiği bir tanesi demoyu zenginleştirebilir — bilemem, sen bak.)*

| # | Mekanik (ID) | Ne / boşluk | Dikkat / canon | 💭 görüş |
|---|---|---|---|---|
| B1 | **Fragment Contract Door** (V0003/V0090/M65) | Map Fragment alınca sonraki kapı bir "kontrat çipi" gösterir: +tehdit/+elite ↔ +reroll/Rare-ağırlık/Echo. **Run-içi, Hub'a dokunmaz.** | ⚙️ Karar#25'i ÇİĞNEMEZ (run-içi). Karar-penceresi ≤5sn, seçilmezse normal oda. Spawn-ağırsa pool+cap. | Run-içi olduğu için demoda küçük versiyonu mümkün; environment-tezini de gösterir. |
| B2 | **Hub Rift Script room-mutator** (V0213/V0061; HoH2#1) | Hub'da kartlarla sonraki run'ın oda-üretim parametrelerini değiştir. | ⚠️ **Karar#25 ile çakışır** (kalıcı hub-meta = Faz 4-5). Revize edersen: OR-yollu + sidegrade-only. | Environment-tezinin en güçlü kanıtı AMA canon'unu değiştirmeni gerektirir — senin çağrın. |
| B3 | **Curse Gate / Voluntary Shadow Debt** (V0050/V0114/V0054; HoH2#2) | Opt-in oda: görüş daralır/heal azalır ↔ cyan Echo/Legendary-ağırlık. | ⚙️ **opt-in ŞART** (HoH2'nin zorla-versiyonu nefret edilen mekanik). 2D-light: gölge üretme, salt radius↓; aktif-light cap. | Risk-ödül derinliği güzel; Curse Gate oda-tipini besler. Demoda tek tutorialize edilmiş gate denenebilir. |
| B4 | **Resonance Forecast + Multiplicative Synergy** (V0004/V0061/M68/M208) | Draft kartlarında "yakın-sinyal" forecast ("Rage+Bleed=Iron Echo"); modifier'lar toplanmaz, çarpılır. | ⚙️ "çarpan" kuralı DraftManager spec'ine yaz (MODIFIER=always-on taksonomisiyle çakışmasın). Tüm sinerjiyi önden çözme (keşfi öldürür) → "yakın sinyal". | Yeni sistem değil, mevcut draft'ı okunur kılar; demoda Warblade-only tag-forecast mümkün. |
| B5 | **Echo Memory Bias + Heir Bequest** (M167/V0085/V0526; M169) | Run-sonu en-çok-kullanılan tag → sonraki run draft +ağırlık (bias) / "neyi miras bırakırım" kasıtlı seçim. | ⚠️ **run-arası kalıcı = Karar#25 kapsamı (Faz 4-5).** Alternatif: aynı-run-içi tek-run bias (run bitince sıfır) = canon-güvenli. | Shattered Echo meta'yı besler ama kalıcı versiyonu canon-revize ister; run-içi versiyonu güvenli. |
| B6 | **Luck = tüm RNG-eğrisini büken tek stat** (HoH2#7, ÖZGÜN) | Tek "şans" statı crit/status/fragment-drop/draft-tier şanslarını birden büker. | ⚙️ pity-sayacı Luck'tan BAĞIMSIZ; Luck cap ±%30 (yoksa snowball). | İlginç build-derinliği ama görünmez stat → "ölü stat" riski; sonraya daha uygun olabilir. |
| B7 | **Placement-Driven Resource** (V0012) | Rage/Focus duvar/uçurum kenarında daha hızlı dolar → IsoRoomBuilder geometrisini combat'a bağlar. | ⚙️ mesafe-kontrol staggered/Job (her-frame raycast değil). | Environment-tezini combat-feel'e bağlayan zarif köprü. |
| B8 | **Status Layering (M62) + Element Manifold (M166)** | Ailment-stacking (Hexer/Elementalist tavanı) / cast-pozisyonu geçen spell'in elementini dönüştürür. | ⚙️ deterministik tile/zone (Noita-sim değil). | Elementalist/Hexer sınıf-derinliği — post-demo çok-sınıf açılınca daha anlamlı. |
| B9 | **Fixed-Verb + Rotating Environmental Rule** (V0013) | Skill seti sabit, oda zemin-kuralı (rüzgar/kırılan-zemin) Director'dan yüklenir. | ⚙️ statik ortamda realtime-light açma; tilemap tek draw-call. | Tek Warblade ile 5 farklı-hisseden oda → sunum için güçlü; sınıfa yük bindirmez. |
| B10 | **Drop&Bank at-risk currency** (M209) | Run-içi Echo ölümde düşer (geri-alma riski) / Hub-bank'a aktarılınca kalıcı. | ⚠️ Hub-bank kısmı Karar#25; **run-içi drop/retrieval kısmı canon-güvenli.** | Run-içi versiyonu mid-run gerilim katar; kalıcı versiyonu meta (sonra). |

---

## ⚙️ ORTAK TEKNİK DİKKAT (Grup A kombinasyonu için — önerilen güvenlik parametreleri, bağlayıcı değil)
Eğer A1+A2+A3(+A4) birlikte girerse "kapanmayan agresyon-döngüsü" riski var. Önerilen denge:
1. **Tüm hasar çarpanları (sweet-spot, crack-penceresi) x3.0 cap'ten ÖNCE** uygulanır; x3.0 nihai kısıtlayıcı.
2. **Flowstep aktifken aggression-heal şarjı −%50** (iki agresyon-ödülü üst üste → biri zayıflar).
3. **Posture-crack penceresi Flowstep STATE'ini sıfırlar** (break = yeni momentum başlangıcı, bedava devam yok).
4. **Sweet-spot bonus max +%20 raw, Posture'a katkı YOK.**

## ⚙️ FPS / PERF (RIMA spawn-ağır → her görsel-yoğun mekanikte)
Damage#/VFX/glyph/ghost/particle = **pool + hard-cap + zero-alloc**, per-event Instantiate/Destroy YASAK. UI = Canvas-rebuild fırtınası yok (world-space mesh-TMP, cached string). 2D-light = radius-only, per-frame dinamik ışık yok (URP 2D 60→20 riski). Build'de profille, steady-state GC≈0, worst-case <16.6ms. *(Detay: studio `01_PIPELINE/fps_optimization_reference.md` + `performance_spawn_guardrail.md`.)*
