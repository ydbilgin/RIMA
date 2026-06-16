# LIVE-TEST RECIPE — beat: panels-quality

Odak: Pause / Settings / Skill Codex / HUD panelleri ACILIYOR + RENDER oluyor + "guzel mi"
(layout / okunabilirlik / hiyerarsi). Overlay UI screenshot'a CIKMAZ -> data-proof zorunlu.

> ⚠️ Bu beat'in dogasi: panellerin GORUNUR olmasi alpha/blocksRaycasts ile kontrol edilir,
> SetActive ile DEGIL. Bir panel canvas'i sahnede HEP aktif durur (DontDestroyOnLoad);
> "acik mi?" sorusunun gercek cevabi = CanvasGroup.alpha == 1 && blocksRaycasts == true.
> activeInHierarchy tek basina YANILTIR (panel kapali iken de true) -> alpha'yi MUTLAKA oku.

---

## MIMARI GERCEKLER (koddan, varsayim degil)
- PauseMenuUI  : canvas GO adi `[PauseMenuUI]`,  sortingOrder 1090, gorunurluk=CanvasGroup.
- SkillCodexUI : canvas GO adi `[SkillCodexUI]`, sortingOrder 1095, gorunurluk=CanvasGroup.
- SettingsMenuUI: canvas GO adi `[SettingsMenuUI]`, sortingOrder 1100, gorunurluk=CanvasGroup.
- HUD: HUDController bir Canvas'ta (ScreenSpaceOverlay), runtime cocuklar: HPFill/HPLabel/ResFill/EchoBalance/InteractionPrompt.
- ESC = GERCEK input: UIManager.escAction `<Keyboard>/escape` -> OnEsc -> hicbiri acik degilse OpenPause().
- Settings ve Skill Codex'in DOGRUDAN HOTKEY'i YOK. Sadece Pause panelindeki butonlardan acilir
  (Btn_SETTINGS -> UIManager.OpenSettings ; Btn_SKILL CODEX -> UIManager.OpenSkillCodex).
- Pause acilinca pause panelini gizleyip ustune Settings/Codex biner (pauseOpen=true kalir).

---

