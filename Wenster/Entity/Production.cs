using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wenster.Entity
{
    public class Production   //定义这个对象获取数据
    {
        public int ID { get; set; }
        public double  Number { get; set; }
        public Production(int id, double number)
        {
            this.ID = id;
            this.Number = number;
        }
    }
}
