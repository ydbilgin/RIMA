# RIMA Teslim Fix Paketi

Bu paket, RIMA'nın mevcut teslim durumunu ve teslim öncesi çözülmesi gereken P0/P1 işleri pratik şekilde toparlar.

Odak:
- Full game değil.
- Bitirme projesi için oynanabilir vertical slice.
- Şu an hedef: 5-node demo akışı + boss + dual-class unlock gösterimi.
- Öncelik: canlı demo sırasında göze çarpacak/akışı kıracak bug'ları temizlemek.

## Paket İçeriği

1. `00_NEREDEYIZ_NE_YAPMALIYIZ.md`
   - RIMA şu an ne durumda?
   - Teslim için hangi hedef doğru?
   - Öncelik sırası ne?

2. `01_P0_WARBLADE_SWORD_RENDER_ATTACH_FIX.md`
   - Greatsword neden zeminin altında kalıyor olabilir?
   - `HandAnchorAttach` ve `OrientationSync` için minimal fix.
   - Ele oturmama/hand offset problemi.

3. `02_P0_DUAL_CLASS_DEMO_FLOW_FIX.md`
   - Dual-class kodda var ama demo akışında gerçekten erişilebilir mi?
   - Boss ölümü sonrası minimum bağlama planı.
   - Post-boss playable room önerisi.

4. `03_PATCH_TASLAKLARI.md`
   - Kopyalanabilir C# patch taslakları.
   - Birebir kör patch değil; Claude/Codex gerçek dosyaya göre uyarlamalı.

5. `04_MANUEL_TEST_CHECKLIST.md`
   - Unity Editor'da teslim öncesi test senaryoları.
   - "Geçti/Kaldı" şeklinde kontrol listesi.

6. `05_CLAUDE_CODE_TEK_PROMPT.md`
   - Claude Code / Codex / Antigravity içine tek parça atılacak görev promptu.

7. `06_HOCAYA_GUNCEL_KAPSAM_METNI.md`
   - Hocaya/rapora konabilecek güncel ve gerçekçi kapsam metni.

8. `07_BACKLOG_ONCELIKLER.md`
   - P0/P1/P2 backlog.
   - Teslimden önce ne yapılır, ne yapılmaz?

## En Kısa Özet

Şu an RIMA için en doğru teslim stratejisi:

```text
Combat → Combat → Merchant → Combat → Boss → Secondary Class Selection → Unlock Draft → kısa post-boss test alanı / oda
```

Post-boss oda yetişmezse bile minimum:

```text
Combat → Combat → Merchant → Combat → Boss → Secondary Class Selection → Unlock Draft → Demo Complete
```

Ama sadece boss ölür ölmez “Demo Complete” çıkıyorsa dual-class sistemi pratikte demo edilmiş sayılmaz.
