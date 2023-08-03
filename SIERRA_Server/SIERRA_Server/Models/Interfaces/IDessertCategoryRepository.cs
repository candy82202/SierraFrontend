using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.Interfaces
{
    public interface IDessertCategoryRepository
    {
        Task<List<Dessert>> GetDessertsByCategoryId(int categoryId);
    }
}
