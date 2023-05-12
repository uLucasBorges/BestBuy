using WebApiBestBuy.Domain.Models;

namespace WebApiBestBuy.Domain.ViewModel;

public class ResultViewModel
{
    public dynamic? Data { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }

    public ResultViewModel()
    {

    }

    public ResultViewModel(Product? data, bool success)
    {
        Data = data;
        Success = success;
    }


    public ResultViewModel(Coupon? data, bool success, string message)
    {
        Data = data;
        Success = success;
        Message = message;
    }

    public ResultViewModel(Token? data, bool success)
    {
        Data = data;
        Success = success;

    }
}
