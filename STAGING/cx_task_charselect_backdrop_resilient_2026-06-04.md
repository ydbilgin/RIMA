ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
CharacterSelect roster-room backdrop'u (room_bg) Start frame'inde cold Resources-index lag'i yüzünden bazen null gelip fallback dark'ta kalıyor. RESILIENT yükleme ekle: null gelirse birkaç frame retry edip yükle. KOD-ONLY, sadece CharacterSelectScreen.cs (paylaşılan RimaUITheme'e DOKUNMA). Sahne .unity'ye dokunma.

# Sorun (doğrulandı)
- `RimaUITheme.CreateFullScreenBackdrop(parent, RoomBackdrop, BackgroundDark)` içeride `RimaGeneratedSpriteCache.Load` → `Resources.Load<Texture2D>(path)` çağırıyor. Start frame'inde session-yeni asset için null dönüyor (mid-play OK). Backdrop BİR KEZ Start'ta kurulduğu için fallback'te kalıyor.
- room_bg artık `Assets/Resources/UI/RIMA/CharacterSelect/room_bg.png` olarak VAR (Default texture, Resources.Load<Sprite> ve <Texture2D> mid-play çalışıyor).

# Dosya (sadece bu)
Assets/Scripts/UI/CharacterSelectScreen.cs

# Fix (surgical)
1. BuildScreen'de CreateFullScreenBackdrop'tan dönen Image referansını sakla (örn `backdropImage`).
2. Eğer `backdropImage.sprite == null` (fallback'e düştü) ise bir coroutine başlat: en fazla ~10 frame boyunca (her frame `yield return null`) `Resources.Load<Sprite>("UI/RIMA/CharacterSelect/room_bg")` dene; non-null gelince:
   - `backdropImage.sprite = spr; backdropImage.color = Color.white; backdropImage.type = Image.Type.Simple;`
   - center-anchor (0.5,0.5) + pivot (0.5,0.5) + bir `AspectRatioFitter` (EnvelopeParent, aspectRatio = spr.rect.width/height) ekle (CreateFullScreenBackdrop'un non-null dalındaki davranışın AYNISI — oradan kopyala).
   - yüklenince coroutine bitir.
3. MonoBehaviour zaten coroutine başlatabilir (CharacterSelectScreen MonoBehaviour). `StartCoroutine` kullan. Null-guard: obje destroy edilirse coroutine güvenli çıksın.
4. Mevcut tüm roster-room mantığına DOKUNMA — sadece backdrop resilient-load ekle.

# Doğrulama (ZORUNLU)
- refresh_unity compile=request → read_console types=error filter=CS → 0 CS hatası.
- Play-mode: CharacterSelect aç, birkaç frame bekle, "Backdrop" Image'inin sprite'ı "room_bg" olmalı (fallback değil). (cold-start'ta coroutine retry ile yüklenmeli.)
- Değişen metotları + compile + backdrop sprite sonucunu profil-DONE'a yaz. Sahne DEĞİŞMEMELİ.
- BELİRSİZLİK → BLOCKED.
