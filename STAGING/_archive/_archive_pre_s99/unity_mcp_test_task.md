ALLOWED FILES: none (read-only UnityMCP test)

TASK: UnityMCP bağlantısını doğrula.

1. UnityMCP find_gameobjects aracını çağır: tüm sahnedeki GameObject'leri listele (ilk 10 yeterli).
2. read_console aracını çağır: son 5 log satırını al.

Her iki araç da başarıyla çalıştıysa DONE, herhangi biri hata verdiyse BLOCKED.

Commit YAPMA.

REPORT FORMAT:
STATUS: DONE / BLOCKED
COMPLETED:
  - find_gameobjects: <kaç obje bulundu veya hata>
  - read_console: <kaç log satırı veya hata>
ERRORS: NONE / <hata mesajı>
FILES_TOUCHED: none
NEXT_SIGNAL: "UnityMCP test done"
