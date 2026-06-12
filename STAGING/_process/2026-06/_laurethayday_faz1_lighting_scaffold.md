ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: gerekirse uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<soru>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç (FAZ 1 — ışık-önce, council-onaylı ilk görev)
RIMA _Arena için DEFAULT-PRESERVING lighting scaffold. Per-oda ışık authoring altyapısı kur AMA profil atanmadıkça mevcut görünüm AYNEN kalsın (regresyon YOK). Plan: `STAGING/VISUAL_MASTER_PLAN_2026-06-11.md` FAZ1. Karar: `STAGING/ROOM_DESIGN_DECISION_2026-06-11.md` §madde4.

# OKU
- `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs` (container kurulumu ~Ensure*, ClearPrevious ~261, Build sırası ~120-130)
- `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs` (alan ekleme noktası)
- `Assets/Scenes/_Arena.unity:983-1006` (mevcut Global Light 2D: type Global, beyaz, intensity 1.4)

# YAP (cerrahi, kod-only — _Arena.unity'yi ELLE METİN DÜZENLEME)
1. **YENİ** `Assets/Scripts/MapDesigner/Room/Data/RoomLightingProfileSO.cs`:
   - `[CreateAssetMenu]` SO. Alanlar: `Color globalColor`, `float globalIntensity`, + `List<PointLightSpec>` (her spec: `Vector2 tileOffset` veya normalized room pozisyonu, `Color color`, `float intensity`, `float radius/innerOuter`). 2-4 point spec yeter.
2. **RoomTemplateSO.cs**: opsiyonel `public RoomLightingProfileSO lightingProfile;` alanı ekle (null-safe; serialize).
3. **IsoRoomBuilder.cs**:
   - `Lighting` child container ekle (mevcut propsContainer/decorationsContainer paterni gibi — Ensure*'ta oluştur, ClearPrevious'ta `DestroyChildren(lightingContainer)` ekle).
   - Build sırasında (markers'tan sonra) `ApplyLighting(template)`: profil VARSA `Lighting` altına `Light2D` instantiate et (Global için globalColor/intensity, point'ler için spec'lerden URP `Light2D` type Point). Profil YOKSA hiçbir şey yapma (mevcut sahne ışığı dokunulmaz).
   - **Opsiyonel reparent (kod, sahne-dosyası değil):** sahnede isimle/komponentle bulduğun mevcut Global Light2D'yi `Lighting` root altına KOD ile reparent et — DEĞERİNİ DEĞİŞTİRME. Bulunamazsa sessiz geç.
   - URP 2D `Light2D` API'sini kullan (`UnityEngine.Rendering.Universal.Light2D`). Asmdef referansı gerekiyorsa ekle.
4. **DOKUNMA:** decoration density / FocalCluster / RoomDecorationPass / Wang / ISO / template-props / RoomRunDirector.

# TEST (EditMode, mevcut Room test paterni)
- Profil-yok yolu: `ApplyLighting(null-profile)` exception atmaz, Lighting root boş kalır, mevcut davranış bozulmaz.
- Profil-var yolu: N point spec → N+1 (veya N) Light2D oluşur, doğru parent (Lighting root) altında.
- Lighting root tek scene-child olarak bulunabilir/gizlenebilir (isim sabit).

# KABUL KRİTERİ
- Mevcut testler derlenir + geçer (0 yeni fail).
- Default (profil-yok) build sonrası _Arena görünümü AYNI (1 global ışık, intensity 1.4 eşdeğeri) — regresyon yok.
- `Lighting` root var, tek child olarak gizlenebilir.
- Wang/autotile/ISO/template-props/decoration DEĞİŞMEDİ.
- Unity AÇIKSA `read_console` ile 0-error teyit; değilse C# syntax dikkatli doğrula + raporda belirt.

# KISIT
- COMMIT ETME (Claude QC + kullanıcı playtest sonrası).
- _Arena.unity'yi elle metin-editle DEĞİŞTİRME (reparent KOD ile, runtime). Scene serialization'a dokunma.
- Belirsizlik (Light2D asmdef, point spec koordinat sistemi) → BLOCKED yaz, tahmin etme.
- Sonuç `CODEX_DONE_laurethayday.md`: değişen/yeni dosyalar, test sonucu, compile durumu, varsayımlar, default-preserving kanıtı.
