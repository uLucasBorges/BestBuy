using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApiBestBuy.Api.Controllers;
using WebApiBestBuy.Domain.Interfaces;
using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.Notifications;
using Xunit;

namespace WebApiBestBuy.Test.Controllers
{
    public class CouponControllerTest
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<INotificationContext> mockNotificationContext;
        private readonly Mock<ICouponRepository> mockCouponRepository;

        public CouponControllerTest()
        {   mockRepository = new MockRepository(MockBehavior.Loose);
            mockCouponRepository = mockRepository.Create<ICouponRepository>();
            mockNotificationContext = mockRepository.Create<INotificationContext>();
        }

        public CouponController createController()
        {
            return new CouponController(mockCouponRepository.Object, mockNotificationContext.Object);
        }

        [Fact]
        [Trait("Coupon","Create")]
        public async Task Deve_Retornar_200_ao_Criar_Cupon()
        {
            Coupon coupon = new Coupon { CouponCode = "newCoupon", DiscountAmount = 1000.0 };

            var controller = createController();
            var result = (ObjectResult) await controller.CreateCoupon(coupon);

            mockCouponRepository.Setup(x => x.CreateCoupon(coupon)).ReturnsAsync(new Coupon { CouponCode = coupon.CouponCode , DiscountAmount = coupon.DiscountAmount });
            mockCouponRepository.Verify(x => x.CreateCoupon(coupon), Times.Once);

            Assert.Equal(200, result.StatusCode);
        }


        [Fact]
        public async Task DeleteCoupon()
        {

        }

        public async Task ApplyCoupon()
        {

        }

        public async Task RemoveCoupom()
        {

        }

    }
}
