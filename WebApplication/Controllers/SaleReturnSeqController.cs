using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApplication.Controllers
{
    public class SaleReturnSeqController : BaseController
    {
        SaleReturnManager SM = new SaleReturnManager();
        SaleManager SaleM = new SaleManager();
        OrderManager OM = new OrderManager();

        /// <summary>
        /// 插入更新页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Edit()
        {
            try
            {
                string return_index = Convert.ToString(Request.Query["return_index"]);
                int seq_id = Convert.ToInt32(Request.Query["seq_id"]);
                ViewBag.return_index = return_index;
                ViewBag.seq_id = seq_id;

                SaleReturn res = SM.SelectSingleBySeqReturnIndex(seq_id,return_index);

                return View(res);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 编辑处理页面
        /// </summary>
        /// <returns></returns>
        public IActionResult EditHandle(SaleReturn saleReturn)
        {
            SaleReturn saleReturnNew = new SaleReturn();
            saleReturnNew.return_index = saleReturn.return_index;
            saleReturnNew.seq_id = saleReturn.seq_id;
            saleReturnNew.return_num = saleReturn.return_num;
            saleReturnNew.return_price = saleReturn.return_price;
            saleReturnNew.return_all_price = saleReturn.return_all_price;
            saleReturnNew.remark = saleReturn.remark;

            int count = SM.Update(saleReturnNew);
            if (count>0)
            {
                return Json("Success");
            }
            else
            {
                return Json("Fail");
            }

        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        public IActionResult Del()
        {
            try
            {
                
                string return_index = Convert.ToString(Request.Query["return_index"]);
                int seq_id = Convert.ToInt32(Request.Query["seq_id"]);

                int count = SM.Del(seq_id, return_index);

                if (count > 0)
                {
                    ViewBag.Message = "删除成功！";
                    return Json("Success");
                }
                else
                {
                    ViewBag.Message = "删除失败！";
                    return Json("Fail");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}