## LAUNCH (dev-direct _Arena)
1. Sahne: `Assets/Scenes/_Arena.unity` ac (manage_scene load).
2. Play'e bas (manage_editor enter playmode). MainMenu DEGIL -> dev-direct akis.
3. ~1-2 sn bekle: HUD + player spawn olsun (HUDController LateBind ~2sn retry'li).
4. read_console -> 0 error sart (panel build hatasi varsa burada cikar).

---

## ADIMLAR (gercek input, BYPASS YOK)

### A) HUD (oyun ekraninda surekli)
- Ekstra aksiyon yok; play'e girince HUD zaten kurulur. Player'a bind olmasini bekle.

### B) Pause panel
- GERCEK tus: ESC bas (Keyboard escape, OnEsc tetikler). Mock/UIManager.OpenPause cagirma.
  (execute_code ile dogrudan OpenPause() = BYPASS; sadece son care ve ACIKCA isaretle.)
- Pause panel gorunmeli: PAUSED basligi + 5 buton (RESUME/SETTINGS/SKILL CODEX/MAIN MENU/QUIT).

### C) Settings panel
- Pause acikken `Btn_SETTINGS` butonuna TIKLA (gercek pointer click; EventSystem uzerinden).
  Pointer simule edilemiyorsa: UIManager.OpenSettings() cagrisi = YARI-BYPASS (input katmani atlanir)
  -> ACIKCA "input bypass: panel-render dogrulandi, click yolu DOGRULANMADI" yaz.
- Settings gorunmeli; Pause panel gizlenmeli (alpha 0), Settings ustte (sortingOrder 1100).

### D) Skill Codex panel
- ESC ile tekrar Pause ac -> `Btn_SKILL CODEX` butonuna TIKLA.
- Codex gorunmeli: baslik + 5 sutunlu sinif secici (10 sinif) + secili sinifin skill listesi.
- Bir sinif butonuna tikla -> liste degismeli (selectedClass + RefreshSkillList).

### E) Geri/cikis
- ESC -> Settings/Codex kapanir, Pause geri gelir. ESC tekrar -> Pause kapanir (ClosePause).

---

## BEKLENEN (somut)
- HUD: HPFill/HPLabel/ResFill/EchoBalance var ve aktif; HPLabel gercek HP gosterir (bos/0 degil).
- Pause: `[PauseMenuUI]` alpha=1, blocksRaycasts=true; Panel altinda Title("PAUSED")+5 Btn_*.
- Settings: `[SettingsMenuUI]` alpha=1; Panel(400x640) icinde section header'lar (Gameplay/Accessibility/Audio/Controls),
  3 Slider, toggle row'lar, bind row'lar. Pause alpha=0.
- Codex: `[SkillCodexUI]` alpha=1; ClassSelector 10 buton, secili sinif skill row'lari Content altinda.
- Aciklik: panel iceriginin panel rect'i icinde kalmasi (tasma yok), text bos degil.

---

## DATA-PROOF (execute_code — overlay UI icin ZORUNLU)
Her panel icin: bul -> alpha/blocksRaycasts oku -> child sayisi + onemli rect boyutu + ornek text oku.

```csharp
// 1) PANEL ACIKLIK + YAPISI (Pause ornegi; Settings/Codex icin GO adini degistir)
var go = GameObject.Find("[PauseMenuUI]");          // veya [SettingsMenuUI] / [SkillCodexUI]
var cg = go.GetComponent<CanvasGroup>();
Debug.Log($"[PROOF] active={go.activeInHierarchy} alpha={cg.alpha} blocks={cg.blocksRaycasts} interact={cg.interactable}");
// alpha==1 && blocksRaycasts==true  => GERCEKTEN acik (activeInHierarchy yetmez)

// 2) PANEL ICERIK SAYIMI + RECT (Pause)
var panel = go.transform.Find("Panel") ?? go.transform.GetChild(0).Find("Panel"); // Pause: root/Panel
int btnCount = 0; foreach (Transform t in panel) if (t.name.StartsWith("Btn_")) btnCount++;
var prt = panel.GetComponent<RectTransform>();
Debug.Log($"[PROOF] Pause Panel childCount={panel.childCount} btnCount={btnCount} rect={prt.rect.width}x{prt.rect.height}"); // beklenen ~356x366, btnCount=5
var title = panel.Find("Title")?.GetComponent<TMPro.TextMeshProUGUI>();
Debug.Log($"[PROOF] Pause Title='{title?.text}'"); // beklenen 'PAUSED'

// 3) SETTINGS: section + slider + toggle sayimi (Panel overlay altinda)
var s = GameObject.Find("[SettingsMenuUI]");
var sPanel = s.transform.Find("Overlay/Panel");
int headers=0, sliders=0, toggles=0, binds=0;
foreach (Transform t in sPanel) {
  if (t.name.StartsWith("Header_")) headers++;
  else if (t.name.StartsWith("Slider_")) sliders++;
  else if (t.name.StartsWith("Toggle_")) toggles++;
  else if (t.name.StartsWith("BindRow_")) binds++;
}
var sRt = sPanel.GetComponent<RectTransform>();
Debug.Log($"[PROOF] Settings headers={headers} sliders={sliders} toggles={toggles} binds={binds} rect={sRt.rect.width}x{sRt.rect.height}");
// beklenen: headers>=4, sliders==3, rect ~400x640

// 4) CODEX: sinif butonu + skill row sayimi
var c = GameObject.Find("[SkillCodexUI]");
var sel = c.GetComponentInChildren<UnityEngine.UI.GridLayoutGroup>(true);   // ClassSelector
int classBtns = sel != null ? sel.transform.childCount : -1;
var content = FindDeep(c.transform, "Content");                              // skill listesi root
int rows=0; if (content!=null) foreach (Transform t in content) if (t.name.StartsWith("Skill_")) rows++;
Debug.Log($"[PROOF] Codex classBtns={classBtns} skillRows={rows}"); // beklenen classBtns==10, rows>0

// 5) HUD: kritik elemanlar var mi + HP text dolu mu
var hpLabel = FindDeep(null, "HPLabel")?.GetComponent<TMPro.TextMeshProUGUI>();
var resFill = FindDeep(null, "ResFill");
var echo    = FindDeep(null, "EchoBalance")?.GetComponent<TMPro.TextMeshProUGUI>();
Debug.Log($"[PROOF] HUD HPLabel='{hpLabel?.text}' ResFill={(resFill!=null)} Echo='{echo?.text}'");
// HPLabel bos/'0' DEGIL; resFill!=null
```
Not: `FindDeep` = sahnede ada gore derin arama helper'i (yoksa Resources.FindObjectsOfTypeAll<RectTransform>()
icinde t.name eslesmesi ile yaz). HUD canvas root adi degisken oldugu icin cocuk adindan (HPLabel/ResFill) ara.

---

