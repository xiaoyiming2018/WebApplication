using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApplication.Controllers
{
    public class MaterialController : BaseController
    {
        MaterialManager materialManager = new MaterialManager();
        OrderManager OM = new OrderManager();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetData(string material_name) {
            List<Material> materials = materialManager.SelectAll(material_name);
            return Json(materials);
        }

        public IActionResult InsertData(Material material) {
            if (material.id > 0)
            {
                int result = materialManager.Update(material);
                if (result > 0)
                {
                    List<Order> orders0 = OM.SelectAll(0,"0000-01-01","2222-01-01", "0000-01-01", "2222-01-01").Where(s=>s.order_name==material.material_name).ToList();
                    List<Order> orders1 = OM.SelectAll(0, "0000-01-01", "2222-01-01", "0000-01-01", "2222-01-01").Where(s => s.order_name == material.material_name).ToList();
                    List<Order> res = new List<Order>();
                    res.AddRange(orders0);
                    res.AddRange(orders1);
                    for (int i = 0; i < res.Count; i++) {
                        res[i].order_price = material.price;
                        res[i].order_picture = material.picture;
                        OM.UpdateOrderSeq(res[i]);
                    }
                    

                    return Json("Success");
                }
                else
                {
                    return Json("Fail");
                }
            }
            else {
                List<Material> materials = materialManager.SelectAll("");
                List<Material> check = materials.Where(s => s.material_name == material.material_name).ToList();
                if (check.Count > 0)
                {
                    return Json("Existence");
                }
                else
                {
                    int result = materialManager.Insert(material);
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
        }

        public IActionResult Del(int[] delList) {
            int flag = 0;
            for (int i=0;i< delList.Length;i++) {
                int result = materialManager.Del(delList[i]);
                if (result>0) {
                    flag++;
                }
            }
            if (flag == delList.Length)
            {
                return Json("Success");
            }
            else {
                return Json("Fail");
            }

            
        }
    }
}
