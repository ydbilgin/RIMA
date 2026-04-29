# Rift Remnant Plan — S43
**Date:** 2026-04-29

## Lore Araştırması

### Rift Sistemi (GDD özeti)
GDD rift'i tek başına ayrıntılı bir kozmoloji bölümü olarak tanımlamıyor. Açık kaynaklar daha çok sistem ve class kararlarından geliyor: zorluk modu isimlerinde Rift/Fracture, universal Rift Parry'nin kaldırılması, Rift Break'in V Burst context phase olması ve bazı class skill/VFX adlarında Rift kullanımı. Bu yüzden aşağıdaki değerlendirme lore kararı değil, mevcut dokümanlardan çıkarılmış üretim önerisidir.

Rift remnant görsel dili için pratik kural: accent rengiyle hairline fracture/crack; silah, vücut veya ekipman üzerinde; aura, büyük glow veya genel VFX değil.

### Kaynak Kanıtları
| Kaynak | Claim | Confidence |
|---|---|---|
| `TASARIM/MASTER_KARAR_BELGESI.md` #22-#23 | Rift Break tüm class'lara bağlanan context-based interactive phase; class-model eşlemesi var. | High |
| `TASARIM/MASTER_KARAR_BELGESI.md` #6 | Universal Rift Parry kaldırılmış; rift her class için otomatik parry lore'u değil. | High |
| `TASARIM/MASTER_KARAR_BELGESI.md` #37-#38 | Ranger tactical rift hunter; Gunslinger rift-tech dual-pistol duelist. | High |
| `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md` | Shadowblade Rift Scar, Ranger Rift Arrow/Rift Step, Gunslinger Rift Dash/Rift Grenade kullanıyor. | High |
| `CURRENT_STATUS.md` Accent Renkleri #163 | Güncel class accent hex ve yerleşimleri kilitli. | High |
| `_STAGING/PROMPTS_S43/styleref_cheatsheet_v1.md` | Anchor kimlikleri ve zorunlu visual detaylar: Warblade sword crack, Brawler rift fracture, vb. | High |
| `_STAGING/anchors/<class>/<class>_anchor.png` | Anchor mevcut görsel durumu: hangi remnant zaten var, hangisi küçük edit veya regen gerektirir. | Medium |

### Class-Rift İlişki Tablosu
| Class | Rift Maruziyeti | Enerji Türü | Rift Remnant? |
|---|---|---|---|
| Warblade | Orta: VFX imzası rift enerji slash; mevcut kılıç crack'i rift diliyle uyumlu. | Rage + cold blue rift/steel accent | Evet, zaten var |
| Shadowblade | Yüksek: Rift Scar/collapse ve faz geçişleri class çekirdeği. | Sever + void/rift scar | Evet |
| Ranger | Yüksek: tactical rift hunter, Rift Arrow/Rift Step. | Focus + rift-mark hunter tech | Evet |
| Ravager | Düşük/Belirsiz: core kaynak Fury ve blood/carnage; doğrudan rift kaynağı yok. | Fury + blood/carnage | Hayır; scar class accent olabilir |
| Ronin | Orta: Void Cleave ve Rift Break timing-chain modeli; core kaynak rift değil. | Draw Tension + void/white cut | Evet, minimal ekipman izi |
| Gunslinger | Yüksek: rift-tech identity, Rift Dash/Rift Grenade. | Heat + rift-tech/brass | Evet |
| Brawler | Orta/Yüksek: cheatsheet'te explicit rift remnant fracture; gameplay core fiziksel Charge. | Charge + amber physical impact | Evet, explicit |
| Elementalist | Düşük/Belirsiz: LMB adı Rift Bolt ama class kararı element/Light synthesis. | Mana + Fire/Frost/Light | Hayır, rift remnant şart değil |
| Hexer | Düşük: curse/hex stack sistemi; rift değil curse enerjisi. | Hex Stacks + curse/dread | Hayır |
| Summoner | Düşük: minion/death/sacrifice sistemi. | Charges + death/necro | Hayır |

## Aksiyon Planı

