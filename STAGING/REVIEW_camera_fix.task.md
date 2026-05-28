ACTIVE RULES: (1) think before coding (2) min code (3) surgical (4) BLOCKED if unclear.
RESPOND INLINE — dosya yazma.

# Amaç
Opus'un yazdığı KAMERA-TAKİP fix'ini review et. Bug: kamera karakteri takip etmiyordu. Kök neden: CameraPunchController her frame `cam.transform.localPosition = originalLocalPos + currentOffset` yazıp kamerayı yakalanan origin'e PINLİYOR, CameraFollow (SmoothDamp) ile kavga ediyordu (velocity 66 birikiyor, pozisyon origin'de sıkışıyor).

# Fix (Opus, 2 dosya — OKU)
1. Assets/Scripts/Combat/Juice/CameraPunchController.cs — transform yazımı KALDIRILDI; `public Vector3 CurrentOffset` expose edildi (CameraShake pattern'i). LateUpdate sadece offset decay eder.
2. Assets/Scripts/Camera/CameraFollow.cs (RIMA.CameraSystem) — base pozisyonu ayrı SmoothDamp; shake+punch offset'i ÜSTÜNE eklenir (`_basePos + fx`); target null ise Player auto-find.

# Sorular (AGREE/DISAGREE + gerekçe)
1. Bu fix kök nedeni doğru çözüyor mu? Kamera artık takip eder + punch/shake juice yine çalışır mı?
2. `_basePos` ayrı tutup fx'i üstüne eklemek doğru mu (feedback-loop yok mu)? SmoothDamp base'i kirletmiyor mu?
3. Edge: punch/shake offset Pixel Perfect ile titreşim/jitter yaratır mı? (PPC şu an DISABLED — ayrı sorun, not düştüm.)
4. Robust RIMA.CameraFollow (Player/CameraFollow.cs) duplicate var, punch'ı okumuyor — dokunmadım. Risk mi, yoksa kullanılmadığı için OK mi?
5. Gözden kaçan: combat'ta hit/kill/dash punch artık görünür mü (offset follow'a ekleniyor)?

# Çıktı: 5 soruya kısa yanıt + "EN KRİTİK 1 BULGU".
