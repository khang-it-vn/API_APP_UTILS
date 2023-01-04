using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppUtils.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Bai7Controller : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<SinhVien> sinhViens = new List<SinhVien>();
            SinhVien sinhVien = new SinhVien()
            {
                HoTen = "Nguyễn Văn A", MSSV="1911061111",Email="nguyenvana@gmail.com"
            };
            sinhViens.Add(sinhVien);

            SinhVien sinhVien1 = new SinhVien()
            {
                HoTen = "Nguyễn Văn B",
                MSSV = "1911061112",
                Email = "nguyenvanb@gmail.com"
            };
            sinhViens.Add(sinhVien1);
            SinhVien sinhVien2 = new SinhVien()
            {
                HoTen = "Nguyễn Văn C",
                MSSV = "1911061113",
                Email = "nguyenvanc@gmail.com"
            };
            sinhViens.Add(sinhVien2);
            SinhVien sinhVien3 = new SinhVien()
            {
                HoTen = "Nguyễn Văn D",
                MSSV = "1911061111",
                Email = "nguyenvand@gmail.com"
            };
            sinhViens.Add(sinhVien3);
            SinhVien sinhVien4 = new SinhVien()
            {
                HoTen = "Nguyễn Văn AE",
                MSSV = "1911061111",
                Email = "nguyenvana@gmail.com"
            };
            sinhViens.Add(sinhVien4);

            return Ok(sinhViens);


        }
    }

    class SinhVien
    {
        public string HoTen { get; set; }
        public string MSSV { get; set; }

        public string Email { get; set; }
    }
}
