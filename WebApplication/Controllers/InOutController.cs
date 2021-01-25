using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApplication.Controllers
{
    public class InOutController : Controller
    {
        InOutManager inOutManager = new InOutManager();
        MaterialManager materialManager = new MaterialManager();

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult GetData(string material_name,int flag,string start_time,string end_time,string day,string month,string year,string store_name) {

            if (material_name==null) {
                material_name = "";
            }
            if (store_name == null)
            {
                store_name = "";
            }

            if (start_time == null)
            {
                start_time = "0001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }
            DateTime dt = DateTime.Now;
            DateTime dt2 = dt.AddMonths(1);
            if (day == "1")
            {
                start_time = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                end_time = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            }
            if (month == "1")
            {
                start_time = dt.AddDays(-(dt.Day) + 1).ToString("yyyy-MM-dd");
                end_time = dt2.AddDays(-dt.Day).ToString("yyyy-MM-dd");
            }
            if (year == "1")
            {
                start_time = dt.AddMonths(-dt.Month + 1).AddDays(-dt.Day + 1).ToString("yyyy-MM-dd");
                end_time = new DateTime(DateTime.Now.Year, 12, 31).ToString("yyyy-MM-dd");
            }

            List<InOut> inOuts = new List<InOut>();

            if (flag == 0)
            {
                inOuts = inOutManager.SelectAll().Where(s => Convert.ToDateTime(s.create_time.ToString("yyyy-MM-dd")) >= Convert.ToDateTime(start_time) &&
                Convert.ToDateTime(s.create_time.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(end_time) &&
                s.material_name.Contains(material_name) && s.store_name.Contains(store_name)
                ).ToList();
            }
            else {
                inOuts = inOutManager.SelectAll().Where(s => Convert.ToDateTime(s.create_time.ToString("yyyy-MM-dd")) >= Convert.ToDateTime(start_time) &&
                Convert.ToDateTime(s.create_time.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(end_time) &&
                s.material_name.Contains(material_name) && s.store_name.Contains(store_name) && s.flag == flag
                ).ToList();
            }

            return Json(inOuts);
        
        }


        public IActionResult InsertData(InOut inOut)
        {
            if (inOut.id > 0)
            {
                int result = inOutManager.Update(inOut);
                if (result > 0)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Fail");
                }
            }
            else
            {
                inOut.create_time = DateTime.Now;
                int result = inOutManager.Insert(inOut);
                if (result > 0)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Fail");
                }
                
            }
        }

        public IActionResult SelectChange(int material_id) {
            Material material = materialManager.SelectAll("").Find(s=>s.id==material_id);
            return Json(material);
        }
        /// <summary>
        /// 获取品名所剩的库存，用于卡关出库量大于库存量
        /// </summary>
        /// <param name="material_id"></param>
        /// <returns></returns>
        public IActionResult GetRemainNum(int material_id)
        {
            List<InOut> inOuts = inOutManager.SelectAll().Where(s => s.material_id == material_id).ToList();
            double res = 0;
            for (int i=0;i< inOuts.Count;i++) {
                res += inOuts[i].inout_num * inOuts[i].flag;
            }
            return Json(res);
        }

        public IActionResult Del(int[] delList)
        {
            int flag = 0;
            for (int i = 0; i < delList.Length; i++)
            {
                int result = inOutManager.Del(delList[i]);
                if (result > 0)
                {
                    flag++;
                }
            }
            if (flag == delList.Length)
            {
                return Json("Success");
            }
            else
            {
                return Json("Fail");
            }


        }

    }
}
