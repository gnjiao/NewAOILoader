using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealResult;
using System.Collections;
using HalconDotNet;
using BasicClass;
using DealGeometry;
using DealResult_EX;
using DealImageProcess;
using DealImageProcess_EX;

namespace DealImageProcess_FunEX
{
    //点线缺陷在透明盖板的正反检测
    public partial class FunLinesDotsPosNegInspect:FunPreprocess
    {
        #region 初始化
        public FunLinesDotsPosNegInspect()
        {
            NameClass = "FunLinesDotsPosNegInspect";
        }
        #endregion 初始化

        public ResultLinesDotsPosNegInspect DealLinesDotsPosNegInspect(ParLinesDotsPosNegInspect par, Hashtable htResult, Hashtable htResult_Mult)
        {

            #region 定义
            ResultLinesDotsPosNegInspect result = new ResultLinesDotsPosNegInspect();
            HTuple width = 0;
            HTuple height = 0;
            HObject ho_Image = null;
            HObject ho_ReduceImage = null;
            HObject ho_ResultPreprocess = null;
            HObject ho_ROI = null;
            HObject ho_ROIPreprocess = null;

            HObject ho_ImageDesReduce = null;
            HObject ho_MultiChannelImage = null;
            HObject ho_MultiChannelEdges = null;//多通道边缘显示图像

            HOperatorSet.GenEmptyObj(out ho_ResultPreprocess);
            HOperatorSet.GenEmptyObj(out ho_ReduceImage);
            HOperatorSet.GenEmptyObj(out ho_ImageDesReduce);
            HOperatorSet.GenEmptyObj(out ho_MultiChannelImage);
            HOperatorSet.GenEmptyObj(out ho_MultiChannelEdges);

            #endregion 定义

            try
            {
                #region 基础功能调用
                if (BasicImageProcess_MultROI(par, result, htResult, out ho_ROI, out ho_ReduceImage, out ho_Image, out width, out height))
                {

                }
                else
                {
                    return result;
                }
                #endregion 基础功能调用
                HObject ho_AffineROI = result.g_ResultPreProcess.resultAffineImage.ImageROIAffine.Ho_Image;
                //TODO 获取ROI 预处理之后的ROITrans
                ho_ROIPreprocess = ho_ROI;
                //多相机引用
                int noCamera = par.NoCameraMult;
                Hashtable htResult_Camera = htResult_Mult["HtResult_Cam" + noCamera.ToString()] as Hashtable;
                ResultPhoto photo = htResult_Camera[par.CellRefImage_Mult.NameCell] as ResultPhoto;
                ImageAll imMult = photo.HtResultImage[par.CellRefImage_Mult.NameCell + "FunPhoto.CameraImage"] as ImageAll;

                //获取图像预处理结果--经过投影变换出来的结果
                ho_ResultPreprocess = result.g_ResultPreProcess.ImageResult.Ho_Image;

                
                //目标相机拍摄的图像也进行区域reduce
                ho_ImageDesReduce.Dispose();

                HOperatorSet.ReduceDomain(imMult.Ho_Image, ho_ROIPreprocess, out ho_ImageDesReduce);
                //求融合图像
                ho_MultiChannelImage.Dispose();
                bool blInspectResult = LinesDotsPosNegInspect(ho_ImageDesReduce, ho_ResultPreprocess, par, result, out ho_MultiChannelImage);
                ho_MultiChannelEdges.Dispose();
                bool blCalibConfirm = CalibConfirm(ho_ROIPreprocess, ho_ImageDesReduce, ho_ResultPreprocess, par, result, width, height, out ho_MultiChannelEdges);

                //求融合图像
                //ho_MultiChannelImage.Dispose();
                //bool blInspectResult = LinesDotsPosNegInspect(imMult.Ho_Image, ho_ResultPreprocess, par, result, out ho_MultiChannelImage);
                //ho_MultiChannelEdges.Dispose();
                //bool blCalibConfirm = CalibConfirm(ho_ROIPreprocess, imMult.Ho_Image, ho_ResultPreprocess, par, result, width, height, out ho_MultiChannelEdges);

                if (blInspectResult)
                {
                    result.LevelError_e = LevelError_enum.Error;
                    result.TypeErrorProcess_e = TypeErrorProcess_enum.Others;
                    result.Annotation = par.NameCell.ToString() + ",点线区域求取错误！";
                    return result;
                }
                //TODO 融合之后的图像赋值

                return result;
            }
            catch (Exception ex)
            {
                result.LevelError_e = LevelError_enum.Error;
                result.TypeErrorProcess_e = TypeErrorProcess_enum.Catch;
                result.Annotation = par.NameCell.ToString() + ",点线区域求取跳转到异常";
                result.SetDefault();
                LogError("FunLinesDotsPosNegInspect", par, ex);
                return result;
            }
            finally
            {
                //对结果进行综合处理
                SetComprehensiveResult(result, par, htResult, true);
                //校准
                DealCalibResult(par, result, htResult);
                #region 记录时间
                WriteRunTime(stopWatch, NameClass, par, result);
                #endregion 记录时间

                #region 记录
                RecordImage(par, result, NameClass, "ReduceImage", ho_ReduceImage);//保存图像到哈希表
                RecordImage(par, result, NameClass, "ImageDesReduce", ho_ImageDesReduce);//保存图像到哈希表
                RecordImage_Record(par, result, NameClass, "MultiChannelImage", ho_MultiChannelImage);
                RecordImage_Record(par, result, NameClass, "MultiChannelEdges", ho_MultiChannelEdges);
                #endregion 记录
            }
        }

