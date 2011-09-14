using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wenster.Entity;
using System.Collections.ObjectModel;

namespace Wenster.DataProvider
{
    public class Provider
    {
        public static List<Production> initializeDataList = new List<Production>();
        public static List<Production> resultDataList = new List<Production>();

        public List<Production> GetInitializeData(int count)
        {
            initializeDataList.Add(new Production(1,393664));
            initializeDataList.Add(new Production(2,397920));
            initializeDataList.Add(new Production(3,392824));
            initializeDataList.Add(new Production(4,405544));
            initializeDataList.Add(new Production(5,390904));
            initializeDataList.Add(new Production(6,72272));
            initializeDataList.Add(new Production(7,326232));
            initializeDataList.Add(new Production(8,406296));
            initializeDataList.Add(new Production(9,397608));
            initializeDataList.Add(new Production(10,394720));
            initializeDataList.Add(new Production(11,403920));
            initializeDataList.Add(new Production(12,389920));
            initializeDataList.Add(new Production(13,70264));
            initializeDataList.Add(new Production(14,353976));
            initializeDataList.Add(new Production(15,426672));
            initializeDataList.Add(new Production(16,399768));
            initializeDataList.Add(new Production(17,394072));
            initializeDataList.Add(new Production(18,397368));
            initializeDataList.Add(new Production(19,378952));
            initializeDataList.Add(new Production(20,67600));
            initializeDataList.Add(new Production(21,303696));
            initializeDataList.Add(new Production(22,404760));
            initializeDataList.Add(new Production(23,400120));
            initializeDataList.Add(new Production(24,404064));
            initializeDataList.Add(new Production(25,407560));
            initializeDataList.Add(new Production(26,395032));
            initializeDataList.Add(new Production(27,70824));
            initializeDataList.Add(new Production(28,335856));
            return initializeDataList;
        }

        public List<Production> GetRealData(int count)
        {
            resultDataList.Add(new Production(1,388440));
            resultDataList.Add(new Production(2,391768));
            resultDataList.Add(new Production(3,388720));
            resultDataList.Add(new Production(4,355016));
            resultDataList.Add(new Production(5,310961.6));
            resultDataList.Add(new Production(6,68000));
            resultDataList.Add(new Production(7,327888));
            return resultDataList;
        }
    }
}