## SCREENSHOT
- DUNYA (arena zemin + player): scene_view capture YAP (Game-view ScreenCapture eski patliyordu; 9.7.3 fix'i denenebilir).
- PANELLER (Pause/Settings/Codex/HUD): ScreenSpaceOverlay -> screenshot'a CIKMAZ.
  -> Bu panellerin gorsel kaniti = DATA-PROOF log'lari. "Screenshot yok -> data-proof esas" diye yaz.
  -> Estetik gozle bakilmasi gerekiyorsa: KULLANICI editor'de bizzat bakar (jury provasinda);
     bunu PASS sayma, "kullanici-gorsel-teyit beklemede" diye ayri isaretle.

## GORSEL-KALITE NASIL DEGERLENDIRILIR ("guzel mi") — proxy metrikler
Overlay screenshot olmadigindan kaliteyi DOLAYLI olcuyoruz:
1. Tasma yok: her child rect'i parent panel rect'i icinde (asagidaki kontrol) -> layout saglikli.
2. Okunabilirlik: TMP text'ler bos degil + fontSize>=9 (kodda min 9-28 arasi) + font asset null degil (garbled = Jersey10 bozuk).
3. Hiyerarsi: Pause btnCount==5; Settings sliders==3 & headers>=4; Codex classBtns==10 -> beklenen kompozisyon.
4. Z-sira: Settings(1100) > Codex(1095) > Pause(1090) -> ust panel altakini ortmeli (alpha kontrolu ile dogrula).
```csharp
// Tasma kontrolu ornegi (panel rect'i world-corner ile child'i kapsiyor mu)
// Basit proxy: child.anchoredPosition + sizeDelta panel sinirlari icinde mi (kabaca).
```
Garbled-text checki: bir TMP.text dolu AMA ekranda bozuk gorunuyorsa -> font asset alt-asset/atlas yok
(Jersey10 known issue). Bu beat'te font asset'in null/eksik OLMADIGINI da logla.

---

## PASS / FAIL (olculebilir)
PASS hepsi saglanirsa:
- [P1] Pause: ESC sonrasi `[PauseMenuUI]` alpha==1 && blocks==true; btnCount==5; Title=='PAUSED'.
- [P2] Settings: gercek-click (veya isaretli input-bypass) sonrasi alpha==1; sliders==3; headers>=4; rect~400x640.
- [P3] Codex: alpha==1; classBtns==10; secili sinif rows>0; sinif degisince rows degisir.
- [P4] HUD: HPLabel dolu (bos/null degil); ResFill var; EchoBalance var.
- [P5] Tasma yok + TMP text'ler dolu + font asset null degil (okunabilirlik).
- [P6] Z-sira: Settings/Codex acikken Pause alpha==0 (ust panel altakini gizler).
FAIL: yukaridakilerden biri saglanmazsa, ya da read_console'da panel build hatasi varsa.

---

## BYPASS-TUZAKLARI (REWARD-02 dersi: GREEN maskeli olabilir)
1. ⚠️ activeInHierarchy==true'yu "acik" sayma. Panel KAPALI iken de aktiftir (sadece alpha=0).
   Gercek olcut = CanvasGroup.alpha==1 && blocksRaycasts==true. Sadece activeInHierarchy bakmak = YANLIS-GREEN.
2. ⚠️ UIManager.OpenPause()/OpenSettings()/OpenSkillCodex() execute_code ile DOGRUDAN cagirmak input
   katmanini (ESC tus + buton click + EventSystem raycast) atlar. Bu, REWARD-02'deki ForceCollect tuzaginin
   aynisi: "panel acildi GREEN" der ama tus/click yolu kirik olabilir. GERCEK ESC + gercek button click kullan;
   simule edemedigin yeri "input-bypass: render OK, input-path DOGRULANMADI" diye ACIKCA isaretle, PASS sayma.
3. ⚠️ "Panel var" != "panel guzel/dolu". childCount>0 ama text'ler bos / font bozuk / icerik panel disina tasmis olabilir.
   Mutlaka ornek TMP.text oku + font asset null kontrol + rect tasma proxy'si.
4. ⚠️ Codex skill listesi BOS gelebilir (SkillDatabase build edilmemis) ama panel acik gorunur -> rows>0 sart.
   rows==0 ise "DB bos" FAIL'i, panel-acik GREEN'i ile karistirma.
5. ⚠️ Eski/stale instance: birden fazla `[PauseMenuUI]` (DontDestroyOnLoad sahne gecisi) -> Find ilkini alir.
   Sahne dev-direct girildigi icin tek olmali; yine de GameObject sayisini logla (>1 ise temizlik gerek).
6. ⚠️ Screenshot'ta panel gormeyince "render olmamis" deme (overlay zaten cikmaz) -> data-proof esas.
