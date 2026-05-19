import os
from PIL import Image

def split_image_into_64x64(image_path, output_dir, prefix="sprite"):
    """
    Splits a 4x4 grid image into 16 individual 64x64 sprites.
    """
    if not os.path.exists(image_path):
        print(f"HATA: Dosya bulunamadı: {image_path}")
        return
        
    os.makedirs(output_dir, exist_ok=True)
    
    with Image.open(image_path) as img:
        img_width, img_height = img.size
        # 4x4 grid olduğunu varsayıyoruz
        cell_width = img_width // 4
        cell_height = img_height // 4
        
        for row in range(4):
            for col in range(4):
                left = col * cell_width
                upper = row * cell_height
                right = left + cell_width
                lower = upper + cell_height
                
                # Hücreyi kes
                cell_img = img.crop((left, upper, right, lower))
                
                # Eğer tam 64x64 değilse, Nearest Neighbor ile 64x64'e ölçekle
                if cell_img.size != (64, 64):
                    cell_img = cell_img.resize((64, 64), Image.Resampling.NEAREST)
                    
                # Dosyayı kaydet
                out_path = os.path.join(output_dir, f"{prefix}_satir{row+1}_sutun{col+1}.png")
                cell_img.save(out_path, "PNG")
                print(f"Kaydedildi: {out_path}")

if __name__ == "__main__":
    # KULLANIM:
    # Bu scripti çalıştırmadan önce aşağıdaki dosya yollarını 
    # indirdiğiniz resimlerin konumuna göre güncelleyin.
    
    # ÖRNEK:
    # grid1_yolu = r"f:\Antigravity Projeler\2d roguelite\RIMA\STAGING\ilk_resim.png"
    # split_image_into_64x64(grid1_yolu, r"f:\Antigravity Projeler\2d roguelite\RIMA\STAGING\Grid1_Parcalar", "grid1")
    
    print("Script hazır. Lütfen dosya yollarını script içine yazıp çalıştırın!")
