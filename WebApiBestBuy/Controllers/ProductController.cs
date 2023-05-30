using WebApiBestBuy.Domain.Notifications;
using Microsoft.AspNetCore.Mvc;
using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.Interfaces;
using ClosedXML.Excel;
using System.Text;

namespace WebApiBestBuy.Api.Controllers
{
    [Route("[Controller]")]
    public class ProductController : BaseController
    {
        IProductRepository _productRepository;
        IWebHostEnvironment _appEnvironment;
        INotificationContext _notificationContext;

        public ProductController(IProductRepository productRepository, IWebHostEnvironment appEnvironment,        INotificationContext notificationContext) : base (notificationContext)
        {
            _productRepository = productRepository;
            _appEnvironment = appEnvironment;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (product.IsValid)
            {
                 await _productRepository.CreateProduct(product); 
            }
            return Response();
        }

        [HttpPost("Create/Lote")]
        public async Task<IActionResult> CreateLoteProduct(IFormFile file)
        {


            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");

            //create folder if not exist
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            //get file extension
            FileInfo fileInfo = new FileInfo(file.FileName);
            string fileName = file.FileName;

            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var arquivo = _appEnvironment.WebRootPath + $"\\Files\\{file.FileName}";

            var xls = new XLWorkbook(arquivo);

            var planilha = xls.Worksheets.FirstOrDefault();

            var totalLinhas = planilha.Rows().Count() + 1;


            for (int i = 2; i <= totalLinhas; i++)
            {
                var produto = new Product
                {
                    Id = 0,
                    Name = planilha.Cell("A" + i).Value.ToString(),
                    Description = planilha.Cell("B" + i).Value.ToString(),
                    CategoryId = int.Parse(planilha.Cell("C" + i).Value.ToString()),
                    ImageUrl = planilha.Cell("D" + i).Value.ToString(),
                    Price = double.Parse(planilha.Cell("E" + i).Value.ToString())
                };

                if (produto.IsValid)
                {
                    await this.CreateProduct(produto);
                } 


            }

            if (Directory.Exists(path))
                Directory.Delete(arquivo);




            return Response();
        }


        [HttpGet("List")]
        public async Task<IActionResult> GetProducts() 
        {  
           var products =  await _productRepository.GetProducts();
            return Response(products);
        }

        [HttpGet("By/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var result = await _productRepository.GetProduct(id);
            return Response(result);
        }

        /// <summary>
        /// s
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateProducts(Product product)
        {
            if (product.IsValid)
            {
               var result = await _productRepository.UpdateProduct(product);

            }

            return Response();
            
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteProducts(int id)
        {
           var result = await _productRepository.DeleteProduct(id);
            
           return Response();
        }
    }
}
