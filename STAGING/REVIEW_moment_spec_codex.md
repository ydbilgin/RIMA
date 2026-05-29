ACTIVE RULES: (1) think before answering (2) concrete file:line (3) flag wrong claims/missed deps (4) UNSURE if can't verify.

# GÖREV — Master moment-to-moment spec FINAL review (teknik/feasibility)
`STAGING/MOMENT_SPEC_S6.md` = 2 workflow (UX+gameplay) + cx + agy girdilerinin Opus-sentezi. Sen YAZMIYORSUN — son teknik review.

Doğrula/eleştir:
1. **"ZATEN ÇALIŞIYOR" listesi doğru mu?** DamageNumberDriver/HitPauseDriver/ScreenShake/RageSystem gerçekten sahnede wired + çalışıyor mu (redo etmeyelim)?
2. **Sıralı spec (rank 1-9) feasibility + bağımlılık:** rank sırası doğru mu? rank4 SkillBar gerçekten rank1+2'ye bağlı mı? Gizli-pahalı veya yanlış-sıralı var mı?
3. **3 bug DOĞRU mu?** (a) MapFragment namespace çakışması (Core vs Environment — fragment gate'i açmıyor) gerçek mi, dosya:satır? (b) boss-death→class-select Victory ile çakışması gerçek mi? (c) duplicate Systems GO?
4. **Kaçırılan** kritik UX/gameplay-beat veya bug var mı?

# ÇIKTI → CODEX_DONE_yekta.md
STATUS: COMPLETED — "zaten çalışıyor" teyit/düzelt + rank-sıra AGREE/DISAGREE + 3-bug doğrulama (dosya:satır) + kaçırılanlar.
