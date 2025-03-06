using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FullstackHW.Models
{
    public class ProductViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }="";
        public string? Description { get; set; }
        public double Price { get; set; }
        [DisplayName("Expire Date")]
        public string? ExpiredDate { get; set; }
        [DisplayName("Image")]
        public string? ImageFileName { get; set; }
        [DisplayName("Source")]
        public string? Source { get; set; } 

    }
}