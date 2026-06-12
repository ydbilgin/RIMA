ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Görev: Attunement Chamber'i "gercek chamber" yap — 10 class crescent yay + merkez dummy (VERIFY-FIRST)

## Dosya
`Assets/Scripts/UI/ChamberSelectBootstrap.cs` (TEK ana dosya). Unity MCP acik (RIMA instance), _Arena yuklu.
Chamber = CharacterSelect sahnesi; oyun MainMenu -> CharacterSelect ile giriliyor.

## Mevcut durum (okudum, satir referanslari)
- Satir 56-59: `ChamberClasses` = sadece { Warblade, Elementalist } → o yuzden sadece 2 figur cikiyor.
- Tum class roster (10): Warblade, Elementalist, Shadowblade, Ranger, Ravager, Ronin, Gunslinger, Brawler, Summoner, Hexer (ClassType enum, Skills/SkillData.cs).
- Kilit: `ClassUnlockPolicy.IsUnlocked` → WB+EL hep acik, digerleri PlayerPrefs ile (default KILITLI). Zaten dogru.
- `GenerateChamberLayout` (567-648): su an 2 DUZ KOLON (sol/sag, StandRowCount=5). Bunu CRESCENT YAY yapacaksin.
- `SpawnEchoStations` (1034-): ChamberClasses uzerinden figur+pedestal kuruyor.
- Dummy: `chamberDummyCell` + `SpawnTrainingDummy` (1110-), su an on-sol koseye yerlesiyor. MERKEZE alacaksin.
- Update() locked-class dali (289-312): kilitli class Echo ile acilabiliyor.
- Kamera fit: satir 40-46 (chamberCameraOrthoSize=5.0 vb).

## Yapilacaklar

### 1) Tum class'lari goster (WB+EL acik, diger 8 GORSEL-KILITLI)
- `ChamberClasses` = 10 class'in TAMAMI (None haric, enum sirasi): Warblade, Elementalist, Shadowblade,
  Ranger, Ravager, Ronin, Gunslinger, Brawler, Summoner, Hexer.
- **DEMO-SAFE KILIT:** WB ve EL DISINDAKI class'lar chamber'da ACILAMAZ/BURUNULEMEZ olmali (Echo ile bile).
  Sebep: kit-less class burunulurse BOS skill-bar = demo kirilir. Update() locked-class dalinda: class
  WB/EL degilse, prompt'u "{CLASS} — Kilitli" (unaffordable stil) goster ve [G]-unlock aksiyonunu ISLEME
  (Unlock cagirma). WB/EL zaten unlocked → "Burun" akisi aynen calisir. Bu kuralı tek noktada uygula
  (orn. private bool IsDemoSelectable(cls) => cls==Warblade||cls==Elementalist;).

### 2) Figurleri CRESCENT YAY diziliminle yerlestir (GenerateChamberLayout rewrite)
- 10 figur, oyuncuya/ona DOGRU acilan bir yarim-ay (semicircle) yay uzerinde, GENIS yayilmis.
- Algoritma (oneri): floor bounds (TryGetFloorBounds) al → merkez axisX=(minX+maxX)/2, arkaY ~ maxY-3.
  Yay merkezi cellC = (axisX, arkaY civari, biraz geri). 10 figur icin i=0..9:
    angle = Lerp(startDeg, endDeg, i/9)  // orn. startDeg=200, endDeg=340 (alt yariya, öne acik)
    rx = yatay yaricap (floor genisliginin ~%40'i), ry = dikey yaricap (rx*0.55, iso squash)
    target = (axisX + round(rx*cos(angle)), arkaY + round(ry*sin(angle)))
    cell = PickNearestWalkableSameRowY/PickNearestWalkableCell ile en yakin yurunebilir hucre, reserved'a ekle.
  Sonuc: alttan-bakan genis bir yay. Soldan saga sirali, ust uste binmeyen.
- Figurler oyuncuya bakar (mevcut idle_south sprite kullanimi aynen).
- Yay floor'a sigmazsa: yaricapi floor bounds'a clamp et. Floor cok darsa (10 figur sigmiyorsa) Chamber
  room template'ini (`Assets/Resources/ChamberSelect/Chamber_CharSelect.asset`) GENISLET — ama once mevcut
  floor'la dene; genisletme gerekiyorsa minimal yap ve raporla.

### 3) Dummy = yayin MERKEZINDE
- `chamberDummyCell` = yayin focal merkezi (axisX, yay merkezinin biraz onu — figurlerin ortasinda, onlerinde).
  Oyuncu spawn'i dummy'nin biraz onunde/altinda kalsin (figurleri ve dummy'yi karsidan gorsun).
- Dummy mevcut immortal/hover/HP-bar davranisi AYNEN korunur (SpawnTrainingDummy degismez, sadece hucre).

### 4) Genis chamber + kamera
- Yay genis oldugu icin kamera tum yayi gostermeli. chamberCameraOrthoSize / fit-multiplier'i yayin
  genisligine gore ayarla (gerekirse buyut) ki 10 figur + dummy + portal kadraja girsin. Asiri zoom-out yapma.

## VERIFY-FIRST (Unity MCP — gorsel kanit)
- CharacterSelect sahnesini yukle (manage_scene load) + play mode → screenshot al.
- KONTROL: 10 figur crescent yayda mi · dummy merkezde mi · WB+EL "Burun" / digerleri "Kilitli" mi ·
  figurler ust-uste binmiyor mu · kadraj tam mi · siyah/bozuk yok mu.
- Screenshot'lari Assets/Screenshots/ altina chamber_crescent_01.. olarak kaydet.
- Sorun varsa (figur void'e dusuyor, kadraj kesik, overlap) duzelt + tekrar screenshot. Cozulmezse BLOCKED + kanit.

## KISITLAR
- Demo akis mantigina (StartRun/AttuneRoutine WB/EL yolu) DOKUNMA.
- Karakter/mob materyallerine dokunma (unlit kalsin).
- ChamberClasses disinda baska sistemleri refactor etme. Sadece bu gorevin gerektirdigi degisiklikler.
- Compile 0-error. Mevcut testlerde 0 yeni fail (once/sonra say). Commit ETME. Hicbir sey SILME.
- Is bitince Unity'yi play'den cikar (stop), idle birak.

## Cikti (CODEX_DONE'a)
- ChamberClasses 10'a acildi (demo-lock IsDemoSelectable ile WB/EL disi acilamaz).
- Crescent yay + merkez dummy + kamera fit yapildi; algoritma ozeti.
- Floor genisletildi mi (evet/hayir + ne).
- Screenshot yollari + her birinde gorsel degerlendirme.
- Compile/test durumu (once/sonra fail sayisi). BLOCKED ise sebep+kanit.
