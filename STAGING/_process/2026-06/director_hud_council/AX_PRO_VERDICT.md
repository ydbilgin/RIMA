# AX PRO VERDICT — Playtest Polish Batch (2026-06-17)

## 1. Triyaj Tablosu

| Sorun | Kök Neden Analizi (Onay/Red) | Fix Kaldıracı (Dosya/Alan) | Veri/Kod | Efor | 2-Gün Demo Önceliği |
|---|---|---|---|---|---|
| **1. Prop Y-sıralama** | **ONAY.** Matematiksel formül (`-y*100`) oyuncuyla aynı, ancak `PropSorterRuntime` propları `"Props"` layer'ına zorluyor. Eğer bu layer "Default" (oyuncu layer) üzerinde kalıyorsa her şey üste çizilir. | `PropDefinitionSO` (`sortingLayerOverride` ile oyuncu layer'ına eşitleme) veya TagManager layer sırası. | DATA | S | P0 (Ship) |
| **2. Prop collider** | **ONAY.** `PropColliderAutoBuilder` sadece `blocksWalkable` okuyor ve offset'i hatalı olarak hep merkeze kaydırıyor. Phase B-2 (`colliderShape`, ratio) KODDA YOK/ATLANMIŞ. | `PropColliderAutoBuilder.cs` (Phase B-2 fieldlarını okuyacak ve pivotu düzeltecek şekilde update). | KOD | M | P0 (Ship) |
| **3. Silah mount** | **ONAY.** `OrientationSync` rotasyonu hallediyor ancak `WeaponDatabaseSO` içindeki `anchorOffset` ve sprite pivot'u yanlış. | `WeaponDatabaseSO` (`anchorOffset`, `gripOffset` ayarları). | DATA | S | P0 (Ship) |
| **4. Yarık yürünebilirliği**| **ONAY.** 1+2'nin varyasyonu. Orijinal kare tam kapalı. Cerrahi çözüm: auto-builder'ı bypass edip prefab'a özel küçük bir collider koymak. | `PropDefinitionSO` (`blocksWalkable = false` yap) + Prefab'a manuel BoxCollider ekle. | DATA | S | P1 |
| **5. Saçma Hitbox (Drop)**| **NETLEŞTİRME.** Yerde yatan obje "kılıç" değil sarı/turkuaz bir elmas (RewardPickup). Hitbox'ın saçma olması prefab'daki trigger collider'ın çok büyük (muhtemelen grid tabanlı tam kare) bırakılmasından kaynaklı. | `RewardPickup` prefab trigger collider radius/size küçültmesi. | DATA | S | P1 |
| **6. HUD Redesign** | İhtiyaç: Kullanıcı sol üstten -> sol alta, modern bir yapı istiyor. | `HUDController.cs` (Anchor ve Position güncellemeleri). | KOD | M | P1 |
| **7. Boss Can Barı** | **KESİN ONAY.** `BossHealthBar.cs:150`'de `hpFill.type = Image.Type.Filled` yapılmış ama SPRITE ATANMAMIŞ. Unity'de sprite=null ise fillAmount hesabı bozulur ve bar hep %100 görünür. | `BossHealthBar.cs` (hpFill'e `uisprite.psd` atanması veya RectTransform Width kullanılması). | KOD | S | P0 (Ship) |
| **8. Director Mode UI** | İhtiyaç: Panel çok dolu, scroll yok, buton UX'i zayıf. | `DirectorMode.cs` (ScrollRect implementasyonu, padding/renk ayarı). | KOD | L | P1 |
| **9. Elementalist 8-yön**| **ONAY.** Sprite yok (PixelLab kredi=0) + VFX prefab bağlantıları eksik. | Elementalist skill prefabları + Sprite üretimi. | DATA | L | **BLOCKED / CUT** |

---

## 2. HUD Redesign (Sorun 6)
**Somut Layout & Öneri (Hades / Diablo esintili):**
- **Yerleşim:** Sol-Alt (Bottom-Left).
  - `AnchorMin/Max`: `(0, 0)`
  - `Pivot`: `(0, 0)`
- **Hiyerarşi & Boyut:**
  - **HP Bar (Üstte/Büyük):** `anchoredPosition = (24, 30)`, `sizeDelta = (260, 20)`. Uçları hafif eğimli (veya maskeli), kalın ve doygun renk (`#C01020` Crimson).
  - **Resource Bar (Altta/İnce):** HP barının hemen altında bitişik. `anchoredPosition = (24, 16)`, `sizeDelta = (220, 8)`. İnce, keskin bir çizgi şeklinde, parlak (`#10A0C0` Cyan).
  - **Bilgi Metinleri (Oda / Echo):** Barların üst sınırına, `(24, 60)` lokasyonuna minimal, italik ve yarı-saydam (`alpha = 0.7`) metin olarak yatay hizalanmalı.
- **Renk ve Animasyon:** Düşük HP'de barda titreme kalsın, ancak arka plan (track) simsiyah olmamalı, hafif transparan Slate (`#1B1F28`, alpha: 0.8) olmalı.

---

## 3. Director Mode Redesign (Sorun 8)
**Somut UI İyileştirme ve Scroll Yaklaşımı:**
- **Scroll Fix:** `DirectorMode.cs` içinde kodla üretilen `classSkillCardsRoot` bir `ScrollRect` bileşenine bağlanmalı.
  - Hiyerarşi: `SkillPanel` -> `Viewport (Mask + Image)` -> `Content (VerticalLayoutGroup)`.
  - `ScrollRect.content` = `classSkillCardsRoot`.
- **Görsel Hiyerarşi:** 
  - Skill kartlarının arkaplanı zeminle aynı renk olmamalı. Kartlar `DirectorRaised` (`#252A35`), Seçili kart `DirectorBorder` veya `DirectorCyan` ince bir outline'a sahip olmalı.
  - Kart içindeki yazılar (Hasar/CD vb.) text olarak yığılmak yerine, ikon + ufak badge'ler şeklinde (Hades boon seçim ekranı gibi) formatlanmalı.
- **Atama Akışı Netliği:** Alt kısımdaki sabit Q/E/R/F butonları kopuk duruyor. Seçilen yeteneğin detay (Inspector) panelinin EN ALTINA, "Bind to:" başlığıyla yan yana dizilmiş yuvarlak şık tuşlar olarak taşınmalı.

---

## 4. 2-Gün Demo Sıralaması (Plan)
Demo tarihine (19 Haziran) yetişmesi için cerrahi sıralama:
1. **SHIP (Hemen Kapatılacaklar):**
   - **Sorun 7 (Boss HP Bar):** 1 satır kod (`hpFill.sprite = Resources.GetBuiltinResource<Sprite>("ui/skin/uisprite.psd");`).
   - **Sorun 1 (Prop Y-Sort):** TagManager üzerinden layer sıralamasını düzelt VEYA propları "Default" layerına eşitle.
   - **Sorun 3 (Silah Mount):** Warblade prefab'ı pivot/offset ayarlarının Data üzerinden ayarlanması.
   - **Sorun 2 (Collider Offset):** `PropColliderAutoBuilder` offset matematiği düzeltilmesi (`box.offset`'in merkeze değil ayak ucuna çekilmesi).
2. **P1 (Öğleden Sonra / Yarınki İşler):**
   - **Sorun 6 (HUD)**: Anchorları 0,0 yapıp y-pozisyonlarını güncellemek.
   - **Sorun 8 (Director Mode)**: `ScrollRect` hiyerarşisinin kodda oturtulması. 
   - **Sorun 4 & 5 (Yarık + Item Hitbox)**: Prefab üzerinden ufak data ayarları (Yarık için manual collider, Drop objesi için radius daraltma).
3. **CUT / BLOCKED:**
   - **Sorun 9 (Elementalist 8-yön):** Demo için sınıf değiştirme kapatılır (veya Elementalist "WIP" etiketiyle sadece tek yönlü haliyle bırakılır). Yeni görsel üretimi bloke.

---

## 5. Belirsizlik / Varsayım Flag'leri
- 🚩 **S5 Varsayımı:** "Saçma hitbox" ifadesi, ekrandaki sarı/turkuaz (Rift-Forged Edge) yer eşyalarının toplanma menzilinin (trigger radius) aşırı büyük olması olarak yorumlandı. Görsel modelin kılıç olmamasının (elmas olmasının) da rahatsız ettiği varsayıldı.
- 🚩 **S2 Phase B-2 Kullanımı:** `PropColliderAutoBuilder`'ın yeni alanları (`colliderShape` vs.) tamamen ignore ettiği doğrulandı. Ancak bunun bilinçli bir "eski sistem bırakma" kararı mı yoksa sadece unutulmuş mu olduğu net değil. Fix önerisi, bu alanları kullanacak şekilde refactor edilmesidir.
