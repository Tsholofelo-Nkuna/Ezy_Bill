using ClientManagement.BusinessLogicLayer.Services;
using ClientManagement.Presentation.Models.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClientManagement.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        public ProductsController(ProductService productService) { 
            _productService = productService;
        }
        // GET: api/<ProductsController>/<action>
        [HttpPost("[action]")]
        public async Task<IEnumerable<ProductDto>> Get([FromBody] ProductDto filters)
        {
            return  await this._productService.Get(filters);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public async Task<ProductDto?> Get(Guid id)
        {
            return (await this._productService.Get(x => !x.Archived && x.Id == id)).FirstOrDefault();
        }

        // POST api/<ProductsController>
        [HttpPost]
        public async Task<bool> Post([FromBody] ProductDto value)
        {
            return await this._productService.AddOrUpdate(new List<ProductDto>() { value });
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        // GET api/<ProductsController>/<Delete>/5
        [HttpDelete("[action]/{id}")]
        public async Task<bool> Delete(Guid id)
        {
            return await this._productService.Delete(new[] { id });
        }
    }
}
