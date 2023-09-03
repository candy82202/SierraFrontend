using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SIERRA_Server.Controllers;
using SIERRA_Server.Models.DTOs.Carts;
using SIERRA_Server.Models.DTOs.Desserts;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIERRA_Server.Tests.Controller
{
    public class DessertsCartControllerTests
    {
        private readonly IDessertCartRepository _repo;
        
        public DessertsCartControllerTests()
        {
            _repo = A.Fake<IDessertCartRepository>();
            
        }
        [Fact]
        public async Task DessertCartController_GetTotal_ReturnRightTotalPrice()
        {
            // Arrange
            var fakeTotalPrice = 400; // 模拟返回的总价格

            // 使用 Task.FromResult 包装 fakeTotalPrice 以返回 Task<int>
            A.CallTo(() => _repo.GetCartTotalPrice("danny")).Returns(Task.FromResult(fakeTotalPrice));

            var controller = new DessertCartPriceController(null, _repo);

            // Act
            var result = await controller.GetCartTotalPrice("danny");

            // Assert
      
            result.Should().Be(fakeTotalPrice); // 直接验证结果是否等于 fakeTotalPrice
      
        }
       // [Fact]
        //public async Task DessertCartController_GetTotal_ReturnRightTotalPrice()
        //{
        //    // Arrange
        //    var fakeCartDto = new CartDTO
        //    {
        //        Id = 1,
        //        Username = "danny",
        //        DessertCartItems = new List<CartItemDTO>
        //{
        //    new CartItemDTO
        //    {
        //        Id = 1,
        //        SpecificationId = 1,
        //        DessertCartId = 1,
        //        DessertId = 1,
        //        Quantity = 2,
        //        Flavor = "Chocolate",
        //        Size = "Small",
        //        DessertImageName = "chocolate_cake.jpg",
        //        DessertName = "Chocolate Cake",
        //        UnitPrice = 100
        //    },
        //    new CartItemDTO
        //    {
        //        Id = 2,
        //        SpecificationId = 2,
        //        DessertCartId = 1,
        //        DessertId = 2,
        //        Quantity = 1,
        //        Flavor = "Vanilla",
        //        Size = "Large",
        //        DessertImageName = "vanilla_cake.jpg",
        //        DessertName = "Vanilla Cake",
        //        UnitPrice = 200
        //    }
        //}
        //    };

        //    // 模拟返回的 CartDTO 对象
        //    object value = A.CallTo(() => _repo.GetCartTotalPrice("danny")).Returns(fakeCartDto));

        //    var controller = new DessertCartPriceController(null, _repo);

        //    // Act
        //    var result = await controller.GetCartTotalPrice("danny");

        //    // Assert
        //    result.Should().Be(400); // 验证总价是否正确（2 * 100 + 1 * 200 = 400）
        //}   



    }
}
