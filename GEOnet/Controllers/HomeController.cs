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

        //[HttpPost]
        //public string inputgeo(geoModel geoModel)
        //{
        //    db.geoModels.Add(geoModel);
        //    db.SaveChanges();
        //    return geoModel.nameID + " координыта занесены в БД";

        //}
        //вводим данные и проверяем существование пользователя
        [HttpPost]
        public async Task<IActionResult> inputgeo(geoModel geoModel)
        {

            var uname = await db.geoUsers.FirstOrDefaultAsync(s => s.username == geoModel.nameID);
            var udevice = await db.geoUsers.FirstOrDefaultAsync(d => d.namedevice == geoModel.geonamedevice);
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
                    geoModel.geoTM = localDate.ToString("HH:mm:ss");                       
                    geoModel.geoDT = localDate.ToString("dd.MM.yyyy");

                    db.geoModels.Add(geoModel);
                    db.SaveChanges();
                    return Ok("Данные внесены в БД");

                }

            }

        }

        [HttpPost]
        public async Task<IActionResult> inputuser(geoUser geoUser)
        {

            var uname = await db.geoUsers.FirstOrDefaultAsync(s => s.username == geoUser.username);
            var udevice = await db.geoUsers.FirstOrDefaultAsync(d => d.namedevice == geoUser.namedevice);
            if ((uname != null) & (udevice != null))
            {
                return NotFound("Указанные пользователь и устройство уже существует в БД");
            }
            else if ((uname != null) & (udevice == null))
            {
                geoUser.tm = localDate.ToString("HH:mm:ss");
                geoUser.dt = localDate.ToString("dd.MM.yyyy");
                db.geoUsers.Add(geoUser);
                db.SaveChanges();
                return Ok("Данные внесены в БД");
            }
            else 
            {
                geoUser.tm = localDate.ToString("HH:mm:ss");
                geoUser.dt = localDate.ToString("dd.MM.yyyy");
                db.geoUsers.Add(geoUser);
                db.SaveChanges();
                return Ok("Данные внесены в БД");
            }

            //if (uname != null)
            //{
            //    return NotFound("Указанный пользователь уже существует в БД");
            //}
            //else
            //{
            //    geoUser.tm = localDate.ToString("HH:mm:ss");
            //    geoUser.dt = localDate.ToString("dd.MM.yyyy");
            //    db.geoUsers.Add(geoUser);
            //    db.SaveChanges();
            //    return Ok("Данные внесены в БД");
            //}

        }

    }

}
