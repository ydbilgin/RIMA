# Acceptance Checklist

## Character Select / Chamber

- [ ] Ekranda 10 farklı class kimliği görünüyor.
- [ ] Her pedestal Warblade klonu değil.
- [ ] Eksik sprite varsa class-specific silhouette placeholder var.
- [ ] Combat HUD kapalı.
- [ ] HP bar görünmüyor.
- [ ] Skill hotbar görünmüyor.
- [ ] Pedestal scale %25-35 küçülmüş.
- [ ] Class label kırpılmıyor.
- [ ] Seçili class glow/ring ile belli.
- [ ] Alt selected class strip var.
- [ ] `[G] Bürün: ClassName` tek key ile görünüyor.
- [ ] `[G] Rift'e Gir` tek key ile görünüyor.

## Room Layout

- [ ] Chamber gereksiz geniş taş halı gibi değil.
- [ ] Selection alanı merkezde sıkı.
- [ ] Rift exit arka kenarda okunur.
- [ ] Normal combat oda 18×12 - 22×14 hedefinde veya görsel olarak daraltılmış.
- [ ] Kenarlarda prop/decal var.
- [ ] Combat core temiz.
- [ ] Boss room normal odadan farklı görünüyor.

## Interaction Prompt Tests

- [ ] InteractionPromptFormatter var.
- [ ] Formatter duplicate key üretmiyor.
- [ ] Localization stringleri hardcoded `[G]`, `[E]`, `[RMB]` taşımıyor.
- [ ] `[G] [G]` lint testte yakalanıyor.
- [ ] `G G` lint testte yakalanıyor.
- [ ] TR promptlar testli.
- [ ] EN promptlar testli.
- [ ] Chamber pedestal prompt testli.
- [ ] Rift exit prompt testli.

## Red List Kontrolü

- [ ] Full wall sistemi eklenmedi.
- [ ] Floorlar baştan üretilmedi.
- [ ] 8 yön portal eklenmedi.
- [ ] Entry portal object eklenmedi.
- [ ] Character select'e stat panel yığılmadı.
- [ ] Warblade fallback her class'a basılmadı.
