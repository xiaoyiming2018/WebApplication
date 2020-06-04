using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using BLL;
using Microsoft.AspNetCore.Http;

namespace WebApplication.Controllers
{
    public class SaleSeqController : BaseController
    {
        SaleManager SM = new SaleManager();
        OrderManager OM = new OrderManager();
        /// <summary>
        /// 插入更新页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Edit()
        {
            try
            {
                string deliver_index = Convert.ToString(Request.Query["deliver_index"]);
                int seq_id = Convert.ToInt32(Request.Query["seq_id"]);    
                Order order = OM.SelectByOrderSeqId(seq_id);

                ViewBag.deliver_index = deliver_index;
                ViewBag.seq_id = seq_id;
                ViewBag.order_index = order.order_index;//订单号，名称谷歌
                ViewBag.order_name = order.order_name;
                ViewBag.unit = order.unit;
                ViewBag.remain_num = order.remain_num;
                ViewBag.order_price = order.order_price;
                ViewBag.order_all_price = order.order_all_price;    

                Sale sale = SM.SelectSingleBySeqIndex(seq_id, deliver_index);
     
                return View(sale);

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
        public IActionResult EditHandle(Sale sale)
        {
            Order orderOld = OM.SelectByOrderSeqId(sale.seq_id);//获取原来的出货数量
            Sale saleOld = SM.SelectSingleBySeqIndex(sale.seq_id,sale.deliver_index);

            Order orderNew = new Order();
            orderNew.seq_id = sale.seq_id;
            orderNew.remain_num = orderOld.remain_num+ saleOld.real_num - sale.real_num;
            orderNew.open_num = orderOld.open_num- saleOld.real_num+sale.real_num;
            orderNew.tui_num = orderOld.tui_num;


            Sale saleNew = new Sale();
            saleNew.deliver_index = sale.deliver_index;
            saleNew.seq_id= sale.seq_id;
            saleNew.deliver_company_head = saleOld.deliver_company_head;
            saleNew.insert_time = DateTime.Now.ToLocalTime().AddHours(8);

            saleNew.real_num = sale.real_num;
            saleNew.deliver_price = sale.deliver_price;
            saleNew.deliver_all_price = sale.deliver_all_price;
            saleNew.remark = sale.remark;


            int countOrder = OM.UpdateSeqNum(orderNew);
            int countSale = SM.UpdateDeliverDetails(saleNew);


            if (countOrder + countSale > 1)
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
                string deliver_index = Convert.ToString(Request.Query["deliver_index"]);
                int seq_id = Convert.ToInt32(Request.Query["seq_id"]);
                Order orderOld = OM.SelectByOrderSeqId(seq_id);//获取原来的出货数量
                Sale saleOld = SM.SelectSingleBySeqIndex(seq_id, deliver_index);
                Order orderNew = new Order();
                orderNew.seq_id = seq_id;
                orderNew.remain_num = orderOld.remain_num + saleOld.real_num;
                orderNew.open_num = orderOld.open_num - saleOld.real_num;
                orderNew.tui_num = orderOld.tui_num;

                OM.UpdateSeqNum(orderNew);


                ViewBag.deliver_index = deliver_index;
                int count = SM.Del(seq_id,deliver_index);
                if (count > 0)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Fail");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取材料规格
        /// </summary>
        /// <returns></returns>
        public IActionResult GetProductPrice(double real_num, double deliver_price)
        {
            ViewBag.deliver_all_price = real_num * deliver_price;
            return View();
        }
    }
}