        /// <summary>
        /// 传入正视和侧视的两幅图片，提取出其中一面的缺陷，返回缺陷区域和融合图片
        /// </summary>
        /// <param name="ho_ImageZ">经过ROI reduce以后的正视图片</param>
        /// <param name="ho_ImageC">经过ROI reduce以后的侧视图片</param>
        /// <param name="ho_LNgRegions">点缺陷区域</param>
        /// <param name="ho_PNgRegions">线缺陷区域</param>
        /// <param name="ho_MultiChannelImage">正侧视融合图像</param>
        /// <returns></returns>
        bool LinesDotsPosNegInspect(HObject ho_ImageZ, HObject ho_ImageC,ParLinesDotsPosNegInspect par, ResultLinesDotsPosNegInspect result, out HObject ho_MultiChannelImage)
        {
            //是否有线状异物
            bool blLNg = false;
            //是否有点状异物
            bool blPNg = false;

            HObject ho_PNgRegions = null, ho_LNgRegions = null;
            HObject ho_CameraZWorkR = null, ho_CameraZWorkG = null, ho_CameraZWorkB = null;
            HObject ho_CameraCTWorkR = null, ho_CameraCTWorkG = null, ho_CameraCTWorkB = null;
            HObject ho_OutputRegionsZSizeSelP, ho_OutputRegionsCSizeSelP, ho_OutputRegionsZSizeSelL, ho_OutputRegionsCSizeSelL;
            HObject ho_ZLSelectedRegions, ho_CTLSelectedRegions, ho_ZPSelectedRegions, ho_CTPSelectedRegions;
            HObject ho_RectangleZLSearch = null, ho_RectangleZPSearch = null;
            HObject ho_DestRegions = null;
            HObject ho_CTPObjectSelected = null, ho_CTLObjectSelected=null;

            HTuple hv_CameraZPArea = new HTuple();
            HTuple hv_CameraZPRow = new HTuple(), hv_CameraZPColumn = new HTuple();
            HTuple hv_CameraCTPArea = new HTuple(), hv_CameraCTPRow = new HTuple();
            HTuple hv_CameraCTPColumn = new HTuple(), hv_CameraZLArea = new HTuple();
            HTuple hv_CameraZLRow = new HTuple(), hv_CameraZLColumn = new HTuple();
            HTuple hv_CameraCTLArea = new HTuple(), hv_CameraCTLRow = new HTuple();
            HTuple hv_CameraCTLColumn = new HTuple();
            HTuple hv_NumberCTP = new HTuple(), hv_NumberZP = new HTuple();
            HTuple hv_NumberCTL = new HTuple(), hv_NumberZL = new HTuple();
            HTuple hv_NumberNG = new HTuple();
            HTuple hv_NumberLNgRegions = new HTuple(), hv_NumberPNgRegions = new HTuple();
            HTuple hv_PNgArea = new HTuple(), hv_PNgRow = new HTuple(), hv_PNgColumn = new HTuple();
            HTuple hv_PNgRow2 = new HTuple(), hv_PNgColumn2 = new HTuple();
            HTuple hv_LNgArea = new HTuple(), hv_LNgRow = new HTuple(), hv_LNgColumn = new HTuple();
            HTuple hv_LNgRow2 = new HTuple(), hv_LNgColumn2 = new HTuple();

            HOperatorSet.GenEmptyObj(out ho_CameraZWorkR);
            HOperatorSet.GenEmptyObj(out ho_CameraZWorkG);
            HOperatorSet.GenEmptyObj(out ho_CameraZWorkB);
            HOperatorSet.GenEmptyObj(out ho_CameraCTWorkR);
            HOperatorSet.GenEmptyObj(out ho_CameraCTWorkG);
            HOperatorSet.GenEmptyObj(out ho_CameraCTWorkB);
            HOperatorSet.GenEmptyObj(out ho_MultiChannelImage);
            HOperatorSet.GenEmptyObj(out ho_OutputRegionsZSizeSelP);
            HOperatorSet.GenEmptyObj(out ho_OutputRegionsCSizeSelP);
            HOperatorSet.GenEmptyObj(out ho_OutputRegionsZSizeSelL);
            HOperatorSet.GenEmptyObj(out ho_OutputRegionsCSizeSelL);
            HOperatorSet.GenEmptyObj(out ho_ZLSelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_CTLSelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_ZPSelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_CTPSelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_PNgRegions);
            HOperatorSet.GenEmptyObj(out ho_LNgRegions);

            HOperatorSet.GenEmptyObj(out ho_RectangleZPSearch);
            HOperatorSet.GenEmptyObj(out ho_RectangleZLSearch);
            HOperatorSet.GenEmptyObj(out ho_DestRegions);

            HOperatorSet.GenEmptyObj(out ho_CTPObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_CTLObjectSelected);


            try
            {
                //彩色图像分解
                ho_CameraZWorkR.Dispose(); ho_CameraZWorkG.Dispose(); ho_CameraZWorkB.Dispose();
                HOperatorSet.Decompose3(ho_ImageZ, out ho_CameraZWorkR, out ho_CameraZWorkG,
                    out ho_CameraZWorkB);
                ho_CameraCTWorkR.Dispose(); ho_CameraCTWorkG.Dispose(); ho_CameraCTWorkB.Dispose();
                HOperatorSet.Decompose3(ho_ImageC, out ho_CameraCTWorkR, out ho_CameraCTWorkG,
                    out ho_CameraCTWorkB);

                //查看图片时，红色缺陷在左侧的为OK片，红色缺陷在右侧（或者重合，偏差很小的时候）的为NG片
                ho_MultiChannelImage.Dispose();
                HOperatorSet.Compose3(ho_CameraCTWorkR, ho_CameraZWorkR, ho_CameraZWorkR, out ho_MultiChannelImage);

                //正视相机点缺陷提取
                ho_OutputRegionsZSizeSelP.Dispose();
                MyTh(ho_CameraZWorkR, out ho_OutputRegionsZSizeSelP, par.DotsTh, par.ClosingRadius, par.OpeningRadius, par.SizeTh);
                //侧视相机点缺陷提取
                ho_OutputRegionsCSizeSelP.Dispose();
                MyTh(ho_CameraCTWorkR, out ho_OutputRegionsCSizeSelP, par.DotsTh, par.ClosingRadius, par.OpeningRadius, par.SizeTh);
                //正视相机线缺陷提取
                ho_OutputRegionsZSizeSelL.Dispose();
                MyTh(ho_CameraZWorkR, out ho_OutputRegionsZSizeSelL, par.LinesTh, par.ClosingRadius, par.OpeningRadius, par.SizeTh);
                //侧视相机线缺陷提取
                ho_OutputRegionsCSizeSelL.Dispose();
                MyTh(ho_CameraCTWorkR, out ho_OutputRegionsCSizeSelL, par.LinesTh, par.ClosingRadius, par.OpeningRadius, par.SizeTh);

                //分离出线状区域
                ho_ZLSelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_OutputRegionsZSizeSelL, out ho_ZLSelectedRegions,
                    "compactness", "and", par.DotsLinesSeperateTh+0.00001, 9999);
                ho_CTLSelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_OutputRegionsCSizeSelL, out ho_CTLSelectedRegions,
                    "compactness", "and", par.DotsLinesSeperateTh + 0.00001, 9999);
                //分离出点状区域
                ho_ZPSelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_OutputRegionsZSizeSelP, out ho_ZPSelectedRegions,
                    "compactness", "and", 1, par.DotsLinesSeperateTh);
                ho_CTPSelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_OutputRegionsCSizeSelP, out ho_CTPSelectedRegions,
                    "compactness", "and", 1, par.DotsLinesSeperateTh);

                HOperatorSet.AreaCenter(ho_ZPSelectedRegions, out hv_CameraZPArea, out hv_CameraZPRow,
                    out hv_CameraZPColumn);
                HOperatorSet.AreaCenter(ho_CTPSelectedRegions, out hv_CameraCTPArea, out hv_CameraCTPRow,
                    out hv_CameraCTPColumn);

                HOperatorSet.AreaCenter(ho_ZLSelectedRegions, out hv_CameraZLArea, out hv_CameraZLRow,
                    out hv_CameraZLColumn);
                HOperatorSet.AreaCenter(ho_CTLSelectedRegions, out hv_CameraCTLArea, out hv_CameraCTLRow,
                    out hv_CameraCTLColumn);

                //判断两幅图中是否存在相关联的区域
                //实际搜索的时候，可能需要增加三个选项
                //1-从左向右搜索还是从右向左搜索
                //2-是搜索OK区域还是NG区域
                //3-搜索到的对应区域点位是否需要删除
                //生成搜索区域
                ho_PNgRegions.Dispose();
                HOperatorSet.GenEmptyObj(out ho_PNgRegions);
                HOperatorSet.TupleLength(hv_CameraCTPRow, out hv_NumberCTP);
                HOperatorSet.TupleLength(hv_CameraZPRow, out hv_NumberZP);
                HOperatorSet.TupleLength(hv_CameraCTLRow, out hv_NumberCTL);
                HOperatorSet.TupleLength(hv_CameraZLRow, out hv_NumberZL);

                //确定点状缺陷
                if (hv_NumberCTP.I != 0 && hv_NumberZP != 0)
                {
                    ho_RectangleZPSearch.Dispose();
                    HOperatorSet.GenRectangle1(out ho_RectangleZPSearch, hv_CameraZPRow - par.UpExtend,hv_CameraZPColumn - par.LeftExtend, 
                        hv_CameraZPRow + par.DownExtend, hv_CameraZPColumn + par.RightExtend);
                    for (int i=0;i<hv_NumberCTP.I;i++)
                    {
                        //这里还需要处理含有某个点可能落在多个区域中的情况
                        ho_DestRegions.Dispose();
                        //Choose all regions containing a given pixel.
                        HOperatorSet.SelectRegionPoint(ho_RectangleZPSearch, out ho_DestRegions,
                            hv_CameraCTPRow.TupleSelect(i), hv_CameraCTPColumn.TupleSelect(i));
                        HOperatorSet.CountObj(ho_DestRegions, out hv_NumberNG);
                        if (hv_NumberNG.I != 0)
                        {
                            //concat_obj (DestRegions, NgRegions, NgRegions)
                            ho_CTPObjectSelected.Dispose();
                            HOperatorSet.SelectObj(ho_CTPSelectedRegions, out ho_CTPObjectSelected,i + 1);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_CTPObjectSelected, ho_PNgRegions, out ExpTmpOutVar_0);
                                ho_PNgRegions.Dispose();
                                ho_PNgRegions = ExpTmpOutVar_0;
                            }
                        }
                    }
                    blPNg = true;//找到点状异物
                    GetNgRegionPar(ho_PNgRegions, out hv_PNgArea, out hv_PNgRow, out hv_PNgColumn, out hv_PNgRow2, out hv_PNgColumn2);
                    for (int i = 0; i < hv_PNgRow.Length; i++)
                    {
                        double x = (hv_PNgColumn.D + hv_PNgColumn2.D) / 2;
                        double y = (hv_PNgRow.D + hv_PNgRow2.D) / 2;
                        double height = hv_PNgRow2.D - hv_PNgRow.D;
                        double width = hv_PNgColumn2.D - hv_PNgColumn.D;
                        result.X_L.Add(x);//添加进所有异物列表
                        result.Y_L.Add(y);
                        result.R_L.Add(0);
                        result.Height_L.Add(height);
                        result.Width_L.Add(width);

                        result.XPNg_L.Add(x);//添加进点状异物列表
                        result.YPNg_L.Add(y);
                        result.blPNg = blPNg;

                    }
                }

                //确定线状缺陷
                if (hv_NumberCTL.I != 0 && hv_NumberZL != 0)
                {
                    ho_RectangleZLSearch.Dispose();
                    HOperatorSet.GenRectangle1(out ho_RectangleZLSearch, hv_CameraZLRow - par.UpExtend, hv_CameraZLColumn - par.LeftExtend, 
                        hv_CameraZLRow + par.DownExtend, hv_CameraZLColumn + par.RightExtend);
                    for (int i = 0; i < hv_NumberCTL.I; i++)
                    {
                        //这里还需要处理含有某个点可能落在多个区域中的情况
                        ho_DestRegions.Dispose();
                        //Choose all regions containing a given pixel.
                        HOperatorSet.SelectRegionPoint(ho_RectangleZLSearch, out ho_DestRegions,
                            hv_CameraCTLRow.TupleSelect(i), hv_CameraCTLColumn.TupleSelect(i));
                        HOperatorSet.CountObj(ho_DestRegions, out hv_NumberNG);
                        if (hv_NumberNG.I != 0)
                        {
                            ho_CTLObjectSelected.Dispose();
                            HOperatorSet.SelectObj(ho_CTLObjectSelected, out ho_CTLObjectSelected, i + 1);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_CTLObjectSelected, ho_LNgRegions, out ExpTmpOutVar_0);
                                ho_LNgRegions.Dispose();
                                ho_LNgRegions = ExpTmpOutVar_0;
                            }
                        }
                    }
                    blLNg = true; //找到线状异物
                    GetNgRegionPar(ho_LNgRegions, out hv_LNgArea, out hv_LNgRow, out hv_LNgColumn, out hv_LNgRow2, out hv_LNgColumn2);
                    for (int i = 0; i < hv_LNgRow.Length; i++)
                    {
                        double x = (hv_LNgColumn.D + hv_LNgColumn2.D) / 2;
                        double y = (hv_LNgRow.D + hv_LNgRow2.D) / 2;
                        double height = hv_LNgRow2.D - hv_LNgRow.D;
                        double width = hv_LNgColumn2.D - hv_LNgColumn.D;
                        result.X_L.Add(x);//添加进所有异物列表
                        result.Y_L.Add(y);
                        result.R_L.Add(0);
                        result.Height_L.Add(height);
                        result.Width_L.Add(width);

                        result.XLNg_L.Add(x);//添加进点状异物列表
                        result.YLNg_L.Add(y);

                        result.blLNg = blLNg;
                    }
                }

                result.Num = result.X_L.Count;

                if (result.Num == 0)
                {
                    result.LevelError_e = LevelError_enum.OK;
                    result.Annotation = par.NameCell.ToString() + "未发现点线异常！";
                    result.SetDefault();
                }     
                return true;
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
            finally
            {
                #region 记录
                RecordHoject(par, result, NameClass, "PNgRegions", ho_PNgRegions);
                RecordHoject(par, result, NameClass, "LNgRegions", ho_LNgRegions);

                RecordHoject(par, result, NameClass, "CameraZWorkR", ho_CameraZWorkR);
                RecordHoject(par, result, NameClass, "CameraZWorkG", ho_CameraZWorkG);
                RecordHoject(par, result, NameClass, "CameraZWorkB", ho_CameraZWorkB);
                RecordHoject(par, result, NameClass, "CameraCTWorkR", ho_CameraCTWorkR);
                RecordHoject(par, result, NameClass, "CameraCTWorkG", ho_CameraCTWorkG);
                RecordHoject(par, result, NameClass, "CameraCTWorkB", ho_CameraCTWorkB);

                RecordHoject(par, result, NameClass, "OutputRegionsZSizeSelP", ho_OutputRegionsZSizeSelP);
                RecordHoject(par, result, NameClass, "OutputRegionsCSizeSelP", ho_OutputRegionsCSizeSelP);
                RecordHoject(par, result, NameClass, "OutputRegionsZSizeSelL", ho_OutputRegionsZSizeSelL);
                RecordHoject(par, result, NameClass, "OutputRegionsCSizeSelL", ho_OutputRegionsCSizeSelL);

                RecordHoject(par, result, NameClass, "ZLSelectedRegions", ho_ZLSelectedRegions);
                RecordHoject(par, result, NameClass, "CTLSelectedRegions", ho_CTLSelectedRegions);
                RecordHoject(par, result, NameClass, "ZPSelectedRegions", ho_ZPSelectedRegions);
                RecordHoject(par, result, NameClass, "CTPSelectedRegions", ho_CTPSelectedRegions);

                RecordHoject(par, result, NameClass, "RectangleZPSearch", ho_RectangleZPSearch);
                RecordHoject(par, result, NameClass, "RectangleZLSearch", ho_RectangleZLSearch);

                if (ho_DestRegions != null)
                {
                    ho_DestRegions.Dispose();
                }

                if (ho_CTLObjectSelected != null)
                {
                    ho_CTLObjectSelected.Dispose();
                }

                if (ho_CTPObjectSelected != null)
                {
                    ho_CTPObjectSelected.Dispose();
                }
                #endregion 记录
            }
        }

        /// <summary>
        /// 进行彩色图像分解和二值化和区域选择
        /// </summary>
        /// <param name="ho_InputImage">输入图片</param>
        /// <param name="ho_OutputRegions">输出区域</param>
        /// <param name="Th">阈值</param>
        /// <param name="ClosingRadius">闭运算</param>
        /// <param name="OpeningRadius">开运算</param>
        /// <param name="SizeTh">尺寸阈值</param>
        /// <returns></returns>
        bool MyTh(HObject ho_InputImage, out HObject ho_OutputRegions, int Th, int ClosingRadius, int OpeningRadius, int SizeTh)
        {
            // Local iconic variables 
            HObject ho_RegionTh, ho_RegionFillUp, ho_RegionClosing;
            HObject ho_RegionOpening, ho_ConnectRegions;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_OutputRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionTh);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectRegions);
            try
            {
                ho_RegionTh.Dispose();
                HOperatorSet.Threshold(ho_InputImage, out ho_RegionTh, Th, 255);
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_RegionTh, out ho_RegionFillUp);
                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_RegionFillUp, out ho_RegionClosing, ClosingRadius);
                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_RegionClosing, out ho_RegionOpening, OpeningRadius);
                ho_ConnectRegions.Dispose();
                HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectRegions);
                ho_OutputRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectRegions, out ho_OutputRegions, (new HTuple("width")).TupleConcat(
                    "height"), "or", (new HTuple(SizeTh)).TupleConcat(SizeTh), (new HTuple(4000)).TupleConcat(
                    4000));
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
            finally
            {
                if (ho_RegionTh != null)
                {
                    ho_RegionTh.Dispose();
                }
                if (ho_RegionFillUp != null)
                {
                    ho_RegionFillUp.Dispose();
                }
                if (ho_RegionClosing != null)
                {
                    ho_RegionClosing.Dispose();
                }
                if (ho_RegionOpening != null)
                {
                    ho_RegionOpening.Dispose();
                }
                if (ho_ConnectRegions != null)
                {
                    ho_ConnectRegions.Dispose();
                }

            }
        }

        //对NG结果区域进行分析
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ho_LNgRegions">NG区域</param>
        /// <param name="Area">面积</param>
        /// <param name="Row1">外包络行起始点</param>
        /// <param name="Col1">外包络列起始点</param>
        /// <param name="Row2">外包络行结束点</param>
        /// <param name="Col2">外包络列结束点</param>
        void GetNgRegionPar(HObject ho_NgRegions, out HTuple hv_Area, out HTuple hv_Row, out HTuple hv_Column, out HTuple hv_Row2, out HTuple hv_Column2)
        {
            hv_Area = null;
            HTuple hv_CenterRow = null;
            HTuple hv_CenterCol = null;

            hv_Row = null;
            hv_Column = null;
            hv_Row2 = null;
            hv_Column2 = null;

            HOperatorSet.AreaCenter(ho_NgRegions, out hv_Area, out hv_CenterRow, out hv_CenterCol);//求取面积和中心
            HOperatorSet.SmallestRectangle1(ho_NgRegions, out hv_Row, out hv_Column, out hv_Row2, out hv_Column2);//包络区域
        }

        /// <summary>
        /// 投影变换重合度校验
        /// </summary>
        /// <param name="ho_Rectangle">异物检测的ROI</param>
        /// <param name="ho_ImageZ">正视图</param>
        /// <param name="ho_ImageC">侧视图</param>
        /// <param name="par"></param>
        /// <param name="result"></param>
        /// <param name="hv_Width"></param>
        /// <param name="hv_Height"></param>
        /// <param name="ho_MultiChannelEdges">边缘融合图像</param>
        /// <returns></returns>
        bool CalibConfirm(HObject ho_Rectangle, HObject ho_ImageZ, HObject ho_ImageC, ParLinesDotsPosNegInspect par, ResultLinesDotsPosNegInspect result, HTuple hv_Width, HTuple hv_Height, out HObject ho_MultiChannelEdges)
        {
            HObject ho_RegionDilation = null, ho_ImageReducedZWorkD = null;
            HObject ho_ImageReducedCTWorkD = null, ho_CameraZWorkRD = null;
            HObject ho_CameraZWorkGD = null, ho_CameraZWorkBD = null, ho_CameraCTWorkRD = null;
            HObject ho_CameraCTWorkGD = null, ho_CameraCTWorkBD = null;
            HObject ho_EdgesCTD = null, ho_EdgesZD = null, ho_RegionCTD = null;
            HObject ho_RegionZD = null, ho_ImageZconst = null, ho_ImageCTconst = null;

            HOperatorSet.GenEmptyObj(out ho_RegionDilation);
            HOperatorSet.GenEmptyObj(out ho_ImageReducedZWorkD);
            HOperatorSet.GenEmptyObj(out ho_ImageReducedCTWorkD);
            HOperatorSet.GenEmptyObj(out ho_CameraZWorkRD);
            HOperatorSet.GenEmptyObj(out ho_CameraZWorkGD);
            HOperatorSet.GenEmptyObj(out ho_CameraZWorkBD);
            HOperatorSet.GenEmptyObj(out ho_CameraCTWorkRD);
            HOperatorSet.GenEmptyObj(out ho_CameraCTWorkGD);
            HOperatorSet.GenEmptyObj(out ho_CameraCTWorkBD);
            HOperatorSet.GenEmptyObj(out ho_EdgesCTD);
            HOperatorSet.GenEmptyObj(out ho_EdgesZD);
            HOperatorSet.GenEmptyObj(out ho_RegionCTD);
            HOperatorSet.GenEmptyObj(out ho_RegionZD);
            HOperatorSet.GenEmptyObj(out ho_ImageZconst);
            HOperatorSet.GenEmptyObj(out ho_ImageCTconst);
            HOperatorSet.GenEmptyObj(out ho_MultiChannelEdges);

            try
            {
                ho_RegionDilation.Dispose();
                HOperatorSet.DilationRectangle1(ho_Rectangle, out ho_RegionDilation, 600, 100);
                ho_ImageReducedZWorkD.Dispose();
                HOperatorSet.ReduceDomain(ho_ImageZ, ho_RegionDilation, out ho_ImageReducedZWorkD);
                ho_ImageReducedCTWorkD.Dispose();
                HOperatorSet.ReduceDomain(ho_ImageC, ho_RegionDilation, out ho_ImageReducedCTWorkD);
                ho_CameraZWorkRD.Dispose(); ho_CameraZWorkGD.Dispose(); ho_CameraZWorkBD.Dispose();
                HOperatorSet.Decompose3(ho_ImageReducedZWorkD, out ho_CameraZWorkRD, out ho_CameraZWorkGD, out ho_CameraZWorkBD);
                ho_CameraCTWorkRD.Dispose(); ho_CameraCTWorkGD.Dispose(); ho_CameraCTWorkBD.Dispose();
                HOperatorSet.Decompose3(ho_ImageReducedCTWorkD, out ho_CameraCTWorkRD, out ho_CameraCTWorkGD, out ho_CameraCTWorkBD);
                ho_EdgesCTD.Dispose();
                HOperatorSet.EdgesSubPix(ho_CameraCTWorkGD, out ho_EdgesCTD, "canny", 1, 10, 30);
                ho_EdgesZD.Dispose();
                HOperatorSet.EdgesSubPix(ho_CameraZWorkGD, out ho_EdgesZD, "canny", 1, 10, 30);
                ho_RegionCTD.Dispose();
                HOperatorSet.GenRegionContourXld(ho_EdgesCTD, out ho_RegionCTD, "margin");
                ho_RegionZD.Dispose();
                HOperatorSet.GenRegionContourXld(ho_EdgesZD, out ho_RegionZD, "margin");
                ho_ImageZconst.Dispose();
                HOperatorSet.GenImageConst(out ho_ImageZconst, "byte", hv_Width, hv_Height);
                ho_ImageCTconst.Dispose();
                HOperatorSet.GenImageConst(out ho_ImageCTconst, "byte", hv_Width, hv_Height);

                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.PaintRegion(ho_RegionZD, ho_ImageZconst, out ExpTmpOutVar_0,
                        255, "margin");
                    ho_ImageZconst.Dispose();
                    ho_ImageZconst = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.PaintRegion(ho_RegionCTD, ho_ImageCTconst, out ExpTmpOutVar_0,
                        255, "margin");
                    ho_ImageCTconst.Dispose();
                    ho_ImageCTconst = ExpTmpOutVar_0;
                }
                ho_MultiChannelEdges.Dispose();
                HOperatorSet.Compose3(ho_ImageCTconst, ho_ImageZconst, ho_ImageZconst, out ho_MultiChannelEdges);
                return true;
            }
            catch (Exception ex)
            {
                LogError("FunLinesDotsPosNegInspect", ex);
                return false;
            }
            finally
            {
                //记录
                RecordHoject(par, result, NameClass, "RegionDilation", ho_RegionDilation);
                RecordHoject(par, result, NameClass, "ImageReducedZWorkD", ho_ImageReducedZWorkD);
                RecordHoject(par, result, NameClass, "ImageReducedCTWorkD", ho_ImageReducedCTWorkD);

                RecordHoject(par, result, NameClass, "EdgesCTD", ho_EdgesCTD);
                RecordHoject(par, result, NameClass, "EdgesZD", ho_EdgesZD);

                RecordHoject(par, result, NameClass, "RegionCTD", ho_RegionCTD);
                RecordHoject(par, result, NameClass, "RegionZD", ho_RegionZD);

                //RecordHoject(par, result, NameClass, "MultiChannelEdges", ho_MultiChannelEdges);

                //释放资源

                if (ho_CameraZWorkRD != null)
                {
                    ho_CameraZWorkRD.Dispose();
                }
                if (ho_CameraZWorkGD != null)
                {
                    ho_CameraZWorkGD.Dispose();
                }
                if (ho_CameraZWorkBD != null)
                {
                    ho_CameraZWorkBD.Dispose();
                }
                if (ho_CameraCTWorkRD != null)
                {
                    ho_CameraCTWorkRD.Dispose();
                }
                if (ho_CameraCTWorkGD != null)
                {
                    ho_CameraCTWorkGD.Dispose();
                }
                if (ho_CameraCTWorkBD != null)
                {
                    ho_CameraCTWorkBD.Dispose();
                }

                if (ho_ImageZconst != null)
                {
                    ho_ImageZconst.Dispose();
                }
                if (ho_ImageCTconst != null)
                {
                    ho_ImageCTconst.Dispose();
                }
            }
        }


    }
}
