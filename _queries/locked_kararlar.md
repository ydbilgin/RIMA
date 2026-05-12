# Locked Kararlar

```dataview
TABLE faz AS "Faz", ozet AS "Özet", tarih AS "Tarih"
FROM "TASARIM" OR "MEMORY"
WHERE status = "LOCKED"
SORT faz ASC
```
