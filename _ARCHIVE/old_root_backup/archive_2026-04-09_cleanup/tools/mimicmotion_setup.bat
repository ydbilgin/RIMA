@echo off
echo ============================================
echo  MimicMotion Kurulum Scripti
echo  RTX 5080 + Python 3.10+ gerektirir
echo ============================================
echo.

cd /d "F:\Antigravity Projeler\2d roguelite"

REM MimicMotion repo klon
if not exist "tools\MimicMotion" (
    echo [1/5] MimicMotion indiriliyor...
    git clone https://github.com/tencent/MimicMotion tools\MimicMotion
) else (
    echo [1/5] MimicMotion zaten mevcut, guncelleniyor...
    cd tools\MimicMotion && git pull && cd ..\..
)

echo.
echo [2/5] Python ortami kuruluyor...
cd tools\MimicMotion
python -m pip install -r requirements.txt

echo.
echo [3/5] PyTorch CUDA kuruluyor (RTX 5080 icin cu128)...
pip install torch torchvision torchaudio --index-url https://download.pytorch.org/whl/cu128

echo.
echo [4/5] Ek bagimliliklar...
pip install accelerate omegaconf einops

echo.
echo [5/5] Model dosyalari indiriliyor (~15GB, bekleniyor)...
python -c "
from huggingface_hub import snapshot_download
print('SVD model indiriliyor...')
snapshot_download('stabilityai/stable-video-diffusion-img2vid-xt', local_dir='../../models/svd')
print('MimicMotion checkpoint indiriliyor...')
snapshot_download('tencent/MimicMotion', local_dir='../../models/mimicmotion')
print('Tamamlandi.')
"

echo.
echo ============================================
echo  Kurulum tamamlandi!
echo  Kullanim: python tools/mimicmotion_run.py
echo ============================================
pause
