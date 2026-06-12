ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Görev: FAZ1 — Oda Işığı (canon RoomLightingProfile + lit-environment, VERIFY-FIRST, demo-safe)

## Amaç
Odalara "tasarlanmış diorama" hissi kazandıracak ışık. Altyapı HAZIR:
- `RoomLightingProfileSO.cs` (globalColor/globalIntensity + List<PointLightSpec>{normalizedRoomPosition,color,intensity,innerRadius,outerRadius}).
- `RoomTemplateSO.lightingProfile` alanı VAR (satır 30).
- `IsoRoomBuilder.ApplyLighting` (satır 656-695) profili tüketiyor: Global Light2D + her spec için Point Light2D,
  hepsi "Lighting" container'a. Profil null → ışık eklenmez (default-preserving).
Senin işin: canon profil ASSET'leri oluştur + demo odalarına ata + zemini lit yap ki ışık havuzları görünsün.

## ⚠️ KRİTİK GERÇEK — UNLIT sprite tuzağı (geçen session bug'ı)
Demo sprite'ları UNLIT materyalde (Sprite-Unlit). Bu BİLİNÇLİ: "Sprite-Lit-Default" moblar SİYAH render
ediyordu, 5 prefab unlit'e çevrilerek düzeltildi (commit 597feb05). 2D Light, UNLIT sprite'ı ETKİLEMEZ.
- **Zemin/duvar/environment tilemap → URP 2D LIT materyale geçir** ki point-light'lar zeminde havuz yapsın.
- **Karakter / mob / düşman / player / boss prefab'larının materyaline DOKUNMA** — UNLIT kalsınlar.
  Siyah-render regresyonu KESİNLİKLE YASAK. Lit'e çevireceğin TEK şey environment (zemin/duvar/dekor), canlılar DEĞİL.

## NLM EXACT KANON (Act1 Shattered Keep — bu değerleri kullan)
- Ambient (Global Light2D): intensity **0.22**, color = SOĞUK desatüre slate-tint (sıcak DEĞİL),
  öneri globalColor ≈ RGB(0.62, 0.66, 0.74). Zemin slate #3A3D42, void mor #3A1A4A okunaklı kalmalı.
- Brazier point-light (ember): color **#E89020** = RGB(0.910, 0.565, 0.125), SICAK; intensity ~1.1;
  innerRadius ~0.4; outerRadius ~3.5. Kenar-yoğun anchor'lara koy (random DEĞİL).
- Rift accent point-light (cyan): color **#00FFCC** = RGB(0.0, 1.0, 0.8); intensity ~0.8; outerRadius ~2.5.
  Cyan toplam sahnenin ≤%15'i kalmalı → odada en fazla 1 cyan light, rift anchor'ına.

## Yapılacaklar

### 1) Canon profil ASSET'leri oluştur (Assets/Data/Rooms/Lighting/ altına)
2-3 yeniden-kullanılır profil yarat (menüden RIMA/Room/Room Lighting Profile veya kod/editor ile):
- `RoomLightingProfile_Combat` — global 0.22 soğuk + 2 brazier ember point (kenar anchor) + opsiyonel 1 cyan rift.
- `RoomLightingProfile_Boss` — global biraz daha moody + ember + daha güçlü cyan rift (boss aggressive-rift).
- `RoomLightingProfile_Special` (Shop/Elite) — ember ağırlık, merchant sıcaklığı; cyan az/yok.
Değerler yukarıdaki kanondan.

### 2) Demo odalarına ata
Demo akışındaki RoomTemplateSO'ları bul (DemoRoomBank / Generated combat odaları + Boss + Shop) ve
`lightingProfile` alanına uygun profili ata. **ÖNCE TEK bir temsili combat odasına ata + doğrula (aşağı),
sonra kalanlara yay.** Hepsine körlemesine atama YAPMA.

### 3) Zemin/environment lit materyal
Temsili odanın floor/wall tilemap renderer materyalini URP 2D lit (Sprite-Lit-Default veya proje URP 2D
lit shader'ı) yap. URP 2D Renderer'ın 2D Light desteklediğini doğrula (proje URP 2D Renderer kullanıyor).
Canlı sprite materyallerine DOKUNMA.

## VERIFY-FIRST (Unity MCP açık: RIMA@... — görsel doğrula, kanıt ver)
- Temsili odayı kur/oynat → screenshot al. KONTROL: zemin ışık havuzlarıyla diorama gibi mi, **yoksa siyaha mı düştü?**
- Brazier'lar sıcak havuz · rift cyan glow · karakterler okunaklı (unlit, siyah DEĞİL).
- **Eğer zemin siyaha düşüyor / sprite'lar kararıyor / okunamıyor → DUR, BLOCKED yaz, geri al, kalan odalara YAYMA.**
  Sebep: global çok düşük ya da lit-setup eksik. Çözemezsen orchestrator'a BLOCKED + screenshot + teşhis bırak.
- Doğrula geçince kalan demo odalarına profilleri ata.

## KISITLAR
- `_Arena.unity` yapısını DEĞİŞTİRME (profil atama RoomTemplateSO asset'lerinde; ışık IsoRoomBuilder'da runtime kuruluyor).
- Canlı sprite (player/mob/boss/enemy) materyallerini DEĞİŞTİRME — unlit kalsın (siyah-render yasağı).
- `RoomLightingProfileSO.cs` / `IsoRoomBuilder.ApplyLighting` mantığını DEĞİŞTİRME — sadece veri/asset üret + ata
  (gerekirse küçük editor util'i OK ama core logic'e dokunma).
- Pre-existing kodu refactor etme. Compile 0-error. Mevcut testlerde 0 yeni fail (fail sayısını öncesi/sonrası raporla).
- Commit ETME — uncommitted bırak.

## Başarı kriteri (CODEX_DONE'a yaz + screenshot yolları)
1. 2-3 canon profil asset'i oluşturuldu (kanon değerlerle).
2. Temsili oda Unity MCP screenshot ile diorama-lit doğrulandı (zemin havuzlu, karakter okunaklı, siyah YOK).
3. Demo odalarına profil atandı (hangi oda→hangi profil listele).
4. Canlı sprite materyalleri unlit (değişmedi) — siyah-render regresyonu yok.
5. Compile temiz, 0 yeni test fail, commit yok. BLOCKED ise sebep + screenshot + teşhis.
