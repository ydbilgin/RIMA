ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
RIMA oda tasarımını "güzel + işlevsel" yapmak için FEASIBILITY/REUSE merceği. ANALYSIS ONLY, kod DEĞİŞTİRME. Sonucu CODEX_DONE.md'ye yaz.

# Bağlam
Kullanıcı odaların referans-proje kalitesinde "güzel tasarlanmış" olmasını istiyor (küçük çıplak elmas-platform DEĞİL). Karar verildi: ISO yok, Wang yok, ışık+dekor+derinlik (bkz `STAGING/TILEMAP_VISUAL_QUALITY_DECISION_2026-06-11.md`). Prop boyut katmanları locked (T1 32px/T2 64px/T3 128px/T4 landmark — `STAGING/PROPS_DOORS_PLACEMENT_PLAN_2026-06-11.md`). Dekorasyon-pass YENİ kuruldu (RoomDecorationPass.cs, flag default-OFF, working tree'de uncommitted). Karakter referansı = 64px.

# SORULAR (feasibility/reuse, file:line kanıtlı)
1. **MAP/ODA BOYUTU:** DemoRoomBank + `Assets/Data/Rooms/Generated/*` içindeki oda template'lerini + boyutlarını (w×h tile) LİSTELE. Oda-tipi başına (Combat/Elite/Boss/Shop/Chamber) hangi boyutlar var, demo hangilerini kullanıyor? En küçük/en büyük? Boyut RoomTemplateSO.bounds'tan mı geliyor, değiştirmek ne kadar iş?
2. **MANTIKSAL PROP YERLEŞİMİ:** RoomDecorationPass + CompositionRoleMap(Generator) + BridsonPoissonAutoPlacer + density tabloları nasıl çalışıyor? Zone'lar (CleanCenter/DecoratedEdge/DoorSafety/FocalCluster/WallBand) prop yerleşimini nasıl yönlendiriyor? "Rastgele saçılmış değil, tasarlanmış" hissi için hangi config/parametre var, ne eksik (focal-point seçimi, oda-kimliği landmark, asimetri)?
3. **ROOM DESIGNER İŞLEVSELLİĞİ:** RIMA'daki oda-authoring tool'larını AUDIT et — Map Designer pencereleri, RoomTemplateBrowserWindow, PropsTab, "Rooms" sekmesi, RoomBrowser, RoomJsonImporter. Her biri NE yapıyor, ÇALIŞIYOR mu yoksa yarım/bozuk mu? Bir tasarımcı uçtan uca güzel oda yapabiliyor mu (floor boya → prop yerleştir → kapı ayarla → önizle)? En büyük işlevsellik boşluğu ne?
4. **EDİTÖR IŞIK YÖNETİMİ:** _Arena + sahnelerde ışık şu an nasıl kurulu (Global Light2D, point light, Light2D)? Işıklar hiyerarşide nasıl organize? Sahne view'da gizlenebilir/parent altında toplanabilir mi? Per-oda veya per-environment ışık authoring yolu var mı, yoksa hardcoded mı?
5. **FEASIBILITY:** (a) odaları büyütmek, (b) room designer'ı işlevsel kılmak, (c) authorable ışık — her birine kaba efor (S/M/L) + en düşük-riskli demo-safe yol.

ÇIKTI: file:line kanıtlı. Kilit-ihlali [LOCK-RİSK] işaretle. Sonucu CODEX_DONE.md'ye yaz.
