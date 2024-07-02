﻿using DesigneryCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryCore.Interfaces
{
    public interface IOrderService
    {
        public List<Order> GetOrdById(int userId);
        bool PutOrder(int Id, string status);
        public List<Order> GetAllOrders();
    }
}
