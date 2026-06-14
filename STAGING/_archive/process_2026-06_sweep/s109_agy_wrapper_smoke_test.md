ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

**RESPOND INLINE ONLY. DO NOT write to any file. Do NOT save to sandbox.**

---

# Amaç

Yeni düzeltilen `agy_dispatch.cmd` wrapper sağlık check'i. Orchestrator (Claude) az önce CRLF + Python ordering fix uyguladı. Sen (agy/Antigravity) bu wrapper üzerinden dispatch edildin — yani fix çalışıyor demek. Şimdi 4 noktayı **inline** doğrula:

## Görev

`F:\Antigravity Projeler\2d roguelite\RIMA\agy_dispatch.cmd` dosyasını oku ve şu 4 maddeyi sırayla raporla:

1. **Line ending:** dosya CRLF mi (`\r\n`) yoksa LF-only mi? (`Get-Content -Raw` ile byte'ları kontrol et — `0x0D 0x0A` ardışık var mı?)
2. **Python search ordering:** `pythonw.exe` arama sırası ne? İlk match'te `goto :found` ile çıkıyor mu? Sıra hangi Python install'ı önceliklendiriyor?
3. **Pywinpty availability:** Seçilecek Python interpreter'da `winpty` modülü kurulu mu? (`python -c "import winpty; print(winpty.__file__)"` denemesi yap, hangisi PASS ediyor?)
4. **Genel sağlık:** Wrapper'da senin görmediğin bir bug/risk var mı? Tek-cümle verdict (PASS / WARN / FAIL + sebep).

## Çıktı formatı

Markdown bullet, max 200 kelime. Yalnızca yukarıdaki 4 madde + sonunda tek satır `VERDICT: PASS|WARN|FAIL — <kısa sebep>`. Spekülasyon yok.
