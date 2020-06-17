using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealResult;
using System.Collections;
using HalconDotNet;
using BasicClass;
using DealImageProcess;
using DealResult_EX;
using DealImageProcess_EX;

namespace DealImageProcess_FunEX
{
    public partial class FunFinShell : FunPreprocess
    {
        #region 初始化
        public FunFinShell()
        {
            NameClass = "FunFinShell";
        }
        #endregion 初始化

        public ResultFinShell DealFinShell(ParFinShell par, Hashtable htResult)
        {
            #region 定义
            HTuple num_Obj = 0;
            ResultFinShell result = new ResultFinShell();

            HTuple width = null;
            HTuple height = null;

            ImageAll imageAll = null;

            HObject ho_Image = null;
            HObject ho_RegionReduced = null;
            HObject ho_ResultPreprocess = null;
            HObject ho_RegionOuter = null;
            HObject ho_RegionInnerOuter = null;
            HObject ho_RegionInner = null;
            HObject ho_RegionIntersected = null;
            HObject ho_RegionConnected = null;
            HObject ho_RegionFillUp = null;
            HObject ho_RegionOpening = null;
            HObject ho_RegionClosing = null;

            HObject ho_RegionWidthSelected = null;
            HObject ho_RegionHeightSelected = null;
            HObject ho_RegionAreaSelected = null;

            HObject ho_RegionUnion1 = null;
            HObject ho_RegionUnion2 = null;

            HObject ho_RegionFinShell = null;

            //求取沿XLD外形切线方向的外轮廓
            HObject ho_xldToCalHeight = null;
            HObject ho_xldToCalWidth = null;
            HObject ho_ho_RegionFinShellSelect = null;
            HObject ho_ho_RegionFinShellSelectContours = null;

            HTuple hv_disMin = null;
            double HeightCalUpper = 0;

            HTuple WidthCalRow = null, WidthCalCol = null;

            HTuple hv_FinShellIntersectRow = null;
            HTuple hv_FinShellIntersectCol = null;
            HTuple hv_FinShellCircleRadius = null;

            HTuple FinShellWidth = null;

            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_RegionReduced);
            HOperatorSet.GenEmptyObj(out ho_ResultPreprocess);
            HOperatorSet.GenEmptyObj(out ho_RegionOuter);
            HOperatorSet.GenEmptyObj(out ho_RegionInnerOuter);
            HOperatorSet.GenEmptyObj(out ho_RegionInner);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersected);
            HOperatorSet.GenEmptyObj(out ho_RegionConnected);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionWidthSelected);
            HOperatorSet.GenEmptyObj(out ho_RegionHeightSelected);
            HOperatorSet.GenEmptyObj(out ho_RegionAreaSelected);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion1);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion2);
            HOperatorSet.GenEmptyObj(out ho_RegionFinShell);

            HOperatorSet.GenEmptyObj(out ho_xldToCalHeight);
            HOperatorSet.GenEmptyObj(out ho_xldToCalWidth);
            HOperatorSet.GenEmptyObj(out ho_ho_RegionFinShellSelect);
            HOperatorSet.GenEmptyObj(out ho_ho_RegionFinShellSelectContours);

            HTuple hv_Area = null;
            HTuple hv_CenterRow = null;
            HTuple hv_CenterCol = null;

            HTuple hv_Row = null;
            HTuple hv_Column = null;
            HTuple hv_Row2 = null;
            HTuple hv_Column2 = null;
            HTuple hv_Phi = null;
            HTuple hv_Width = null;
            HTuple hv_Height = null;
            #endregion 定义

            try
            {
                #region 基础功能调用
                //if (BasicImageProcess(par, result, htResult, out ho_RegionReduced, out ho_Image, out width, out height))
                //{

                //}
                //else
                //{
                //    return result;
                //}
                #endregion 基础功能调用

                //获取图像预处理结果，图像是二值化后再进行处理
                ho_ResultPreprocess = result.g_ResultPreProcess.ImageResult.Ho_Image;

                #region 真实轮廓
                //string nameStdEdge = par.NameCellActualEdge;
                string nameStdEdge = "C9";
                /***** 得到当前片的平滑轮廓******/
                ResultRaisedEdge resultRaise = (ResultRaisedEdge)htResult[nameStdEdge];
                if (resultRaise == null)
                {
                    result.LevelError_e = LevelError_enum.OK;
                    result.Annotation = par.NameCell.ToString() + ",当前片的平滑轮廓获取异常";
                    result.SetDefault();
                    return result;
                }
                HObject ho_StdEdge =( (ImageAll)resultRaise.HtResultImage["C9FunRaisedEdge.RegionRemainingRemovedSmoothed"]).Ho_Image;

                #endregion 真实轮廓

                //以下为崩缺检查的算法
                //获取图像预处理结果，图像是二值化后再进行处理
                ho_ResultPreprocess = result.g_ResultPreProcess.ImageResult.Ho_Image;
                //通过残留检查平滑后的轮廓获取平行的ROI区域
                ho_RegionOuter.Dispose();
                ho_RegionInner.Dispose();
                ho_RegionInnerOuter.Dispose();
                GenParalRegionFromXld(ho_StdEdge, out ho_RegionOuter, out ho_RegionInnerOuter, out ho_RegionInner, par.Width_Paral, par.OuterStdShift_Paral, par.InnerStdShift_Paral);


                HObject ho_PreRegion = result.g_ResultPreProcess.resultBinary.RegionResult.Ho_Image;
                ho_RegionIntersected.Dispose();
                if (par.WorkingRegion == "Outer")//残留检测，选区为边缘外侧
                {
                    HOperatorSet.Intersection(ho_PreRegion, ho_RegionOuter, out ho_RegionIntersected);
                }
                else if (par.WorkingRegion == "Inner")//崩缺检测，选区为边缘内侧
                {
                    HOperatorSet.Intersection(ho_ResultPreprocess, ho_RegionInner, out ho_RegionIntersected);
                }
                else
                {
                    return result;
                }
                HOperatorSet.CountObj(ho_RegionIntersected, out num_Obj);
                if (num_Obj == 0)
                {
                    result.LevelError_e = LevelError_enum.OK;
                    result.Annotation = par.NameCell.ToString() + ",选区内无异常";
                    result.SetDefault();
                    return result;
                }
                ho_RegionConnected.Dispose();
                HOperatorSet.Connection(ho_RegionIntersected, out ho_RegionConnected);
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_RegionConnected, out ho_RegionFillUp);

                #region 开运算，闭运算
                ho_RegionOpening.Dispose();

                if (par.OpeningCircle != 0)
                {
                    HOperatorSet.OpeningCircle(ho_RegionFillUp, out ho_RegionOpening, par.OpeningCircle);
                }
                else
                {
                    ho_RegionOpening = ho_RegionFillUp.Clone();
                }
                HOperatorSet.CountObj(ho_RegionOpening, out num_Obj);
                if (num_Obj == 0)
                {
                    result.LevelError_e = LevelError_enum.OK;
                    result.Annotation = par.NameCell.ToString() + ",开运算后异常点消失";
                    result.SetDefault();
                    return result;
                }

                ho_RegionClosing.Dispose();
                if (par.ClosingCircle != 0)
                {
                    HOperatorSet.ClosingCircle(ho_RegionOpening, out ho_RegionClosing, par.ClosingCircle);
                }
                else
                {
                    ho_RegionClosing = ho_RegionOpening.Clone();
                }
                #endregion 开运算，闭运算

                //把贝壳宽度/高度/面积超出阈值的，都挑选出来
                ho_RegionWidthSelected.Dispose();
                ho_RegionHeightSelected.Dispose();
                ho_RegionAreaSelected.Dispose();
                HOperatorSet.SelectShape(ho_RegionClosing, out ho_RegionWidthSelected, "width", "and", par.MinWidth, par.MaxWidth);
                HOperatorSet.SelectShape(ho_RegionWidthSelected, out ho_RegionHeightSelected, "height", "and", par.MinHeight, par.MaxHeight);
                HOperatorSet.SelectShape(ho_RegionHeightSelected, out ho_RegionAreaSelected, "area", "and", par.MinArea, par.MaxArea);
                ho_RegionUnion1.Dispose();
                //HOperatorSet.Union2(ho_RegionWidthSelected, ho_RegionHeightSelected, out ho_RegionUnion1);
                ho_RegionUnion2.Dispose();
                //HOperatorSet.Union2(ho_RegionUnion1, ho_RegionAreaSelected, out ho_RegionUnion2);
                HOperatorSet.CountObj(ho_RegionAreaSelected, out num_Obj);
                if (num_Obj == 0)
                {
                    result.LevelError_e = LevelError_enum.OK;
                    result.Annotation = par.NameCell.ToString() + ",区域特征提取后异常点消失";
                    result.SetDefault();
                    return result;
                }
                //提取到的贝壳(Shell)OR残留(Fin)
                ho_RegionFinShell.Dispose();
                ho_RegionFinShell = ho_RegionAreaSelected.Clone();
                //HOperatorSet.Connection(ho_RegionUnion2, out ho_RegionFinShell);

                //对获取到的残留或者崩缺进行blob分析
                HOperatorSet.AreaCenter(ho_RegionFinShell, out hv_Area, out hv_CenterRow, out hv_CenterCol);
                double[] dblHeightArry = new double[hv_CenterRow.Length];//存放高度值的数组
                double[] dblWidthArry = new double[hv_CenterRow.Length];

                #region 求取包络
                switch (par.SmallestSurround_e)
                {
                    case SmallestSurround_enum.Rect2:
                        HOperatorSet.SmallestRectangle2(ho_RegionFinShell, out hv_Row, out hv_Column, out hv_Phi, out hv_Width, out hv_Height);

                        break;

                    case SmallestSurround_enum.Circle:
                        HOperatorSet.SmallestCircle(ho_RegionFinShell, out hv_Row, out hv_Column, out hv_Width);
                        break;

                    case SmallestSurround_enum.TanLineRect:
                        if (par.WorkingRegion == "Outer")//残留检测，选区为边缘外侧
                        {
                            HeightCalUpper = par.OuterStdShift_Paral + par.Width_Paral * 1.5;
                            HOperatorSet.GenParallelContourXld(ho_StdEdge, out ho_xldToCalHeight, "regression_normal", HeightCalUpper);
                            HOperatorSet.GenParallelContourXld(ho_StdEdge, out ho_xldToCalWidth, "regression_normal", par.OuterStdShift_Paral);
                        }
                        else//崩缺检测，选区为边缘内测
                        {
                            HeightCalUpper = par.InnerStdShift_Paral + par.Width_Paral * 1.5;
                            HOperatorSet.GenParallelContourXld(ho_StdEdge, out ho_xldToCalHeight, "regression_normal", -HeightCalUpper);
                            HOperatorSet.GenParallelContourXld(ho_StdEdge, out ho_xldToCalWidth, "regression_normal", -par.InnerStdShift_Paral);
                        }
                        for (int j = 0; j < hv_CenterRow.Length; j++)
                        {
                            try
                            {
                                HOperatorSet.SelectObj(ho_RegionFinShell, out ho_ho_RegionFinShellSelect, j + 1);
                                HOperatorSet.GenContourRegionXld(ho_ho_RegionFinShellSelect, out ho_ho_RegionFinShellSelectContours, "border");


                                //求残留区域和参考xld平行的XLD
                                //HObject ho_ContoursIntersected = null;
                                //HObject ho_TestImage = null;
                                //HObject ho_TestImageResult = null;
                                //HOperatorSet.GenImageConst(out ho_TestImage, "byte", width, height);
                                //HOperatorSet.PaintXld(ho_xldToCalHeight, ho_TestImage, out ho_TestImageResult, 255);
                                //HOperatorSet.PaintXld(ho_ho_RegionFinShellSelectContours, ho_TestImageResult, out ho_TestImageResult, 255);
                                //HOperatorSet.WriteImage(ho_TestImageResult, "bmp", 128, "E:\\DOC\\德龙\\0427测试\\XKY4.27\\TestImage"+j.ToString());



                                //计算残留高度
                                HOperatorSet.DistanceCcMin(ho_xldToCalHeight, ho_ho_RegionFinShellSelectContours, "fast_point_to_segment", out hv_disMin);
                                dblHeightArry[j] = Math.Round(HeightCalUpper - hv_disMin.D, 1);

                                
                                HOperatorSet.IntersectionContoursXld(ho_xldToCalWidth, ho_ho_RegionFinShellSelectContours, "mutual", out hv_FinShellIntersectRow, out hv_FinShellIntersectCol, out hv_FinShellCircleRadius);

                                double dblFinShellWidth = 0;
                                HTuple hv_WidthTemp;
                                double[] IntersectPointRow = hv_FinShellIntersectRow.DArr;
                                double[] IntersectPointCol = hv_FinShellIntersectCol.DArr;
                                //求取边缘上最远的两个点的距离
                                for (int k = 0; k < hv_FinShellIntersectRow.Length-1; k++)
                                {
                                    for (int l = k+1; l < hv_FinShellIntersectRow.Length; l++)
                                    {
                                        HOperatorSet.DistancePp(IntersectPointRow[k], IntersectPointCol[k], IntersectPointRow[l], IntersectPointCol[l], out hv_WidthTemp);
                                        if (hv_WidthTemp.D > dblFinShellWidth)
                                        {
                                            dblFinShellWidth = hv_WidthTemp.D;
                                        }
                                    }
                                }
                                
                                //HOperatorSet.GenContourPolygonXld(out ho_ContoursIntersected, hv_FinShellIntersectRow, hv_FinShellIntersectCol);
                                //HOperatorSet.LengthXld(ho_ContoursIntersected, out FinShellWidth);

                                //计算残留宽度
                                dblWidthArry[j] = Math.Round(dblFinShellWidth, 1);
                            }
                            catch
                            {

                            }
                        }
                        break;

                    default://默认矩形1
                        HOperatorSet.SmallestRectangle1(ho_RegionFinShell, out hv_Row, out hv_Column, out hv_Row2, out hv_Column2);
                        break;
                }
                


                #endregion 求取包络

                double x = 0;
                double y = 0;

                for (int j = 0; j < hv_CenterRow.Length; j++)
                {
                    #region 输出坐标类型
                    switch (par.TypeOutCoord)
                    {
                        case "面积中心":
                            x = Math.Round(hv_CenterCol.ToDArr()[j], 0);
                            y = Math.Round(hv_CenterRow.ToDArr()[j], 0);
                            break;

                        case "包络中心":
                            if (par.SmallestSurround_e == SmallestSurround_enum.Rect1)
                            {
                                x = Math.Round((hv_Column.ToDArr()[j] + hv_Column2.ToDArr()[j]) / 2, 3);
                                y = Math.Round((hv_Row.ToDArr()[j] + hv_Row2.ToDArr()[j]) / 2, 3);

                            }
                            else
                            {
                                x = Math.Round(hv_Column.ToDArr()[j], 3);
                                y = Math.Round(hv_Row.ToDArr()[j], 3);
                            }
                            break;
                    }
                    #endregion 输出坐标类型


                    result.X_L.Add(x);
                    result.Y_L.Add(y);
                    result.Area_L.Add(hv_Area.IArr[j]);//求取面积
                    result.Rectangularity_L.Add(0);
                    result.Circularity_L.Add(0);
                    #region 求取包络的集合
                    switch (par.SmallestSurround_e)
                    {
                        case SmallestSurround_enum.Rect2:
                            result.R_L.Add(Math.Round(hv_Phi.DArr[j], 5));//求取角度                                    
                            result.Height_L.Add(Math.Round(hv_Height.DArr[j], 3));//获取矩形的长宽
                            result.Width_L.Add(Math.Round(hv_Width.DArr[j], 3));
                            result.Radius_L.Add(0);
                            break;

                        case SmallestSurround_enum.Circle:
                            result.Radius_L.Add(Math.Round(hv_Width.DArr[j], 3));
                            result.R_L.Add(0);
                            result.Height_L.Add(0);
                            result.Width_L.Add(0);
                            break;
                        case SmallestSurround_enum.TanLineRect: //切线矩形，即为方向与xld切线平行方向的矩形
                            result.Radius_L.Add(0);
                            result.R_L.Add(0);
                            result.Height_L.Add(dblHeightArry[j]);
                            result.Width_L.Add(dblWidthArry[j]);
                            break;

                        default://默认矩形1
                            result.R_L.Add(0);
                            double rectWidth = (hv_Column2.IArr[j] - hv_Column.IArr[j]) / 2;
                            result.Width_L.Add(Math.Round(rectWidth, 3));
                            double rectHeight = (hv_Row2.IArr[j] - hv_Row.IArr[j]) / 2;
                            result.Height_L.Add(Math.Round(rectHeight, 3));
                            result.Radius_L.Add(0);
                            break;
                    }
                    #endregion 求取包络的集合
                }

                //区域个数
                result.Num = result.X_L.Count;
                if (result.Num == 0)
                {
                    result.LevelError_e = LevelError_enum.OK;
                    result.Annotation = par.NameCell.ToString() + "FinShell个数为0";
                    result.SetDefault();
                }
                //添加显示
                AddDisplay(par.g_ParOutput, result);

                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
            finally
            {
                //对结果进行综合处理
                SetComprehensiveResult(result, par, htResult,false);

                //校准
                DealCalibResult(par, result, htResult);

                #region 记录时间
                WriteRunTime(stopWatch, NameClass, par, result);
                #endregion 记录时间

                #region 记录

                RecordHoject(par, result, NameClass, "RegionReduced", ho_RegionReduced);
                RecordHoject(par, result, NameClass, "RegionInner", ho_RegionInner);
                RecordHoject(par, result, NameClass, "RegionOuter", ho_RegionOuter);
                RecordHoject(par, result, NameClass, "RegionInnerOuter", ho_RegionInnerOuter);

                RecordHoject(par, result, NameClass, "RegionIntersected", ho_RegionIntersected);
                RecordHoject(par, result, NameClass, "RegionConnected", ho_RegionConnected);
                RecordHoject(par, result, NameClass, "RegionFillUp", ho_RegionFillUp);

                RecordHoject(par, result, NameClass, "RegionOpening", ho_RegionOpening);
                RecordHoject(par, result, NameClass, "RegionClosing", ho_RegionClosing);
                RecordHoject(par, result, NameClass, "RegionWidthSelected", ho_RegionWidthSelected);

                RecordHoject(par, result, NameClass, "RegionHeightSelected", ho_RegionHeightSelected);
                RecordHoject(par, result, NameClass, "RegionAreaSelected", ho_RegionAreaSelected);
                RecordHoject(par, result, NameClass, "RegionUnion1", ho_RegionUnion1);

                RecordHoject(par, result, NameClass, "RegionUnion2", ho_RegionUnion2);
                RecordHoject(par, result, NameClass, "RegionFinShell", ho_RegionFinShell);
                #endregion 记录

                ho_xldToCalHeight.Dispose();
                ho_xldToCalWidth.Dispose();
                ho_ho_RegionFinShellSelect.Dispose();
                ho_ho_RegionFinShellSelectContours.Dispose();
            }
        }

        /// <summary>
        /// 根据输入的轮廓生成相应的平行轮廓区域
        /// </summary>
        /// <param name="ho_ParalXld">输入的轮廓</param>
        /// <param name="ho_RegionOuter">加上偏移值的外轮廓</param>
        /// <param name="ho_RegionInnerOuter">加上偏移值的内外轮廓</param>
        /// <param name="Width">轮廓宽度</param>
        /// <param name="OuterStdShift">外轮廓偏移值</param>
        /// <param name="InnerStdShift">内轮廓偏移值</param>
        void GenParalRegionFromXld(HObject ho_ParalXld, out HObject ho_RegionOuter, out HObject ho_RegionInnerOuter, out HObject ho_RegionInner, int Width, double OuterStdShift, double InnerStdShift)
        {
            #region 定义
            ho_RegionOuter = null;
            ho_RegionInnerOuter = null;
            ho_RegionInner = null;

            HObject ho_ContourSorted = null;
            HObject ho_ContourOuter = null;
            HObject ho_ContourOuterShift = null;
            HObject ho_ContourInner = null;
            HObject ho_ContourInnerShift = null;

            HObject ho_DiffOuter = null;
            HObject ho_DiffInnerOuter = null;
            HObject ho_DiffInner = null;

            HTuple hv_Row = null;
            HTuple hv_Col = null;
            HTuple hv_RowInverse = null;
            HTuple hv_ColInverse = null;

            HTuple hv_RowOuter = null;
            HTuple hv_ColOuter = null;
            HTuple hv_RowOuterShift = null;
            HTuple hv_ColOuterShift = null;
            HTuple hv_RowInner = null;
            HTuple hv_ColInner = null;
            HTuple hv_RowInnerShift = null;
            HTuple hv_ColInnerShift = null;

            HTuple hv_RowOuterInverse = null;
            HTuple hv_ColOuterInverse = null;
            HTuple hv_RowInnerInverse = null;
            HTuple hv_ColInnerInverse = null;

            HTuple hv_RowOuterConcat = null;
            HTuple hv_RowInnerOuterConcat = null;
            HTuple hv_RowInnerConcat = null;
            HTuple hv_ColOuterConcat = null;
            HTuple hv_ColInnerOuterConcat = null;
            HTuple hv_ColInnerConcat = null;

            HOperatorSet.GenEmptyObj(out ho_ContourSorted);
            HOperatorSet.GenEmptyObj(out ho_ContourOuter);
            HOperatorSet.GenEmptyObj(out ho_ContourOuterShift);
            HOperatorSet.GenEmptyObj(out ho_ContourInner);
            HOperatorSet.GenEmptyObj(out ho_ContourInnerShift);

            HOperatorSet.GenEmptyObj(out ho_DiffOuter);
            HOperatorSet.GenEmptyObj(out ho_DiffInnerOuter);
            HOperatorSet.GenEmptyObj(out ho_DiffInner);

            #endregion 定义

            try
            {
                HOperatorSet.GetContourXld(ho_ParalXld, out hv_Row, out hv_Col);
                int length = hv_Row.Length;

                if (hv_Col[0] < hv_Col[length - 1])
                {
                    HOperatorSet.TupleInverse(hv_Row, out hv_RowInverse);
                    HOperatorSet.TupleInverse(hv_Col, out hv_ColInverse);
                    hv_Row = hv_RowInverse;
                    hv_Col = hv_ColInverse;
                }

                HOperatorSet.GenContourPolygonXld(out ho_ContourSorted, hv_Row, hv_Col);

                //生成平行轮廓
                //Width_Paral--区域的宽度
                //OuterStdShift_Paral--外部求取区域的偏移
                //InnerStdShift_Paral--内部求取区域的偏移
                HOperatorSet.GenParallelContourXld(ho_ContourSorted, out ho_ContourOuter, "regression_normal", Width + OuterStdShift);
                HOperatorSet.GenParallelContourXld(ho_ContourSorted, out ho_ContourOuterShift, "regression_normal", OuterStdShift);
                HOperatorSet.GenParallelContourXld(ho_ContourSorted, out ho_ContourInner, "regression_normal", -Width - InnerStdShift);
                HOperatorSet.GenParallelContourXld(ho_ContourSorted, out ho_ContourInnerShift, "regression_normal", -InnerStdShift);

                //XLD-Arry
                HOperatorSet.GetContourXld(ho_ContourOuter, out hv_RowOuter, out hv_ColOuter);
                HOperatorSet.GetContourXld(ho_ContourOuterShift, out hv_RowOuterShift, out hv_ColOuterShift);
                HOperatorSet.GetContourXld(ho_ContourInner, out hv_RowInner, out hv_ColInner);
                HOperatorSet.GetContourXld(ho_ContourInnerShift, out hv_RowInnerShift, out hv_ColInnerShift);

                //Inverse
                HOperatorSet.TupleInverse(hv_RowOuter, out hv_RowOuterInverse);
                HOperatorSet.TupleInverse(hv_ColOuter, out hv_ColOuterInverse);
                HOperatorSet.TupleInverse(hv_RowInner, out hv_RowInnerInverse);
                HOperatorSet.TupleInverse(hv_ColInner, out hv_ColInnerInverse);

                //concat
                HOperatorSet.TupleConcat(hv_RowOuterInverse, hv_RowOuterShift, out hv_RowOuterConcat);
                HOperatorSet.TupleConcat(hv_ColOuterInverse, hv_ColOuterShift, out hv_ColOuterConcat);
                HOperatorSet.TupleConcat(hv_RowInnerInverse, hv_RowInnerShift, out hv_RowInnerConcat);
                HOperatorSet.TupleConcat(hv_ColInnerInverse, hv_ColInnerShift, out hv_ColInnerConcat);
                HOperatorSet.TupleConcat(hv_RowOuterInverse, hv_RowInner, out hv_RowInnerOuterConcat);
                HOperatorSet.TupleConcat(hv_ColOuterInverse, hv_ColInner, out hv_ColInnerOuterConcat);

                HOperatorSet.GenContourPolygonXld(out ho_DiffOuter, hv_RowOuterConcat, hv_ColOuterConcat);
                HOperatorSet.GenContourPolygonXld(out ho_DiffInnerOuter, hv_RowInnerOuterConcat, hv_ColInnerOuterConcat);
                HOperatorSet.GenContourPolygonXld(out ho_DiffInner, hv_RowInnerConcat, hv_ColInnerConcat);

                HOperatorSet.GenRegionContourXld(ho_DiffOuter, out ho_RegionOuter, "filled");
                HOperatorSet.GenRegionContourXld(ho_DiffInnerOuter, out ho_RegionInnerOuter, "filled");
                HOperatorSet.GenRegionContourXld(ho_DiffInner, out ho_RegionInner, "filled");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                ho_ContourSorted.Dispose();
                ho_ContourOuter.Dispose();
                ho_ContourOuterShift.Dispose();
                ho_ContourInner.Dispose();
                ho_ContourInnerShift.Dispose();

                ho_DiffOuter.Dispose();
                ho_DiffInnerOuter.Dispose();
                ho_DiffInner.Dispose();
            }
        }

    }
}
