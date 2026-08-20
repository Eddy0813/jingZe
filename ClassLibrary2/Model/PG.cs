
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JingZeServer.Model
{
    public class PG
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 订单总数
        /// </summary>
        public int OrderNum { get; set; }
        /// <summary>
        /// 完工数
        /// </summary>
        public int CompletionNum { get; set; }
        /// <summary>
        /// 产品状态
        /// </summary>
        public string OrderProNo { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProNo { get; set; }
        /// <summary>
        /// 品名
        /// </summary>
        public string ProName { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Spec { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public string Customer { get; set; }
        /// <summary>
        /// 毛坯浴缸重
        /// </summary>
        public float MPWeigth { get; set; }
        /// <summary>
        /// 皮重
        /// </summary>
        public float PGWeigth { get; set; }
        /// <summary>
        /// 毛重
        /// </summary>
        public float MGWeigth { get; set; }
        /// <summary>
        /// 净重
        /// </summary>
        public float JGWeigth { get; set; }
        /// <summary>
        /// 完工数量
        /// </summary>
        public int FinishNum {  get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        public string ProID {  get; set; }


    }
}
