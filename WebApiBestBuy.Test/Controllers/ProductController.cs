
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApiBestBuy.Api.Controllers;
using WebApiBestBuy.Domain.Interfaces;
using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.Notifications;
using WebApiBestBuy.Domain.ViewModel;

namespace WebApiBestBuy.Test.Controllers
{
    public class ProductControllerTests
    {

        private readonly MockRepository mockRepository;
        private readonly Mock<IProductRepository> mockProductRepository;
        private readonly Mock<INotificationContext> mockNotificationContext;

        public ProductControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Loose);
            this.mockProductRepository = mockRepository.Create<IProductRepository>();
            this.mockNotificationContext = this.mockRepository.Create<INotificationContext>();
        }

        public ProductController createController()
        {
            return new ProductController(mockProductRepository.Object, mockNotificationContext.Object);
        }

        //public Product createProductValid() => new Product(0, "Playstation 5", 1500.0, "VideoGame de ultima geração", 3, "testando.com");
        
        //public Product createProductInvalid() =>  new Product();
        



        [Trait("Pproduto", "Listar")]
        [Fact(DisplayName = "Deve retornar 200 na listagem de Produtos")]
        public async Task Deve_Retornar_200_Na_Listagem_Alunos()
        {
            var productControler = this.createController();

            var result = (ObjectResult)await productControler.GetProducts();


            mockProductRepository.Setup(x => x.GetProducts()).ReturnsAsync(new ResultViewModel { Success  = true}).Verifiable();

            mockProductRepository.Verify(x => x.GetProducts(), Times.Once);


            Assert.Equal(200, result.StatusCode);

        }

        [Trait("Produto", "Procurar por Id")]
        [Theory(DisplayName = "Deve retornar 200 na listagem de Produtos")]
        [InlineData(12)]
        [InlineData(20)]
        public async Task Deve_Retornar_200_Buscar_Produto(int ID)
        {
            var productController = this.createController();

            var result = (ObjectResult)await productController.GetProduct(ID);


            mockProductRepository.Setup(x => x.GetProduct(ID)).ReturnsAsync(new ResultViewModel { Success = true }).Verifiable();

            mockProductRepository.Verify(x => x.GetProduct(ID), Times.Once);


            Assert.Equal(200, result.StatusCode);

        }

        [Trait("Produto", "Criar")]
        [Fact(DisplayName = "Deve retornar 200 na criação de um novo Produto")]
        public async Task Deve_Retornar_200_Criacao_Produto()
        {
            var productController = this.createController();

            var product = new Product(0, "Playstation 5", 1500.0, "VideoGame de ultima geração", 3, "testando.com");


            var result = (ObjectResult)await productController.CreateProduct(product);

            mockProductRepository.Setup(x => x.CreateProduct(product)).ReturnsAsync(new Product()).Verifiable();

            mockProductRepository.Verify(x => x.CreateProduct(product), Times.Once);


            Assert.Equal(result.StatusCode, StatusCodes.Status200OK);
        }



        [Trait("Produto", "Atualizar")]
        [Fact(DisplayName = "Deve retornar 200 na criação de um novo Produto")]
        public async Task Deve_Retornar_200_Atualizacao_Produto()
        {
            var product = new Product(0, "Playstation 5", 1500.0, "VideoGame de ultima geração", 3, "testando.com");

            var productController = this.createController();

            var result = (ObjectResult) await productController.UpdateProducts(product);

            mockProductRepository.Setup(x => x.UpdateProduct(product)).ReturnsAsync(new ResultViewModel { Success = true }).Verifiable();

            mockProductRepository.Verify(x => x.UpdateProduct(product), Times.Once);

            Assert.Equal(200, result.StatusCode);

        }

        [Trait("Produto", "Deletar")]
        [Fact(DisplayName = "Deve retornar 200 na deleção de um Produto")]
        public async Task Deve_Retornar_200_Deletar_Produto()
        {
            var product = this.createController();

            var result = (ObjectResult)await product.DeleteProducts(1);


            mockProductRepository.Setup(x => x.DeleteProduct(1)).ReturnsAsync(true).Verifiable();

            mockProductRepository.Verify(x => x.DeleteProduct(1), Times.Once);

            Assert.Equal(200, result.StatusCode);

        }
    }
}
