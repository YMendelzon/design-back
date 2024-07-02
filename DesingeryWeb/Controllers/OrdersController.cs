using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
namespace DesingeryWeb.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {

        private readonly IOrderService _order;
        private readonly ILogger<OrdersController> _logger;
        public OrdersController(ILogger<OrdersController> logger, IOrderService orderService)
        {
            _order = orderService;
            _logger = logger;

        }



        [HttpGet("GetOrdById/{userId}")]
        public List<Order> GetOrdById(int userId)
        {
            
            return _order.GetOrdById(userId);
        }


        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<List<Order>>> GetAllOrders()
        {

            return _order.GetAllOrders();
        }

        [HttpPut("PutOrder/{Id}")]
        public async Task<ActionResult<bool>> PutOrder(int Id, string status)
        {
            return _order.PutOrder(Id, status);
        }
    }
}
