using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Main_EX;
using BasicClass;

namespace Main
{
    public partial class BaseDealComprehensiveResult_Main
    {
        /// <summary>
        /// 相机综合设置Main_EX
        /// </summary>
        /// <param name="noCamera"></param>
        /// <returns></returns>
        public override BaseDealComprehensiveResult GetDealComprehensiveResult(int noCamera)
        {
            try
            {
                BaseDealComprehensiveResult baseDealComprehensive = null;
                switch (noCamera)
                {
                    case 1:
                        baseDealComprehensive = DealComprehensiveResult1.D_I;
                        break;

                    case 2:
                        baseDealComprehensive = DealComprehensiveResult2.D_I;
                        break;

                    case 3:
                        baseDealComprehensive = DealComprehensiveResult3.D_I;
                        break;

                    case 4:
                        baseDealComprehensive = DealComprehensiveResult4.D_I;
                        break;

                    case 5:
                        baseDealComprehensive = DealComprehensiveResult5.D_I;
                        break;

                    case 6:
                        baseDealComprehensive = DealComprehensiveResult6.D_I;
                        break;

                    case 7:
                        baseDealComprehensive = DealComprehensiveResult7.D_I;
                        break;

                    case 8:
                        baseDealComprehensive = DealComprehensiveResult8.D_I;
                        break;
                }
                return baseDealComprehensive;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return null;
            }
        }
    }
}
