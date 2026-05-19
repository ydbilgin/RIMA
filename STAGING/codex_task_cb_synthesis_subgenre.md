# Codex Task — CB Sub-Genre Synthesis + Class System Final

**Tarih:** 2026-05-18 (devam — 2. dispatch)
**Profile:** laurethayday (xhigh)
**Effort:** high
**Output:** `STAGING/CODEX_TASK_cb_synthesis_subgenre_DONE.md`

---

## ACTIVE RULES
(1) think before answering (2) min commentary, no fluff (3) surgical — sadece sorulan sorulara cevap ver (4) BLOCKED yaz eğer kararsızsan

## NLM ACCESS
If you need RIMA design context, query NLM first:
`uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"`

Direct-read sadece: bu dosya + `STAGING/CODEX_TASK_cb_pivot_design_review_DONE.md` (1. dispatch verdict) + `F:/LaurethStudio/02_GAMES/CircuitBreaker/00_FULL_DESIGN_DOC.md`

---

## 1. KONTEKST — 1. dispatch sonrası şu konuşmalar oldu

1. dispatch verdict (`STAGING/CODEX_TASK_cb_pivot_design_review_DONE.md`) okundu, PASS. Kullanıcı sonra şu yön değişikliklerini önerdi:

### Shift 1: Mass-clear mantığı (Hades-tempo değil)
- "Su koy + elektrik = AOE" doğası gereği toplu kill mekaniği
- 5-8 düşman cascade boş, 25-50 düşman cascade tatmin edici
- Mob density yukarı: 30-50 peak/oda (önce 10-15 Hades, sonra 15-25, son 30-50)

### Shift 2: Map yapısı evrim (3 ana opsiyon konuşuldu)
- Önce Hades chambers (10-15 düşman, oda-oda)
- Sonra Risk of Rain 2 level transition (manuel + 30-80 düşman + level değişimi)
- Sonra PoE Atlas (dungeon seçim, atlas grid)
- Şimdi Hero Siege Act/Floor (5 dungeon × 3 floor + boss, MVP linear → post-MVP atlas)

### Shift 3: Sub-genre netleşmesi
- "Hack-n-slash" 2 anlamlı: melee combo HnS (Dead Cells) vs ARPG HnS (PoE/LE/Diablo)
- CB ARPG-lite kategorisinde — top-down + skill cooldown + element synergy + mass clear
- Hero Siege loop incelendi: Hub → Act seç → Dungeon (5-10 floor + boss) → Act boss → next Act
- Halls of Torment / Soulstone Survivors / Death Must Die / Loop Hero gibi "ARPG roguelite" türü proven

### Shift 4: Sentez sub-genre arayışı
- Kullanıcı: "tek bir tür değil, bütün türlerin birleşimi sentez bir şey"
- Claude öneri: **"Cascade ARPG"** veya **"Battlefield Alchemy Roguelite"**
- CB'nin USP'si **REAL-TIME GENERATIVE** combat (Reactive değil, Passive değil)
- Pazar boşluğu: Into the Breach (turn-based generative) + Noita (real-time ama kontrolsüz) arasında — CB real-time + kontrollü

### Class system karar
- RoR2 modeli en uygun: 4 skill + Form ultimate + run boyu modifier
- MVP: 3 class (Aquamancer/Pyrotechnist/Stormcaller)
- Post-MVP roadmap: 5 → 8 → 12+ class (Hero Siege scale Faz 4)
- Skill variant unlock RoR2 challenge muadili

---

## 2. CB FINAL DESIGN SNAPSHOT — bu konuşma sonrası

### Mekanik özet

