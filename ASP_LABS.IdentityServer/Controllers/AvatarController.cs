using ASP_LABS.IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using static System.Formats.Asn1.AsnWriter;

namespace ASP_LABS.IdentityServer.Controllers
{
    public class AvatarController : Controller
    {
        IWebHostEnvironment _env;
        UserManager<ApplicationUser>  _manager;
        public AvatarController(IWebHostEnvironment env,UserManager<ApplicationUser> manager)
        {
            _env = env;
            _manager = manager;
        }


        [Authorize]
        [Route("[controller]")]
        public IActionResult Index()
        {

            var id = _manager.GetUserId(User);


            var path = Path.Combine(_env.ContentRootPath, "Images", $"{id}.jpg");
            
            if(!System.IO.File.Exists(path)) 
            {
                path = Path.Combine(_env.ContentRootPath, "Images", "defaultAvatar.png");
            }

            var typeProvider = new FileExtensionContentTypeProvider();
            string type; 
            typeProvider.TryGetContentType(path,out type);

            FileStream fs = new FileStream(path, FileMode.Open);
            return File(fs, type);
        }
    }
}
