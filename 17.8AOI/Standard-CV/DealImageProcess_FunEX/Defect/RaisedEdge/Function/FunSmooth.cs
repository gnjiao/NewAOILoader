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

namespace DealImageProcess_EX
{
    partial class FunSmoothing : FunPreprocess
    {
        #region 初始化
        public FunSmoothing()
        {
            NameClass = "FunSmooth";
        }
        #endregion 初始化

        public void DealSmoothing(ParRaisedEdgeSmooth parRaisedEdgeSmooth, HObject ho_ImageBinary, HObject ho_RegionBinary, ResultRaisedEdge result, Hashtable htResult)
        {
            ParSmooth par = parRaisedEdgeSmooth.g_ParSmooth;

            #region 定义
            HObject ho_ROI = null;
            HObject ho_FilterROI = null;
            HObject ho_FilterInverseROI = null;
            // Local control variables 
            HTuple hv_Width = null, hv_Height = null;
            HTuple hv_RowInit = new HTuple(), hv_ColInit = new HTuple();
            HTuple hv_Row1 = null, hv_Col1 = null;
            HTuple hv_Row2 = null, hv_Col2 = null;
            HTuple hv_AreaDilation = new HTuple(), hv_RowDilation = new HTuple();
            HTuple hv_ColumnDilation = new HTuple(), hv_NumberSmooth = new HTuple();
            HTuple hv_RowS = new HTuple();
            HTuple hv_ColS = new HTuple(), hv_InvertedCol = new HTuple();
            HTuple hv_InvertedRow = new HTuple(), hv_ConcatCol = new HTuple();
            HTuple hv_ConcatRow = new HTuple(), hv_NumberRegionDilation1 = new HTuple();

            // Local iconic variables 
            //怎么把图片和region导入
            HObject ho_ImageReduced;
            HObject ho_Region, ho_RegionDilation;
            HObject ho_rectErosion1, ho_BorderOrigin;
            HObject ho_ConnectedRegionDilation = null;
            HObject ho_BinImage = null;
            HObject ho_ImageBinReduced = null, ho_BorderOriginal = null, ho_SmoothedBoaderOriginal = null;
            HObject ho_SelectedSmoothedContours = null, ho_ObjectSelected = null;
            HObject ho_ContourConcat = null, ho_RegionParall = null, ho_ImageParall = null;
            HObject ho_RegionError = null, ho_ConnectedRegions = null, ho_RegionErosionError = null;
            HObject ho_RegionDilationError = null;

            //求取缺陷
            HObject ho_BorderFull = null;
            HObject ho_SmoothedContoursFull = null;
            HObject ho_ParaFull = null;
            HObject ho_ParaRegionFull = null;
            HObject ho_RegionROIFull = null;
            HObject ho_ErrorFull = null;
            HObject ho_FilterErrorFull = null;//崩缺求取中，剪掉台阶面崩缺后的缺陷
            HObject ho_ErrorRegioinFull = null;
            HObject ho_ErrorConnectFull = null;
            HObject ho_ErrorSelectFull = null;
            HObject ho_ErrorXldFull = null;

            HObject ho_SmoothedContoursFullLongest = null;
            HOperatorSet.GenEmptyObj(out ho_SmoothedContoursFullLongest);

            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation);
            HOperatorSet.GenEmptyObj(out ho_rectErosion1);
            HOperatorSet.GenEmptyObj(out ho_BorderOrigin);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegionDilation);
            HOperatorSet.GenEmptyObj(out ho_BinImage);
            HOperatorSet.GenEmptyObj(out ho_ImageBinReduced);
            HOperatorSet.GenEmptyObj(out ho_BorderOriginal);
            HOperatorSet.GenEmptyObj(out ho_SmoothedBoaderOriginal);
            HOperatorSet.GenEmptyObj(out ho_SelectedSmoothedContours);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_ContourConcat);
            HOperatorSet.GenEmptyObj(out ho_RegionParall);
            HOperatorSet.GenEmptyObj(out ho_ImageParall);
            HOperatorSet.GenEmptyObj(out ho_RegionError);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionErosionError);
            HOperatorSet.GenEmptyObj(out ho_RegionDilationError);
            HOperatorSet.GenEmptyObj(out ho_BorderFull);
            HOperatorSet.GenEmptyObj(out ho_SmoothedContoursFull);


            HOperatorSet.GenEmptyObj(out ho_FilterInverseROI);
            HOperatorSet.GenEmptyObj(out ho_FilterErrorFull);
            HOperatorSet.GenEmptyObj(out ho_ParaFull);
            HOperatorSet.GenEmptyObj(out ho_ParaRegionFull);
            #endregion 定义

            try
            {
                int smooth = (par.SmoothValue / 2) * 2 + 1;//平滑系数只能是奇数

                HOperatorSet.GetImageSize(ho_ImageBinary, out hv_Width, out hv_Height);
                //求取ROI
                FunROI funCirROI = new FunROI();
                bool blResult = false;
                string anno = "";
                ho_ROI = funCirROI.CreateOneROI(parRaisedEdgeSmooth.g_ParROI.g_ParROIExcute_L[0], htResult, out blResult, out anno);


                //对ROI区域进行腐蚀，剔除多余轮廓的边界
                HOperatorSet.ErosionCircle(ho_ROI, out ho_rectErosion1, 20);
                HOperatorSet.ReduceDomain(ho_ImageBinary, ho_rectErosion1, out ho_ImageReduced);

                //通过ROI截图
                HOperatorSet.Intersection(ho_RegionBinary, ho_ROI, out ho_RegionDilation);

                //对二值化图片求取轮廓
                HOperatorSet.ThresholdSubPix(ho_ImageReduced, out ho_BorderOrigin, 100);

                //迭代十次
                for (int i = 0; i < par.Num; i++)
                {
                    hv_Row1 = new HTuple();
                    hv_Col1 = new HTuple();
                    hv_Row2 = new HTuple();
                    hv_Col2 = new HTuple();

                    //剔除主体之外的噪声点
                    ho_ConnectedRegionDilation.Dispose();
                    HOperatorSet.Connection(ho_RegionDilation, out ho_ConnectedRegionDilation);
                    HOperatorSet.AreaCenter(ho_ConnectedRegionDilation, out hv_AreaDilation, out hv_RowDilation, out hv_ColumnDilation);
                    ho_RegionDilation.Dispose();

                    //保留主体
                    HOperatorSet.SelectShape(ho_ConnectedRegionDilation, out ho_RegionDilation, "area", "and", 50000, int.MaxValue);
                    //主题二值化以后的图片
                    ho_BinImage.Dispose();
                    HOperatorSet.RegionToBin(ho_RegionDilation, out ho_BinImage, 255, 0, hv_Width, hv_Height);

                    //二值化缩减轮廓
                    ho_ImageBinReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_BinImage, ho_rectErosion1, out ho_ImageBinReduced);

                    //***************************
                    //生成带有方向梯度的边界，用平滑后的边界和原始边界的平移生成轮廓处理
                    //生成内边界
                    ho_BorderOriginal.Dispose();
                    HOperatorSet.ThresholdSubPix(ho_ImageBinReduced, out ho_BorderOriginal, 100);

                    //角点非圆弧
                    ho_SmoothedBoaderOriginal.Dispose();
                    HOperatorSet.SmoothContoursXld(ho_BorderOriginal, out ho_SmoothedBoaderOriginal, smooth);

                    //筛选轮廓
                    ho_SelectedSmoothedContours.Dispose();
                    HOperatorSet.SelectContoursXld(ho_SmoothedBoaderOriginal, out ho_SelectedSmoothedContours, "contour_length", par.SelectAreaLow, 99999999, -0.5, 0.5);
                    HOperatorSet.CountObj(ho_SelectedSmoothedContours, out hv_NumberSmooth);
                    if ((int)(new HTuple(hv_NumberSmooth.TupleGreater(1))) != 0)
                    {
                        int numSmooth = hv_NumberSmooth.ToIArr()[0] - 1;
                        for (int j = 0; j < numSmooth; j++)
                        {
                            ho_ObjectSelected.Dispose();
                            HOperatorSet.SelectObj(ho_SelectedSmoothedContours, out ho_ObjectSelected, j + 1);
                            HOperatorSet.GetContourXld(ho_ObjectSelected, out hv_RowS, out hv_ColS);
                            HOperatorSet.TupleConcat(hv_RowInit, hv_RowS, out hv_RowInit);
                            HOperatorSet.TupleConcat(hv_ColInit, hv_ColS, out hv_ColInit);
                        }
                    }
                    else
                    {
                        HOperatorSet.GetContourXld(ho_SelectedSmoothedContours, out hv_RowInit, out hv_ColInit);
                    }

                    //生成平行轮廓的点
                    HTuple[] rowCol = null;
                    GenParallPoint(par.Position, par.DefectType, (int)par.GapIn, (int)par.GapOut, hv_RowInit, hv_ColInit, out rowCol);
                    hv_Row1 = rowCol[0];
                    hv_Col1 = rowCol[1];
                    hv_Row2 = rowCol[2];
                    hv_Col2 = rowCol[3];
                    //生成平行轮廓包围的区域
                    HOperatorSet.TupleInverse(hv_Col2, out hv_InvertedCol);
                    HOperatorSet.TupleInverse(hv_Row2, out hv_InvertedRow);

                    HOperatorSet.TupleConcat(hv_Col1, hv_InvertedCol, out hv_ConcatCol);
                    HOperatorSet.TupleConcat(hv_Row1, hv_InvertedRow, out hv_ConcatRow);

                    //select_contours_xld (SmoothedContours, SelectedSmoothedContours, 'contour_length', 50, 99999999, -0.5, 0.5)

                    //HOperatorSet.SelectContoursXld(ho_ContourConcat, out ho_ContourConcat, "contour_length", par.ErrorAreaValue, 99999999, -0.5, 0.5);

                    if (hv_ConcatRow.Length == 0)
                    {
                        result.LevelError_e = LevelError_enum.Error;
                        result.Annotation = par.NameCell.ToString() + "hv_ConcatRow的数量为0";
                        result.SetDefault();
                        return;
                    }

                    ho_ContourConcat.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_ContourConcat, hv_ConcatRow, hv_ConcatCol);

                    //生成平行轮廓region
                    ho_RegionParall.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_ContourConcat, out ho_RegionParall, "filled");

                    ho_ImageParall.Dispose();
                    HOperatorSet.ReduceDomain(ho_BinImage, ho_RegionParall, out ho_ImageParall);
                    ho_RegionError.Dispose();
                    HOperatorSet.Threshold(ho_ImageParall, out ho_RegionError, 100, 255);
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_RegionError, out ho_ConnectedRegions);

                    //轮廓的内边界先外移1个像素，再膨胀靠近（忽略最小1个像素的地方）
                    ho_RegionErosionError.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_RegionErosionError, "area", "and", par.ErrorAreaValue, 99999999);

                    //膨胀缺陷
                    ho_RegionDilationError.Dispose();
                    HOperatorSet.DilationCircle(ho_RegionErosionError, out ho_RegionDilationError, par.DilationDefectValue);

                    //剔除残留
                    HOperatorSet.Difference(ho_RegionDilation, ho_RegionDilationError, out ho_RegionDilation);

                    HOperatorSet.CountObj(ho_RegionDilationError, out hv_NumberRegionDilation1);
                    if ((int)(new HTuple(hv_NumberRegionDilation1.TupleEqual(0))) != 0)
                    {
                        break;
                    }
                }

                //完整图片
                ho_BinImage.Dispose();
                HOperatorSet.RegionToBin(ho_RegionDilation, out ho_BinImage, 255, 0, hv_Width, hv_Height);

                ho_ImageBinReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_BinImage, ho_rectErosion1, out ho_ImageBinReduced);

                //完整轮廓
                ho_BorderFull.Dispose();
                HOperatorSet.ThresholdSubPix(ho_ImageBinReduced, out ho_BorderFull, 100);

                ho_SmoothedContoursFull.Dispose();
                HOperatorSet.SmoothContoursXld(ho_BorderFull, out ho_SmoothedContoursFull, smooth);

                HTuple hv_ho_SmoothedContoursFullNumber = null;
                HOperatorSet.CountObj(ho_SmoothedContoursFull, out hv_ho_SmoothedContoursFullNumber);//计算平滑区域的xld个数

                //如果完整轮廓为空，则返回
                if (hv_ho_SmoothedContoursFullNumber == 0)
                {
                    result.LevelError_e = LevelError_enum.Error;
                    result.Annotation = par.NameCell.ToString() + "ho_SmoothedContoursFull的数量为0";
                    result.SetDefault();
                    return;
                }
                //计算完整轮廓的长度
                HTuple hv_SmoothedContoursFullLength=null;
                HOperatorSet.LengthXld(ho_SmoothedContoursFull,out hv_SmoothedContoursFullLength);
                //找到完整轮廓中最长的一段，剔除干扰小段
                HOperatorSet.SelectObj(ho_SmoothedContoursFull, out ho_SmoothedContoursFullLongest, (((hv_SmoothedContoursFullLength.TupleSortIndex()
                        )).TupleSelect((new HTuple(hv_SmoothedContoursFullLength.TupleLength())) - 1)) + 1);


                HTuple[] rowColFull = null;
                HOperatorSet.GetContourXld(ho_SmoothedContoursFullLongest, out hv_RowInit, out hv_ColInit);
                GenParallPoint(par.Position, par.DefectType, 0, (int)par.GapOut, hv_RowInit, hv_ColInit, out rowColFull);
                hv_Row1 = rowColFull[0];
                hv_Col1 = rowColFull[1];
                hv_Row2 = rowColFull[2];
                hv_Col2 = rowColFull[3];
                //生成平行轮廓包围的区域
                HOperatorSet.TupleInverse(hv_Col2, out hv_InvertedCol);
                HOperatorSet.TupleInverse(hv_Row2, out hv_InvertedRow);

                HOperatorSet.TupleConcat(hv_Col1, hv_InvertedCol, out hv_ConcatCol);
                HOperatorSet.TupleConcat(hv_Row1, hv_InvertedRow, out hv_ConcatRow);

                ho_ContourConcat.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_ContourConcat, hv_ConcatRow, hv_ConcatCol);
                HOperatorSet.GenRegionContourXld(ho_ContourConcat, out ho_RegionROIFull, "filled");
                HOperatorSet.ReduceDomain(ho_ImageBinary, ho_RegionROIFull, out ho_ErrorFull);

                ho_FilterErrorFull.Dispose();
                ho_FilterInverseROI.Dispose();
                if (par.DefectType == "Shell" && parRaisedEdgeSmooth.g_ParROI.g_ParROIExcute_L.Count > 1)
                {
                    ho_FilterROI = funCirROI.CreateOneROI(parRaisedEdgeSmooth.g_ParROI.g_ParROIExcute_L[1], htResult, out blResult, out anno);
                    HOperatorSet.Complement(ho_FilterROI, out ho_FilterInverseROI);
                    HOperatorSet.ReduceDomain(ho_ErrorFull, ho_FilterInverseROI, out ho_FilterErrorFull);
                }
                else
                {
                    ho_FilterErrorFull = ho_ErrorFull.Clone();
                }

                HOperatorSet.Threshold(ho_FilterErrorFull, out ho_ErrorRegioinFull, 100, 255);
                HOperatorSet.Connection(ho_ErrorRegioinFull, out ho_ErrorConnectFull);

                HTuple numError = 0;
                HTuple rowFull = null;
                HTuple colFull = null;
                HTuple disMin = null;
                HTuple disMax = null;

                HTuple disMinSort = null;
                HTuple disMinSortIndex = null;
                HTuple areaError = null;
                HTuple rowError = null;
                HTuple colError = null;
                HOperatorSet.CountObj(ho_ErrorConnectFull, out numError);
                HObject ho_ErrorSelectedPart;
                for (int i = 0; i < numError; i++)
                {
                    HOperatorSet.SelectObj(ho_ErrorConnectFull, out ho_ErrorSelectedPart, i + 1);
                    HOperatorSet.GenContourRegionXld(ho_ErrorSelectedPart, out ho_ErrorXldFull, "border");
                    HOperatorSet.GetContourXld(ho_ErrorXldFull, out rowFull, out colFull);
                    HOperatorSet.DistancePc(ho_SmoothedContoursFullLongest, rowFull, colFull, out disMin, out disMax);
                    HOperatorSet.TupleSort(disMin, out disMinSort);
                    HOperatorSet.TupleSortIndex(disMin, out disMinSortIndex);
                    //for (int j = 0; j < disMax.Length; j++)
                    //{
                    if (disMinSort[disMinSort.Length-1] > par.NormalTh)
                        {
                            HOperatorSet.AreaCenter(ho_ErrorSelectedPart, out areaError, out rowError, out colError);
                            result.X_L.Add(colFull[disMinSortIndex[disMinSort.Length - 1].I]);
                            result.Y_L.Add(rowFull[disMinSortIndex[disMinSort.Length - 1].I]);
                            result.R_L.Add(0);
                            double dist = disMinSort[disMinSort.Length - 1];
                            result.Height_L.Add(Math.Round(dist, 1));
                            result.Area_L.Add(areaError.ToDArr()[0]);
                            break;
                        }
                    //}
                }

                //区域个数
                result.Num = result.X_L.Count;
                if (result.Num == 0)
                {
                    //如果前一次结果为error,那么即使这次的结果是OK，也要保持ERROR的状态
                    if (result.LevelError_e == LevelError_enum.Error)
                    {
                        result.SetDefault();
                    }
                    else
                    {
                        result.LevelError_e = LevelError_enum.OK;
                        result.Annotation = par.NameCell.ToString() + "缺陷个数为0";
                        result.SetDefault();
                    }
                }

                //添加显示
                AddDisplay(par.g_ParOutput, result);
            }
            catch (Exception ex)
            {
                result.LevelError_e = LevelError_enum.Error;
                result.Annotation = par.NameCell.ToString() + ex.Message.ToString();
                result.SetDefault();
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                #region 记录
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_ImageBinary", ho_ImageBinary);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_ROI", ho_ROI);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_FilterROI", ho_FilterROI);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_rectErosion1", ho_rectErosion1);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_ImageReduced", ho_ImageReduced);

                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_Region", ho_Region);

                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_RegionDilation", ho_RegionDilation);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_BorderOrigin", ho_BorderOrigin);

                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_ConnectedRegionDilation", ho_ConnectedRegionDilation);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_BinImage", ho_BinImage);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_BorderOriginal", ho_BorderOriginal);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_SmoothedBoaderOriginal", ho_SmoothedBoaderOriginal);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_SelectedSmoothedContours", ho_SelectedSmoothedContours);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_ObjectSelected", ho_ObjectSelected);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_ContourConcat", ho_ContourConcat);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_RegionParall", ho_RegionParall);


                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_FilterErrorFull", ho_FilterErrorFull);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_ImageParall", ho_ImageParall);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_RegionError", ho_RegionError);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_ConnectedRegions", ho_ConnectedRegions);

                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_RegionErosionError", ho_RegionErosionError);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_RegionDilationError", ho_RegionDilationError);

                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_BorderFull", ho_BorderFull);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_SmoothedContoursFull", ho_SmoothedContoursFull);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_SmoothedContoursFullLongest", ho_SmoothedContoursFullLongest);

                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_RegionROIFull", ho_RegionROIFull);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_ErrorConnectFull", ho_ErrorConnectFull);
                #endregion 记录

            }
        }


        /// <summary>
        /// 生成平行轮廓的点
        /// </summary>
        void GenParallPoint(string position, string typeDefect, int gapIn, int gapOut, HTuple hv_Row, HTuple hv_Col, out HTuple[] rowCol)
        {
            rowCol = new HTuple[4];
            HTuple hv_Row1 = null;
            HTuple hv_Col1 = null;
            HTuple hv_Row2 = null;
            HTuple hv_Col2 = null;
            try
            {
                //生成外边界,通过判断四个角给出边界位置
                switch (position)
                {
                    case "左下":
                        //产品在左下角
                        //1个缓冲像素区域，15个平行轮廓区域
                        hv_Row1 = hv_Row - gapIn;
                        hv_Col1 = hv_Col + gapIn;
                        hv_Row2 = hv_Row - gapOut;
                        hv_Col2 = hv_Col + gapOut;

                        if (typeDefect=="Shell")//相当于右上
                        {
                            hv_Row1 = hv_Row + gapIn;
                            hv_Col1 = hv_Col - gapIn;
                            hv_Row2 = hv_Row + gapOut;
                            hv_Col2 = hv_Col - gapOut;
                        }
                        break;

                    case "右下":
                        //产品在右下角
                        //1个缓冲像素区域，15个平行轮廓区域
                        hv_Row1 = hv_Row - gapIn;
                        hv_Col1 = hv_Col - gapIn;
                        hv_Row2 = hv_Row - gapOut;
                        hv_Col2 = hv_Col - gapOut;

                        if (typeDefect == "Shell")//相当于左上
                        {
                            hv_Row1 = hv_Row + gapIn;
                            hv_Col1 = hv_Col + gapIn;
                            hv_Row2 = hv_Row + gapOut;
                            hv_Col2 = hv_Col + gapOut;
                        }
                        break;

                    case "右上":
                        //产品在右上角
                        //1个缓冲像素区域，15个平行轮廓区域
                        hv_Row1 = hv_Row + gapIn;
                        hv_Col1 = hv_Col - gapIn;
                        hv_Row2 = hv_Row + gapOut;
                        hv_Col2 = hv_Col - gapOut;

                        if (typeDefect == "Shell")//相当于左下
                        {
                            hv_Row1 = hv_Row - gapIn;
                            hv_Col1 = hv_Col + gapIn;
                            hv_Row2 = hv_Row - gapOut;
                            hv_Col2 = hv_Col + gapOut;
                        }
                        break;

                    case "左上":
                        //产品在左上角
                        //1个缓冲像素区域，15个平行轮廓区域
                        hv_Row1 = hv_Row + gapIn;
                        hv_Col1 = hv_Col + gapIn;
                        hv_Row2 = hv_Row + gapOut;
                        hv_Col2 = hv_Col + gapOut;

                        if (typeDefect == "Shell")//相当于右下
                        {
                            hv_Row1 = hv_Row - gapIn;
                            hv_Col1 = hv_Col - gapIn;
                            hv_Row2 = hv_Row - gapOut;
                            hv_Col2 = hv_Col - gapOut;
                        }
                        break;

                    default:
                        //产品在左下角
                        //1个缓冲像素区域，15个平行轮廓区域
                        hv_Row1 = hv_Row - gapIn;
                        hv_Col1 = hv_Col + gapIn;
                        hv_Row2 = hv_Row - gapOut;
                        hv_Col2 = hv_Col + gapOut;
                        break;
                }

                rowCol[0] = hv_Row1;
                rowCol[1] = hv_Col1;
                rowCol[2] = hv_Row2;
                rowCol[3] = hv_Col2;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
    }
}
