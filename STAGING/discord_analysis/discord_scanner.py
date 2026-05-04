import time
import random
import os
import sys
import mss
import pyautogui
from datetime import datetime

def main():
    if len(sys.argv) < 3:
        print("Kullanım: python discord_scanner.py <kanal_adi> <kaydirma_sayisi>")
        print("Örnek: python discord_scanner.py mcp-and-vibe-coding 20")
        sys.exit(1)

    channel_name = sys.argv[1]
    try:
        num_scrolls = int(sys.argv[2])
    except ValueError:
        print("Hata: Kaydırma sayısı bir sayı olmalıdır.")
        sys.exit(1)

    base_dir = os.path.join("STAGING", "discord_analysis", channel_name)
    screenshots_dir = os.path.join(base_dir, "screenshots")
    
    if not os.path.exists(screenshots_dir):
        os.makedirs(screenshots_dir, exist_ok=True)

    print(f"[{channel_name}] kanalı için tarama başlatılıyor...")
    print("!!! LÜTFEN ŞİMDİ DİSCORD PENCERESİNİ AÇIN VE ODAKLAYIN !!!")
    print("5 saniye içinde tarama başlayacaktır...")
    
    for i in range(5, 0, -1):
        print(f"Başlıyor: {i}...")
        time.sleep(1)

    print("\n--- TARAMA BAŞLADI (Durdurmak için terminale tıklayıp CTRL+C yapabilirsiniz) ---")

    with mss.mss() as sct:
        for i in range(num_scrolls):
            # Ekran görüntüsü al (Birincil monitör)
            timestamp = datetime.now().strftime("%Y%m%d_%H%M%S")
            filename = f"capture_{i+1:03d}_{timestamp}.png"
            screenshot_path = os.path.join(screenshots_dir, filename)
            
            # Monitör 1'i çek
            monitor = sct.monitors[1]
            sct_img = sct.grab(monitor)
            mss.tools.to_png(sct_img.rgb, sct_img.size, output=screenshot_path)
            
            print(f"[{i+1}/{num_scrolls}] Ekran kaydedildi: {filename}")

            # Yukarı kaydır (PageUp, Discord'da geçmiş mesajları yüklemek için harikadır)
            pyautogui.press('pageup')
            
            # Rastgele bekleme süresi (insan taklidi ve görsellerin yüklenmesi için)
            wait_time = random.uniform(2.5, 4.5)
            print(f"-> {wait_time:.1f} saniye bekleniyor...")
            time.sleep(wait_time)

    print("\n--- TARAMA TAMAMLANDI ---")
    print(f"Tüm görüntüler şu klasöre kaydedildi: {screenshots_dir}")

if __name__ == "__main__":
    main()
