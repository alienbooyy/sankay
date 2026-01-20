using SankayPOS.Database;

namespace SankayPOS.Utils;

public static class SampleDataInitializer
{
    public static void InitializeSampleData()
    {
        // Check if data already exists
        var existingProducts = DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM Products");
        if (existingProducts != null && Convert.ToInt32(existingProducts) > 0)
            return; // Data already exists
        
        // Add sample raw materials
        var rawMaterialRepo = new RawMaterialRepository();
        rawMaterialRepo.AddRawMaterial("Kıyma", "gr", 5000, 1000, 0.15m);
        rawMaterialRepo.AddRawMaterial("Un", "gr", 10000, 2000, 0.02m);
        rawMaterialRepo.AddRawMaterial("Domates Sosu", "ml", 3000, 500, 0.05m);
        rawMaterialRepo.AddRawMaterial("Peynir", "gr", 2000, 500, 0.20m);
        rawMaterialRepo.AddRawMaterial("Zeytin", "gr", 1500, 300, 0.08m);
        rawMaterialRepo.AddRawMaterial("Ayran (Süt)", "ml", 5000, 1000, 0.03m);
        rawMaterialRepo.AddRawMaterial("Kola", "ml", 3000, 500, 0.10m);
        rawMaterialRepo.AddRawMaterial("Patates", "gr", 4000, 1000, 0.04m);
        rawMaterialRepo.AddRawMaterial("Tavuk", "gr", 3000, 800, 0.12m);
        rawMaterialRepo.AddRawMaterial("Soğan", "gr", 2000, 500, 0.03m);
        
        // Add sample products
        var productRepo = new ProductRepository();
        productRepo.AddProduct("Lahmacun", 35.00m, "Ana Yemek");
        productRepo.AddProduct("Pide Karışık", 85.00m, "Ana Yemek");
        productRepo.AddProduct("Pide Peynirli", 70.00m, "Ana Yemek");
        productRepo.AddProduct("Pizza Karışık", 95.00m, "Ana Yemek");
        productRepo.AddProduct("İskender", 120.00m, "Ana Yemek");
        productRepo.AddProduct("Tavuk Şiş", 100.00m, "Ana Yemek");
        productRepo.AddProduct("Adana Kebap", 110.00m, "Ana Yemek");
        productRepo.AddProduct("Patates Kızartması", 40.00m, "Yan Ürün");
        productRepo.AddProduct("Ayran", 10.00m, "İçecek");
        productRepo.AddProduct("Kola", 15.00m, "İçecek");
        productRepo.AddProduct("Fanta", 15.00m, "İçecek");
        productRepo.AddProduct("Soda", 8.00m, "İçecek");
        productRepo.AddProduct("Su", 5.00m, "İçecek");
        productRepo.AddProduct("Çay", 5.00m, "İçecek");
        productRepo.AddProduct("Türk Kahvesi", 20.00m, "İçecek");
        
        // Add sample recipes
        var recipeRepo = new RecipeRepository();
        
        // Lahmacun recipe (Product ID 1)
        recipeRepo.AddRecipe(1, 1, 50);  // 50gr Kıyma
        recipeRepo.AddRecipe(1, 2, 120); // 120gr Un
        recipeRepo.AddRecipe(1, 3, 30);  // 30ml Domates Sosu
        recipeRepo.AddRecipe(1, 10, 20); // 20gr Soğan
        
        // Pide Karışık recipe (Product ID 2)
        recipeRepo.AddRecipe(2, 1, 80);  // 80gr Kıyma
        recipeRepo.AddRecipe(2, 2, 200); // 200gr Un
        recipeRepo.AddRecipe(2, 4, 50);  // 50gr Peynir
        recipeRepo.AddRecipe(2, 3, 40);  // 40ml Domates Sosu
        
        // Pide Peynirli recipe (Product ID 3)
        recipeRepo.AddRecipe(3, 2, 180); // 180gr Un
        recipeRepo.AddRecipe(3, 4, 120); // 120gr Peynir
        
        // Patates Kızartması recipe (Product ID 8)
        recipeRepo.AddRecipe(8, 8, 200); // 200gr Patates
        
        // Ayran recipe (Product ID 9)
        recipeRepo.AddRecipe(9, 6, 250); // 250ml Ayran (Süt)
        
        // Kola recipe (Product ID 10)
        recipeRepo.AddRecipe(10, 7, 330); // 330ml Kola
    }
}
