# COUNCIL — PixelLab weaponless/armed state prompt-craft + eksik asset envanteri (2026-06-16)

ACTIVE RULES: (1) think before answering (2) min — no speculation, cite mechanics (3) surgical — answer only what is asked (4) flag uncertainty explicitly.
NLM ACCESS: RIMA tasarım bağlamı gerekirse: `NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>"`. Direct-read: bu brief + STAGING. (NLM auth expired olabilir → o zaman kod/brief temelli cevap ver, belirt.)
GRAPHIFY: bu görev kod-grafı gerektirmez (asset/prompt-craft analizi).

> **Bağlam:** RIMA = 2D top-down ARPG, demo 19 Haziran. Karakter sprite'ları **PixelLab MCP** ile üretiliyor (create_character / create_character_state / animate_character mode=v3). Stil: high top-down 3/4, 120x120 canvas, 8 yön (5 üret + 3 mirror flipX), PPU64, 10-12fps. Kullanıcı PixelLab'i KENDİ koşturuyor; orchestrator promptları yazıyor + state'leri MCP'den üretti.
> **Silah kararı (önceki council, LOCKED):** Demo = silah animasyona GÖMÜLÜ bake (sprite-sheet'te bone/per-frame el-pivot YOK). Silahsız + runtime weapon-mount = POST-DEMO.

---

## SORUN — gözlem
warblade için 4 "state" üretildi (mid-run / strike-windup / breathing-idle / flinch). Bunların animasyonda silahın **yön ve state'ler arası tutarsız** (her frame/yön farklı kılıç geometrisi) çıkma riski var. Ayrıca kullanıcı POST-DEMO weapon-mount pipeline'ı için **silahsız** (eli boş, silah sonradan monte edilecek) state üretmeyi de istiyor ama nasıl prompt yazacağını bilmiyor.

## KANIT — gerçekte kullanılan state promptları (`create_character_state`, base warblade char `2656075d-d113-4f18-a6c1-94b5a6b8bf65`)
```
mid-run:        "same warblade, high top-down 2D game sprite, mid-run pose, forward lean, one foot planted one foot trailing, SWORD kept readable at side, preserve armor and silhouette, no VFX, no redesign"
strike-windup:  "same warblade, high top-down 2D game sprite, sword strike windup pose, BLADE pulled back before a heavy slash, feet braced, torso twisted, preserve armor and WEAPON IDENTITY, no VFX, no redesign"
breathing-idle: "same warblade, high top-down 2D game sprite, guarded breathing idle stance, SWORD ready but relaxed, weight balanced, preserve armor and silhouette, no VFX, no redesign"
flinch:         "same warblade, high top-down 2D game sprite, hit reaction flinch pose, upper body recoiling, guard broken, SWORD still in hand, preserve armor and silhouette, no VFX, no redesign"
```
(animate_character action metinleri de "sword steady / stable sword silhouette / basic sword slash" diyor.)

## ORCHESTRATOR HİPOTEZİ (adversarial inceleyin — neyi yanlış kurdum?)
1. **Kök neden:** Her state promptu silahı İSMEN ("sword", "blade", "weapon identity") çağırıyor AMA hepsi base karakterden BAĞIMSIZ üretiliyor (ortak armed-anchor yok) → PixelLab her state için kendi kılıç geometrisini sentezliyor → state/yön arası tutarsız silah.
2. **Silahsız yapmak için:** silahı isimlendiren her kelimeyi (sword/blade/slash/wield) çıkar; bunun yerine **uzuv mekaniğini** tarif et ("right arm drawn back across the body, fist clenched as if gripping a two-handed haft, hands empty") + açık negatif ("no weapon, empty hands, nothing in hands"). Yani "kılıcını savuruyor" DEĞİL, "kolunu şöyle savuracak şekilde geri çekiyor, eli kavrar pozisyonda".
3. **Tutarlı-silahlı yapmak için (demo bake):** önce tek **armed anchor** state üret (silah net), sonra pose-state'leri O anchor'dan türet (palette-snap=TRUE) → aynı kılıç propagate olur.

---

## İSTENEN ÇIKTILAR (3 advisor — cx kod/MCP-aware, ax Pro derin/tasarım, ax Flash yalın)

### Q1 — Kök neden doğru mu? (adversarial)
Hipotez 1'i doğrula/çürüt. PixelLab `create_character_state` mekaniği: bağımsız state'lerde silah ismi geçince model tutarsız mı üretir? `reference_image` / init-image / palette-snap / AI-freedom parametreleri tutarlılığı nasıl etkiler? Tutarlılığı garantilemenin DOĞRU yolu nedir?

### Q2 — KALICI PROMPT-CRAFT BİLGİSİ (asıl teslimat — gelecek oyunlarda da kullanılacak)
İki reçete, **kopyala-yapıştır prompt şablonu + DO/DON'T kelime listesi** ile:
- **(A) SİLAHSIZ state** (weapon-mount pipeline için): eli boş, uzuv-mekaniği-odaklı tarif. "kılıcı savuruyor" yerine "kolu/eli şöyle hareket ediyor" formülasyonu. Hangi kelimeler silah çağırır (yasak liste), hangi ifadeler boş-el garantiler, negatif-prompt nasıl yazılır.
- **(B) TUTARLI-SİLAHLI state seti** (demo bake): armed-anchor-first pipeline; palette-snap/init-image ile silah propagasyonu; state'ler arası geometri kilidi.
- Her iki reçetede: high-top-down 3/4 + 8-yön tutarlılığı + "no redesign/preserve silhouette" disiplini nasıl korunur.
- **Genellenebilir kural seti:** "PixelLab state üretiminde X istiyorsan prompt'ta Y yap/yapma" — gelecek projeler için ~8-12 maddelik prompt-craft kuralı.

### Q3 — EKSİK / YAPILMASI GEREKEN ASSET ENVANTERİ (demo 19 Haz, 3 gün)
Demo golden-path + sunum için hâlâ EKSİK olan görsel/asset'leri tespit + önceliklendir. Bilinen adaylar (doğrula + ekle/çıkar):
- warblade armed-anchor REDO (tutarlı kılıç) + idle/run/LMB animasyonları (P1 demo)
- Elementalist'in dümdüz sarı-top mermisini değiştiren **energy bolt** core sprite
- VFX core seti (fireball / fire-impact / glacial-spike / frozen-orb / light-beam) — demo'da gerekli mi yoksa post-demo mu?
- run-map sembolleri / chrome (StS-tarzı node haritası) — demo'da görünüyor mu?
- alt portal bar + mavi-ışın beam (T5) görseli
- büyük cliff mapler için tile/backdrop
Her madde için: **demo-kritik mi / post-demo mu**, **kim yapmalı (kullanıcı-PixelLab / orchestrator-MCP / Unity)**, **3 günde gerçekçi mi**, **golden-path videosunda görünüyor mu**. Net öncelik sırası ver.

---
## ÇIKTI FORMATI
Yanıtını şu dosyaya yaz: `STAGING/_process/2026-06/pixellab_anim_council/RESP_<advisor>.md` (advisor = cx / axpro / axflash). Q1/Q2/Q3 başlıklı. Q2 prompt şablonları **kopyala-yapıştır hazır** olsun. Türkçe açıklama + İngilizce promptlar. Spekülasyon yapma; emin değilsen "BELİRSİZ" yaz.
```
