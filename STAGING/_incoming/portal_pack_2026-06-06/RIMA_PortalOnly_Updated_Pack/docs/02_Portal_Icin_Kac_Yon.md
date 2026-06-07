# 02 — Portal İçin Kaç Yön Üretilmeli?

Bu konu en kritik kararlardan biri.

## Yanlış yaklaşım 1: 8 yön portal üretmek
Bu gereksiz pahalıdır çünkü:
- asset üretimini şişirir
- yerleştirme kurallarını karmaşıklaştırır
- procedural odalarda okunurluğu düşürür
- demo için getirisi çok düşüktür

## Yanlış yaklaşım 2: Tek slot / tek portal
Bu da fazla kısırdır çünkü:
- seçim hissi zayıf olur
- branching / risk-reward hissi azalır
- her odanın çıkışı aynı görünür

## Doğru yaklaşım
### 1) Portal facing direction
Portal asset'i için **1 ana facing direction** yeter.

Sebep:
- Portallar back edge'de duracak.
- Oyuncu her zaman aşağıdan yukarıya doğru bakıyor/ilerliyor.
- O yüzden portalın tüm varyantlarının farklı yönlere dönmesi gerekmiyor.
- Aynı ana portal modeli solda, ortada ve sağda kullanılabilir.

### 2) Socket / slot sayısı
Room template tarafında 4 socket yeter:

- ENTRY_S
- EXIT_NW
- EXIT_N
- EXIT_NE

Bunlardan:
- ENTRY_S = oyuncunun geldiği alt giriş
- EXIT_NW = sol arka portal slotu
- EXIT_N = orta arka portal slotu
- EXIT_NE = sağ arka portal slotu

## Çok önemli ayrım
### "Yön" ile "slot" aynı şey değil
- **Yön sayısı** = assetin kaç farklı açıdan çizileceği
- **Slot sayısı** = odada kaç ayrı yerleşim noktasının destekleneceği

RIMA için demo seviyesinde:
- yön sayısı = 1
- çıkış slotu sayısı = 3
- giriş slotu sayısı = 1

## Portal tür sayısı
Ayrıca içerik türü için 5 portal varyantı üret:
1. Combat
2. Elite
3. Reward
4. Heal / Lore
5. Boss

Ama bunlar yön farkı değil, **işlev farkı**.

## En mantıklı üretim şeması
### Tek portal gövdesi
- 1 stone arch / rift frame

### Üstüne değişenler
- ikon / rune
- üst tepe amblemi
- küçük trim farkı
- reward badge
- particle yoğunluğu
- renk aksanı

Yani 5 tam ayrı mimari yapı yapmak zorunda değilsin.
Aynı ana gövdeyi farklı portal türlerine skin gibi uygula.

## Nihai cevap
"Kapı için kaç yön üretilmeli?"
**1 portal yönü üret, 3 exit slotu destekle.**
