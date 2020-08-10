using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApplication.Controllers
{
    public class InventoryController : Controller
    {
        InOutManager inOutManager = new InOutManager();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetInventoryInfo(string material_name) {

            List<InOut> inOuts = new List<InOut>();

            if (string.IsNullOrEmpty(material_name))
            {
                material_name = "";
                inOuts = inOutManager.SelectInventoryInfo();
            }
            else {
                string[] materials = material_name.Split(",");
                for (int i=0;i<materials.Length;i++) {
                    if (materials[i]=="") {
                        continue;
                    }
                    InOut inOut = inOutManager.SelectInventoryInfo().Find(s=>s.material_name==materials[i]);
                    inOuts.Add(inOut);
                }
            }
            return Json(inOuts);
        }

        public IActionResult GetnventoryTrendDay(string material_name)
        {

            if (string.IsNullOrEmpty(material_name))
            {
                material_name = "";
            }

            DateTime dt = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
            //获得本月月初时间
            DateTime startMonth = dt.AddMonths(-1);

            List<InOut> inOuts = inOutManager.SelectInventoryTrendDay()
                .Where(s => s.material_name.Contains(material_name) && s.create_time>= startMonth)
                .ToList();
            return Json(inOuts);
        }

        public IActionResult GetnventoryTrendMonth(string material_name)
        {

            if (string.IsNullOrEmpty(material_name))
            {
                material_name = "";
            }

            DateTime dt = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
            //获得本年第一个月的第一天
            DateTime startYear = dt.AddMonths(-12);

            List<InOut> inOuts = inOutManager.SelectInventoryTrendMonth()
                .Where(s => s.material_name.Contains(material_name) && s.create_time >= startYear)
                .ToList();
            return Json(inOuts);
        }

        public IActionResult GetnventoryTrendYear(string material_name)
        {

            if (string.IsNullOrEmpty(material_name))
            {
                material_name = "";
            }

            List<InOut> inOuts = inOutManager.SelectInventoryTrendYear()
                .Where(s => s.material_name.Contains(material_name))
                .ToList();
            return Json(inOuts);
        }
    }
}
