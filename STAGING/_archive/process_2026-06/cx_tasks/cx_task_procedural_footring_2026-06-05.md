ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
CharSelect v3.3 mini: seçim ayak-halkası SPRITE yerine PROSEDÜREL (kullanıcı isteği — "bazen tam oturmuyor"). Kök neden: halka karakter kutusuna göre konumlanıyor/ölçekleniyor; kutu FIT ölçeğiyle karakter başına değişiyor.

# Dosya
`Assets/Scripts/UI/CharacterSelectScreen.cs` (+ texture üretimi RimaUITheme'e de konabilir). KOD-ONLY, .unity YASAK.

# İş
1. **Runtime elips-halka sprite üretimi** (bir kez, cache'li static): `Texture2D` ~160×100, anti-aliased elips RING (iç boşluk + ~6-8px halka kalınlığı + dışa yumuşak glow falloff alpha). Beyaz çiz — renk Image.color ile (cyan #00FFCC). Pattern referansı: `RimaUITheme.PassiveIcon` runtime Sprite.Create yaklaşımı. Point-filter KULLANMA (bu UI halkası, AA istenir — Bilinear bırak).
2. **Konumlama fix (asıl iş):** halka her karakterin **FIT ayak noktasına** anchor'lanır (sprite'ın feet-pivot'ı zaten hesaplı — aynı noktayı kullan), boyutu TÜM karakterlerde SABİT (karakter kutu ölçeğinden bağımsız; iso oranı: height = width × 0.61, genişlik ≈ karo genişliğinin ~%80'i — mevcut görünür halka boyutuyla benzer başla, serialized `[SerializeField] float footRingWidth` ile ayarlanabilir).
3. Mevcut sprite-tabanlı halka/hover-ring yolunu kaldır; **hover halkası da aynı prosedürel sprite'ı** daha soluk alpha ile kullanır. 0.2s fade-in davranışı (v3.2) korunur. Statiklik korunur (pulse/dönme yok).
4. Eski halka sprite referansı/yüklemesi temizlenir (asset dosyasını silme — sadece kod referansı).

# Doğrulama (BU TASK ax-Gemini-3.5-Flash'a verildi — Unity'ye DOKUNMA kuralı)
- SADECE `dotnet build RIMA.Runtime.csproj` ile doğrula (PASS şart). **UnityMCP/Unity editor KULLANMA** (başka agent Unity'yi sürüyor — çakışma yasak). Play-verify'ı sonra orchestrator yapacak.
- Sonucu `STAGING/_ax_done_footring.md` dosyasına yaz: değişen satır aralıkları + build çıktısı özeti + bilinen kısıtlar.
