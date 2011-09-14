using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wenster.Entity
{
    public class Result
    {
        public double Alpha { get; set; }
        public double Beta { get; set; }
        public double Gamma { get; set; }

        public List<Variable> Prediction { get; set; }

        public double Average { get; set; }
    }
}
