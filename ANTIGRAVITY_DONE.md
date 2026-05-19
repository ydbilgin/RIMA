# Antigravity Done Log
> Arşiv (S95 öncesi): `STAGING/_archive/antigravity_done_pre_s95_20260520.md`

---

## [2026-05-20] Combat Juice Cleanup + Prop Parent Fix

### Görev 1: Çift Event Bug
- Durum: DONE
- Değiştirilen dosyalar:
  - [BasicAttackBehaviorBase.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Combat/BasicAttack/BasicAttackBehaviorBase.cs)
- Silinen çağrılar:
  - `HitStop.Instance?.FreezeLight()`
  - `LightPulse.Emit(new Color(0.4f, 0.7f, 1f), 1.5f, 0.10f)`
  - `DamagePopup.Show(col.transform.position, finalDmg)`
  - `CameraShake.Instance?.Shake(0.18f, 0.12f)`
- Notlar: [MarkPulseBehavior.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Combat/BasicAttack/MarkPulseBehavior.cs) dosyasında `CombatEventBus.PublishHit` / `PublishKill` çağrıları yer almadığı için (sadece legacy çağrılar bulunduğu için) yönerge gereği dosyaya dokunulmadı.

### Görev 2: Prop Parent Fix
- Durum: DONE
- Props_Root: Oluşturuldu (Scene root seviyesinde, position `(0,0,0)`, rotation `(0,0,0)`, scale `(1,1,1)` transform değerleriyle [PathC_BaseTest.unity](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scenes/Demo/PathC_BaseTest.unity) sahnesine eklendi ve kaydedildi.)
- Değiştirilen dosyalar:
  - [RimaUnifiedPainterWindow.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Editor/RimaUnifiedPainterWindow.cs)
  - [PathC_BaseTest.unity](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scenes/Demo/PathC_BaseTest.unity)
- Notlar:
  - Grid veya Tilemap parent olarak belirlendiğinde prefablarda oluşacak dikey basıklığı engellemek için otomatik `Props_Root` yönlendirmesi eklendi.
  - Kullanıcının yönlendirmesi ve ek isteği doğrultusunda objeler adlarına ve kategorilerine göre `Walls`, `Statues`, `WallMountings` (duvara asılan), `Patches` (yama/halı), `Mobs` ve `FloorProps` (yere konan) adında alt gruplara ayrıştırılarak hiyerarşide gruplandı.
  - Penceredeki tüm silme, damlalık, kaydetme, yükleme ve duvar birleştirme fonksiyonları alt grup yerleşimlerini de kapsayacak şekilde rekürsif (`GetRecursiveChildren`) olarak güncellendi.
