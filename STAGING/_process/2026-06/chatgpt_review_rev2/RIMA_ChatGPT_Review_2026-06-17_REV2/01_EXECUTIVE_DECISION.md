# 1 — Executive Decision REV2

## Genel hüküm

Paket, **çalışan bir teknik demo** gösteriyor; henüz **görsel olarak bitmiş bir oyun veya profesyonel tool showcase** göstermiyor. Runtime assert'leri güven veriyor, fakat ekran görüntülerinin bir bölümü yanlış yakalanmış ve bazı ekranlar placeholder seviyesinde.

## Korunacaklar

- Build Mode'un mevcut isometric world-space çalışma düzlemi
- 128×64 diamond tile yerleşim mantığı ve gridin mevcut floor dışına uzanabilmesi
- Build working-copy izolasyonu, undo/redo, prop ve tile placement çekirdeği
- Director spawn/stat/telemetry altyapısı
- Ana menü arka planı ve genel RIMA renk yönü
- Draft'ın üç kartlı temel yerleşimi
- Codex'in bilgi mimarisi temeli
- Oda tile setinin genel kimliği

## Demo öncesi değişmesi gerekenler

- Director Mode'un görsel kabuğu ve hiyerarşisi
- Build Mode'da grid kontrastı, hover/footprint ve placement feedback
- HUD ölçeği
- Boss anchor/scale/health bar/intro sunumu
- Siyah blob gibi görünen sprite ve sahne objeleri
- Skill synergy metinleri
- Yanlış veya eksik screenshot capture'ları

## Claude'un izlenimine tepkim

| Claude tezi | Kararım | Neden |
|---|---|---|
| Director Mode en zayıf halka | **Katılıyorum** | Sorun yalnız turuncu wireframe değil; viewport'u boğan paneller, minik font ve görev hiyerarşisinin belirsizliği asıl problem. |
| Build Mode fonksiyonel ama grid/palet zayıf | **Kısmen katılıyorum** | Palet ve feedback zayıf. Fakat diamond gridin eğik olması ve mevcut odanın dışına devam etmesi kusur değil; isometric tile authoring için bilinçli çalışma düzlemidir. Yapısal redesign değil UX polish gerekir. |
| Combat'ta siyah blob ve void ucuz gösteriyor | **Katılıyorum, genişletiyorum** | Düz zemin, üst siyah bant, ışık dağılımı ve iç detaysız silüetler de aynı hissi üretiyor. |
| HUD fazla minimal | **Kesinlikle katılıyorum** | Mevcut 1080p çıktıda HP/resource bilgisi pratikte okunmuyor. |
| Codex profesyonel | **Kısmen katılıyorum** | En düzgün bilgi mimarilerinden biri; yine de küçük metin ve zayıf seçili durum nedeniyle final değil. |
| Boss atmosferik ve güçlü | **Katılmıyorum** | Boss HUD ile çakışıyor, bar placeholder gibi ve odada merchant kalıntıları var. Harita atmosferik; boss sunumu henüz değil. |
| Merchant güçlü | **Katılmıyorum** | İşlevsel prototip; ürünler hâlâ renkli kare ve havada yazı seviyesinde. |
| Draft düzeldi | **Kısmen katılıyorum** | Üç kart hiyerarşisi anlaşılır fakat synergy cümleleri mekanik sonucu açıklamıyor. |

## Build Mode için düzeltilmiş yorum

Build Mode ekranında görülen büyük diamond grid:

- isometric hücreleri doğru okumak,
- floor/walkable/overlay katmanlarını boyamak,
- mevcut oda şeklinin dışında yeni zemin üretmek,
- prop footprint'ini gerçek tile koordinatına oturtmak

için gereklidir.

Bu nedenle gridin viewport boyunca devam etmesi veya siyah çalışma alanında görünmesi tek başına hata değildir. Hata sayılabilecek şey, gridin aktif sanatı bastırması veya kullanıcıya hangi hücre/layer/footprint üzerinde çalıştığını yeterince anlatmamasıdır.

## Asıl sistemik problem

Ekranlar üç farklı olgunluk seviyesinde karışıyor:

1. **Oyun artı:** menü arka planı, dungeon tile seti
2. **Fonksiyonel UI:** settings, draft, Codex, Build Mode çekirdeği
3. **Debug/placeholder:** Director chrome, boss bar, merchant kareleri, bazı silüetler

Demo öncesindeki hedef bütün sistemi yeniden çizmek değil; üçüncü grubu ikinci gruba taşımak ve Build Mode gibi zaten doğru temele sahip araçları yanlışlıkla yeniden tasarlamamaktır.

## Canon kararı

- Proje tonu: **Fractured Epic**, dramatik ve canlı, grimdark değil.
- Build grid: **isometric diamond world-space authoring grid**, korunacak.
- Director arayüzü: profesyonel runtime editor düzenine geçirilecek.
- Eski ultra-minimal HUD ölçüleri demo için kullanılmayacak.
