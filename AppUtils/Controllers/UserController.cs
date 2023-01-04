using AppUtils.Data;
using AppUtils.Models;
using AppUtils.Prototypes;
using AppUtils.Services.IRepositories;
using AppUtils.Utils;
using BingMapsRESTToolkit;
using BingMapsRESTToolkit.Extensions;
using GMap.NET;
using GMap.NET.MapProviders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AppUtils.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepository _userService;
        private IRepairerRepository _repairerService;


        public UserController (IUserRepository userService, IRepairerRepository repairerService)
        {
            _userService = userService;
            _repairerService = repairerService;
        }

        /// <summary>
        /// Tạo mới tài khoản user
        /// </summary>
        /// <param name="account">Account{HoTen: String, SoDienThoai: String, MatKhau String}</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAccount(AccountUserModel account)
        {
            User user = new User
            {
                UID = Guid.NewGuid(),
                HoTen = account.HoTen,
                NumberPhone = account.SoDienThoai,
                MatKhau = account.MatKhau,
                DiaChi= "chưa cập nhật",
                Avatar = "hinhanhdefault.png"
            };
            try
            {

                _userService.Add(user);
                _userService.Save();
                return Ok(new ApiResponse
                {
                    Data = user,
                    Message = "Create New Account",
                    Success = true
                });
            }catch(Exception e)
            {
                return Ok(new ApiResponse
                {
                    Data = null,
                    Message = "Create New Account",
                    Success = false
                });
            }
           
        }

        /// <summary>
        /// Lấy danh sách user
        /// </summary>
        /// <returns>List<User></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_userService.GetAll().ToList());
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="account">Account{SoDienThoai: String, TaiKhoan: String}</param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserModelLogin account)
        {
            User user = findAccount(account);
            if (user != null)
            {
                return Ok(new ApiResponse
                {
                    Data = user,
                    Message = "Login",
                    Success = true
                });
            }    
            return Ok(new ApiResponse
            {
                Data = null,
                Message = "Login",
                Success = false
            });

        }

        /// <summary>
        /// Tìm kiếm tài khoản user
        /// </summary>
        /// <param name="account">Account{SoDienThoai: String, TaiKhoan: String}</param>
        /// <returns>return account if exists else null</returns>
        private User findAccount(UserModelLogin account)
        {
            User user = _userService.GetByCondition(u => u.NumberPhone.Equals(account.SoDienThoai) && u.MatKhau.Equals(account.MatKhau)).FirstOrDefault();
            return user;
        }

        

        /// <summary>
        /// Lấy thông tin thợ sửa chữa 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("RepairerInfomation")]
        public async Task<IActionResult> GetInfoOfRepairer(Guid id)
        {
            Repairer repairer = findRepairerByID(id);
            if(repairer != null)
            {
                return Ok(new ApiResponse
                {
                    Data = repairer,
                    Message = "Get Infomation of Repairer By ID",
                    Success = true
                });
            }
            return Ok(new ApiResponse
            {
                Data = null,
                Message = "Get Infomation of Repairer By ID",
                Success = false
            });

        }

        /// <summary>
        /// Lấy thông tin thợ sửa chửa bằng id
        /// </summary>
        /// <param name="id">id bác sĩ cần tìm</param>
        /// <returns></returns>
        private Repairer findRepairerByID(Guid id)
        {
            Repairer repairer = _repairerService.GetById(id);
            return repairer;
        }

        [HttpGet("GetLatitude")]
        public async Task<IActionResult> GetLatitude(String query)
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

                //Do something with the result.
                return Ok(result);
            }
            return BadRequest();
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



        [HttpGet("GetDistance")]
        public async Task<IActionResult> GetDistance()
        {
            Dictionary<String, Double> lat = await GetLatitudeOfUser("Đ. Trường Lưu, Trường Thạnh, Quận 9, Thành phố Hồ Chí Minh, Vietnam");


            double result = CalculateDistance(10.80406, 106.82387, 10.81855, 106.81028);
            return Ok(result);
            
        }

        /// <summary>
        /// Tìm kiếm thợ sửa chữa gần đây
        /// </summary>
        /// <param name="diaChi"></param>
        /// <returns></returns>
        [HttpGet("GetRepairerArround")]
        public async Task<IActionResult> GetRepairerArround(String diaChi)
        {
            Dictionary<String, Double> lat = await GetLatitudeOfUser(diaChi);

            // Lấy danh sách các repairer đang hoạt động
            List<Repairer> repairers = _repairerService.GetByCondition(r => r.TrangThaiHoatDong).ToList();

            List<RepairerDistance> repairerDistances = new List<RepairerDistance>();
            foreach (Repairer repairer in repairers)
            {
                // Tạo Một repairerDistance
                RepairerDistance repairerDistance = new RepairerDistance()
                {
                    ID = repairer.ID,
                    HoTen = repairer.HoTen,
                    TrangThaiHoatDong = repairer.TrangThaiHoatDong,
                    DiaChi = repairer.DiaChi,
                    DOB = repairer.DOB,
                    Avatar = repairer.Avatar,
                    Email = repairer.Email,
                    GioiTinh = repairer.GioiTinh,
                    Latitude = repairer.Latitude,
                    Longitude = repairer.Longitude,
                    MatKhau = repairer.MatKhau,
                    NumberPhone = repairer.NumberPhone,

                };

                //tính khoảng cách
                double distance = CalculateDistance(repairer.Latitude, repairer.Longitude, lat.GetValueOrDefault("latitude"), lat.GetValueOrDefault("longitude"));
                repairerDistance.Distance = distance;

                // thêm vào danh sách các repairer đang hoạt động
                repairerDistances.Add(repairerDistance);
            }    

            
            return Ok(new ApiResponse
            {
                Data= repairerDistances,
                Message = "Get Repairer Arround",
                Success = true
            });

        }

        /// <summary>
        /// Tính khoản cách từ 2 điểm
        /// </summary>
        /// <param name="prevLat">LatitudeLast</param>
        /// <param name="prevLong"></param>
        /// <param name="currLat"></param>
        /// <param name="currLong"></param>
        /// <returns>Distance (meter)</returns>
        private double CalculateDistance(double prevLat, double prevLong, double currLat, double currLong)
        {
            const double degreesToRadians = (Math.PI / 180.0);
            const double earthRadius = 6371; // kilometers

            // convert latitude and longitude values to radians
            var prevRadLat = prevLat * degreesToRadians;
            var prevRadLong = prevLong * degreesToRadians;
            var currRadLat = currLat * degreesToRadians;
            var currRadLong = currLong * degreesToRadians;

            // calculate radian delta between each position.
            var radDeltaLat = currRadLat - prevRadLat;
            var radDeltaLong = currRadLong - prevRadLong;

            // calculate distance
            var expr1 = (Math.Sin(radDeltaLat / 2.0) *
                         Math.Sin(radDeltaLat / 2.0)) +

                        (Math.Cos(prevRadLat) *
                         Math.Cos(currRadLat) *
                         Math.Sin(radDeltaLong / 2.0) *
                         Math.Sin(radDeltaLong / 2.0));

            var expr2 = 2.0 * Math.Atan2(Math.Sqrt(expr1),
                                         Math.Sqrt(1 - expr1));

            var distance = (earthRadius * expr2);
            return distance * 1000;  // return results as meters
        }

        /// <summary>
        /// Chỉnh sửa thông tin cá nhân user
        /// </summary>
        /// <param name="newInfo"></param>
        /// <returns></returns>
        [HttpPatch("EditInfo")]
        public async Task<IActionResult> EditInformation (UserEditProfileModel newInfo)
        {
            User user = _userService.GetById(newInfo.UID);
            try
            {
                if (user != null)
                {
                    user.HoTen = newInfo.HoTen;
                    user.Email = newInfo.Email;
                    user.DiaChi = newInfo.DiaChi;
                    //user.DOB = newInfo.DOB;
                    //user.GioiTinh = newInfo.GioiTinh;
                    _userService.Update(user);
                    _userService.Save();
                    return Ok(new ApiResponse
                    {
                        Data = user,
                        Message = "Edit info user",
                        Success = true
                    });
                }
            }catch(Exception e)
            {
                return Ok(new ApiResponse
                {
                    Data = null,
                    Message = e.Message,
                    Success = false
                });
            }
            return Ok(new ApiResponse
            {
                Data = null,
                Message = "Edit info user",
                Success = false
            });

        }

        // Send code submit

        [HttpGet("SendCodeTest")]
        public async Task<IActionResult> SendCodeTest(String numberphone)
        {
            WebClient client = new WebClient();
            Stream s = client.OpenRead(String.Format("http://rest.esms.vn/MainService.svc/json/SendMultipleMessage_V4_get?Phone=0987546775&Content=OTP&ApiKey=F3FAACB7A18F3F2101C9812566936D&SecretKey=E81A02C3E7AB54938E85F16574DF95&SmsType=2&Brandname=100Baotrixemay2"));
            StreamReader reader = new StreamReader(s);
            String result = reader.ReadToEnd();
            return Ok(result);
        }

        private const int CODE_RAND_TEST = 6666;

        /// <summary>
        /// So sánh mã code user cung cấp với hệ thống gửi đi
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("CompareCode")]
        public async Task<IActionResult> CompareCode(Guid uid, int code)
        {
            User user = _userService.GetById(uid);
            if(user != null && code == CODE_RAND_TEST)
            {
                return Ok(new ApiResponse
                {
                    Data = user,
                    Message = "Compare code random of user",
                    Success = true
                });
            }
            return Ok(new ApiResponse
            {
                Data = null,
                Message = "Compare code random of user",
                Success = false
            });
        }

        [HttpPatch("UpdatePassword")]
        public async Task<IActionResult> Updatepassword(Guid uid, String matKhau)
        {
            User user = _userService.GetById(uid);
            try
            {
                if(user != null)
                {
                    user.MatKhau = matKhau;
                    _userService.Update(user);
                    _userService.Save();
                }
                return Ok(new ApiResponse
                {
                    Data = user,
                    Message = "Update Password",
                    Success = true
                });
            }catch(Exception e)
            {
                return Ok(new ApiResponse
                {
                    Data = null,
                    Message = e.Message,
                    Success = false
                });
            }

        }

        [HttpGet("GetUIDByNumberPhone")]
        public async Task<IActionResult> GetUIDByNumberPhone(String numberPhone)
        {
            User user = _userService.GetByCondition(c => c.NumberPhone.Equals(numberPhone)).FirstOrDefault();
            if(user != null)
            {
                return Ok(new ApiResponse
                {
                    Data = user,
                    Success = true,
                    Message = "Get uid by number phone"
                });
            }
            return Ok(new ApiResponse
            {
                Data = null,
                Success = false,
                Message = "Get uid by number phone"
            });
        }
        [HttpPost("UploadImages")]
        public IActionResult UploadImages(Guid uid, IFormFile image)
        {
            User user = _userService.GetById(uid);
           try
            {
                if (image.Length > 0)
                {
                    String fileName = "avatar" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Millisecond + ".png";
                    user.Avatar = fileName;
                    _userService.Update(user);
                    _userService.Save();
                    using (FileStream fileStream = System.IO.File.Create(@"D:\DOANMobile\AppUtils\AppUtils\Public\images\" + fileName))
                    {
                        image.CopyTo(fileStream);
                        fileStream.Flush();

                    }
                    return Ok(new ApiResponse
                    {
                        Data = fileName,
                        Message = "Upload Image",
                        Success = true,
                    });
                }
               
            }
            catch (Exception e)
            {

                return Ok(new ApiResponse
                {
                    Data =null,
                    Message = "Upload Image",
                    Success = false,
                });
            }
        }
    }

}
