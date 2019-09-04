using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GEOnet.Models;
using Microsoft.EntityFrameworkCore;

namespace GEOnet.Controllers
{
    public class HomeController : Controller
    {
        DateTime localDate = DateTime.Now;

        geoContext db;
        public HomeController(geoContext context)
        {
            db = context;
        }

        //вывод всего содержимого БД
        public IActionResult Index()
        {
            return View(db.geoUsers.ToList());
        }

        public IActionResult geoall()
        {
            return View(db.geoModels.ToList());
        }
        //поиск по nameID
        public async Task<IActionResult> printname(string searchName)
        {
            var name = from n in db.geoModels
                       select n;
            if (!String.IsNullOrEmpty(searchName))
            {
                name = name.Where(s => s.nameID.Contains(searchName));
            }

            return View(await name.ToListAsync());
        }

        // ввод пользователя и его координат
        [HttpGet]
        public IActionResult inputgeo(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpGet]
        public IActionResult inputuser(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        //test JSON--
        //возвращает в виде JSON
        [HttpGet]
        public async Task<ActionResult<IEnumerable<geoUser>>> GetgeoUsers()
        {
            return await db.geoUsers.ToListAsync();
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<geoUser>> GetgeoUser(int Id)
        {
            var user = await db.geoUsers.FindAsync(Id);
            if (user == null)
            {
                return NotFound("БД пусто");
            }
            return user;
        }

        //тест создание пользователя в БД с помощью JSON.
        //Неполучалось принять данные из JSON без [FromBody]
        [HttpPost]
        public async Task<ActionResult<geoUser>> PostgeoUser([FromBody]geoUser item)
        {

            item.tm = localDate.ToString("HH:mm:ss");
            item.dt = localDate.ToString("dd.MM.yyyy");

            db.geoUsers.Add(item);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetgeoUser), new { id = item.Id }, item);
        }

        //----
        // **********************************
        //вводим данные и проверяем существование пользователя
        [HttpPost]
        public async Task<IActionResult> inputgeo(geoModel ingeoModel)
        {

            var uname = await db.geoUsers.FirstOrDefaultAsync(s => s.username == ingeoModel.nameID);
            var udevice = await db.geoUsers.FirstOrDefaultAsync(d => d.namedevice == ingeoModel.geonamedevice);
            if (uname == null)
            {
                return NotFound("Указанный пользователь не существует в БД");
            }
            else
            {
                if (udevice == null)
                {
                    return NotFound("Указанного устройства нет в БД");
                }
                else
                {
                    ingeoModel.geoTM = localDate.ToString("HH:mm:ss");                       
                    ingeoModel.geoDT = localDate.ToString("dd.MM.yyyy");

                    db.geoModels.Add(ingeoModel);
                    db.SaveChanges();
                    return Ok("Данные внесены в БД");

                }

            }

        }

        //создаём пользователя
        [HttpPost]
        public async Task<IActionResult> inputuser(geoUser ingeoUser)
        {

            var uname = await db.geoUsers.FirstOrDefaultAsync(s => s.username == ingeoUser.username);
            var udevice = await db.geoUsers.FirstOrDefaultAsync(d => d.namedevice == ingeoUser.namedevice);
            if ((uname != null) & (udevice != null))
            {
                return NotFound("Указанные пользователь и устройство уже существует в БД");
            }
            else if ((uname != null) & (udevice == null))
            {
                ingeoUser.tm = localDate.ToString("HH:mm:ss");
                ingeoUser.dt = localDate.ToString("dd.MM.yyyy");
                db.geoUsers.Add(ingeoUser);
                db.SaveChanges();
                return Ok("Данные внесены в БД");
            }
            else 
            {
                ingeoUser.tm = localDate.ToString("HH:mm:ss");
                ingeoUser.dt = localDate.ToString("dd.MM.yyyy");
                db.geoUsers.Add(ingeoUser);
                db.SaveChanges();
                return Ok("Данные внесены в БД");
            }

        }
        //-***************************************
        //-**********-JSON-BEGIN********************
        [HttpPost]
        public async Task<IActionResult> inputgeoJSON([FromBody]geoModel ingeoModel)
        {

            var uname = await db.geoUsers.FirstOrDefaultAsync(s => s.username == ingeoModel.nameID);
            var udevice = await db.geoUsers.FirstOrDefaultAsync(d => d.namedevice == ingeoModel.geonamedevice);
            if (uname == null)
            {
                return NotFound("Указанный пользователь не существует в БД");
            }
            else
            {
                if (udevice == null)
                {
                    return NotFound("Указанного устройства нет в БД");
                }
                else
                {
                    ingeoModel.geoTM = localDate.ToString("HH:mm:ss");
                    ingeoModel.geoDT = localDate.ToString("dd.MM.yyyy");

                    db.geoModels.Add(ingeoModel);
                    db.SaveChanges();
                    return Ok("Данные внесены в БД");

                }

            }

        }

        [HttpPost]
        public async Task<IActionResult> inputuserJSON([FromBody]geoUser ingeoUser)
        {

            var uname = await db.geoUsers.FirstOrDefaultAsync(s => s.username == ingeoUser.username);
            var udevice = await db.geoUsers.FirstOrDefaultAsync(d => d.namedevice == ingeoUser.namedevice);
            if ((uname != null) & (udevice != null))
            {
                return NotFound("Указанные пользователь и устройство уже существует в БД");
            }
            else if ((uname != null) & (udevice == null))
            {
                ingeoUser.tm = localDate.ToString("HH:mm:ss");
                ingeoUser.dt = localDate.ToString("dd.MM.yyyy");
                db.geoUsers.Add(ingeoUser);
                db.SaveChanges();
                return Ok("Данные внесены в БД");
            }
            else
            {
                ingeoUser.tm = localDate.ToString("HH:mm:ss");
                ingeoUser.dt = localDate.ToString("dd.MM.yyyy");
                db.geoUsers.Add(ingeoUser);
                db.SaveChanges();
                return Ok("Данные внесены в БД");
            }

        }
        //-**********-JSON-END-********************
    }

}
