using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvcPager;
using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApplication.Controllers
{
    public class OrderSeqController : BaseController
    {
        OrderManager OMM = new OrderManager();
        SaleManager SM = new SaleManager();
        SaleReturnManager SRM = new SaleReturnManager();
        MaterialManager MM = new MaterialManager();
        /// <summary>
        /// 用户列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        public IActionResult Index(string start_time, string end_time, string order_name, string unit,string activation, 
            int pageindex = 1, int pagesize = 20)
        {
            
            int order_id = Convert.ToInt32(Request.Query["order_id"]);
            Order order = OMM.SelectById(order_id);
            ViewBag.order_index = order.order_index;
            ViewBag.company_order_index = order.company_order_index;
            ViewBag.company_name = order.company_name;
            if (activation == null)
            {
                ViewBag.activation = "off";
            }
            else
            {
                ViewBag.activation = activation;
            }


            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;
            ViewBag.order_id = order_id;
            ViewBag.unit = unit;
            ViewBag.order_name = order_name;

            if (start_time == null)
            {
                start_time = "0001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }

            var objList = OMM.SelectSeqByOrderId(order_id, start_time,end_time, order_name, unit);
            return View(objList);
            
        }

        /// <summary>
        /// 插入更新页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Edit()
        {
            try
            {
                int order_id = Convert.ToInt32(Request.Query["order_id"]);
                ViewBag.order_id = order_id;
                Order order = OMM.SelectById(order_id);
                ViewBag.order_index = order.order_index;
                ViewBag.company_name = order.company_name;
                ViewBag.company_order_index = order.company_order_index;
                ViewBag.order_time = DateTime.Now.ToLocalTime().AddHours(8).ToString("yyyy-MM-dd");


                ViewBag.search_company_name = Convert.ToString(Request.Query["company_name"]);
                ViewBag.search_order_index = Convert.ToString(Request.Query["order_index"]);
                ViewBag.search_company_order_index = Convert.ToString(Request.Query["company_order_index"]);
                ViewBag.search_purchase_person = Convert.ToString(Request.Query["purchase_person"]);
                ViewBag.search_order_name = Convert.ToString(Request.Query["order_name"]);

                ViewBag.search_start_time = Convert.ToString(Request.Query["start_time"]);
                ViewBag.search_end_time = Convert.ToString(Request.Query["end_time"]);
                ViewBag.search_deliver_start_time = Convert.ToString(Request.Query["deliver_start_time"]);
                ViewBag.search_deliver_end_time = Convert.ToString(Request.Query["deliver_end_time"]);

                int seq_id = Convert.ToInt32(Request.Query["seq_id"]);
                if (seq_id > 0)
                {
                    var obj = OMM.SelectByOrderSeqId(seq_id);
                    return View(obj);
                }
                else
                {
                    return View();
                }
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
        public IActionResult EditHandle(Order order)
        {

            //验证账户是否存在
            int seq_id = 0;

            string order_index = order.order_index;
            string company_order_index = order.company_order_index;
            int customer_id = order.customer_id;
            int order_id = order.order_id;
            ViewBag.order_id = order_id;

            DateTime order_time = order.order_time;
            DateTime deliver_time = order.deliver_time;
            string order_name = order.order_name;
            string purchase_person = order.purchase_person;
            string unit = order.unit;

            double order_num = order.order_num;
            double order_price = order.order_price;
            double order_all_price = order.order_all_price;
            string order_picture = order.order_picture;
            string remark = order.remark;

            if (order.seq_id >0)
            {
                seq_id = order.seq_id;
            }

            int count = 0;
            Order objOrder = new Order();
            objOrder.order_index = order_index;
            objOrder.company_order_index = company_order_index;
            objOrder.customer_id = customer_id;

            objOrder.order_id = order_id;
            objOrder.order_time = order_time;
            objOrder.deliver_time = deliver_time;
            objOrder.order_name = order_name;
            objOrder.order_num = order_num;
            objOrder.purchase_person = purchase_person;
            objOrder.unit = unit;
            objOrder.order_price = order_price;
            objOrder.order_all_price = order_all_price;
            objOrder.order_picture = order_picture;
            objOrder.remark = remark;

            if (seq_id > 0)
            {
                objOrder.seq_id = seq_id;
                count = OMM.UpdateOrderSeq(objOrder);
                OMM.UpdateOrder(objOrder);


                Material material = MM.SelectAll().Find(s=>s.material_name==order_name);
                material.price = order_price;
                material.picture = order_picture;
                MM.Update(material);
            }
            else
            {
                count = OMM.InsertOrderSeq(objOrder);
            }
            if (count > 0)
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
                int count = 0;
                int order_id = Convert.ToInt32(Request.Query["order_id"]);
                Order order = new Order();
                ViewBag.order_id = order_id;

                int seq_id = Convert.ToInt32(Request.Query["seq_id"]);
                List<Order> orderList = OMM.SelectOrderSeqList(order_id);

                List<Sale> sales = SM.SelectByOrderId(order_id).Where(s=>s.seq_id==seq_id).ToList();
                //在删除order之前需判断一下该order是否已开送货单，若开了则提示无法删除送货单
                if (sales.Count >= 1)
                {
                    return Json("Existence");
                }
                else
                {
                    if (orderList.Count > 1)
                    {
                        count = OMM.DelOrderSeq(seq_id);
                        ViewBag.key = "OrderSeq";

                    }
                    else
                    {
                        count = OMM.DelOrderSeq(seq_id);
                        OMM.DelOrder(order_id);
                        ViewBag.key = "Order";
                    }

                    if (count > 0)
                    {
                        return Json("Success");
                    }
                    else
                    {
                        return Json("Fail");
                    }
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
        public IActionResult GetProductPrice(double order_num, double order_price)
        {
            ViewBag.order_all_price = order_num * order_price;
            return View();
        }

        public IActionResult HistoryIndex(string start_time, string end_time, string order_name, string unit, string activation)
        {
            
            int order_id = Convert.ToInt32(Request.Query["order_id"]);
            //从order_info中获取相关信息
            Order order = OMM.SelectById(order_id);
            ViewBag.order_index = order.order_index;
            ViewBag.company_order_index = order.company_order_index;
            ViewBag.company_name = order.company_name;

            //从orderseq_info中获取订单信息
            List<Order> orderseqList = OMM.SelectOrderSeqList(order_id);
            ViewBag.orderseqList = orderseqList;

            double order_num = 0;
            double order_all_price = 0;
            for (int i=0;i< orderseqList.Count;i++)
            {
                order_num += orderseqList[i].order_num;
                order_all_price += orderseqList[i].order_all_price;
            }
            ViewBag.order_num = order_num;
            ViewBag.order_all_price = order_all_price;

            
            ViewBag.start_time = start_time;
            ViewBag.end_time = end_time;
            ViewBag.order_id = order_id;
            ViewBag.unit = unit;
            ViewBag.order_name = order_name;

            if (start_time == null)
            {
                start_time = "0001-01-01";
            }
            if (end_time == null)
            {
                end_time = "2222-01-01";
            }

            var objList = OMM.SelectSeqByOrderId(order_id, start_time, end_time,  order_name, unit);
            return View(objList);
        }
        
    }
}