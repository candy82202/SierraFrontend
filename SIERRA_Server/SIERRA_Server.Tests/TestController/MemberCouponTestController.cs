using FakeItEasy;
using FluentAssertions;
using SIERRA_Server.Controllers;
using SIERRA_Server.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIERRA_Server.Tests.TestController
{
    //使用FakeItEasy、FluentAssertions進行單元測試
    //FakeItEasy=>用來創建和管理測試中的假物件（mocks、stubs、spies）。這些假物件有助於隔離被測試的單元，讓你可以專注於測試特定的程式碼區塊，而不受其他相依程式碼的影響。
    //FluentAssertions=>這個套件提供了更加豐富的斷言語法，有助於更方便地撰寫和執行測試。
    public class MemberCouponTestController
    {
        private readonly IMemberCouponRepository _repo;
        public MemberCouponTestController()
        {
            _repo = A.Fake<IMemberCouponRepository>();
        }
        [Fact]
        public void MemberCouponController_UseCouponAndCalculateDiscountPrice_ReturnInt()
        {
            //Arrange
            var memberId = 1;
            var memberCouponId = 1;
            var controller = new MemberCouponsController(_repo);

            //Act
            var result = controller.UseCouponAndCalculateDiscountPrice(memberId, memberCouponId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<int>>();
            result.Equals(0);
        }
        //但是使用FakeItEasy來創造模擬的資料庫只能確定這隻方法被叫用的狀況，返回的類別等等
        //雖然不會改動到資料庫的資料，卻也不能知道跟真正資料庫互動的情況，也無從得知取出的物件以及其中運算的正確性。
        //若要測試上述提到的兩種情況=>在一開始寫程式時要考慮到進行單元測試的難易度
        //1.將資料寫死在程式中
        //2.建立專門測試的資料庫
    }
}
