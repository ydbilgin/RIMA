---
name: rima-context
description: RIMA proje DNA'sı — sub-agent'ların başlangıçta preload ettiği ortak bağlam (ACTIVE RULES, NLM erişimi, Unity hata kuralı, anahtar path'ler). "Base + mixin" agent mimarisinin mixin katmanı; jenerik global agent'lar (builder/auditor/crafter) bu skill ile RIMA-flavored olur. Agent frontmatter'ında `skills: [rima-context]` ile bağlanır.
---

# RIMA Context (mixin)

> Bu skill = RIMA agent'larının ortak preamble'ı. Jenerik base agent davranışını DEĞİŞTİRMEZ, üstüne proje DNA'sı ekler. Tek yerde tutulur (DRY) — değişiklik buradan tüm agent'lara yayılır.

## ACTIVE RULES (Karpathy 4 — her görev)
1. **Kod yazmadan önce düşün.** Varsayımları listele; belirsizlikte flag at, körü körüne devam etme; çok yorum varsa hepsini sun.
2. **Minimum kod.** Problemi çözen en az kod; spekülatif feature/abstraction yok.
3. **Cerrahi değişiklik.** Sadece görevin gerektirdiği dosyalar. İlgisiz kodu refactor/silme etme; pre-existing dead code'u not düş.
4. **Hedef odaklı.** Doğrulanabilir başarı kriteri; başarısızsa `BLOCKED` yaz, sessizce partial bırakma.

## NLM ACCESS (tasarım bağlamı gerekiyorsa)
```
NB=$(cat .claude/nlm.local 2>/dev/null); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>"
```
Direct-read SADECE: `CURRENT_STATUS.md` · `.claude/PROJECT_RULES.md` · kod (`Assets/Scripts/**`, `Assets/Editor/**`) · `STAGING/**` · memory dosyaları. Tasarım/durum sorusu → NLM. (Notebook ID gizli/gitignored — repo'ya konmaz.)

## UNITY ERROR CHECK (Unity'ye dokunan görevde)
İş bitince `mcp__UnityMCP__read_console` (Error+Warning) çağır:
- Hata **kendi değişikliğinden** → **ÇÖZ** (çözemezsen BLOCKED).
- Hata **önceden var / ilgisiz** → raporda **BİLDİR** (silme/refactor etme).
- Console durumu her zaman raporda; "compile oldu" yetersiz. Aynı anda TEK Unity-süren ajan.

## Anahtar path'ler / giriş noktaları
- `PROJECT_INDEX.md` — tek-ekran giriş haritası · `CODE_MAP.md` — god-node + kod yolları
- `CURRENT_STATUS.md` — anlık iş (RESUME) · `.claude/PROJECT_RULES.md` — tam kurallar
- God-node'lar: `UI/BuildModeController.cs` · `UI/DirectorMode.cs` · `MapDesigner/Room/Runtime/{RoomRunDirector,IsoRoomBuilder}.cs`

## Çıktı ekonomisi
Dosyaya yaz, dönüş ≤10 satır özet + yol. Rapor içeriğini dönüşe gömme (E1). Türkçe rapor = tam Türkçe karakter.
