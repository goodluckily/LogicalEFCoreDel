using LogicalEFCoreDel.Extensions;
using LogicalEFCoreDel.Model;
using Microsoft.AspNetCore.Mvc;

namespace LogicalEFCoreDel.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly LogicalDbContext _logicalDbContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,LogicalDbContext logicalDbContext)
        {
            _logger = logger;
            _logicalDbContext = logicalDbContext;
        }

        [HttpPost("AddUsers")]
        public ActionResult AddUsers([FromQuery]bool IsDelete=false)
        {
            _logicalDbContext.UserInfo.Add(new UserInfo
            {
                Id=Guid.NewGuid().ToString(),
                Name="",
                Age=1,
                IsDeleted= IsDelete,
            });
            _logicalDbContext.SaveChanges();
            return Ok();
        }


        [HttpGet("GetUsers")]
        public ActionResult GetUsers()
        {
            var users = _logicalDbContext.UserInfo.IncludeSoftDeleted(false).ToList();
            return new JsonResult(users);
        }

        [HttpDelete("DelUsers")]
        public ActionResult DelUsers([FromQuery] string Id)
        {
            var model = _logicalDbContext.UserInfo.IncludeSoftDeleted(true).Where(x=>x.Id== Id).FirstOrDefault();
            if (model != null) 
            {
                //_logicalDbContext.UserInfo.Remove(model);
                _logicalDbContext.UserInfo.SoftDelete(model);

                
                _logicalDbContext.SaveChanges();
                return Ok();
            }
            return Content("Ê§°Ü!");
        }


        [HttpPost("AddAttachMent")]
        public ActionResult AddAttachMent([FromQuery] bool IsDelete = false)
        {
            _logicalDbContext.AttachMent.Add(new AttachMent
            {
                Id = Guid.NewGuid().ToString(),
                FileName = "1",
                FileType ="2",
                UploadDate= DateTime.Now,
                IsDeleted = IsDelete,
            });
            _logicalDbContext.SaveChanges();
            return Ok();
        }

        [HttpGet("GetAddAttachMent")]
        public ActionResult GetAddAttachMent()
        {
            var attaches = _logicalDbContext.AttachMent.ToList();
            return new JsonResult(attaches);
        }
    }
}
