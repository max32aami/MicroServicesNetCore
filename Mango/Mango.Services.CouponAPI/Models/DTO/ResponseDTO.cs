namespace Mango.Services.CouponAPI.Models.DTO
{
    public class ResponseDTO
    {
        public object? Result { get; set; }
        public bool IsSucess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
