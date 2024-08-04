using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryCore.Services;
using Microsoft.AspNetCore.Authorization;
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

        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;
        public OrdersController(ILogger<OrdersController> logger, IOrderService orderService)
        {
            _orderService = orderService;
            _logger = logger;
        }


        //to check this function
        [HttpGet("GetOrderByOrderId/{orderId}")]
        //[Authorize(Roles = "1,2,3")]

        public async Task<Order> GetOrderByOrderId(int orderId)
        {
            return _orderService.GetOrderByOrderId(orderId);
        }


        [HttpGet("GetOrderByUserId/{userId}")]
        //[Authorize(Roles = "1,2,3")]

        public async Task<List<Order>> GetOrderByUserId(int userId)
        {            
            return _orderService.GetOrderByUserId(userId);
        }

        [HttpGet("GetAllOrders")]
        //[Authorize(Roles = "3")]

        public async Task<ActionResult<List<Order>>> GetAllOrders()
        {
            return _orderService.GetAllOrders();
        }

        [HttpPut("PutOrder/{Id}")]
        //[Authorize(Roles = "3")]

        public async Task<ActionResult<bool>> PutOrder([FromBody] PutOrderObject orderObject)
        {
            return _orderService.PutOrder(orderObject);
        }

        [HttpPost("PostOrder")]
        [Authorize]

        public async Task<ActionResult<int>> PostOrder(Order order) 
        {
            return _orderService.PostOrder(order);
        }

    }
}
