using System.ComponentModel;

namespace ProductManage.Models
{
    public class QueryRequest
    {

        [DisplayName("流水號")]
        public int ProductId { get; set; }

        [DisplayName("產品名稱")]
        public string Name { get; set; }

        [DisplayName("產品號碼")]
        public string? ProductNumber { get; set; }


        [DisplayName("顏色")]
        public string? Color { get; set; }


        [DisplayName("原定價")]
        public decimal? StandardCost { get; set; }

        [DisplayName("銷售價格")]
        public decimal? ListPrice { get; set; }


        [DisplayName("尺寸")]
        public string? Size { get; set; }


        [DisplayName("重量")]
        public decimal? Weight { get; set; }

        public DateTime SellStartDate { get; set; }

        public DateTime? SellEndDate { get; set; }

        public DateTime? DiscontinuedDate { get; set; }
    
    }
}
