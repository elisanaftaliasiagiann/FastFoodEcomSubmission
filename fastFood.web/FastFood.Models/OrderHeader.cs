﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FastFood.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        public DateTime TimeOfPick { get; set; }
        public DateTime DateOfPick { get; set; }
        public double SubTotal { get; set; }
        public double OrderTotal { get; set; }
        public string CouponCode { get; set; }
        public double CouponDis { get; set; }
        public string TransId { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

    }
}