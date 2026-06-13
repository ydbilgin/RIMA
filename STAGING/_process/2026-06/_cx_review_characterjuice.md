ACTIVE RULES: (1) think before judging (2) evidence-based — diff'i kendin oku (3) surgical — SADECE git diff/dosya oku, HİÇBİR ŞEY değiştirme (4) emin değilsen UNCERTAIN.
NLM ACCESS: Gerekmez.

# Amaç — CharacterJuice REVIEW (writer=Claude Opus sub-agent; sen bağımsız reviewer'sın)
Yeni `Assets/Scripts/VFX/CharacterJuice.cs` + iki Warblade prefab wiring'i. Writer raporu: `STAGING/_process/2026-06/_opus_characterjuice_2026-06-13.md` (önce oku). BUGÜN CANLI DEMO — kriter: "demoda görünür kusur çıkarır mı?"

## Doğrula (git diff, uncommitted):
1. **Pixel-snap bob:** localPosition.y offset'i gerçekten 1/64 katlarına snap'leniyor mu (PPU 64)? Scale animasyonu kullanılmamış mı?
2. **Root dokunulmazlığı:** SADECE Body (+HandAnchor aynı offset) oynuyor; root/collider/hitbox pozisyonu etkilenmiyor mu? HandAnchor offset'i silahın el hizasını koruyor mu (tilt Body'de ama silah tilt'lenmiyor — görsel kopukluk yaratır mı, değerlendir)?
3. **Guard'lar:** Health.IsDead'de durma · Time.timeScale==0'da donma (unscaled time YOK) · enableJuice toggle · null-guard'lar (PlayerController/PlayerAttack yokken NRE yok).
4. **Event hijyeni:** OnComboStep aboneliği OnEnable/OnDisable simetrik mi (leak yok)?
5. **🔑 ANA SORU — IsoSorter etkileşimi:** IsoSorter sort order'ı world-Y×100'den hesaplıyor; 1px bob (≈0.0156 birim) order'ı ±1-2 oynatabilir → oyuncuyla aynı Y bandındaki düşman/prop'ta SIRALAMA TİTREMESİ riski. IsoSorter'ın hangi transform'un Y'sini okuduğuna bak (Body mi root mu): (a) root okuyorsa risk YOK de; (b) Body okuyorsa riski değerlendir ve MİNİMAL fix öner (örn. IsoSorter'a root-Y kullandırma VEYA bob'u sort hesabından düşme) — fix'i SEN YAPMA, sadece öner.
6. Prefab wiring: iki Warblade prefab'ında ×1 component, Player.prefab'da YOK (writer çift-ekleme yaşayıp düzeltmiş — doğrula), mob'larda YOK.

## RAPOR → `CODEX_DONE_<kendi profilin>.md`
Madde başına PASS/FAIL/UNCERTAIN + kanıt (satır no). Sonda: GENEL VERDİKT + commit'e uygun mu + (5) için net öneri.
