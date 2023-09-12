
using System.ComponentModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApiBestBuy.Api.Controllers;
using WebApiBestBuy.Domain.Interfaces.Repositories;
using WebApiBestBuy.Domain.Interfaces.Services;
using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.Notifications;
using WebApiBestBuy.Domain.ViewModel;

namespace WebApiBestBuy.Test.Controllers
{
    public class ProductControllerTests
    {

        private readonly MockRepository mockRepository;
        private readonly Mock<IProductService> mockProductService;
        private readonly Mock<INotificationContext> mockNotificationContext;

        public ProductControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Loose);
            this.mockProductService = mockRepository.Create<IProductService>();
            this.mockNotificationContext = this.mockRepository.Create<INotificationContext>();
        }

        public ProductController createController()
        {
            return new ProductController(mockProductService.Object, mockNotificationContext.Object);
        }


        //public Product createProductValid() => new Product(0, "Playstation 5", 1500.0, "VideoGame de ultima geração", 3, "testando.com");

        //public Product createProductInvalid() =>  new Product();




        [Trait("Pproduto", "Listar")]
        [Fact(DisplayName = "Deve retornar 200 na listagem de Produtos")]
        public async Task Deve_Retornar_200_Na_Listagem_Alunos()
        {
            var productControler = this.createController();

            var result = (ObjectResult)await productControler.GetProducts();


            mockProductService.Setup(x => x.GetProducts()).ReturnsAsync(new ResultViewModel { Success = true }).Verifiable();

            mockProductService.Verify(x => x.GetProducts(), Times.Once);


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


            mockProductService.Setup(x => x.GetProduct(ID)).ReturnsAsync(new ResultViewModel { Success = true }).Verifiable();

            mockProductService.Verify(x => x.GetProduct(ID), Times.Once);


            Assert.Equal(200, result.StatusCode);

        }

        [Trait("Produto", "Criar")]
        [Fact(DisplayName = "Deve retornar 200 na criação de um novo Produto")]
        public async Task Deve_Retornar_200_Criacao_Produto()
        {
            var productController = this.createController();

            var product = new Product(0, "Playstation 5", 1500.0, "VideoGame de ultima geração", 3, "testando.com");


            var result = (ObjectResult)await productController.CreateProduct(product);

            mockProductService.Setup(x => x.CreateProduct(product)).ReturnsAsync(new Product()).Verifiable();

            mockProductService.Verify(x => x.CreateProduct(product), Times.Once);


            Assert.Equal(result.StatusCode, StatusCodes.Status200OK);
        }



        [Trait("Produto", "Atualizar")]
        [Fact(DisplayName = "Deve retornar 200 na criação de um novo Produto")]
        public async Task Deve_Retornar_200_Atualizacao_Produto()
        {
            var product = new Product(0, "Playstation 5", 1500.0, "VideoGame de ultima geração", 3, "testando.com");
            var newProduct = new Product(0, "Playstation 5", 1500.0, "VideoGame de ultima geração", 3, "testando.com");

            var productController = this.createController();

            var result = (ObjectResult)await productController.UpdateProducts(product);

            mockProductService.Setup(x => x.UpdateProduct(product))/*.ReturnsAsync(new ResultViewModel { Success = true })*/.Verifiable();

            mockProductService.Verify(x => x.UpdateProduct(product), Times.Once);

            Assert.Equal(200, result.StatusCode);

        }

        [Trait("Produto", "Deletar")]
        [Fact(DisplayName = "Deve retornar 200 na deleção de um Produto")]
        public async Task Deve_Retornar_200_Deletar_Produto()
        {
            var product = this.createController();

            var result = (ObjectResult)await product.DeleteProducts(1);


            mockProductService.Setup(x => x.DeleteProduct(1))/*.ReturnsAsync(true).Verifiable()*/;

            mockProductService.Verify(x => x.DeleteProduct(1), Times.Once);

            Assert.Equal(200, result.StatusCode);

        }
    }
}


