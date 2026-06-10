# 02 — P0 Fix: Dual-Class Demo Flow

## Sorun

Dual-class sistemi kodda var:

- `PlayerClassManager.SelectSecondaryClass()`
- `OnSecondaryClassSelected`
- `ClassSelectionTrigger.Trigger()`
- `SkillBarUI` secondary seçiminde controller re-resolve ediyor.
- `DraftManager` secondary seçiminden sonra unlock draft açabiliyor.

Ama demo run içinde boss ölümü doğrudan Victory/DemoComplete'e gidiyorsa sistem pratikte erişilebilir değil.

Bu şu an en tehlikeli durum:

```text
Sistem kodda var ama oyuncu doğal akışta göremiyor.
```

Bu teslimde kötü görünür. Çünkü raporda dual-class anlatıp canlı demoda sadece T tuşuyla açarsan “debug feature” gibi durur. Debug feature’ı jüriye core mechanic diye satmak da yazılım dünyasının küçük günahlarından biri.

## Hedef Demo Akışı

İdeal teslim akışı:

```text
Combat → Combat → Merchant → Combat → Boss → Class Selection → Unlock Draft → PostBossCombat → Demo Complete
```

PostBossCombat yetişmezse minimum:

```text
Combat → Combat → Merchant → Combat → Boss → Class Selection → Unlock Draft → Demo Complete
```

## Fix Prensibi

Boss öldüğünde:

1. Boss death efekti / clear akışı başlar.
2. Victory hemen gösterilmez.
3. PlayerClassManager class selection event'i fırlatır.
4. UI secondary seçimini açar.
5. Oyuncu secondary class seçer.
6. Warblade secondary slotları açılır.
7. DraftManager unlock draft gösterir.
8. Draft tamamlanınca:
   - ya PostBossCombat'a geçilir,
   - ya DemoComplete gösterilir.

## Minimum Kod Değişikliği

### PenitentSovereign.cs

Eğer field varsa:

```csharp
[SerializeField] private bool suppressClassSelectOnDeath = true;
```

Teslim build için default false olmalı:

```csharp
[SerializeField] private bool suppressClassSelectOnDeath = false;
```

Boss death handler içinde:

```csharp
private void HandleDeath()
{
    if (dead) return;
    dead = true;

    rb.linearVelocity = Vector2.zero;
    healthBar?.Hide();

    if (!suppressClassSelectOnDeath)
    {
        PlayerClassManager.Instance?.TriggerClassSelection();
    }

    // Eski DemoComplete/Victory çağrısı varsa buraya dikkat:
    // Class selection bitmeden doğrudan victory gösterme.
}
```

Ama daha temiz yaklaşım:

- Boss death sadece event fırlatsın.
- RoomRunDirector boss clear sonrası akışı yönetsin.

## RoomRunDirector Entegrasyonu

RoomRunDirector içinde boss clear / run complete bölümünde şu ayrım olmalı:

```csharp
if (CurrentRoomType == RIMA.RoomType.Boss)
{
    StartCoroutine(BossClearDualClassSequence());
    return;
}
```

Önerilen coroutine:

```csharp
private IEnumerator BossClearDualClassSequence()
{
    lifecycle.MarkCleared();

    bool secondaryPicked = PlayerClassManager.Instance != null &&
                           PlayerClassManager.Instance.SecondaryClass != ClassType.None;

    if (!secondaryPicked && PlayerClassManager.Instance != null)
    {
        bool done = false;
        void Handler(ClassType _) => done = true;

        PlayerClassManager.Instance.OnSecondaryClassSelected += Handler;
        PlayerClassManager.Instance.TriggerClassSelection();

        yield return new WaitUntil(() => done || PlayerClassManager.Instance.SecondaryClass != ClassType.None);

        PlayerClassManager.Instance.OnSecondaryClassSelected -= Handler;
    }

    // DraftManager secondary seçiminden sonra kendi unlock draft'ını açıyorsa bekle.
    yield return new WaitUntil(() => DraftManager.Instance == null || !DraftManager.Instance.IsDraftActive);

    // Seçenek A: Post-boss oda varsa oraya geç.
    if (HasPostBossNode())
    {
        AdvanceToPostBossNode();
        yield break;
    }

    // Seçenek B: Post-boss oda yoksa demo complete göster.
    lifecycle.MarkVictory();
    ShowDemoComplete();
}
```

Bu kod birebir dosyana göre uyarlanmalı. Ama akış mantığı bu.

## Post-Boss Oda Gerekli mi?

### Evet, en iyi sonuç için gerekli.

Çünkü dual-class sadece seçim ekranı değil; oynanışa yansımalı.

Post-boss oda kısa olabilir:

- 1 küçük arena.
- 2-3 zayıf enemy.
- Amaç: secondary slot/skill gerçekten çalışıyor mu göstermek.
- 30-60 saniyelik test yeter.

### Yetişmezse

En azından şu sıralama olmalı:

```text
Boss ölür
→ Class selection açılır
→ Player secondary seçer
→ Unlock draft açılır
→ Skill bar 6 slot gösterir
→ Demo complete
```

Bu bile boss ölür ölmez DemoComplete’ten daha iyi.

## DungeonGraph / Demo Sequence Değişikliği

Eğer `DungeonGraph.BuildDemoSequence()` sabit 5 node ise geçici olarak şuna çıkar:

```text
0 Combat
1 Combat
2 Merchant
3 Combat
4 Boss
5 Combat / PostBoss
```

PostBoss node özel room type olmak zorunda değil. Combat node olabilir ama `postBossTest = true` gibi ayrı flag daha temiz olur.

Minimal ve hızlı çözüm:

- Boss node'un child'ı olarak 1 Combat node ekle.
- Boss clear sonrası victory değil, child node'a door/advance aç.
- Bu oda bitince victory göster.

## Kabul Kriterleri

Bu fix başarılı sayılırsa:

- Oyuncu T tuşuna basmadan boss sonrası class selection görür.
- Secondary class seçimi bir kez yapılır.
- Seçim sonrası Warblade slotları 6'ya genişler.
- SkillBarUI secondary controller'ı resolve eder.
- DraftManager unlock draft açar.
- PostBossCombat varsa secondary skill kullanılabilir.
- PostBossCombat yoksa en azından unlock draft sonrası DemoComplete görünür.
- Boss death ile DemoComplete aynı frame/aynı akışta class selection'ı ezmez.

## Teslim İçin Önerilen Secondary Seçenekleri

Hepsini açmaya gerek yok.

En güvenli seçenekler:

1. Elementalist
   - Görsel olarak anlaşılır.
   - Mana sistemi net.
   - Warblade + elemental skill sunumu iyi görünür.

2. Ranger
   - Mark/trap mantığı daha taktiksel ama görsel anlatımı iyi olabilir.

Shadowblade/Ronin daha sonra. Çünkü faz/energy/tension gibi sistemler canlı demoda açıklama ister; açıklama gerekiyorsa demo zayıflar.
