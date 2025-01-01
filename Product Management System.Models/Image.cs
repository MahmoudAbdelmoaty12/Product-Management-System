using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product_Management_System.Models
{
    public class Image : BaseEntity
    {
        [Base64String]
        public string Name { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }//**
        public Product Product { get; set; }//**
    }
}
