# RIMA Class Stat Modeli — Claude Handoff Paketi

Bu paket, RIMA'nın class stat sistemi için ChatGPT karar çıktısını Claude/Codex/Unity tarafında uygulanabilir hale getirmek için hazırlandı.

## Paket amacı

Demo hedefi: **dengeleme altyapısının canlı ayarlanabilir şekilde gösterilmesi**.

Kritik karar:
- `Phys/AP split` korunur.
- Gemini'nin önerdiği tek `damageMult` sadece debug/global override olarak kullanılabilir.
- Production stat omurgası: `maxHP`, `physPower`, `abilityPower`, `attackSpeedMult`, `moveSpeed`.
- UI tarafında class anlatımı 5-bar kalır: Hasar / Dayanıklılık / Hız / Kontrol / Zorluk.

## Dosya haritası

| Dosya | Ne işe yarar? |
|---|---|
| `claude_prompt/00_CLAUDE_PROMPT_COPYPASTE.md` | Claude'a doğrudan yapıştırılacak ana prompt |
| `docs/01_A_DECISION_PHYS_AP_VS_DAMAGE_MULT.md` | A maddesi: Phys/AP split vs tek damageMult karar gerekçesi |
| `docs/02_B_CLASS_NUMERIC_TABLE.md` | B maddesi: 10 class sayısal tablo + gerekçeler |
| `docs/03_C_BALANCE_DEBUG_TOOLS.md` | C maddesi: demo/denge tool önerileri |
| `docs/04_IMPLEMENTATION_BACKLOG.md` | Claude için uygulanabilir görev listesi |
| `docs/05_BALANCE_TELEMETRY_SPEC.md` | Test verisi/log sistemi önerisi |
| `data/class_stats_v01.csv` | 10 class stat tablosu CSV |
| `data/class_stats_v01.json` | Aynı tablo JSON, Unity SO üretimine uygun |
| `data/class_stats_v01.md` | Kısa tablo markdown |
| `code_snippets/*.cs` | Unity için starter C# taslakları |

## Claude'a kullanım önerisi

1. Bu paketi RIMA repo köküne veya ayrı bir çalışma klasörüne koy.
2. `claude_prompt/00_CLAUDE_PROMPT_COPYPASTE.md` içeriğini Claude'a yapıştır.
3. Claude'a önce sadece analiz/plan çıkarttır, sonra implement ettir.
4. İlk implementation hedefi: mevcut sistemi kırmadan `DamagePacket + DamageCalculator + ClassStatProfile` eklemek.
5. Debug panel ve telemetry ikinci adım.

## Tasarım uyarısı

Bu paket animasyon/feel işini sayısal statlarla çözmeye çalışmıyor. Demo için `moveSpeed` ve `attackSpeedMult` placeholder olarak var, ama final class hissi animasyon timing, recovery, hit-stop, dash-cancel ve skill state pencereleriyle verilmeli.
