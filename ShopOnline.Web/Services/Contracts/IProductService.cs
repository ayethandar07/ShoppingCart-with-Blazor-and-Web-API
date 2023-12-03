using ShopOnline.Web.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetItems();

        Task<ProductDto> GetItem(int id);
    }
}
