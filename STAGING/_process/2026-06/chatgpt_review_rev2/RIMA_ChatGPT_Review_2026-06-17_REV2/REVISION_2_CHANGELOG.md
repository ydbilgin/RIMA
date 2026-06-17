# Revision 2 Changelog — Build Mode Interpretation Correction

**Tarih:** 2026-06-17  
**Neden:** İlk inceleme, Build Mode'daki geniş diamond grid'i kısmen genel bir UI/layout sorunu gibi yorumladı. Bu yorum eksikti.

## Düzeltilen ana karar

Build Mode'un eğik ve oda dışına devam eden çalışma düzlemi, RIMA'nın **128×64 isometric diamond tile** üretim mantığının doğal sonucudur. Amaç yalnız mevcut odanın içinde prop seçmek değil; isometric hücreleri okuyarak zemini genişletmek, floor/walkable/overlay katmanlarına tile çizebilmek ve mevcut oda sınırının dışında yeni alan oluşturabilmektir.

Bu nedenle:

- Diamond world-space grid **korunacak**.
- Grid mevcut floor şeklinin dışına devam edebilir.
- Siyah/boş editör çalışma alanı tek başına hata sayılmayacak.
- Grid düzleştirilmeyecek, screen-space kare gride çevrilmeyecek.
- Grid yalnız mevcut room bounds içine kırpılmayacak.
- Build Mode zorunlu olarak yeni Director layout'una taşınmayacak.

## Gerçek iyileştirme kapsamı

Build Mode için karar artık **redesign değil, UX polish pass**:

- grid kontrastı ve görünürlük seviyeleri,
- hover edilen hücre,
- footprint preview,
- geçerli/geçersiz yerleştirme geri bildirimi,
- aktif layer/tool bilgisi,
- seçili asset/prop vurgusu,
- undo/redo ve working-copy durumunun görünür olması,
- tekrar eden panel/tab hiyerarşisinin sadeleştirilmesi.

## Pakette değişenler

- `05_BUILD_MODE_REDESIGN.md` kaldırıldı.
- Yerine `05_BUILD_MODE_UX_POLISH.md` eklendi.
- Yanlış yönlendirebilecek `visuals/build_mode_proposed_layout.png` kaldırıldı.
- `00`, `01`, `03`, `04`, `07`, `09`, `DECISIONS.json` ve `MANIFEST.md` güncellendi.
- Build Mode işi P0 yapısal redesign olmaktan çıkarılıp **P1 kontrollü polish** olarak sınıflandırıldı.
