ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
`cx_dispatch.py`'ye profil enable/disable/list yonetimi + gorunur --help ekle. Boylece kullanici bir profili (orn. yekta) KAYITLI tutup dispatch'ten ATLATABILIR (passive/ignore), ve secenekleri --help'te gorebilir. SURGICAL: sadece `cx_dispatch.py` (+ gerekiyorsa `cx_profiles.local.json`). Mevcut davranisi BOZMA.

# Baglam (mevcut kod)
- `cx_dispatch.py` argparse kullaniyor (`--task-file`, `--effort`, `--profile`).
- `_load_profile_config()` repo-kokundeki `cx_profiles.local.json`'u okuyor: `{"disabled": [...], "priority": [...]}`.
- `_ordered_eligible()` priority+oldest-LastRefresh ile siralayip disabled'lari ATLIYOR.
- `get_profiles()` = `cx accounts`'tan profilleri parse ediyor.

# Eklenecekler (argparse, --task-file GEREKTIRMEDEN calismali; islem yapip exit)
1. `--list` : kesfedilen tum profilleri tablo bas: NAME | enabled/DISABLED | priority-rank | LastRefresh (varsa). disabled = cx_profiles.local.json'daki listeye gore. Sonra exit (dispatch YOK).
2. `--disable <NAME>` : `cx_profiles.local.json`'daki `disabled` listesine NAME ekle (dosya yoksa olustur, `priority`yi koru, idempotent — zaten varsa "already disabled" de). Onay mesaji bas + exit.
3. `--enable <NAME>` : `disabled` listesinden NAME cikar (yoksa "already enabled" de). Onay mesaji bas + exit.
4. NAME dogrulama: `--disable/--enable` verilen NAME `cx accounts` profillerinden biri degilse UYARI bas ama yine de izin ver (ileride eklenecek profil olabilir) — sadece "warning: not a currently-known profile" notu.
5. argparse help text'leri NET olsun ki `python cx_dispatch.py --help` bu 3 secenegi acikca gostersin. Help epilog'a kisa ornek ekle:
   `Examples: python cx_dispatch.py --list | --disable yekta | --enable yekta`

# Kisitlar
- Mevcut dispatch akisini, profil secimini, quota-aware/LastRefresh mantigini DEGISTIRME. Sadece yeni argümanlar + onlarin handler'lari + help.
- `cx_profiles.local.json` yaz/oku JSON robust olsun (bozuk/eksik dosyaya tolerans, priority alanini koru).
- Windows; ascii-safe output.

# Dogrulama (CODEX_DONE_<profile>.md'ye yapistir)
- `python cx_dispatch.py --help` ciktisi (yeni secenekler gorunur mu).
- `python cx_dispatch.py --list` ciktisi.
- Round-trip: `python cx_dispatch.py --disable __smoketest__` -> `--list` (DISABLED gorunur) -> `--enable __smoketest__` -> `--list` (geri enabled) -> ve cx_profiles.local.json son hali temiz.
- `python -c "import ast; ast.parse(open('cx_dispatch.py').read())"` ile syntax OK.
