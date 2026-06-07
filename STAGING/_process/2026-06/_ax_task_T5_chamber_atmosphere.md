# GÖREV T5: Chamber atmosfer pass'i (Unity implementasyonu — ax)

Sen RIMA Unity projesinde çalışan bir geliştirici ajansın. UnityMCP araçların var (Unity AÇIK olmalı — değilse işi yapma, "UNITY KAPALI" yazıp çık). Proje kökü: `F:\Antigravity Projeler\2d roguelite\RIMA`.

## Bağlam (kendi başına yeterli)
Attunement Chamber = karakter seçim odası; runtime'da `Assets/Scripts/UI/ChamberSelectBootstrap.cs` kurar (oda şablonu: `Assets/Data/Rooms/Special/Chamber_CharSelect.asset`). Bugün temizlendi (pillar'lar kaldırıldı, 10 farklı sınıf heykeli, Brawler dummy, kamera 5.8 ortho). KALAN = atmosfer: oda "tören alanı" gibi hissettirmeli. Kaynak kararlar: `STAGING/MASTER_PLAN_FINAL_2026-06-06.md` (T5) + `STAGING/UXFLOW_DECISION_2026-06-07.md` (chamber maddeleri).

## KURALLAR
- SADECE ChamberSelectBootstrap.cs (+ gerekirse Chamber_CharSelect.asset + küçük yardımcı script) — başka sisteme dokunma
- Legacy dosyalara (RoomLoader/Gate/GateBehavior/RuntimeRoomManager) ASLA dokunma
- Yeni asset üretme/import etme — eldekiler yeterli
- UI metinleri `RIMA.Loc.T(anahtar)` üzerinden (Assets/Scripts/Core/Loc.cs — yeni anahtar gerekirse TR/EN çiftiyle ekle, Türkçe TAM karakterli)
- COMMIT YAPMA. Sahne KAYDETME. Çıkışta play mode kapalı.

## İşler
1. **Pedestal ölçek küçültme:** EchoPedestal görselleri %30 küçülsün (sprite scale — heykeller/etiketler pedestal'a göre daha okunur olur). [G] bürünme menzilleri bozulmamalı — gerekiyorsa menzil sabit kalsın.
2. **Cyan yönlendirme:** oyuncu spawn'ından pedestal hilaline ve oradan kapıya İNCE cyan zemin izi/işaret dizisi (mevcut sprite'lardan reuse: `Assets/Resources/Props/` veya FloorRiftCrack benzeri; yoksa 2-3px'lik kodla çizilen LineRenderer ipucu — abartısız).
3. **Bürünme VFX:** [G] ile sınıfa bürününce 0.4-0.6s cyan burst + heykelin canlanma flash'ı (basit: SpriteRenderer flash + scale pop + varsa mevcut partikül prefab'ı reuse — yeni partikül sistemi yaratma).
4. **Varış halkası:** oyuncu chamber'a spawn olduğunda ayaklarının altında `arrival_ring` görseli (`STAGING/imagegen/assets/batch3_2026-06-07/arrival_ring.png` → `Assets/Resources/Props/`'a Point/PPU64 import et — TEK istisna import) 0.8s fade+spin.
5. **URP 2D ışık dokunuşu:** kapıda mevcut ışık varsa yoğunlaştır; pedestal hilaline hafif cyan ambient point light(lar) — performans için ≤6 ışık. (URP 2D Light2D; sahne karartması ZATEN varsa dengesini bozma.)

## DOĞRULAMA
- Compile 0 error (read_console)
- Play probe: MainMenu→BAŞLA→chamber → screenshot: `STAGING/_process/2026-06/t5_chamber_atmosphere.png` (halkalar+iz+ışık görünür) + bürünme anı: `t5_attune_vfx.png`
- Mevcut chamber testleri yeşil kalır

## ÇIKTI
`STAGING/_process/2026-06/_done_T5_chamber.md` dosyasına: iş 1-5 DONE/BLOCKED + değişen dosya:satır + 2 screenshot path'i + compile/test durumu.
