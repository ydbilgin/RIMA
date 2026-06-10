# 06 — Hocaya / Rapora Güncel Kapsam Metni

Bu metin hocaya gönderilecek veya raporda kapsam kısmında kullanılabilecek şekilde yazıldı.

```text
Hocam merhaba,

RIMA projesinin bitirme projesi kapsamını tam oyun yerine oynanabilir bir vertical slice olarak netleştirdim.

Projenin uzun vadeli hedefi 2D roguelite aksiyon oyunu olarak daha geniş bir sınıf, oda, boss ve build sistemi içermek. Ancak bitirme projesi kapsamında hedefim, bu sistemlerin temelini gösteren baştan sona oynanabilir kısa bir demo akışı sunmak.

Teslim kapsamında hedeflediğim demo akışı şu şekildedir:

- Oyuncunun Warblade sınıfı ile başlaması
- Oda bazlı ilerleme sistemi
- Combat odalarında düşman spawn ve oda temizleme akışı
- Oda sonunda ability / upgrade draft seçimi
- Basit merchant / ara oda örneği
- Boss karşılaşması
- Boss sonrası secondary class seçimi
- Secondary class seçimi sonrası skill slot / draft sisteminin güncellenmesi
- Ölüm veya demo tamamlanma akışı

Teknik olarak odaklandığım sistemler:

- Unity 2D üzerinde room-run yönetimi
- Data-driven skill / enemy / room yapısı
- Oyuncu combat sistemi
- Class resource ve skill sistemi
- Enemy AI ve boss state machine
- Draft / reward sistemi
- Dual-class prototipi
- Combat feedback, HUD ve temel UI akışları

Yapay zeka destekli araçları görsel fikir geliştirme, referans üretimi, kod inceleme ve iterasyon sürecinde yardımcı araç olarak kullanıyorum. Ancak Unity içindeki sistem entegrasyonu, demo akışı, oynanış testleri ve nihai proje düzenlemesi tarafımdan yürütülmektedir.

Bu kapsamla proje, tam oyun içeriğine yayılmadan; roguelite oda döngüsünü, combat sistemini, boss karşılaşmasını ve dual-class fikrini gösteren oynanabilir bir teknik demo olarak tamamlanacaktır.

Saygılarımla,
Yasin Derya Bilgin
```

## Daha Teknik Sunum Cümlesi

Sunumda şu şekilde özetlenebilir:

```text
RIMA, Unity 6 LTS ile geliştirilen 2D roguelite action-RPG vertical slice prototipidir. Projede oda tabanlı ilerleme, düşman karşılaşmaları, skill draft sistemi, boss state machine ve boss sonrası açılan dual-class progression prototipi yer almaktadır. Sistemler mümkün olduğunca data-driven tasarlanarak ileride yeni sınıf, düşman ve oda eklemeye uygun bir altyapı hedeflenmiştir.
```
