# TASK A1 — Import 10 RIMA props from PixelLab → wire into Build Mode (F2) catalog

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files/assets only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

UNITY ERROR CHECK: iş bitince read_console (Error+Warning); kendi hatanı ÇÖZ, ilgisiz/önceden-var hatayı BİLDİR (silme), raporda console durumunu yaz.

E1 OUTPUT: Sonucu DOSYAYA yaz (DONE dosyası); dönüşün ≤10 satır özet + yol. Rapor içeriğini dönüşe gömme.

## AMAÇ
PixelLab'de üretilmiş 10 RIMA-stil prop'u Unity'ye import et ve Build Mode (F2) prop palette'inde seçilip yerleştirilebilir hale getir. Build Mode catalog veri-güdümlü: `PropRegistrySO.allProps` içindeki her `PropDefinitionSO` otomatik F2 PROPS sekmesinde karta dönüşür (`Assets/Scripts/UI/BuildMode/BuildModeAssetCatalog.cs:101-130`). Yani: sprite import → PropDefinitionSO oluştur → PropRegistry'ye ekle → F2 otomatik gösterir.

## ⚠️ KRİTİK BAĞLAM (oku)
- **PixelLab balance = 0** (5125/5000, kota aşıldı). Bu prop'lar **yeniden üretilemez** → veri kaybı = kalıcı. Dikkatli ol.
- Prop'lar **REVIEW status** (her biri 16 aday frame). `get_object` 16 public backblaze URL döndürür.
- **`select_object_frames` ÇAĞIRMA** — o geri-dönüşsüz (seçilmeyen 15 adayı siler). Bunun yerine: en iyi frame'in **public URL'sini doğrudan curl ile indir** (URL'ler token'sız erişilebilir görünüyor). Böylece tüm adaylar review'da korunur.
  - Fallback: curl 403/auth-required dönerse → o ZAMAN `select_object_frames(id, indices=[seçtiğin])` çağır + sonra `get_object`'ün completed download link'ini kullan.

## 10 PROP (object_id | isim | blocksWalkable)
| id | name | block? |
|----|------|--------|
| b86850b1-199b-475b-b936-8488778680a4 | Barrel (weathered RIMA) | YES |
| 3a326bc2-4759-4c81-a72f-3bc336d10b36 | Crate (weathered RIMA) | YES |
| eaeffda5-8ff5-4334-aa15-6040d2fd4add | OrnateChest | YES |
| 02ed5245-4bcf-46e0-ae64-50515b08136f | IronBrazier | YES |
| 47031106-559d-4474-a0c7-443cdb06c517 | BrokenPillar | YES |
| 7ca28f7a-735d-4a08-9db3-5abc9de32c9f | ClothBanner | NO (flat decor) |
| e77ec4e5-6419-4b8e-a335-30d9c77716dd | StatueFragment | YES |
| bc6809ea-128e-47ce-bea2-a40b1d395148 | RubblePile | NO (flat scatter) |
| afe05ba6-158b-4b74-873c-af91737d9caa | WallTorch (RIMA) | YES |
| e1666568-5a57-4562-9eb4-a1975d3fa255 | BurlapSack | NO (flat scatter) |

## PROP BAŞINA PIPELINE
1. `get_object(id)` → 16 adayı incele.
2. **TEK en iyi frame seç** — kriter: temiz silüet, dik/ortalanmış, keskin pixel (AA-blur YOK), şeffaf arka plan, RIMA paleti (muted desaturated slate-gray + void-purple). Stray pixel/artifact/bozuk oran olan frame'i atla. (Frame'ler tek prop'un varyasyonları; çoğu benzer → en temizini al.)
3. Seçtiğin frame'in public URL'sini **doğrudan indir** (curl) → `Assets/Art/Props/Pixellab/<Name>.png`. (`select_object_frames` ÇAĞIRMA — yukarıdaki kritik nota bak.)
4. Import ayarları (.meta veya manage_asset): Texture Type=Sprite (2D and UI), Sprite Mode=Single, **Pixels Per Unit=64**, Filter Mode=Point (no filter), Compression=None, Max Size yeterli (≥256), Generate Physics Shape=off, alpha transparency korunur.
5. `PropDefinitionSO` asset oluştur → `Assets/Data/Props/Pixellab/<Name>.asset`. **PATTERN = `Assets/Data/Props/Brazier.asset`'i birebir kopyala** (script guid `2f23466e85ce4503b8122ed7fd82e622`). Alanlar:
   - `propId`: YENİ 32-hex GUID (her prop benzersiz; Brazier'ınkini KOPYALAMA)
   - `displayName`: tablodaki isim (boşluklu okunur ad: "Ornate Chest" vb.)
   - `worldSprite`: import edilen sprite ref `{fileID: 21300000, guid: <png-sprite-guid>, type: 3}`
   - `footprintSize {x:1,y:1}`, `spriteAnchor {x:0,y:0}`
   - block=YES olanlar: Brazier.asset collider değerlerini kullan (blocksWalkable=1, blocksMovement=1, colliderShape=1, colliderFootprintRatio=0.7, colliderLayer=Walls).
   - block=NO olanlar: blocksWalkable=0, blocksMovement=0, colliderShape=0, isTrigger=0, colliderLayer=Default (zemin dekoru, üstünden yürünür).
   - `variantSprites: []`, diğer alanlar Brazier default.
   - (manage_scriptable_object MCP tool'u kullanabilirsin VEYA .asset+.meta YAML doğrudan yaz.)
6. Registry'ye ekle: `Assets/Resources/Props/PropRegistry.asset` → `allProps` listesine 10 yeni SO'nun guid ref'ini ekle (mevcut 9 girişin altına append, format: `- {fileID: 11400000, guid: <so-guid>, type: 2}`).

## DOĞRULAMA (zorunlu)
- `execute_code`: `PropRegistrySO`'yu yükle → `RebuildIndex()` → `AllProps.Count` artık ≥19 mu (9 eski + 10 yeni). Yeni 10'un her birinin `worldSprite != null` olduğunu assert et. Sonra `BuildModeAssetCatalog().Build(registry)` çağır → Props grup entries sayısı = AllProps (icon'lu) sayısı mı, 10 yeni prop displayName ile var mı? Compact özet-string döndür.
- `read_console` (Error+Warning): kendi hatanı çöz, önceden-var/ilgisiz hatayı bildir.

## RAPOR
`STAGING/_process/2026-06/demo_fix_tasks/DONE_A1_props_import.md` yaz:
- Prop başına: seçilen frame index + sprite path + propId + block durumu
- PropRegistry AllProps count (önce→sonra)
- F2 catalog doğrulama sonucu (Props grup entry sayısı, 10 yeni görünüyor mu)
- Console durumu (0-sürpriz)
- select_object_frames çağrıldı mı (fallback'e düşüldü mü)
