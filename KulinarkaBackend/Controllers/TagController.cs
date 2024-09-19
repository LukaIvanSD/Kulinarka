using Kulinarka.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kulinarka.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class TagController : BaseController
    {
        private readonly ITagService tagService;
        public TagController(ITagService tagService)
        {
            this.tagService = tagService;
        }
        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            var result = await tagService.GetAllAsync();
            return HandleResponse(result);
        }
    }
}
