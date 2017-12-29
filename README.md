# Requirements

You will be asked to build a simple window console application that will manipulate data in CSV files.
This application also allows user to export some their specific reports to the files as well.

## Input files:
```category.csv:``` This file contains the list of product categories

CategoryId: int

CategoryName: string

CategoryDescription: string

DeletedFlag: boolean


```product.csv:``` This file contains the list of products
```
ProductId: int
ProductName: string
Price: double
ProductDescription: string
ExpiredDate: Date
CategoryId: int? (foreign key to category data.This value is nullable)
DeletedFlag: boolean
```
## Functionalities:
- Create a generic method to read the data from CSV files. This method should allow user to use as below
```csharp

    List<Category> listCategories = csvHelper.ReadFromFile<Category>("category.csv");
    List<Product>  listProducts = csvHelper.ReadFromFile<Product>("product.csv");
```
- Create an extension method of List<T> to write data to CSV file. This method should allow user to use as below

```csharp 
    var listProduct = new List<Product>();
    listProduct.Add(new Product());
    listProduct.WriteToFile("product.csv");
```
- Export reports to files (In case the product does not belong to any Category, it's category name should be "Others")
    + Number of products per category --> count_product_per_category.csv.
    Sample output: 
        ```
        CategoryName,ProductCount
        Food,10
        Beverage,15
        Others,5
        ```
    + List products will expire by next month --> product_expire_next_month.csv
    Sample output: 
    ```
        ProductId,ProductName,CategoryName,ExpireDate
        1,Banana,Food,03/01/2017
        2,Milk,Beverage,03/05/2017
        3,Sugar,Others,03/10/2017
        ```
- Create unit tests for all main functions (code coverage > 80%)
- Make your code clean and well structure/format
