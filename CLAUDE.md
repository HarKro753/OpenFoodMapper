## Database

- We use Entity Framework for handeling our Database All Database Models are defined under /Database/Models/
- The DatabaseContext.cs is definder under /Database/DatabaseContext.cs
- Note that the Database already already is intact. It already holds data. But it also has constraints in form of primary keys ... defined so that we dont need to manually check a lot of stuff e.g. we dont need to check for duplicates before inserting the database should handle that

## Data

- We work with a dataset from OpenFood which consists of a giant CSV file, which we have split into multiple parts: part_aa.csv ... part_bp.csv
- The Dataset contains no Headers the columns however are defined under CsvSchema.cs

## Purpose

- The Purpose of this Script is to map Data from the large csv DataSet into our Database.
- The script is supposed to always map all the Data into the Database.
- The script must be quick utilizing parallel computing to split the task into multiple subtasks
- The script must give use a way to always fill up the data in our desired schema from scratch and work with already established databases
- That is why we dont delete Data we would only try to upsert
- Our general approach is to map fields in the CSV which are defined as strings separated by commas into separate relations so that we can work more easily with them
