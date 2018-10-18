namespace RateReader.Api.Controllers
{
    public interface ICurrencyRequestVlidator
    {
        bool IsValid(int year, int? month, out string message);
    }
}