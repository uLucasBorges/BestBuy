
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApiBestBuy.Api.Controllers;
using WebApiBestBuy.Domain.Interfaces.Repositories;
using WebApiBestBuy.Domain.Interfaces.Services;
using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.Notifications;

namespace WebApiBestBuy.Test.Controllers
{
    public class CouponControllerTest
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<INotificationContext> mockNotificationContext;
        private readonly Mock<ICouponService> mockCouponService;

        public CouponControllerTest()
        {   mockRepository = new MockRepository(MockBehavior.Loose);
            mockCouponService = mockRepository.Create<ICouponService>();
            mockNotificationContext = mockRepository.Create<INotificationContext>();
        }

        public CouponController createController()
        {
            return new CouponController(mockCouponService.Object, mockNotificationContext.Object);
        }

        [Fact]
        [Trait("Coupon","Create")]
        public async Task Deve_Retornar_200_ao_Criar_Cupon()
        {
            Coupon coupon = new Coupon { CouponCode = "newCoupon", DiscountAmount = 1000.0 };

            var controller = createController();
            var result = (ObjectResult) await controller.CreateCoupon(coupon);

            mockCouponService.Setup(x => x.CreateCoupon(coupon));/*.ReturnsAsync(new Coupon { CouponCode = coupon.CouponCode , DiscountAmount = coupon.DiscountAmount });*/
            mockCouponService.Verify(x => x.CreateCoupon(coupon), Times.Once);

            Assert.Equal(200, result.StatusCode);
        }


        [Fact(Skip = "Método não implementado")]
        public async Task DeleteCoupon()
        {

        }


        [Fact(Skip = "Método não implementado")]

        public async Task ApplyCoupon()
        {

        }


        [Fact(Skip = "Método não implementado")]

        public async Task RemoveCoupom()
        {

        }

    }
}
