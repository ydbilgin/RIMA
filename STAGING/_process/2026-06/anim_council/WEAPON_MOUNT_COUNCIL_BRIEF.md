# COUNCIL — warblade animasyonları: silahlı-bake mi, silahsız-char + el-mount mu?

ACTIVE RULES: (1) think (2) min (3) surgical (4) BLOCKED if unclear.
NLM ACCESS: gerekirse NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>". (auth dolmuşsa TASARIM/ + kod oku.)
UNITY YOK: salt analiz/karar.

## OLAY (kullanıcı bildirdi)
PixelLab `create_character_state` ile warblade pose-state'leri üretildi. **Kullanıcının warblade base sprite'ı SİLAHSIZ olmasına rağmen state'lere KILIÇ bake edildi** (state prompt'u "sword readable / preserve weapon identity" varsaymıştı = hatalı varsayım). RIMA mevcut pipeline = silahları AYRI mount eder (`feedback_user_draws_weapons_claude_mounts`: kullanıcı silah çizer, Claude import+pivot/mount).

## KARAR SORUSU (kullanıcı: "council'e de sor")
Demo için warblade animasyonlarını:
- **(A) SİLAHLI-BAKE:** silahı sprite/animasyona göm (state zaten kılıçlı). Basit render, sprite-anim'e (frame-by-frame, bone YOK) uygun; AMA silah swap edilemez.
- **(B) SİLAHSIZ + EL-MOUNT:** karakteri silahsız üret, silahı ayrı sprite olarak ele/socket'e bağla, ayrı konumla. Loot-variety (roguelite!) destekler; AMA RIMA **sprite-sheet animasyon** kullanıyor (skeletal/bone YOK) → hareketli ele her frame silah hizalamak ZOR (per-frame hand-pivot data gerekir).

## BAĞLAM
- RIMA = roguelite (silah loot/çeşitlilik teorik değerli) AMA **demo tek-sınıf + muhtemelen tek başlangıç-silahı**.
- Golden-path video warblade'i **LMB vuruş yaparken** gösteriyor → silah görünür olmalı, akıcı olmalı.
- Anim = 120x120 8-yön sprite-sheet (5 üret + 3 mirror flipX), 10-12fps. Bone/skeletal mount altyapısı YOK (doğrula).
- 5 gün, Claude 4.8 kotası ~%3. Üretim = PixelLab + kullanıcı.

## CEVAPLA (net karar + 1 satır neden)
- **W1:** Demo için (A) silahlı-bake mi (B) silahsız+mount mu? NEDEN? (sprite-anim bone-yok gerçeği + demo-scope + loot-variety-post-demo + 5 gün ışığında)
- **W2:** Mevcut silah-bake'li state'ler → KORU (A seçilirse) mu, SİL+silahsız-rejenerasyon (B seçilirse) mu? State prompt'u nasıl düzeltilmeli (A: "weapon consistent"; B: "empty hands, no weapon")?
- **W3:** Seçilen yolun demo-sonrası loot-variety'e etkisi (A: her silah için ayrı anim-set mi? B: mount altyapısı post-demo mu kurulur?). Over-engineer ETME.
- **W4:** TEK CÜMLE TAVSİYE (demo için).

## ÇIKTI (E1)
Dosyaya yaz: cx → `WEAPON_cx.md` · ax_pro → `WEAPON_axpro.md` · ax_flash → `WEAPON_axflash.md` (bu klasörde). Orchestrator'a dönüş ≤8 satır + yol.
