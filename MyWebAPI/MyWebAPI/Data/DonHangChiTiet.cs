namespace MyWebAPI.Data
{
    public class DonHangChiTiet
    {
        public Guid MaDh { get; set; } 
        public Guid MaHh { get; set; } 
        public int SoLuong { get; set; }
        public double DonGia { get; set; }
        public byte GiamGia { get; set; }

        //relationships
        public DonHang DonHang { get; set; } // Navigation property to DonHang
        public HangHoa HangHoa { get; set; } // Navigation property to HangHoa
    }
}
