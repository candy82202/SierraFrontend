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
            var chocoDiscount = await _discountrepo.GetDiscountGroupsByGroupId(6);
            return chocoDiscount;
        }
        public async Task<List<DessertDiscountDTO>> GetStrawberryDiscountGroups()
        {
            // 呼叫 DiscountGroupId 是 7 的 repository 
            var strawberryDiscount = await _discountrepo.GetDiscountGroupsByGroupId(7);
            return strawberryDiscount;
        }
        public async Task<List<DessertDiscountDTO>> GetMochaDiscountGroups()
        {
            //  呼叫 DiscountGroupId 是 8 的 repository  
            var mochaDiscount = await _discountrepo.GetDiscountGroupsByGroupId(8);
            return mochaDiscount;
        }
        public async Task<List<DessertDiscountDTO>> GetTaroDiscountGroups()
        {
            //  呼叫 DiscountGroupId 是 9 的 repository  
            var taroDiscount = await _discountrepo.GetDiscountGroupsByGroupId(9);
            return taroDiscount;
        }
        public async Task<List<DessertDiscountDTO>> GetAlcoholDiscountGroups()
        {
            //  呼叫 DiscountGroupId 是  10 的 repository 
            var alcoholDiscount = await _discountrepo.GetDiscountGroupsByGroupId(10);
            return alcoholDiscount;
        }
        public async Task<List<DessertDiscountDTO>> GetSuggestDiscountGroups(int dessertId)
        {
            //if (dessertId== _)
            //  呼叫 DiscountGroupId 是  10 的 repository 
            var alcoholDiscount = await _discountrepo.GetDiscountGroupsByGroupId(10);
            var taroDiscount = await _discountrepo.GetDiscountGroupsByGroupId(9);
            var hotdessert = await _repo.GetHotProductsAsync();
           

            if (dessertId = alcoholDiscount.dessertId) { return alcoholDiscount; }
            else if (dessertId = taroDiscount.dessertId) { return taroDiscount; }
            return hotdessert;
        }
    }
}