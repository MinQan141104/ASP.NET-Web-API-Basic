using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Models;
using static MyWebAPI.Models.HangHoaVM;

namespace MyWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangHoaController : ControllerBase
    {
        public static List<HangHoa> hangHoas = new List<HangHoa>();

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(hangHoas);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                var hanghoa = hangHoas.SingleOrDefault(h => h.MaHangHoa == Guid.Parse(id));
                if (hanghoa == null)
                {
                    return NotFound($"Hang hoa with ID {id} not found.");
                }
                return Ok(hanghoa);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Create(HangHoaVM hangHoaVM)
        {
            var hanghoa = new HangHoa()
            {
                MaHangHoa = Guid.NewGuid(),
                TenHangHoa = hangHoaVM.TenHangHoa,
                DonGia = hangHoaVM.DonGia
            };
            hangHoas.Add(hanghoa);

            return CreatedAtAction(nameof(GetAll), new { id = hanghoa.MaHangHoa }, hanghoa);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, HangHoa hangHoaVM)
        {
            try
            {
                var hanghoa = hangHoas.SingleOrDefault(h => h.MaHangHoa == Guid.Parse(id));
                if (hanghoa == null)
                {
                    return NotFound($"Hang hoa with ID {id} not found.");
                }
                if (id != hanghoa.MaHangHoa.ToString())
                {
                    return BadRequest();
                }

                hanghoa.TenHangHoa = hangHoaVM.TenHangHoa;
                hanghoa.DonGia = hangHoaVM.DonGia;

                return Ok(hanghoa);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Remove(string id)
        {
            try
            {
                var hanghoa = hangHoas.SingleOrDefault(h => h.MaHangHoa == Guid.Parse(id));
                if (hanghoa == null)
                {
                    return NotFound($"Hang hoa with ID {id} not found.");
                }
                hangHoas.Remove(hanghoa);
                return Ok(hanghoa);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
