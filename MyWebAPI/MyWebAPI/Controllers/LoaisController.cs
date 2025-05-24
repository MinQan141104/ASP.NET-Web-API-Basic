using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Data;
using MyWebAPI.Models;

namespace MyWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaisController : ControllerBase
    {
        private readonly MyDbContext _context;

        public LoaisController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var dsLoai = _context.Loais.ToList();

            return Ok(dsLoai);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var loai = _context.Loais.SingleOrDefault(l => l.MaLoai == id);
            if (loai == null)
            {
                return NotFound();
            }

            return Ok(loai);
        }

        [HttpPost]
        public IActionResult CreateLoai(LoaiModel model)
        {
            try
            {
                var loai = new Loai
                {
                    TenLoai = model.TenLoai
                };
                _context.Add(loai);
                _context.SaveChanges();
                return Ok(loai);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateLoai(int id, LoaiModel model)
        {
            var loai = _context.Loais.SingleOrDefault(l => l.MaLoai == id);
            if (loai == null)
            {
                return NotFound();
            }
            loai.TenLoai = model.TenLoai;
            _context.Update(loai);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