| Class | Rift Remnant | Nereye | Form | Aksiyon | Claude ile Uyum |
|---|---|---|---|---|---|
| Warblade | Zaten var | Sword fuller + chest plate crack | Cold blue hairline crack | DOKUNMA | Uyumlu |
| Shadowblade | Evet | Göz/veil üstü ve dagger edge | Çok zayıf void purple eye slit + mevcut dagger edge korunur | Edit Image | Uyumlu |
| Ranger | Evet | Bow limb/tip | Gold hairline crack, aura yok | Edit Image | Farklı: Regenerate değil |
| Ravager | Rift değil, class scar | Sternum rune scar | Blood red channel scar/glow, rift diye etiketlenmemeli | Regenerate | Kısmi: action uyumlu, lore etiketi farklı |
| Ronin | Evet, minimal | Scabbard edge | Pure white hairline crack | Edit Image | Uyumlu |
| Brawler | Evet, explicit | Left shoulder to forearm | Amber fracture-cracks in skin, tattoo değil | Regenerate | Uyumlu |
| Gunslinger | Evet | Pistol barrel/cylinder | Brass-yellow hairline crack | Edit Image | Uyumlu |
| Elementalist | Yok | N/A | Element orb zaten class enerjisi | Regenerate | Uyumlu |
| Hexer | Yok | N/A | Lantern curse light, rift değil | Regenerate | Uyumlu |
| Summoner | Yok | N/A | Staff/palm necro green, rift değil | Regenerate | Uyumlu |

## Claude'dan Farklı Görüşler

1. Ranger: Claude `Regenerate` demiş; ben `Edit Image` öneriyorum. Anchor kimliği okunuyor, bow sol elde ve değişiklik yalnızca bow üzerinde gold hairline crack. Task kriterine göre renk/küçük detay ekleme Edit Image kapsamı.
2. Ravager: `Regenerate` kararına katılıyorum, çünkü hair/rune scar gibi büyük kimlik düzeltmeleri var. Ancak bunu rift remnant olarak değil, Fury/blood class scar accent olarak etiketlemek daha doğru. Mevcut lore doğrudan rift kullanımı göstermiyor.
3. Elementalist: LMB adında `Rift Bolt` geçiyor; buna rağmen ana kararlar Elemental State + Fire/Frost/Light üzerine. Bu yüzden rift remnant eklememek doğru, ama dokümanda isim/lore belirsizliği var.

## Edit Image Prompt Notları

- Shadowblade: Add a tiny void purple eye glow above the face veil; keep daggers, pose, clothing, and silhouette unchanged; no aura.
- Ranger: Add a thin gold hairline crack on the bone bow limb/tip; keep hair, pose, quiver, body, and bow hand unchanged; no glow cloud.
- Ronin: Add a very thin pure white crack along the scabbard edge only; keep katana sheathed and body pose unchanged.
- Gunslinger: Add a small brass-yellow hairline crack on each pistol barrel/cylinder mechanism; keep hair, skin, coat, and dual lowered pistols unchanged.

## Assumptions and Gaps

- GDD'de rift sistemi açık bir lore bölümü olarak yazılı değil; çıkarımlar class skill kararları, master kararlar ve prompt cheatsheet üzerinden yapıldı.
- Anchor görsel incelemesi 128px üretim dosyalarına bakılarak yapıldı; PixelLab edit kalitesi final QC gerektirir.
- `Rift remnant` ile `class accent VFX` ayrımı özellikle Ravager ve Elementalist için lore kararı gerektiriyor; final karar Claude'a ait.

## Reviewer Checklist

| Check | Status |
|---|---|
| 10 karakter analiz edildi | PASS |
| GDD/SINIF_KARAR referans alındı | PASS |
| MASTER_KARAR ve CURRENT_STATUS accent tablosu referans alındı | PASS |
| Her kararda Edit Image / Regenerate gerekçelendirildi | PASS |
| Claude analizinden farklı görüşler açık yazıldı | PASS |
| PNG dosyalarına yazma yapılmadı | PASS |

## Self-Check
- [x] 10 karakter analiz edildi
- [x] GDD/SINIF_KARAR referans alındı
- [x] Her kararda Edit Image / Regenerate gerekçelendirildi
- [x] Accent renkleri CURRENT_STATUS tablosundan alındı
