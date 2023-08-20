using SIERRA_Server.Models.DTOs.Desserts;

namespace SIERRA_Server.Models.Interfaces
{
    public interface IDessertRepository
    {
        Task<List<DessertsIndexDTO>> GetMoldCake();      
        Task<List<DessertsIndexDTO>> GetPresents();
        Task<List<DessertsIndexDTO>> GetLongCake();
        Task<List<DessertsIndexDTO>> GetSnack();
        Task<List<DessertsIndexDTO>> GetRoomTemperature();
         Task<List<DessertListDTO>> GetHotProductsAsync();
        Task<List<DessertsIndexDTO>> GetDessertByName(string dessertName);
        Task<List<string>> GetDessertNames();
    }
}
