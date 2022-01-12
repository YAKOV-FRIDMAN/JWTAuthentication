using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JWTAuthentication.Authentication;
using JWTAuthentication.InputModels;
using JWTAuthentication.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;

namespace JWTAuthentication.Controllers
{
    //[Authorize(Roles = UserRoles.Admin)]
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext applicationDbContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            this.userManager = userManager;
            this.applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpPost]
        [Route("insertpost")]
        public async Task<IActionResult> InsertPost([FromForm] PostInput post)
        {
            // get user id
            var userId = (await userManager.FindByNameAsync(User.Identity.Name)).Id;
            post.UserId = userId;

            // Write the file in folder
            byte[] fileBytes = new byte[] { };
            using (var ms = new MemoryStream())
            {
                post.File.CopyTo(ms);
                fileBytes = ms.ToArray();
            }
            string filePath = @"C:\Users\User\Downloads\JWTAuthentication\JWTAuthentication\Files\" + post.File.FileName;
            System.IO.File.WriteAllBytes(filePath, fileBytes);
            
            // save file path in db
            var result = await applicationDbContext.ServerFiles.AddAsync(new ServerFiles
            {
                FilePath = filePath,
                FileType = Enums.FileType.All,
                ModifiedByUserID = 0,
                ModifiedDate = DateTime.Now,
                OwnerID = 0,
                OwnerType = Enums.OwnerType.System,
                TimeStamp = DateTime.Now,
            });
            await applicationDbContext.SaveChangesAsync();

            // save th post in db
            (post as Post).File = result.Entity.ID;
            applicationDbContext.Posts.Add(post);
            await applicationDbContext.SaveChangesAsync();

            return Ok();

        }

        [HttpGet]
        [Route("GetFile")]
        public async Task<IActionResult> GetFile(int PathID)
        {
            var curFile = applicationDbContext.ServerFiles.FirstOrDefault(_=>_.ID == PathID);
            string filePath = curFile.FilePath;
            byte[] imageData = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(imageData, MimeTypes.GetMimeType(Path.GetExtension(filePath)), "image");
        }
    }
}
