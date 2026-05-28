# Codex Task: AttackTokenManager Implementation

## Bağlam
RIMA 2D roguelite, Unity URP. Mob savaş sistemi `BaseMobBehavior.cs` + `MobAttack_*.cs` bileşenleri üzerine kurulu.
Namespace: `RIMA.Combat`. Scripts klasörü: `Assets/Scripts/`.

## Tasarım Kuralları
- Global token havuzu: **2 MeleeToken + 1 RangedToken** (odada 20 düşman olsa bile)
- Tokeni olan düşman saldırır; olmayanlar tehditkâr biçimde dolaşır ama saldırmaz
- Token, saldırı animasyonu/coroutine bitince iade edilir

## Yapılacaklar

### Adım 1 — AttackTokenManager.cs oluştur
Dosya yolu: `Assets/Scripts/Combat/AttackTokenManager.cs`

Sınıf gereksinimleri:
```
namespace RIMA.Combat
{
    public enum AttackTokenType { Melee, Ranged }

    public class AttackTokenManager : MonoBehaviour
    {
        public static AttackTokenManager Instance { get; private set; }

        [SerializeField] private int maxMeleeTokens = 2;
        [SerializeField] private int maxRangedTokens = 1;

        // Aktif token sahiplerini takip et (hangi enemy hangi token'ı tutuyor)
        // TryConsumeToken(GameObject enemy, AttackTokenType type) → bool
        // ReturnToken(GameObject enemy, AttackTokenType type)
        // OnEnemyDeath(GameObject enemy) → sahip olduğu token'ı iade et
        // Awake() → singleton setup
    }
}
```

Tam implementasyonu yaz. Dictionary<GameObject, AttackTokenType> ile aktif sahipleri tut.
`int _activeMelee`, `int _activeRanged` sayaçlarıyla takip et.

### Adım 2 — BaseMobBehavior.cs'e token check ekle
`Assets/Scripts/Enemies/BaseMobBehavior.cs` dosyasını oku.
FixedUpdate içinde saldırı tetikleme bloğunu bul (attackTimer <= 0 olan yer).
Oraya şu mantığı ekle:

```csharp
// Saldırıdan önce token al
var tokenType = /* MobAttack_Throw varsa Ranged, yoksa Melee */;
if (!AttackTokenManager.Instance.TryConsumeToken(gameObject, tokenType)) return;
attackTimer = attackCooldown;
OnAttackReady?.Invoke(dir);
```

Token tipini belirlemek için: GameObject'te `MobAttack_Throw` veya `SeamCrawler_Homing` component'i varsa Ranged, yoksa Melee.
`GetComponent` çağrısını Awake'te cache'le, her frame çağırma.

### Adım 3 — MobAttack_*.cs token iadesi
`Assets/Scripts/Enemies/Attacks/` klasöründeki tüm `MobAttack_*.cs` dosyalarını bul.
Her birinde saldırı coroutine'inin sonuna (windingUp veya benzeri flag false'a döndükten sonra) şunu ekle:
```csharp
AttackTokenManager.Instance?.ReturnToken(gameObject, _tokenType);
```
`_tokenType` field'ını her attack component'e ekle, Awake'te set et.

### Adım 4 — Compile kontrol
`read_console` ile Unity console'u oku. Compilation error var mı kontrol et.
Error varsa düzelt ve tekrar kontrol et. Warning'ler OK.

### Adım 5 — Sonuç
CODEX_DONE.md'ye yaz:
- Hangi dosyalar oluşturuldu/değiştirildi
- Compile: PASS / FAIL (error varsa listele)
