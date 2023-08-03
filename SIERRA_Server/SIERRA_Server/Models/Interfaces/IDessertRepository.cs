using SIERRA_Server.Models.DTOs.Desserts;

namespace SIERRA_Server.Models.Interfaces
{
    public interface IDessertRepository
    {
       Task<List<DessertListDTO>> GetHotProductsAsync();
        Task<List<DessertsIndexDTO>> GetPresents();
    }
}
