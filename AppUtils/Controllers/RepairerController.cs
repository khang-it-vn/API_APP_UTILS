using AppUtils.Data;
using AppUtils.Models;
using AppUtils.Prototypes;
using AppUtils.Services.IRepositories;
using BingMapsRESTToolkit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppUtils.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class RepairerController : ControllerBase
    {
        private IUserRepository _userService;
        private IRepairerRepository _repairerService;


        public RepairerController(IUserRepository userService, IRepairerRepository repairerService)
        {
            _userService = userService;
            _repairerService = repairerService;
        }

        /// <summary>
        /// Đăng nhập - Login
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(RepairerModelLogin account)
        {
            Repairer repairer = findRepairerByModel(account);
            if( repairer != null)
            {
                return Ok(new ApiResponse
                {
                    Data = repairer,
                    Message = "Login",
                    Success = true
                });
            }
            return Ok(new ApiResponse
            {
                Data = "Wrong username or password",
                Message = "Login",
                Success = false
            });
        }

        /// <summary>
        /// Tìm thông tin thợ sửa chữa 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        private Repairer findRepairerByModel(RepairerModelLogin account)
        {
            Repairer repairer = _repairerService.GetByCondition(r => r.NumberPhone.Equals(account.SoDienThoai) && account.MatKhau.Equals(r.MatKhau)).FirstOrDefault();
            return repairer;
        }

        /// <summary>
        /// Bật/Tắt chế độ hoạt động
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("Active")]
        public async Task<IActionResult> ActiveNonActived(Guid id)
        {
            Repairer repairer = findRepairerById(id);
            if(repairer != null)
            {
                try
                {
                    repairer.TrangThaiHoatDong = !repairer.TrangThaiHoatDong;
                    _repairerService.Update(repairer);
                    _repairerService.Save();

                    return Ok(new ApiResponse
                    {
                        Message = "Active | Non",
                        Data = repairer,
                        Success = true
                    });
                }catch(Exception e)
                {
                    return Ok(new ApiResponse
                    {
                        Data = e.Message,
                        Success = false,
                        Message = "error while active account"
                    });
                }
            }
            return Ok(new ApiResponse
            {
                Data = "ID not exists",
                Success = false,
                Message = "Active | Non"
            });
        }

        /// <summary>
        /// Tìm kiếm tài khoản thợ sửa chữa theo id
        /// </summary>
        /// <param name="id">id: Guid</param>
        /// <returns>null if not exists else return repairer</returns>
        private Repairer findRepairerById(Guid id)
        {
            Repairer repairer = _repairerService.GetById(id);
            return repairer;
        }

        /// <summary>
        /// Lấy thông tin người dùng từ số điện thoại
        /// </summary>
        /// <param name="soDienThoai">Số điện thoại: String</param>
        /// <returns></returns>
        [HttpGet("InformationUser")]
        public async Task<IActionResult> GetInformationUserByNumberPhone(String soDienThoai)
        {
            User user = findUserByNumberPhone(soDienThoai);
            if(user != null)
            {
                Dictionary<String,double> lat = await GetLatitudeOfUser(user.DiaChi);

                UserLatLng userLatLng = new UserLatLng()
                {
                    UID = user.UID,
                    HoTen = user.HoTen,
                    
                    DiaChi = user.DiaChi,
                    DOB = user.DOB,
                    Avatar = user.Avatar,
                    Email = user.Email,
                    GioiTinh = user.GioiTinh,
                    Latitude = lat.GetValueOrDefault("latitude") ,
                    Longitude = lat.GetValueOrDefault("longitude"),
                    MatKhau = user.MatKhau,
                    NumberPhone = user.NumberPhone,
                };

                return Ok(new ApiResponse
                {
                    Data = userLatLng,
                    Message = "Information Of User",
                    Success = true
                });
            }
            return Ok(new ApiResponse
            {
                Data = null,
                Message = "Information Of User",
                Success = false
            }); 
        }

        /// <summary>
        /// Tìm kiếm user theo số điện thoại
        /// </summary>
        /// <param name="soDienThoai">Số điện thoại: String</param>
        /// <returns>null if not exists else User </returns>
        private User findUserByNumberPhone(string soDienThoai)
        {
            User user = _userService.GetByCondition(u => u.NumberPhone.Equals(soDienThoai)).FirstOrDefault();
            return user;
        }

        [HttpPost("CreateNewAccount")]
        public async Task<IActionResult> CreateNewRepairer(RepairerModel account)
        {
            Repairer repairer = new Repairer()
            {
                ID = Guid.NewGuid(),
                HoTen = account.HoTen,
                MatKhau = account.MatKhau,
                NumberPhone = account.SoDienThoai,
                DiaChi = account.DiaChi,
                Latitude = account.Latitude,
                Longitude = account.Longitude,
            };
            try
            {
                _repairerService.Add(repairer);
                _repairerService.Save();
                return Ok(repairer);
            }catch (Exception e)
            {
                return BadRequest(e.Message);
            } 
            return NoContent();
            
        }

        private async Task<Dictionary<String, Double>> GetLatitudeOfUser(String query)
        {
            //Create a request.
            var request = new GeocodeRequest()
            {
                Query = query,
                BingMapsKey = "Aoic4Sz682M0RXoba2iGgamH_t-4bbIOHGE0ayhqpCMlazlTzlAx1cv6-gpQRBMM"
            };

            //Process the request by using the ServiceManager.
            var response = await request.Execute();

            if (response != null &&
                response.ResourceSets != null &&
                response.ResourceSets.Length > 0 &&
                response.ResourceSets[0].Resources != null &&
                response.ResourceSets[0].Resources.Length > 0)
            {
                var result = response.ResourceSets[0].Resources[0] as BingMapsRESTToolkit.Location;

                Double latitude = result.Point.Coordinates[0];
                Double longitude = result.Point.Coordinates[1];

                Dictionary<String, Double> lat = new Dictionary<String, Double>();

                lat.Add("latitude", latitude);
                lat.Add("longitude", longitude);
                return lat;


            }
            return null;
        }

        [HttpGet("GetImageByName")]
        public async Task<IActionResult> GetImageByName(String imageName)
        {
            try
            {
                string pathImage = @"D:\DOANMobile\AppUtils\AppUtils\Public\images\" + imageName;

                byte[] imageByte = System.IO.File.ReadAllBytes(pathImage);

                return File(imageByte, "image/png");

            }
            catch(Exception e)
            {
                return Ok(null);
            }
        }
    }
}
