using OfferApp.Shared.DTO;
using OfferApp.Domain.Entities;
using OfferApp.Core.Mappings;

namespace OfferApp.Core.Services
{
    internal sealed class MenuService : IMenuService
    {
        private readonly IEnumerable<Menu> _menus;

        public MenuService(IEnumerable<MenuDto> menuDtos)
        {
            _menus = menuDtos.Select(menu => menu.AsEntity())
                             .ToList();
        }

        public IEnumerable<MenuDto> GetMenus()
        {
            return _menus.Select(menu => menu.AsDto());
        }

        public MenuDto? GetMenuById(int id)
        {
            return _menus.FirstOrDefault(m => m.Id == id)?.AsDto();
        }
    }
}
