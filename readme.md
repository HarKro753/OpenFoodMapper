# Commands

```bash
head -n 50 en.openfoodfacts.org.products.csv > sample.csv
```

```bash
tail -n +2 en.openfoodfacts.org.products.csv | split -l 50000 -a 2 - part_
```
