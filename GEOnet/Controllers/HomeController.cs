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
        geoContext db;
        public HomeController(geoContext context)
        {
            db = context;
        }

        //вывод всего содержимого БД
        public IActionResult Index()
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


        [HttpPost]
        public string inputgeo(geoModel geoModel)
        {
            db.geoModels.Add(geoModel);
            db.SaveChanges();
            return geoModel.nameID + " координыта занесены в БД";
        }



    }

}