| Sistem | Spec |
|---|---|
| **Camera** | Top-down 2D pixel 32px |
| **Control** | WASD + mouse aim + LMB primary + RMB drop + Q/E swap + 1/2/3 skill + R ultimate |
| **Class** | 3 MVP (RoR2 modeli, her class 4 skill + Form) |
| **Tetik silahı** | 5 element × 2-mod (trigger + drop) |
| **Tile state** | 7 base + 3 hibrit MVP (Slush, Emülsiyon, Volatile) |
| **Status hybrid** | 3 MVP (Conductive, Steaming, Combusting) |
| **Combo system** | 4-tier streak (5/15/30/50 kill) |
| **Modifier** | 12-18 MVP (ModifierDef SO + interpreter, NO node graph) |
| **Map yapısı** | Hero Siege Act + Floor (5 dungeon × 3 floor + Act boss) |
| **Mob density** | 30-50 peak / oda, 220-280 total/run, 60 peak boss arena |
| **Wave ritmi** | 3-faz oda (Setup 8-12 / Surge 30-50 / Cascade 20-30) |
| **Run uzunluk** | 22-25 dk (6 dungeon × 3-4 dk + boss 4-5 dk) |
| **Zorluk modu** | Voltage 0-32 (Hades Heat muadili, 15 voltage modifier) |
| **Currency** | 3-tier (Spark run / Cinder meta / Echo endgame) |
| **Sub-genre** | "Cascade ARPG" veya "Battlefield Alchemy Roguelite" — sentez |

---

## 3. CODEX'TEN İSTENEN — 8 yeni soru

### Soru 1: Sub-genre verdict

Claude'un 2 sentez önerisi:
- **"Cascade ARPG"** — kısa, akılda kalıcı, capsule-friendly
- **"Battlefield Alchemy Roguelite"** — descriptive, USP keskin

3. seçenek:
- Sen daha iyi bir sentez sub-genre adı önerebilir misin? CB'nin USP'sini tek cümlede açıklayan.

