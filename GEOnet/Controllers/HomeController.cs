using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GEOnet.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using GeoJSON;

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



        //поиск по nameID не точный. Отключил 08.09.2019
        public async Task<IActionResult> printname(string searchName)
        {
            var name = from n in db.geoModels
                       select n;
            if (!String.IsNullOrEmpty(searchName))
            {
                name = name.Where(s => s.NameUser.Contains(searchName));
            }

            return View(await name.ToListAsync());
        }

        //==========
        //поиск по username и namedevice
        public async Task<IActionResult> searchnamedev(string searchName, string searchDev)
        {
            var name = from n in db.geoModels
                       select n;
            if (!String.IsNullOrEmpty(searchName))
            {
                name = name.Where(s => s.NameUser.Contains(searchName));
            }

            if (!String.IsNullOrEmpty(searchDev))
            {
                name = name.Where(d => d.geonamedevice == searchDev);
            }

            return View(await name.ToListAsync());
        }
        //==========
        //===map показваем одну точку=====
        public async Task<IActionResult> geomap(string Name,string Dev,string Latitude,string Longitude, string GeoTM,string GeoDT)
        {
            //преобразуем "," в "."
            Latitude = Latitude.Replace(",", ".");  
            Longitude = Longitude.Replace(",", ".");
            //передаём координаты в пердставление geomap
            ViewData["Name"] = Name;
            ViewData["Dev"] = Dev;
            ViewData["Latitude"] = Latitude;
            ViewData["Longitude"] = Longitude;
            ViewData["GeoTM"] = GeoTM;
            ViewData["GeoDT"] = GeoDT;
            return View();
        }
        //===========
        //==Показываем много точек======
        public async Task<IActionResult> geomapall(string Name, string Dev)
        {
            var name = from n in db.geoModels
                       select n;
            if (!String.IsNullOrEmpty(Name))
            {
                name = name.Where(s => s.NameUser.Contains(Name));
            }

            if (!String.IsNullOrEmpty(Dev))
            {
                name = name.Where(d => d.geonamedevice == Dev);
            }

            string location = "";
            foreach (var gm in name)
            {
                location = location + "[" + gm.LatitudeX.ToString().Replace(',', '.') + "," + gm.LongitudeY.ToString().Replace(',', '.') + "],";
            }
            string resultlocation = "[" + location + "]";

            ViewData["Name"] = Name;
            ViewData["Dev"] = Dev;
            ViewData["Resultlocation"] = resultlocation;

            return View();
        }
        //===========
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
        [HttpGet]
        public IActionResult rmuser(int id)
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

            var uname = await db.geoUsers.FirstOrDefaultAsync(s => s.username == ingeoModel.NameUser);
            var udevice = await db.geoUsers.FirstOrDefaultAsync(d => d.namedevice == ingeoModel.geonamedevice);
            if (uname == null)
            {
                return NotFound("Указанный пользователь не существует в БД");
            }
            else
            {
                if (udevice == null)
                {
                    return View("Views/Shared/DevNoDB.cshtml");
                }
                else
                {
                    ingeoModel.geoTM = localDate.ToString("HH:mm:ss");                       
                    ingeoModel.geoDT = localDate.ToString("dd.MM.yyyy");

                    db.geoModels.Add(ingeoModel);
                    db.SaveChanges();
                    return View("Views/Shared/adduserOK.cshtml");

                }

            }

        }

        //создаём пользователя
        [HttpPost]
        public async Task<IActionResult> inputuser(geoUser ingeoUser)
        {
            if ((ingeoUser.username == null) | (ingeoUser.namedevice == null))
            {
                return View("Views/Shared/AddUserNO.cshtml");
            }
            else
            {
                var uname = await db.geoUsers.FirstOrDefaultAsync(s => s.username == ingeoUser.username);
                var udevice = await db.geoUsers.FirstOrDefaultAsync(d => d.namedevice == ingeoUser.namedevice);
                if ((uname != null) & (udevice != null))
                {
                    return View("Views/Shared/UserDev0.cshtml");
                }
                else if ((uname != null) & (udevice == null))
                {
                    ingeoUser.tm = localDate.ToString("HH:mm:ss");
                    ingeoUser.dt = localDate.ToString("dd.MM.yyyy");
                    db.geoUsers.Add(ingeoUser);
                    db.SaveChanges();
                    return View("Views/Shared/adduserOK.cshtml");
                }
                else
                {
                    ingeoUser.tm = localDate.ToString("HH:mm:ss");
                    ingeoUser.dt = localDate.ToString("dd.MM.yyyy");
                    db.geoUsers.Add(ingeoUser);
                    db.SaveChanges();
                    return View("Views/Shared/adduserOK.cshtml");
                }
            }
        }
        //-***************************************
        //-*********Удаление пользователя********
        [HttpPost]
        public async Task<IActionResult> rmuser(geoUser ingeoUser)
        {
            //if ((ingeoUser.username == null) | (ingeoUser.namedevice == null))
            //{
            //    //return Ok("Укажите имя пользователя и название устройства ");
            //    return View("Views/Shared/AddUserNO.cshtml");
            //}
            //else
            //{

             var uname = await db.geoUsers.FirstOrDefaultAsync(s => s.username == ingeoUser.username);
                var uname0 = await db.geoModels.FirstOrDefaultAsync(s => s.NameUser == ingeoUser.username);
                if (uname != null)
                {
                    db.geoUsers.Remove(uname); //удаляем пользоваиеля
                if (uname0 != null)
                {
                    db.geoModels.Remove(uname0); //удаляем координаты
                }
                    db.SaveChanges(); //Сохраняем изменения в БД

            }
            return View("Views/Shared/DellUser.cshtml");
           // }
        }

        //-**********-Получение координат в формате JSON ********************
        [HttpPost]
        public async Task<IActionResult> inputgeoJSON([FromBody]geoModel ingeoModel)
        {
        var uname = await db.geoUsers.FirstOrDefaultAsync(s => s.username == ingeoModel.NameUser);
        var udevice = await db.geoUsers.FirstOrDefaultAsync(d => d.namedevice == ingeoModel.geonamedevice);
        if (uname == null)
        {
        return View("Views/Shared/UserDev0.cshtml");
        }
        else
        {
        if (udevice == null)
        {
        return View("Views/Shared/DevNoDB.cshtml");
        }
        else
        {
        ingeoModel.geoTM = localDate.ToString("HH:mm:ss");
        ingeoModel.geoDT = localDate.ToString("dd.MM.yyyy");                                                           
        db.geoModels.Add(ingeoModel);
        db.SaveChanges();
        return View("Views/Shared/adduserOK.cshtml");
        }
        }
        }

        //Создание пользователя с использованием JSON
        [HttpPost]
        public async Task<IActionResult> inputuserJSON([FromBody]geoUser ingeoUser)
        {
        var uname = await db.geoUsers.FirstOrDefaultAsync(s => s.username == ingeoUser.username);
        var udevice = await db.geoUsers.FirstOrDefaultAsync(d => d.namedevice == ingeoUser.namedevice);
        if ((uname != null) & (udevice != null))
        {
        return View("Views/Shared/UserDev0.cshtml");
        }
        else if ((uname != null) & (udevice == null))
        {
        ingeoUser.tm = localDate.ToString("HH:mm:ss");
        ingeoUser.dt = localDate.ToString("dd.MM.yyyy");
        db.geoUsers.Add(ingeoUser);
        db.SaveChanges();
        return View("Views/Shared/adduserOK.cshtml");
        }
        else
        {
        ingeoUser.tm = localDate.ToString("HH:mm:ss");
        ingeoUser.dt = localDate.ToString("dd.MM.yyyy");
        db.geoUsers.Add(ingeoUser);
        db.SaveChanges();
        return View("Views/Shared/adduserOK.cshtml");
        }
        }
    }

}
