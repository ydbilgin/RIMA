ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Bağımsız ax QC'sinin bulduğu 3 gerçek nit'i CERRAHİ düzelt (min kod). Sahne dosyasına dokunma.

# Dosyalar (sadece bunlar)
- Assets/Scripts/UI/CharacterSelectScreen.cs
- Assets/Scripts/Skills/SkillDatabase.cs
- Assets/Scripts/UI/SettingsMenuUI.cs

# FIX 1 — CharacterSelectScreen.cs RefreshSkillList (~line 602-603)
Sorun: eski skill satırları `Destroy()` ile siliniyor (frame sonu ertelenir) → yeni satırlar eklenince VerticalLayoutGroup 1 frame eski+yeni'yi üst üste hesaplıyor (flash).
Fix: çocuğu önce parent'tan kopar, sonra Destroy:
```
for (int i = skillContent.childCount - 1; i >= 0; i--) {
    var c = skillContent.GetChild(i);
    c.SetParent(null, false);
    Destroy(c.gameObject);
}
```

# FIX 2 — Deterministik SkillDatabase (NRE / ilk-frame boş)
Sorun: `EnsureSkillDatabase()` (CharacterSelectScreen.cs ~808-818) `FindAnyObjectByType<SkillDatabase>()` ile Awake'i HENÜZ çalışmamış bir SkillDatabase döndürebilir → `GetAll()` boş/NRE. Ayrıca BuildScreen'deki ilk SelectClass DB hazır olmadan sorgulayabilir.
Fix (iki küçük dokunuş):
1. SkillDatabase.cs: `private bool built;` ekle. `Build()` başında `if (built) return; built = true;` (çift-build önle) + db'yi temiz başlat. Yeni public method:
   ```
   public void EnsureBuilt() { if (!built) Build(); }
   ```
   Awake'teki `Build()` çağrısı aynen kalsın (built guard çift-build'i engeller).
2. CharacterSelectScreen.cs `EnsureSkillDatabase()`: hem `existing` hem yeni `AddComponent` yolunda dönen instance üzerinde `EnsureBuilt()` çağır (örn: `var db = existing ?? go.AddComponent<SkillDatabase>(); db.EnsureBuilt(); return db;`). Ve `SkillDatabase.Instance != null` yolunda da `Instance.EnsureBuilt()`.
3. CharacterSelectScreen.cs `BuildScreen()`: ilk `SelectClass(...)` çağrısından ÖNCE bir kez `EnsureSkillDatabase()` çağır (DB ilk frame'de hazır olsun).

# FIX 3 — SettingsMenuUI.cs gameplay-only row gap (~line 467-472 RegisterGameplayOnlyRow)
Sorun: menüde (Player yok) Aim/Dash row'ları SetActive(false) ile gizleniyor ama manuel `float y` pozisyonlama yüzünden altlarında 52px BOŞLUK kalıyor ("GAMEPLAY" başlığı altında delik).
Fix (minimum): Player yokken bu gameplay-only row'ları **hiç oluşturma** ve y-imlecini ilerletme (gizlemek yerine atla). Yani BuildUI'da Aim/Dash row'unu eklemeden önce `GameObject.FindGameObjectWithTag("Player") == null` ise o row'u skip et + `y` o row kadar düşmesin. Player varsa (gameplay) eskisi gibi oluştur. RegisterGameplayOnlyRow artık gerekmiyorsa sadeleştir (ama başka yerde kullanılıyorsa dokunma — kontrol et).

# Doğrulama (ZORUNLU)
- Unity MCP: refresh_unity compile=request → read_console types=error filter=CS → 0 CS hatası.
- Değiştirilen metot/satırları + compile sonucunu profil-DONE dosyasına yaz. Sahne DEĞİŞMEMELİ.
- BELİRSİZLİK → BLOCKED.
