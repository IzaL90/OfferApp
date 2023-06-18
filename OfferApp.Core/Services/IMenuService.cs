using OfferApp.Core.DTO;

namespace OfferApp.Core.Services
{
    public interface IMenuService : IService
    {
        IEnumerable<MenuDto> GetMenus();

        MenuDto? GetMenuById(int id);
    }
}
