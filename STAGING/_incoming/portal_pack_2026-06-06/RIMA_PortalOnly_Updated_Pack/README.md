# RIMA — Portal Only Updated Pack

Bu güncellemenin ana farkı şudur:

- Wall ana çözüm DEĞİL.
- Floor zaten mevcut olduğu için bu pakette floor üretimi öncelik değil.
- Ana runtime yaklaşımı: **floating island + cliff + portal + birkaç prop**.
- Kapı mantığı artık "duvara oturan kapı" değil, **dünya-içi portal**.

Bu paketin odak noktası:
1. Portal için kaç yön/kaç slot gerektiğini netleştirmek
2. Duvarsız veya düşük çevreli room yaklaşımını savunmak
3. Gereken minimum assetleri floor hariç tanımlamak
4. Claude/Codex'e verilecek net üretim brief'i sunmak

## En kısa sonuç
- **Floor üretme**: sende zaten base floor set var.
- **Normal odalarda full wall üretme**.
- **Portal için 1 ana facing direction yeter**.
- **Ama room graph'ta 3 exit slot desteklenmeli**:
  - EXIT_NW
  - EXIT_N
  - EXIT_NE
- **Entry için ayrıca 1 güney giriş noktası yeter**:
  - ENTRY_S

Yani:
- asset yön sayısı açısından portal = **1 ana yön**
- room socket / placement açısından portal = **3 çıkış slotu + 1 giriş slotu**

Bu, hem en okunur hem en ekonomik hem de procedural sistem için en mantıklı çözümdür.
