# Room Size and Camera Review

| Oda | Plan | Öneri |
|---|---:|---:|
| Combat | 22×15 | doğru, minimum bu |
| Shop | 16×12 | doğru |
| Boss | 28×18 | doğru ama merkez boş kalmalı |
| Chamber | 28-32×20 | kalsın |

22×15 odada 6 mob üstüne çıkmak görsel karmaşa ve collider sıkışması yaratır.

Sabit ortho 5.0 doğru başlangıç. Inspector'da tunable kalmalı:

```csharp
[SerializeField] private bool useFixedDemoCamera = true;
[SerializeField] private float fixedOrthographicSize = 5.0f;
```

4.7 daha yakın ama görüş daralır. 5.0 demo default. 5.3 güvenli ama karakter hissi küçülür.
