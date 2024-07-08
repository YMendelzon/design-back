using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DesingeryWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;
        private readonly ILogger<OrderItemController> _logger;
        public OrderItemController(ILogger<OrderItemController> logger, IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
            _logger = logger;
        }

        //to check this function
        [HttpGet("GetAllOrderItems")]
        public List<OrderItem> GetAllOrderItems()
        {
            return _orderItemService.GetAllOrderItems();
        }


        [HttpGet("GetOrderItemByOrdId/{orderId}")]
        public List<OrderItem> GetOrderItemByOrdId(int orderId)
        {
            return _orderItemService.GetOrderItemByOrdId(orderId);
        }

        [HttpPost("PostOrderItem")]
        public async Task<ActionResult<bool>> PostOrderItem(OrderItem orderItem)
        {
            return _orderItemService.PostOrderItem(orderItem);
        }
    }
}

