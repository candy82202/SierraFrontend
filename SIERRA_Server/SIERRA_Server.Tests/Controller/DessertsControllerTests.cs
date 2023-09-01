using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SIERRA_Server.Controllers;
using SIERRA_Server.Models.DTOs.Desserts;
using SIERRA_Server.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIERRA_Server.Test.Controller
{
    public class DessertsControllerTests
    {
        private readonly IDessertRepository _repo;
        private readonly IDessertDiscountRepository _discountrepo;
        public DessertsControllerTests()
        {
            _repo = A.Fake<IDessertRepository>();
            _discountrepo = A.Fake<IDessertDiscountRepository>();
        }
        [Fact]
        public async Task DessertsController_GetMoldCake_ReturnNotNullAndRightCount()
        {
            // Arrange
            var fakeDesserts = new List<DessertsIndexDTO>
            {
                new DessertsIndexDTO(10.0m) { DessertName = "Mold Cake 1" ,UnitPrice=100,DessertImageName="Image",Flavor="草莓",Size="6入",DiscountPrice=8},
                new DessertsIndexDTO(10.0m) { DessertName = "Mold Cake 2",UnitPrice=200,DessertImageName="Image",Flavor="巧克",Size="6入",DiscountPrice=8 }
                // 是需求新增假資料
            };

            // 設置假repo以在調用 GetMoldCake 時返回 fakeDesserts
            A.CallTo(() => _repo.GetMoldCake()).Returns(fakeDesserts);

            // 使用假repository創建控制器的實例 
            var controller = new DessertsController(null, null, _repo, null);

            // Act
            var result = await controller.GetMoldCake();

            // Assert 確保回傳的結果不為空，而且是'OkObjectResult'類型。
            result.Should().NotBeNull().And.BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            //檢查回傳的值是否可以分配給'List<DessertsIndexDTO>'的物件。
            var model = okResult.Value.Should().BeAssignableTo<List<DessertsIndexDTO>>().Subject;
            //這裡檢查列表的元素數量
            model.Should().HaveCount(2);
        }

        [Fact]
        public async Task DessertsController_GetMoldCake_ReturnRightDiscountPrice()
        {
            // Arrange
            var fakeDesserts = new List<DessertsIndexDTO>
    {//加上m表示數值是decimal型別
        new DessertsIndexDTO(80.0m) { UnitPrice = 100, DiscountPrice = 80 },
        new DessertsIndexDTO(160.0m) { UnitPrice = 200, DiscountPrice = 80 }
        // Add more fake data if needed
    };

            A.CallTo(() => _repo.GetMoldCake()).Returns(fakeDesserts);

            var controller = new DessertsController(null, null, _repo, null);

            // Act
            var result = await controller.GetMoldCake();

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            var model = okResult.Value.Should().BeAssignableTo<List<DessertsIndexDTO>>().Subject;

            // 迴圈找出模型中的每個甜點並驗證其折扣價格
            for (int i = 0; i < model.Count; i++)
            {
                var expectedDiscountPrice = Math.Round((decimal)model[i].UnitPrice * (decimal)model[i].DiscountPrice / 100, 0, MidpointRounding.AwayFromZero);
                //這裡斷言，期望地i個甜點的DessertDiscountPrice屬性會等於預先計算的折扣價格expectedDiscountPrice
                model[i].DessertDiscountPrice.Should().Be(expectedDiscountPrice);
            }
        }
    }
}

