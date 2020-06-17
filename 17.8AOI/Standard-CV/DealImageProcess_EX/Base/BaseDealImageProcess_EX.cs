using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealImageProcess;
using BasicClass;
using System.Reflection;

namespace DealImageProcess_EX
{
    [Serializable]
    public class BaseDealImageProcess_EX : BaseParImageProcess
    {
        /// <summary>
        /// 获取相关算子的相关名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public BasePropForRef GetPropForRef(string type)
        {
            try
            {
                BasePropForRef basePropForRef = new BasePropForRef();
                switch (type)
                {
                    case "边缘凸起":
                        basePropForRef.NameSpace = "DealImageProcess_EX";
                        basePropForRef.NameClass = "ParRaisedEdgeSmooth";
                        basePropForRef.NameClassFun = "FunRaisedEdge";

                        basePropForRef.NameFunForDeal = "DealRaisedEdge";
                        basePropForRef.NameClassFun = "FunRaisedEdge";

                        basePropForRef.NameWin = "WinRaisedEdge";
                        basePropForRef.NameResult = "FunRaisedEdge";
                        break;

                    case "表面缺陷检测(亮暗点团)":
                        basePropForRef.NameSpace = "DealImageProcess_EX";
                        basePropForRef.NameClass = "ParLinesDotsPosNegInspect";
                        basePropForRef.NameClassFun = "FunLinesDotsPosNegInspect";

                        basePropForRef.NameFunForDeal = "DealLinesDotsPosNegInspect";
                        //basePropForRef.NameClassFun = "FunRaisedEdge";

                        basePropForRef.NameWin = "WinLinesDotsPosNegInspect";
                        basePropForRef.NameResult = "FunLinesDotsPosNegInspect";
                        break;
                }
                return basePropForRef;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取窗体静态创建的方法
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public MethodInfo GetMethod(string type)
        {
            try
            {
                Assembly ass = this.GetType().Assembly;
                BasePropForRef basePropForRef = GetPropForRef(type);
                Type typeClass = ass.GetType(basePropForRef.NameSpace + "." + basePropForRef.NameWin);
                MethodInfo method = typeClass.GetMethod("GetWinInst");

                return method;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
