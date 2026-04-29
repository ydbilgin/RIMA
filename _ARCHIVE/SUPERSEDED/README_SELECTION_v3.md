# PixelArt Round Select v3

Uretim standardi:
- Her class icin 10 round
- Her round icin 4 varyant uretildi
- Her round'dan otomatik 1 secim yapildi
- Sonuc: class basina 40 uretim + 10 secilmis

Klasor yapisi:
- `<class>/raw/` -> 40 varyant (her varyant icin 512 ve 128 dosya; toplam 80 dosya)
- `<class>/selected10/` -> 10 secilmis 128px sprite
- `<class>/<class>_selected10_sheet.png` -> 10 secimin tek sheet gorunumu

Class listesi:
- warblade
- elementalist
- shadowblade
- ranger
- gunslinger
- hexer
- summoner
- brawler
- ronin
- ravager

Not:
- Uretim ComfyUI SDXL + Pixel Art XL LoRA ile yapildi.
- Ollama bu uretim pipeline'inda kullanilmadi.
