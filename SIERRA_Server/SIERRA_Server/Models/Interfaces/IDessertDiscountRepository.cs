using Microsoft.AspNetCore.Mvc;
using SIERRA_Server.Models.DTOs.Desserts;

namespace SIERRA_Server.Models.Interfaces
{
    public interface IDessertDiscountRepository
    {
        Task<List<DessertDiscountDTO>> GetChocoDiscountGroups();
    }
}
