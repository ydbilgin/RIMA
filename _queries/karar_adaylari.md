# Karar Adayları

```dataview
TABLE 
    tarih AS "Tarih", 
    faz AS "Faz", 
    ozet AS "Özet"
FROM "TASARIM" OR "MEMORY"
WHERE status = "KARAR_ADAYI"
SORT tarih DESC
```