**Beklenen:** 3 sub-genre önerisi sırala (Claude'unkilerle dahil), her birine: capsule pitch, Steam tag list, pazar pozisyon kıyaslama (Hero Siege/Hades/Magicraft/RoR2 ile).

### Soru 2: Real-time generative combat USP doğrulama

Claude analizi: CB'nin türü pazarda boş — **real-time generative action roguelite**.

| Combat tipi | Örnek | Uygun mu CB için? |
|---|---|---|
| Reactive | Hades, Dead Cells | Hayır |
| Passive | VS, Brotato | Hayır |
| Reactive ARPG | Diablo, PoE | Yarı |
| Turn-based generative | Into the Breach | Hayır (turn) |
| **Real-time generative** | **CB tek** | **EVET** |

**Soru:** Bu analiz doğru mu? CB gerçekten kimsenin yapmadığı bir niş mi, yoksa ben görmüyorum (bilmediğim oyun var)? Eğer var olan oyun varsa: hangi, ne kadar yakın, anti-klon zorunlu mu?

### Soru 3: Map yapısı final lock

3 opsiyon kıyasla, NET verdict ver:
- **A. RoR2 linear levels** (6 level + final boss)
- **B. Hero Siege Act/Floor** (5 dungeon × 3 floor + Act boss)
- **C. PoE Atlas** (atlas grid, MVP fazla)

**Beklenen:** Tablolu kıyas + 1 NET öneri + 16 hafta MVP scope etkisi.

### Soru 4: Class system spec

RoR2 model + Form ultimate + skill variant unlock (post-MVP) önerildi.

| Class element | MVP spec | Faz 2 spec |
|---|---|---|
| Sayı | 3 | 5 |
| Skill/class | 4 + Form | 4 + Form + 2-3 variant/skill |
| Variant unlock | YOK | Cinder meta |
| Build crafting | 5 modifier slot | 5 modifier + skill variant + relic pool |

**Soru:** Bu spec final mı? Eksikler:
- Class identity nasıl korunur (3 class'ın overlap olmaması)?
- Skill variant unlock sistemi Codex impl 1 hafta mı 2 hafta mı?
- Class-bound tetik silahı vs evrensel tetik silahı? (Aquamancer sadece su tetik, yoksa 5 tetik herkese?)
- Form ultimate cooldown 30 sn yeterli mi yoksa Voltage tier'la 45 sn mi?

### Soru 5: Sentez sub-genre 16 hafta MVP fit

Sentez sub-genre içinde MVP'ye sığması gerekenler:
- ARPG iskelet (class + skill + Act dungeon)
- Roguelite döngü (run + meta currency)
- Mass-clear volume (30-50 peak)
- Element grammar (5 element × tile)
- Spatial cascade USP (2-mod silah + hibrit + ultimate)

**Soru:** Bu 5 sentez katman 16 hafta MVP'de tek "demo MVP" olarak çalışır mı, yoksa bir katman MUTLAKA post-MVP olmalı?

1. dispatch MVP cut: 3 class + 2-mod + 3 hibrit + 12-18 modifier + 5 oda + 1 boss
2. dispatch revize ihtiyacı: + Hero Siege Act/Floor + 6 dungeon × 3 floor + 30-50 mob peak

Bu **kabarmayı** Codex onayla — yeni 16 hafta breakdown gerekirse re-write et.

### Soru 6: Pazar pozisyon + Steam tag stratejisi

CB sentez sub-genre olarak market'e nasıl konumlanır?

| Boyut | Hero Siege | Magicraft | Soulstone Survivors | **CB** |
|---|---|---|---|---|
| Tür | ARPG roguelite | Spellcraft roguelite | VS + Diablo | ? |
| Fiyat | $14.99 | $15.99 | $7.99 | ? |
| Satış | 1M+ | 700K | 500K+ | TARGET? |
| Tag | ARPG, Hack&Slash | Spellcraft, Magic | Bullet Hell, RPG | ? |

**Soru:** CB için **Steam tag öncelik sırası** + **fiyat noktası** + **satış hedefi** (Year 1).

### Soru 7: Anti-klon yeni risklere karşı

Sentez genişleme = yeni klon riskleri:
- Hero Siege klonu (Act/Floor + sınıf + loot)
- RoR2 klonu (manuel + class + level + time pressure)
- ARPG genel klonu (PoE/LE/Diablo)
- VS/Brotato klonu (mass-clear + roguelite)

**Beklenen:** Her klon riski için 1 cümle anti-klon mitigasyon + CB'nin ayrışma noktasını tek cümle vurgula.

### Soru 8: Vizyon vs MVP ayrım — solo dev roadmap

Kullanıcı: "Oyun şu an basit bir prototip fikir, bu geliştirilecek class eklenecek özellik mekanik eklenecek."

= **vizyon belge** ile **MVP belge** ayrımı şart.

**Soru:** CB'nin **3 belge yapısı** önerini ver:
- `VISION_DOC.md` (uzun-vadeli, Faz 2-3-4 dahil, full class roster, full atlas, sezon sistem)
- `MVP_PLAN.md` (16 hafta concrete)
- `ROADMAP.md` (Faz 2 ay 4-6, Faz 3 ay 6-12, Faz 4 ay 12+)

Her belgenin section listesi + hangi karar nereye gider.

---

## 4. OUTPUT FORMATI

Output: `STAGING/CODEX_TASK_cb_synthesis_subgenre_DONE.md`

Yapı:

```markdown
# CB Synthesis Sub-Genre + Class System Final — Codex Verdict

## 1. Sub-genre 3 öneri
- Cascade ARPG: ...
- Battlefield Alchemy Roguelite: ...
- [Codex'in 3. önerisi]: ...
- NET KARAR: ...

## 2. Real-time generative combat USP doğrulama
[market gap analysis + anti-klon]

## 3. Map yapısı final lock (A/B/C)
[verdict + 16 hafta MVP impact]

## 4. Class system spec final
[RoR2 4-skill + Form + variant unlock spec]

## 5. 16 hafta MVP fit
[5 katman sığar mı + revize timeline]

## 6. Pazar pozisyon + Steam tag + fiyat
[tablo + öneri]

## 7. Anti-klon mitigasyon
[4 oyun × mitigasyon]

## 8. 3-belge yapı önerisi
[VISION / MVP / ROADMAP section listeleri]

## 9. EXECUTIVE SUMMARY
[5 madde, kullanıcı için]
```

**Bitiş:** `CODEX SYNTHESIS COMPLETE` satırı.

**Tahmini cevap uzunluğu:** 1000-1800 satır markdown.
