# 2 — Capture QA

Bu paket “25 state” diye sunuluyor, fakat dosyaların bir bölümü görsel olarak o state'i göstermiyor. Assert PASS ile screenshot kanıtını birbirine karıştırmamak gerekir. İnsanlar bunu şaşırtıcı biçimde sık yapıyor; bir ekranın dosya adını değiştirmek, içeriğini sihirli biçimde değiştirmiyor.

## Kesin bulgular

| Dosya | Bulgu | Karar |
|---|---|---|
| `08_director_spawn_assert.png` | Spawn sonrası alt durum mesajı görünüyor | Geçerli görsel kanıt |
| `09_director_stats_tab.png` | `08` ile byte-byte aynı | Stats tab görsel olarak kanıtlanmıyor; tekrar çekilmeli |
| `19_tab_charactersheet.png` | Character Sheet paneli görünmüyor | Geçersiz capture |
| `20_skill_offer_draft.png` | Draft paneli görünmüyor | Geçersiz capture |
| `21_runmap_open.png` | `20` ile byte-byte aynı; Run Map görünmüyor | Geçersiz capture |
| `10_buildmode_entry.png` / `11_buildmode_prop_placed.png` | Fark çok küçük; yerleştirilen prop açıkça seçilemiyor | Ghost/selection outline ile yeniden çekilmeli |
| `05_hud_normal_fullhp.png` / `13_combat_mid_enemies.png` | Görsel ayrım çok zayıf | State etiketi eklenmeli veya daha belirgin sahne seçilmeli |

## Tekrar capture standardı

Her state için screenshot'ta şu üç şey aynı anda görünmeli:

1. **State'in kendisi:** açık panel, seçili tab, yerleştirilen obje veya değişen stat
2. **State doğrulaması:** küçük bir dev-only status satırı veya assert sonucu
3. **Bağlam:** sahnenin hangi modda ve hangi room'da olduğu

### Dosya adı şablonu

`NN_system_state_expected-result.png`

Örnek:

- `09_director_stats_physPower_177.png`
- `19_character_sheet_open_warblade.png`
- `20_skill_draft_open_3_cards.png`
- `21_runmap_open_current_node_highlight.png`

## Capture sonrası otomatik kontrol

Claude şu kontrolleri eklemeli:

- Aynı SHA-256 çıkan farklı state screenshot'larını FAIL say
- Açılması beklenen root panel `activeInHierarchy == true` değilse capture alma
- Screenshot'ta panel canvas bounds ekran içinde değilse FAIL
- Capture metadata JSON'a `activePanel`, `selectedTab`, `timeScale`, `scene`, `expectedVisibleText` yaz

Görsel özet: `visuals/capture_qa_failures.png`
