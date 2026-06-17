# AX_PRO VISION VERDICT: Greatsword Mount Tuning
**Tarih:** 2026-06-18

## 1. Idle Duruşu & SE Yönü İçin Somut Değerler
Büyük kılıç (greatsword) idle durumunda thrust yapar gibi karakterin baktığı yöne uzanmamalı; ağırlığından dolayı yere doğru (drag) sarkıtılmalı veya sırtta (shoulder) dinlenmelidir.
**SE (Sağ-Aşağı) yönü için önerilen değerler:**
*   `anchorOffset`: **(0.02, 0.15)** (Yüksekliği ele indirir)
*   `handOffsets[SE]`: **(-0.08, -0.04)** (Kılıcı sağ ele alır)
*   `weaponRotations[SE]`: **-135** (Kılıç ileri SE'ye değil, sağ yanda aşağı SW'ye bakar)

## 2. Sağ El Kopukluk Sorunu (Offset Yönü)
Karakter SE yönüne bakarken gerçek sağ eli ekranın **sol** tarafındadır. Mevcut `handOffsets[SE]` X değeri **pozitif (+0.08)** olduğu için kılıç karakterin sol el hizasında (ekranın sağında) ve havada kalıyor. Kabzanın sağ ele oturması için X offset'i **negatif (-0.08)** olmalıdır.

## 3. 8 Yön Eğilimi ve Düzeltme Tablosu
Mevcut durumda silah sürekli bakılan yöne dönük (thrust). İdeal idle pozisyonu için silah her zaman "sağ elde yanda/arkada" yere dönük olmalıdır:

| Yön | Sağ El Offset (X,Y) | Rotation (Yön) | Görünüm Mantığı | Sort |
| :--- | :--- | :--- | :--- | :--- |
| **S** | (-0.08, -0.04) | 135 (NW) | Sırtta / Arkaya sürüklenir | Behind |
| **SE** | (-0.08, -0.04) | -135 (SW) | Sağ elde, sola aşağı sarkıtılır | Front |
| **E** | (-0.04, -0.04) | -135 (SW) | Sağ el arkada, kılıç sürüklenir | Behind |
| **NE** | (0.08, -0.04) | -45 (SE) | Sağ elde, sağa aşağı sarkıtılır | Front |
| **N** | (0.08, -0.04) | -45 (SE) | Arkaya sağa doğru sarkıtılır | Behind |
| **NW** | (0.08, -0.04) | -45 (SE) | Sağ elde, sağa aşağı sarkıtılır | Behind |
| **W** | (0.04, -0.04) | -45 (SE) | Sağ el arkada, kılıç sürüklenir | Behind |
| **SW** | (-0.08, -0.04) | -135 (SW) | Sağ elde, sola aşağı sarkıtılır | Front |
*(W, NW, SW için kılıcın sırt yönüne göre flipY=true mantığı korunabilir)*

## 4. anchorOffset.y = 0.33 Analizi
Değer **çok yüksek**. Karakterin pivotu ayaklarda ise, 0.33 değeri kabzayı göğüs/bel hizasına çıkarıyor. Closeup görselinde sağ el diz hizasındayken kılıç havada kalmış. Eli tam bulması için `anchorOffset.y` değeri **0.15** ile **0.18** aralığına indirilmelidir.
