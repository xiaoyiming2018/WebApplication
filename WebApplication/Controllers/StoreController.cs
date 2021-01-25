using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApplication.Controllers
{
    public class StoreController : Controller
    {
        StoreManager storeManager = new StoreManager();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetData(string store_name)
        {

            if (store_name == null)
            {
                store_name = "";
            }

            List<Store> stores = storeManager.SelectAll().Where(s => s.store_name.Contains(store_name)).ToList();
            
            return Json(stores);

        }


        public IActionResult InsertData(Store store)
        {
            if (store.id > 0)
            {
                int result = storeManager.Update(store);
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
                store.create_time = DateTime.Now;
                int result = storeManager.Insert(store);
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

        public IActionResult Del(int[] delList)
        {
            int flag = 0;
            for (int i = 0; i < delList.Length; i++)
            {
                int result = storeManager.Del(delList[i]);
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
