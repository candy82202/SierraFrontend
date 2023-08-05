using Microsoft.AspNetCore.Mvc;
using SIERRA_Server.Models.DTOs;
using SIERRA_Server.Models.DTOs.Desserts;
using SIERRA_Server.Models.Interfaces;
using System.Threading.Tasks;

namespace SIERRA_Server.Models.Services
{
    public class DessertService
    {
        private IDessertRepository _repo;
        private IDessertDiscountRepository _discountrepo;

        public DessertService(IDessertRepository repo)
        {
            _repo = repo;
           
        }
        public DessertService(IDessertDiscountRepository discountrepo)
        {
          
            _discountrepo = discountrepo;
        }
        public async Task<List<DessertListDTO>> GetHotProductsAsync()
        {
            var hotdessert = await _repo.GetHotProductsAsync();
            return hotdessert;
        }
        public async Task<List<DessertsIndexDTO>> GetPresents()
        {
            var presents = await _repo.GetPresents();
            return presents;
        }
        public async Task<List<DessertsIndexDTO>> GetLongCake()
        {
            var longCake = await _repo.GetLongCake();
            return longCake;
        }
        public async Task<List<DessertsIndexDTO>> GetSnack()
        {
            var snack = await _repo.GetSnack();
            return snack;
        }
        public async Task<List<DessertsIndexDTO>> GetMoldCake()
        {
            var moldCake = await _repo.GetMoldCake();
            return moldCake;
        }
        public async Task<List<DessertsIndexDTO>> GetRoomTemperature()
        {
            var roomTemperature = await _repo.GetRoomTemperature();
            return roomTemperature;
        }
        public async Task<List<DessertDiscountDTO>> GetChocoDiscountGroups()
        {
            var chocoDiscount = await _discountrepo.GetChocoDiscountGroups();
            return chocoDiscount;
        }
    }
}