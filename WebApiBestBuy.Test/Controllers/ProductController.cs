
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApiBestBuy.Api.Controllers;
using WebApiBestBuy.Domain.Interfaces;
using WebApiBestBuy.Domain.Notifications;
using WebApiBestBuy.Domain.ViewModel;

namespace WebApiBestBuy.Test.Controllers
{
    public class ProductControllerTests
    {

        private readonly MockRepository mockRepository;
        private readonly Mock<IProductRepository> mockProductRepository;
        private readonly Mock<INotificationContext> mockNotificationContext;
        private readonly ResultViewModel resultViewModel;

        public ProductControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Loose);
            this.mockProductRepository = mockRepository.Create<IProductRepository>();
            this.mockNotificationContext = this.mockRepository.Create<INotificationContext>();
            this.resultViewModel = this.mockRepository.Create<ResultViewModel>().Object;
            this.resultViewModel.Success = true;
        }

        public ProductController createController()
        {
            return new ProductController(mockProductRepository.Object, mockNotificationContext.Object);
        }



        [Trait("Pproduto", "Listar")]
        [Fact(DisplayName = "Deve retornar 200 na listagem de Produtos")]
        public async Task Deve_Retornar_200_Na_Listagem_Alunos()
        {
            var productControler = this.createController();

            var result = (ObjectResult)await productControler.GetProducts();


            mockProductRepository.Setup(x => x.GetProducts()).ReturnsAsync(resultViewModel);

            mockProductRepository.Verify(x => x.GetProducts(), Times.Once);

            Assert.Equal(200, result.StatusCode);
        
        }

        [Trait("Produto", "Procurar por Id")]
        [Theory(DisplayName = "Deve retornar 200 na listagem de Produtos")]
        [InlineData(12)]
        [InlineData(20)]
        public async Task Deve_Retornar_200_Buscar_Aluno(int ID)
        {
            var productController = this.createController();

            var result = (ObjectResult)await productController.GetProduct(ID);
            

            mockProductRepository.Setup(x => x.GetProduct(ID)).ReturnsAsync(new ProductResultVM { Success = true });

            mockProductRepository.Verify(x => x.GetProduct(ID), Times.Once);


            Assert.Equal(200, result.StatusCode);
        }



        }
}
