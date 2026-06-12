# Gorev: Guncel-isikli sahne screenshot'lari (UnityMCP ile) — review icin

Tek bir RIMA Unity instance acik (Editor), _Arena sahnesi yuklu, idle, play mode'da DEGIL.
UnityMCP araclarini DOGRUDAN kullan. Birden fazla instance gorursen mcpforunity://instances ile
listele + set_active_instance "RIMA" yap.

## Amac
Bu session'da yeni bir isik kurulumu uygulandi (sahne global 0.22 + player hero follow-light + ember/cyan
point-light'lar). Bu YENI isikli halin guncel oyun-ici screenshot'larini al (ChatGPT review icin).

## Adimlar
1. manage_editor action=play → play mode'a gir.
2. manage_camera action=screenshot, output_folder='Assets/Screenshots',
   screenshot_file_name='current_01_arena_combat_lit', include_image=false → ilk combat odasini cek.
   (Beklenti: moody/karanlik zemin + oyuncu etrafinda sicak hero-light havuzu + cliff'lerde cyan.)
3. 1-2 ek kare daha al (current_02_arena, current_03_arena) — farkli an/kamera olursa.
4. manage_editor action=stop → play'den cik.
5. manage_scene action=load ile build settings'teki MainMenu sahnesini yukle →
   manage_camera screenshot current_10_mainmenu.
6. CharacterSelect (chamber) sahnesini yukle → screenshot current_11_charselect.
7. _Arena sahnesini geri yukle (manage_scene load Assets/Scenes/_Arena.unity).

## KISIT
- Hicbir asset/script/sahne DEGISTIRME, SADECE screenshot al ve sahne yukle.
- Islem bitince Unity'yi idle/stop birak (play mode kapali).

## Cikti (final mesaj)
- Aldigin TUM screenshot dosya yollarinin listesi.
- Her screenshot icin 1 cumle isik degerlendirmesi: zemin moody mi, oyuncu etrafinda sicak hero-pool
  gorunuyor mu, ember (sicak turuncu) ve cyan glow var mi, okunaklilik nasil.
