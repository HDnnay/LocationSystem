using LocationSystem.Domain.Entities;
using LocationSystem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LocationSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly MenuRepository _menuRepository;

        public MenusController(MenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        // GET: api/Menus
        [HttpGet]
        public async Task<IActionResult> GetMenus()
        {
            try
            {
                var menus = await _menuRepository.GetAllMenusAsync();
                return Ok(menus);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Menus/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenu(Guid id)
        {
            try
            {
                var menu = await _menuRepository.GetMenuByIdAsync(id);
                if (menu == null)
                {
                    return NotFound();
                }
                return Ok(menu);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Menus
        [HttpPost]
        public async Task<IActionResult> CreateMenu([FromBody] MenuCreateDto menuDto)
        {
            try
            {
                var menu = new Menu(
                    menuDto.Name,
                    menuDto.Path,
                    menuDto.Icon,
                    menuDto.Order,
                    menuDto.ParentId
                );
                
                var createdMenu = await _menuRepository.CreateMenuAsync(menu);
                return CreatedAtAction(nameof(GetMenu), new { id = createdMenu.Id }, createdMenu);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Menus/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenu(Guid id, [FromBody] MenuUpdateDto menuDto)
        {
            try
            {
                var menu = await _menuRepository.GetMenuByIdAsync(id);
                if (menu == null)
                {
                    return NotFound();
                }
                
                menu.Update(
                    menuDto.Name,
                    menuDto.Path,
                    menuDto.Icon,
                    menuDto.Order,
                    menuDto.ParentId
                );
                
                await _menuRepository.UpdateMenuAsync(menu);
                return Ok(menu);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Menus/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(Guid id)
        {
            try
            {
                await _menuRepository.DeleteMenuAsync(id);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class MenuCreateDto
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public Guid? ParentId { get; set; }
    }

    public class MenuUpdateDto
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public Guid? ParentId { get; set; }
    }
}
