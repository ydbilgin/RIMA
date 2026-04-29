@echo off
chcp 65001 > nul
echo ================================================
echo  RIMA - ComfyUI Kurulum (RTX 5080 / Blackwell)
echo ================================================
echo.

:: ─── 1. ComfyUI klonla ───────────────────────────────────────────────────────
if exist "F:\ComfyUI\.git" (
    echo [1/6] ComfyUI zaten mevcut, guncelleniyor...
    cd /d F:\ComfyUI
    git pull
) else (
    echo [1/6] ComfyUI klonlaniyor...
    git clone https://github.com/comfyanonymous/ComfyUI F:\ComfyUI
    cd /d F:\ComfyUI
)

:: ─── 2. PyTorch kur (RTX 5080 Blackwell - CUDA 12.8) ─────────────────────────
echo.
echo [2/6] PyTorch kuruluyor (CUDA 12.8 - RTX 5080)...
echo     NOT: Bu indirme buyuk olabilir (~2.5 GB), bekleyin...
pip install torch torchvision torchaudio --index-url https://download.pytorch.org/whl/cu128 --quiet
if %errorlevel% neq 0 (
    echo     cu128 bulunamadi, cu124 deneniyor...
    pip install torch torchvision torchaudio --index-url https://download.pytorch.org/whl/cu124 --quiet
)

:: ─── 3. ComfyUI gereksinimleri ────────────────────────────────────────────────
echo.
echo [3/6] ComfyUI gereksinimleri kuruluyor...
pip install -r F:\ComfyUI\requirements.txt --quiet

:: ─── 4. Diffusers + uretim kutuphaneleri ─────────────────────────────────────
echo.
echo [4/6] Diffusers ve AI kutuphaneleri kuruluyor...
pip install diffusers transformers accelerate safetensors --quiet
pip install huggingface_hub google-genai pillow tqdm sentencepiece --quiet

:: ─── 5. Custom nodes ─────────────────────────────────────────────────────────
echo.
echo [5/6] Custom nodes kuruluyor...
cd /d F:\ComfyUI\custom_nodes

if not exist "ComfyUI-Manager" (
    git clone https://github.com/ltdrdata/ComfyUI-Manager --quiet
    echo     [OK] ComfyUI-Manager
) else (
    echo     [ATLA] ComfyUI-Manager zaten var
)

if not exist "ComfyUI-AnimateDiff-Evolved" (
    git clone https://github.com/Kosinkadink/ComfyUI-AnimateDiff-Evolved --quiet
    echo     [OK] AnimateDiff-Evolved
) else (
    echo     [ATLA] AnimateDiff-Evolved zaten var
)

if not exist "ComfyUI_IPAdapter_plus" (
    git clone https://github.com/cubiq/ComfyUI_IPAdapter_plus --quiet
    echo     [OK] IPAdapter Plus
) else (
    echo     [ATLA] IPAdapter Plus zaten var
)

if not exist "comfyui-nodes-base" (
    git clone https://github.com/Suzie1/ComfyUI_Comfyroll_CustomNodes --quiet
    echo     [OK] Comfyroll (sprite sheet utils)
) else (
    echo     [ATLA] Comfyroll zaten var
)

:: ─── 6. Model klasor yapisi ──────────────────────────────────────────────────
echo.
echo [6/6] Klasor yapisi olusturuluyor...
mkdir F:\ComfyUI\models\checkpoints 2>nul
mkdir F:\ComfyUI\models\loras 2>nul
mkdir F:\ComfyUI\models\animatediff_models 2>nul
mkdir F:\ComfyUI\models\ipadapter 2>nul
mkdir F:\ComfyUI\models\clip_vision 2>nul
mkdir F:\ComfyUI\hf_cache 2>nul

:: ─── Sonuc ───────────────────────────────────────────────────────────────────
echo.
echo ================================================
echo  Kurulum tamamlandi!
echo ================================================
echo.
echo  Simdi siradaki adim:
echo  python tools\rima_models.py
echo  (Modelleri indirir - ~8 GB, bir kez yapilir)
echo.
echo  ComfyUI web UI icin:
echo  cd F:\ComfyUI
echo  python main.py
echo  Tarayici: http://127.0.0.1:8188
echo.
pause
