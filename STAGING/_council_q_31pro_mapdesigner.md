# Soru: RIMA Map Designer'ı "işlevsel ve güzel" yapmak — ideal araç tasarımı

RIMA = Unity 6 URP 2D izometrik roguelite. İçerik tamamen veri-güdümlü: RoomTemplateSO asset'leri (26 oda; walkable mask + kapılar + prop listesi + overlay mask) → IsoRoomBuilder runtime'da izometrik yüzen ada olarak kurar (zemin + otomatik cliff + checker + prop). Prop otomatik yerleşimi: Bridson Poisson-disk + kompozisyon rol haritası. Ayrıca yeni "Room Browser" penceresi var: listeden odaya tıkla → _Arena sahnesinde anında kur (sunum/QC için).

Mevcut "Map Designer": 7-sekmeli birleşik editör penceresi (RIMA/Map Designer) — ama ESKİ veri sistemi (RoomData) döneminden kalma; RoomTemplateSO pipeline'ına tam bağlı değil. Kullanıcı yarın sunumda canlı oda düzenleme göstermek istiyor; genel istek: "istediğimiz gibi işlevsel, güzel".

## Senin lensin: DERİN MİMARİ / araç-UX tasarımı
1. Bu projenin İDEAL level-design aracı nasıl görünür? Solo geliştirici + veri-güdümlü oda sistemi için temel iş akışı ne olmalı (template seç → görsel düzenle → kaydet → anında oyunda gör)?
2. Editör penceresi mi, sahne-içi araç mu, ikisi mi? Mevcut parçaların (7-sekme designer, Room Browser, IsoRoomBuilder, auto-placer) tek tutarlı akışta birleşimi nasıl olmalı?
3. Hangi düzenleme yetenekleri birinci sınıf olmalı: walkable/hole boyama, kapı yerleşimi (K/B/D — güney yok kuralı), prop ekle/taşı/sil, auto-placer parametreli çağrı, overlay path boyama, anlık IsoRoomBuilder önizleme?
4. "Güzel" kısmı: bir editör aracında kullanılabilirlik standartları (önizleme thumbnail'ları, undo, dirty-state göstergesi, validasyon uyarıları — güney kapı, ada bütünlüğü, prop taşması)?
5. Aşamalandırma: bir gecede yapılabilir Faz-1 (sunumda etkileyici) vs sonraya Faz-2.

Türkçe, madde madde, somut öneri.
