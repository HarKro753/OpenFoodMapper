# Commands

```bash
head -n 50 en.openfoodfacts.org.products.csv > sample.csv
```

```bash
tail -n +2 en.openfoodfacts.org.products.csv | split -l 50000 -a 2 - part_
```

```bash
dotnet restore OpenFoodUploader.csproj
```

## Production

```bash
cd src
Production__ConnectionString="Host=192.168.178.186;Port=5432;Database=cibrusprod;Username=myuser;Password=1234" ASPNETCORE_ENVIRONMENT=Production dotnet ef database update 20260113122104_optional
```

## Staging

```bash
cd src
Staging__ConnectionString="Host=192.168.178.186;Port=5432;Database=cibrustest;Username=myuser;Password=1234" ASPNETCORE_ENVIRONMENT=Staging dotnet ef database update 20260113122104_optional
```

{"@t":"2026-01-15T12:21:55.3958793Z","@mt":"Completed processing batch with {ProductCount} products, {CategoryCount} categories, {AdditiveCount} additives, {IngredientCount} ingredients, {CountryCount} countries, {AllergenCount} allergens, {FoodGroupCount} food groups, {LabelCount} labels","ProductCount":1000,"CategoryCount":224,"AdditiveCount":62,"IngredientCount":0,"CountryCount":975,"AllergenCount":0,"FoodGroupCount":153,"LabelCount":154}

- ingredients, alltergenes
