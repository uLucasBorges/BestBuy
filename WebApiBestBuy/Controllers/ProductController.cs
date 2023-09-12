using WebApiBestBuy.Domain.Notifications;
using Microsoft.AspNetCore.Mvc;
using WebApiBestBuy.Domain.Models;
using ClosedXML.Excel;
using System.Text;
using WebApiBestBuy.Domain.Interfaces.Repositories;
using WebApiBestBuy.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebApiBestBuy.Api.Controllers
{
    [Route("[Controller]")]
    [Authorize]
    public class ProductController : BaseController
    {
        IProductService _productservice;

        public ProductController(IProductService productService, INotificationContext notificationContext) : base (notificationContext)
        {
            _productservice = productService;
            //_appEnvironment = appEnvironment;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (product.IsValid)
            await _productservice.CreateProduct(product); 
            
            return Response();
        }



        [HttpGet("List")]
        public async Task<IActionResult> GetProducts() 
        {  
           var products =  await _productservice.GetProducts();
            return Response(products);
        }

        [HttpGet("By/{id}")]
        public async Task<IActionResult> GetProduct(int id = 0)
        {
            var result = await _productservice.GetProduct(id);
            return Response(result);
        }

      
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateProducts(Product product)
        {
            if (product.IsValid)
            await _productservice.UpdateProduct(product);

         
            return Response();
            
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteProducts(int id = 0)
        {
           await _productservice.DeleteProduct(id);
            
           return Response();
        }


        //[HttpPost("Create/Lote")]
        //public async Task<IActionResult> CreateLoteProduct(IFormFile file)
        //{


        //    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");

        //    //create folder if not exist
        //    if (!Directory.Exists(path))
        //        Directory.CreateDirectory(path);


        //    //get file extension
        //    FileInfo fileInfo = new FileInfo(file.FileName);
        //    string fileName = file.FileName;

        //    string fileNameWithPath = Path.Combine(path, fileName);

        //    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
        //    {
        //        file.CopyTo(stream);
        //    }

        //    var arquivo = _appEnvironment.WebRootPath + @$"\Files\{file.FileName}";

        //    var xls = new XLWorkbook(arquivo);

        //    var planilha = xls.Worksheets.FirstOrDefault();

        //    var totalLinhas = planilha.Rows().Count() + 1;


        //    for (int i = 2; i <= totalLinhas; i++)
        //    {
        //        var produto = new Product
        //        {
        //            Id = 0,
        //            Name = planilha.Cell("A" + i).Value.ToString(),
        //            Description = planilha.Cell("B" + i).Value.ToString(),
        //            CategoryId = int.Parse(planilha.Cell("C" + i).Value.ToString()),
        //            ImageUrl = planilha.Cell("D" + i).Value.ToString(),
        //            Price = double.Parse(planilha.Cell("E" + i).Value.ToString())
        //        };

        //        if (produto.IsValid)
        //        {
        //            await this.CreateProduct(produto);
        //        }


        //    }

        //    DirectoryInfo di = new DirectoryInfo(path);

        //    foreach (FileInfo fileDI in di.GetFiles())
        //    {
        //        fileDI.Delete();
        //    }


        //    return Response();
        //}
    }
}
