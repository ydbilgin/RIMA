ACTIVE RULES: (1) think before acting (2) min steps, no speculation (3) surgical — only the MCP calls listed (4) BLOCKED if unclear, report don't guess.

NLM ACCESS: Bu görev için NLM gerekmez (saf runtime doğrulama). Gerekirse: `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<soru>"`. Direct-read sadece: CURRENT_STATUS.md / kod / STAGING.

# Amaç
RIMA demo'sunun "editörsüz dengeliyorum" ayağını **bağımsız tanık olarak** play-mode'da data-proof'la doğrula: canlı `physPower` slider'ı gerçekten hasarı ölçeklendiriyor mu, düşman spawn oluyor mu, telemetry/console temiz mi. Orchestrator (Opus) zaten 1 kez koşturdu (physPower 50→250 ⇒ finalDamage 50→250). Sen BAĞIMSIZ tekrar üretip kendi sayılarını raporla. **Bu görsel/screenshot inceleme DEĞİL — sayısal data-proof.**

## KRİTİK KISITLAR
- **Unity tek socket (port 6402).** Sen koşarken başka kimse Unity'ye dokunmuyor. Seri çalış.
- **finally garantisi:** Hata olsa bile EN SON `manage_editor stop` çağır (Unity play-mode'da kalmasın → sonraki ajan bloklanır).
- **Overlay UI screenshot'ta çıkmaz** — o yüzden data-proof. Screenshot opsiyonel (world-space).

## ADIMLAR (tam bu sırayla, UnityMCP araçları)
1. `set_active_instance` → instance: `RIMA@ed023e0b`
2. `manage_editor` action=`play`
3. Kısa bekle, sonra `execute_code` action=`execute` ile AŞAĞIDAKİ kodu BİREBİR çalıştır:

```csharp
var sb = new System.Text.StringBuilder();
System.Func<string,System.Type> FindT = (n) => System.AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(a => { try { return a.GetTypes(); } catch { return new System.Type[0]; } })
    .FirstOrDefault(t => t.FullName == n || t.Name == n);
if (!Application.isPlaying) return "BLOCKED: not playing yet, retry execute_code";
var dmType = FindT("RIMA.DirectorMode");
var dm = GameObject.FindObjectOfType(dmType, true);
System.Func<string,object[],object> Inv = (name,args) => dmType.GetMethod(name).Invoke(dm, args);
var pcmType = FindT("PlayerClassManager");
var pcm = pcmType.GetProperty("Instance", System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.Static).GetValue(null);
sb.AppendLine("PlayerClassManager.Instance=" + (pcm!=null?"LIVE":"NULL"));
try { Inv("SelectClassForValidation", new object[]{ System.Enum.Parse(FindT("ClassType"), "Warblade") }); sb.AppendLine("class=Warblade"); } catch(System.Exception e){ sb.AppendLine("class EX:"+(e.InnerException?.Message??e.Message)); }
var pktType = FindT("RIMA.Balance.DamagePacket");
System.Func<System.Type,string,object> EnumOr0 = (t,n)=>{ try { return System.Enum.Parse(t,n);} catch { return System.Enum.GetValues(t).GetValue(0);} };
object packet = pktType.GetMethod("Create").Invoke(null, new object[]{ 100, EnumOr0(FindT("DamageType"),"Physical"), EnumOr0(FindT("DamageSourceType"),"BasicAttack"), null, null, "council_e2e", false, 1.5f, EnumOr0(FindT("ElementTag"),"None"), false });
object defender = System.Activator.CreateInstance(FindT("RIMA.Balance.ClassStatRuntime"));
var calcM = FindT("RIMA.Balance.DamageCalculator").GetMethod("Calculate");
var resType = FindT("RIMA.Balance.DamageCalculationResult");
System.Func<float,int> Dmg = (pp)=>{ Inv("SetStatForValidation", new object[]{ "physPower", pp }); object atk = pcmType.GetMethod("EnsureCurrentPrimaryStats").Invoke(pcm,null); object res = calcM.Invoke(null,new object[]{ packet, atk, defender }); return (int)resType.GetField("finalDamage").GetValue(res); };
int lo = Dmg(50f); int hi = Dmg(250f);
sb.AppendLine("[DMG-SCALE] physPower=50 => finalDamage="+lo+" | physPower=250 => finalDamage="+hi+" | SCALES_UP="+(hi>lo)+" delta=+"+(hi-lo));
Inv("SelectFirstSpawnEnemyForValidation", null);
int acc=0; for(int i=0;i<3;i++){ if((bool)Inv("SpawnSelectedEnemyAtForValidation", new object[]{ new Vector2(-2f+i*2f,-1.5f) })) acc++; }
sb.AppendLine("[SPAWN] accepted="+acc+"/3 live="+(int)Inv("DirectorSpawnedEnemyCountForValidation",null));
sb.AppendLine("[TELEMETRY] events="+(int)Inv("TelemetryEventCountForValidation",null)+" dps="+((float)Inv("TelemetryDpsForValidation",null)).ToString("F2"));
return sb.ToString();
```

4. `read_console` action=`get` types=`["error","warning"]` count=`25` → hata/uyarı var mı kaydet
5. **HER DURUMDA** `manage_editor` action=`stop`

## BEKLENEN (orchestrator referansı — sen kendi sayınla karşılaştır)
- `PlayerClassManager.Instance=LIVE`
- `[DMG-SCALE] ... SCALES_UP=True` ve `physPower=250` hasarı `physPower=50`'den BELİRGİN yüksek (referansta 50→250, delta=+200)
- `[SPAWN] accepted=3/3 live=3`
- console: 0 error (0 warning ideal)

## RAPOR FORMATI (CODEX_DONE değil — şu dosyaya yaz: `STAGING/playtest_caps_2026-06-13/council_opus_e2e.md`)
```
# Council E2E — ax Claude Opus 4.6 (bağımsız tanık)
- Instance LIVE: <evet/hayır>
- DMG-SCALE: physPower50=<x> physPower250=<y> SCALES_UP=<bool> delta=<d>
- SPAWN: <acc>/3 live=<n>
- TELEMETRY: events=<e> dps=<v>
- CONSOLE: <0 error / N error: liste>
- play-mode stop edildi: <evet/hayır>
- VERDİKT: <referansla UYUŞTU / SAPMA: ...>
```
Sapma veya hata görürsen **BLOCKED** yaz, tahmin etme.
