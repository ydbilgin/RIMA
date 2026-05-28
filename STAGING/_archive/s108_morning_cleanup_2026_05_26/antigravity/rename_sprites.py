import os
import shutil

base_dir = r"f:\Antigravity Projeler\2d roguelite\RIMA\STAGING\antigravity"
out_dir = os.path.join(base_dir, "Tum_Karakterler_Isimlendirilmis")
os.makedirs(out_dir, exist_ok=True)

g1_dir = os.path.join(base_dir, "Grid1_Sprites")
g2_dir = os.path.join(base_dir, "Grid2_Sprites")

def get_src(grid, r, c):
    folder = g1_dir if grid == 1 else g2_dir
    return os.path.join(folder, f"grid{grid}_satir{r}_sutun{c}.png")

def copy_rename(grid, r, c, new_name):
    src = get_src(grid, r, c)
    dst = os.path.join(out_dir, f"{new_name}.png")
    if os.path.exists(src):
        shutil.copy2(src, dst)
        print(f"Kopyalandı: {new_name}.png")
    else:
        print(f"Bulunamadı: {src}")

mappings = [
    # ANA 10 SINIF (PRIMARY)
    (1, 1, 1, "01_Warblade_Ana"),
    (2, 2, 2, "02_Brawler_Ana"),
    (1, 1, 4, "03_Ravager_Ana"),
    (2, 2, 4, "04_Ranger_Ana"),
    (2, 2, 3, "05_Shadowblade_Ana"),
    (2, 3, 2, "06_Gunslinger_Ana"),
    (1, 1, 3, "07_Ronin_Ana"),
    (1, 1, 2, "08_Elementalist_Ana"),
    (2, 2, 1, "09_Hexer_Ana"),
    (2, 3, 1, "10_Summoner_Ana"),
    
    # ALTERNATİFLER VE NPC'LER
    (1, 4, 4, "Alternatif_Warblade_veya_Tempest_Zirhli_Kadin"),
    (1, 4, 1, "Alternatif_Brawler_Kesis_Kel"),
    (1, 4, 3, "Alternatif_Ranger_Yasli_Avci"),
    (1, 4, 2, "Alternatif_Shadowblade_Mor_Kapsonlu"),
    (1, 3, 4, "Alternatif_Gunslinger_Kirmizi_Sackuyrugu"),
    (2, 4, 4, "Alternatif_Elementalist_Doga_Yalinayak"),
    (2, 4, 1, "NPC_GuildMaster_Gumus_Sacli"),
    (2, 3, 3, "NPC_Demirci_Kahverengi_Yelek"),
    (1, 3, 3, "NPC_YetenekHocasi_Yasli_Kemerli"),
    (2, 3, 4, "NPC_GorevVeren_KisaSac_Paltolu"),
    (2, 4, 2, "NPC_Sari_SivriSacli_Genc"),
    (2, 4, 3, "Kullanilmayan_Modern_Gri_Kapsonlu"),
]

# Kalan tüm dosyaları da "Unassigned_" olarak kopyalayalım
processed = set()
for grid, r, c, name in mappings:
    copy_rename(grid, r, c, name)
    processed.add((grid, r, c))

for g in [1, 2]:
    for r in range(1, 5):
        for c in range(1, 5):
            if (g, r, c) not in processed:
                copy_rename(g, r, c, f"Yedek_Grid{g}_Satir{r}_Sutun{c}")

print("İşlem tamamlandı!")
