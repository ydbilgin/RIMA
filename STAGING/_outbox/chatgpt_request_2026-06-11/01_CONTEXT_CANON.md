# RIMA — Act 1 "Shattered Keep" KANON (NotebookLM canonical, 2026-06-11)

## Lore
Shattered Keep = bir zamanlar düzenli (muhafız salonları, hücreler, tapınaklar) antik bir yapı; dünyaları tüketen **Rift March**'ı durdurmak için "The Architect" tarafından KASITLI parçalandı (**The Fracturing**). Erken odalar yapıyı, geç odalar **yarayı** gösterir. Felsefe: **"Vivid Vulnerability"** (canlı kırılganlık). Duvarsız, havada süzülen izometrik taş adalar (Hades Elysium tarzı).

## RENK PALETİ (KESİN — bu hex'leri kullan)
- **Zemin/Yapı:** koyu arduvaz/granit — **Slate `#3A3D42`**. Zemin ASLA açık/parlak değil. Nötr çizilir; ışık/gölge Unity dinamik Light2D ile (bake YOK).
- **Void/Arka plan derinlik:** **Deep Purple `#3A1A4A`** → siyaha. Sonsuz boşluk. (MAVİ DEĞİL — MOR.)
- **Aksan 1 — Cyan Rift:** **Neon Cyan `#00FFCC`**. Kırık mühürler + sızan büyü = emissive çatlak/rün. **Ekranın ≤ %15'i** (asla daha fazla). Adaların void'e bakan dış kenarlarında ince cyan rim-light.
- **Aksan 2 — Warm Ember:** **Warm Orange `#E89020`**. SADECE mangal/meşale. Cyan ile dramatik zıtlık.

## IŞIK HİSSİ
Genel ortam loş + soğuk (**intensity ~0.22**). Odak = sıcak mangal + cyan rün parlamaları. Adanın alt uçurum yüzeyleri yerel ışık ALMAZ → karanlığa/void'e kaybolur (drop-shadow + gradient = derinlik). **Void "unlit"tir** — savaş patlamaları arka planı aydınlatıp "karton dekor"a çevirmez.

## VOID vs RIFT
- **Void (mor/siyah):** gerçekliğin kesilip atıldığı yutucu uçurum. Boşluk/yutuluş hissi.
- **Rift (cyan):** yaşayan, dengesiz kozmik enerji — hem tehdit hem mühür-çatlağı; neon parlaklık = yıkım ortasındaki umut kıvılcımı (oyuncu gücü).

## "TASARLANMIŞ ODA" KANONİK KURALLARI
1. **Dövüş alanı temiz, detay "Visual Shell"de:** oynanabilir zemin her zaman temiz/okunur; tüm görsel karmaşa+hikaye = zemini saran duvar-kalıntısı, uçurum kenarı, heykel, devasa zincir demirleri.
2. **Random dağılım YASAK (anchored):** detay mantıksal kaynağa bağlı — çatlaklar bir **Rift yırtığından dışa yayılır**, moloz **yıkık sütun dibinde** birikir. Erozyon SADECE adanın void'e bakan dış kenarında.
3. **Zemin şişirme yok:** kamera kenarını kapatmak için devasa yürünebilir padding YOK. Arka/Kuzey'e kalın mimari (8-12 tile duvar), ön/Güney'e alçak engel. Sınır = uçurumlar.
4. **Görsel gürültü ≤ %15:** zemin decor (çatlak/kan/yosun) yoğunluğu 640×360'ta okunurluğu bozmayacak şekilde asla %15'i geçmez.
5. **Merkez daima temiz** (özellikle Elite/Boss): merkezde büyük yapı yok, engeller kenarda (flank). Her odada ≥2 temiz dash kulvarı.

## SANAT YÖNÜ
High top-down 3/4 (~70-80°, Hades/Children of Morta). Oyun pixel-art (PixelLab). Arka plan FAR/parallax olduğu için painterly-moody KABUL (low-detail, arkada kalır) ama palet/lore'a uy.
