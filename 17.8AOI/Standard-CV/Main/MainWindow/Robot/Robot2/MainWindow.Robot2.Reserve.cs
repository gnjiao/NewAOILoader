using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DealRobot;
using DealPLC;
using Common;
using SetPar;
using DealFile;
using BasicClass;
using DealConfigFile;

namespace Main
{
    partial class MainWindow
    {
        #region 实现方法选择
        void R_Inst_Others2_event(int index)
        {
            switch (index)
            {
                case 1:
                    Robot2Others1();
                    break;

                case 2:
                    Robot2Others2();
                    break;

                case 3:
                    Robot2Others3();
                    break;

                case 4:
                    Robot2Others4();
                    break;

                case 5:
                    Robot2Others5();
                    break;

                case 6:
                    Robot2Others6();
                    break;
            }

            //再次判断选择
            if (index>4)
            {
                    
            }
        }
        #endregion 实现方法选择

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Robot2Others1()
        {
            try
            {
                
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Robot2Others2()
        {
            try
            {
                
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Robot2Others3()
        {
            try
            {
                
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Robot2Others4()
        {
            try
            {
               
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Robot2Others5()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Robot2Others6()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        
    }
}
