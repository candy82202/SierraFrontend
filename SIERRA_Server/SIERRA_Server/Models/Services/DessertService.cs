using SIERRA_Server.Models.DTOs;
using SIERRA_Server.Models.DTOs.Desserts;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Models.Services
{
    public class DessertService
    {
        private IDessertRepository _repo;

        public DessertService(IDessertRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<DessertListDTO>> GetHotProductsAsync()
        {
            var hotdessert = await _repo.GetHotProductsAsync();
            return hotdessert;
        }

    }
}
