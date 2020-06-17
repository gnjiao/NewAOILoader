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
    public partial class FunRaisedEdge : FunPreprocess
    {
        #region 初始化
        public FunRaisedEdge()
        {
            NameClass = "FunRaisedEdge";
        }
        #endregion 初始化

        public ResultRaisedEdge DealRaisedEdge(ParRaisedEdgeSmooth parRaisedEdgeSmooth, Hashtable htResult)
        {
            ParRaisedEdge par = parRaisedEdgeSmooth.g_ParRaisedEdge;
            
            #region 定义
            double Amp = 0;
            //正常检测阈值
            double NormalInspectTh = 10;

            ImageAll imageAll = null;
            ResultPreProcess resultPreProcess = null;

            HTuple num_Obj = 0;
            ResultRaisedEdge result = new ResultRaisedEdge();

            HTuple width = null;
            HTuple height = null;

            HObject ho_Image = null;
            HObject ho_RegionReduced = null;
            HObject ho_binaryArea = null;
            HObject ho_binaryAreaUnion = null;
            HObject ho_binaryImage = null;

            HObject ho_binaryAreaUp = null;
            HObject ho_binaryImageUp = null;
            HObject ho_binaryAreaDown = null;
            HObject ho_binaryImageDown = null;

            HObject ho_RegionFinShell = null;

            ////////****************////////
            HObject ho_ThROI = null;
            HObject ho_CircleImageReduced = null;
            HObject ho_CircleImageReducedOpening = null;
            HObject ho_CircleImageReducedOpeningBin = null;
            HObject ho_CircleEdges = null;
            HObject ho_ContoursSplit = null;

            HObject ho_UnionContours = null;
            HObject ho_LongestContour = null;
            HObject ho_ContourA = null;
            HObject ho_TriRegion = null, ho_CircleRegion = null, ho_CircleRegionComplement = null;
            HObject ho_CircleRegionOuterLimit = null, ho_CircleRegionInnerLimit = null;
            HObject ho_TriCirIntersectRegion = null;
            HObject ho_TriCirReduced = null, ho_TriCirReducedThRegion = null, ho_TriCirReducedContours = null;
            HObject ho_TriCirRegionConnect1 = null,ho_TriCirRegionIntersection1 = null;
            HObject ho_TriCirRegionConnect2 = null, ho_TriCirRegionIntersection2 = null;
            HObject ho_TriCirBiggested1 = null;
            HObject ho_TriCirBiggested2 = null;
            HObject ho_InterestROI1 = null;
            HObject ho_InterestROI2 = null;
            HObject ho_TriCirRegionSelect = null;
            //ROI区域分上下求
            HObject ho_ROIUp = null, ho_ROIDown = null;
            HObject ho_ROIUpReduce = null, ho_ROIDownReduce = null;
            //封闭C角的M直线四边形
            HObject ho_MLineQuadRegion = null;
            HObject ho_UpQuadPaintImage = null;

            HObject ho_TriCirReducedContoursSelect = null;

            HObject ho_CcutRegionReduceXld = null, ho_CcutRegionReduceRegion;
            HObject ho_CcutRegionReduceImage = null;
            

            HTuple hv_Length = null;
            HTuple hv_RowCir = null, hv_ColumnCir = null, hv_RadiusCir = null;
            HTuple hv_StartPhi = null, hv_EndPhi = null, hv_PointOrder = null;
            HTuple hv_IntersectionP1Row = null, hv_IntersectionP1Col = null;
            HTuple hv_IntersectionP2Row = null, hv_IntersectionP2Col = null;

            HTuple hv_IntersectionCenterP1Row = null, hv_IntersectionCenterP1Col = null;
            HTuple hv_IntersectionCenterP2Row = null, hv_IntersectionCenterP2Col = null;

            HTuple hv_IsOverlapping1=null, hv_IsOverlapping2 = null;
            HTuple hv_RowC1 = null, hv_ColumnC1 = null, hv_RowC2 = null, hv_ColumnC2 = null;

            HTuple hv_TriCirRegionArea = null,hv_TriCirRegionRow= null,hv_TriCirRegionColumn = null;
            HTuple hv_IsSubset = null;

            HTuple hv_SegmentCirCrossRow1 = null, hv_SegmentCirCrossCol1 = null, hv_SegmentCirCrossLength1=null;
            HTuple hv_SegmentCirCrossRow2 = null, hv_SegmentCirCrossCol2 = null, hv_SegmentCirCrossLength2 = null;

            HTuple hv_TriPointRow1 = new HTuple(), hv_TriPointCol1 = new HTuple();
            HTuple hv_TriPointRow2 = new HTuple(), hv_TriPointCol2 = new HTuple();

            HTuple hv_rowA = null, hv_colA = null;
            int InterestROI1Index = -1;
            int InterestROI2Index = -1;

            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_RegionReduced);
            HOperatorSet.GenEmptyObj(out ho_binaryArea);
            HOperatorSet.GenEmptyObj(out ho_binaryAreaUnion);
            HOperatorSet.GenEmptyObj(out ho_binaryImage);

            HOperatorSet.GenEmptyObj(out ho_binaryAreaUp);
            HOperatorSet.GenEmptyObj(out ho_binaryImageUp);

            HOperatorSet.GenEmptyObj(out ho_binaryAreaDown);
            HOperatorSet.GenEmptyObj(out ho_binaryImageDown);

            HOperatorSet.GenEmptyObj(out ho_ThROI);
            HOperatorSet.GenEmptyObj(out ho_CircleImageReduced);
            HOperatorSet.GenEmptyObj(out ho_CircleEdges);
            HOperatorSet.GenEmptyObj(out ho_ContoursSplit);
            HOperatorSet.GenEmptyObj(out ho_UnionContours);
            HOperatorSet.GenEmptyObj(out ho_LongestContour);
            HOperatorSet.GenEmptyObj(out ho_ContourA);
            HOperatorSet.GenEmptyObj(out ho_TriRegion);
            HOperatorSet.GenEmptyObj(out ho_CircleRegion);
            HOperatorSet.GenEmptyObj(out ho_CircleRegionComplement);

            HOperatorSet.GenEmptyObj(out ho_TriCirIntersectRegion);
            HOperatorSet.GenEmptyObj(out ho_TriCirReduced);
            HOperatorSet.GenEmptyObj(out ho_TriCirReducedThRegion);
            HOperatorSet.GenEmptyObj(out ho_TriCirReducedContours);
            HOperatorSet.GenEmptyObj(out ho_TriCirReducedContoursSelect);

            HOperatorSet.GenEmptyObj(out ho_TriCirRegionConnect1);
            HOperatorSet.GenEmptyObj(out ho_TriCirRegionIntersection1);
            HOperatorSet.GenEmptyObj(out ho_TriCirBiggested1);

            HOperatorSet.GenEmptyObj(out ho_TriCirRegionConnect2);
            HOperatorSet.GenEmptyObj(out ho_TriCirRegionIntersection2);
            HOperatorSet.GenEmptyObj(out ho_TriCirBiggested2);
            HOperatorSet.GenEmptyObj(out ho_TriCirRegionSelect);

            HOperatorSet.GenEmptyObj(out ho_CircleRegionOuterLimit);
            HOperatorSet.GenEmptyObj(out ho_CircleRegionInnerLimit);

            HOperatorSet.GenEmptyObj(out ho_CircleImageReducedOpening);
            HOperatorSet.GenEmptyObj(out ho_CircleImageReducedOpeningBin);

            HOperatorSet.GenEmptyObj(out ho_InterestROI1);
            HOperatorSet.GenEmptyObj(out ho_InterestROI2);

            HOperatorSet.GenEmptyObj(out ho_CcutRegionReduceXld);
            HOperatorSet.GenEmptyObj(out ho_CcutRegionReduceRegion);
            HOperatorSet.GenEmptyObj(out ho_CcutRegionReduceImage);


            
            HOperatorSet.GenEmptyObj(out ho_ROIUp);
            HOperatorSet.GenEmptyObj(out ho_ROIDown);
            HOperatorSet.GenEmptyObj(out ho_ROIUpReduce);
            HOperatorSet.GenEmptyObj(out ho_ROIDownReduce);
            HOperatorSet.GenEmptyObj(out ho_MLineQuadRegion);
            HOperatorSet.GenEmptyObj(out ho_UpQuadPaintImage);

            #endregion 定义

            try
            {
                
                #region 基础功能调用
                //if (BasicImageProcess(parRaisedEdgeSmooth, result, htResult, out ho_RegionReduced, out ho_Image, out width, out height))
                //{
                    
                //}
                //else
                //{
                //    return result;
                //}

                #region Record
                stopWatch.Restart();
                #endregion Record

                //删除前一次的记录   
                DeleteImage(NameClass, parRaisedEdgeSmooth);

                //设置基础属性
                SetBasicAttribute(parRaisedEdgeSmooth, result);

                #region 从Hash表中获取图像
                string nameCellInput = "";
                string nameImageInput = "";
                imageAll = GetHtImage(parRaisedEdgeSmooth, htResult, out nameCellInput, out nameImageInput, out width, out height);
                result.NameCellInput = nameCellInput;
                result.NameImageInput = nameImageInput;
                if (imageAll == null)
                {
                    result.LevelError_e = LevelError_enum.Error;
                    result.TypeErrorProcess_e = TypeErrorProcess_enum.CameraImageError;
                    result.Annotation = parRaisedEdgeSmooth.NameCell.ToString() + parRaisedEdgeSmooth.Annotation + "失败";
                    result.SetDefault();//赋值默认值
                    return result;
                }
                ho_Image = imageAll.Ho_Image;
                result.XImage = width;
                result.YImage = height;
                #endregion 从Hash表中获取图像

                #region 图像预处理
                string nameCellPolygon1 = par.NameCellPolygon1; // 计算M直线交点的单元格
                ResultCrossLines resultCrossLine1 = (ResultCrossLines)htResult[nameCellPolygon1]; //得到直线交点


                


                //    HTuple hv_CcutRegionReduceRow = null, hv_CcutRegionReduceCol = null;

                //    hv_CcutRegionReduceRow = new HTuple();
                //    hv_CcutRegionReduceRow = hv_CcutRegionReduceRow.TupleConcat(TriPointY1);
                //    hv_CcutRegionReduceRow = hv_CcutRegionReduceRow.TupleConcat(height);
                //    hv_CcutRegionReduceRow = hv_CcutRegionReduceRow.TupleConcat(TriPointY2);
                //    hv_CcutRegionReduceRow = hv_CcutRegionReduceRow.TupleConcat(resultCrossLine1.Y);

                //    hv_CcutRegionReduceCol = new HTuple();
                //    hv_CcutRegionReduceCol = hv_CcutRegionReduceCol.TupleConcat(TriPointX1);
                //    hv_CcutRegionReduceCol = hv_CcutRegionReduceCol.TupleConcat(width);
                //    hv_CcutRegionReduceCol = hv_CcutRegionReduceCol.TupleConcat(TriPointX2);
                //    hv_CcutRegionReduceCol = hv_CcutRegionReduceCol.TupleConcat(resultCrossLine1.X);
                //    ho_ContourA.Dispose();
                //    ho_CcutRegionReduceXld.Dispose();
                //    HOperatorSet.GenContourPolygonXld(out ho_CcutRegionReduceXld, hv_CcutRegionReduceRow, hv_CcutRegionReduceCol);
                //    ho_CcutRegionReduceRegion.Dispose();
                //    HOperatorSet.GenRegionContourXld(ho_CcutRegionReduceXld, out ho_CcutRegionReduceRegion, "filled");
                //    ho_CcutRegionReduceImage.Dispose();
                //    HOperatorSet.ReduceDomain(ho_Image, ho_CcutRegionReduceRegion, out ho_CcutRegionReduceImage);
                //    //进行二值化的图像就是经过区域削减后的图像
                //    //imageAll.Ho_Image = ho_CcutRegionReduceImage.Clone();
                //    ImageAll imageAllReduced = new ImageAll();
                //    imageAllReduced.Ho_Image = ho_CcutRegionReduceImage;
                //    imageAllReduced.BitmapSource = null;
                //    resultPreProcess = DealPreprocess(parRaisedEdgeSmooth, parRaisedEdgeSmooth.g_ParPreprocess, imageAllReduced, ho_Image, htResult);
                //}
                //else
                //{
                    //resultPreProcess = DealPreprocess(parRaisedEdgeSmooth, parRaisedEdgeSmooth.g_ParPreprocess, imageAll, ho_Image, htResult);
                //}
                //获取拟合圆形的ROI
                bool blResult = false;
                string anno = "";
                if (par.DefectType == "Fin" && par.OutlineType == 0) // 求C角的残留
                {
                    double TriPointX1 = 0, TriPointY1 = 0, TriPointX2 = 0, TriPointY2 = 0;
                    double TriPointX3 = 0, TriPointY3 = 0, TriPointX4 = 0, TriPointY4 = 0;
                    HTuple dist1 = null;
                    HTuple dist2 = null;
                    //找到M直线交点中，第一条直线两端点和交点距离较远的点
                    HOperatorSet.DistancePp(resultCrossLine1.YBegin_L[0], resultCrossLine1.XBegin_L[0], resultCrossLine1.Y, resultCrossLine1.X, out dist1);
                    HOperatorSet.DistancePp(resultCrossLine1.YEnd_L[0], resultCrossLine1.XEnd_L[0], resultCrossLine1.Y, resultCrossLine1.X, out dist2);
                    if (dist1 > dist2)
                    {
                        TriPointX1 = resultCrossLine1.XBegin_L[0];
                        TriPointY1 = resultCrossLine1.YBegin_L[0];
                        TriPointX4 = resultCrossLine1.XEnd_L[0];
                        TriPointY4 = resultCrossLine1.YEnd_L[0];
                    }
                    else
                    {
                        TriPointX1 = resultCrossLine1.XEnd_L[0];
                        TriPointY1 = resultCrossLine1.YEnd_L[0];
                        TriPointX4 = resultCrossLine1.XBegin_L[0];
                        TriPointY4 = resultCrossLine1.YBegin_L[0];
                    }
                    //找到M直线交点中，第二条直线两端点和交点距离较远的点
                    HOperatorSet.DistancePp(resultCrossLine1.YBegin_L[1], resultCrossLine1.XBegin_L[1], resultCrossLine1.Y, resultCrossLine1.X, out dist1);
                    HOperatorSet.DistancePp(resultCrossLine1.YEnd_L[1], resultCrossLine1.XEnd_L[1], resultCrossLine1.Y, resultCrossLine1.X, out dist2);
                    if (dist1 > dist2)
                    {
                        TriPointX2 = resultCrossLine1.XBegin_L[1];
                        TriPointY2 = resultCrossLine1.YBegin_L[1];
                        TriPointX3 = resultCrossLine1.XEnd_L[1];
                        TriPointY3 = resultCrossLine1.YEnd_L[1];
                    }
                    else
                    {
                        TriPointX2 = resultCrossLine1.XEnd_L[1];
                        TriPointY2 = resultCrossLine1.YEnd_L[1];
                        TriPointX3 = resultCrossLine1.XBegin_L[1];
                        TriPointY3 = resultCrossLine1.YBegin_L[1];
                    }
                    //生成一个四边形选区，将图像中的这个四边形选区涂黑，保证图像一端的封闭性
                        HTuple hv_CcutMLineQuadRow = null, hv_CcutMLineQuadCol = null;

                    hv_CcutMLineQuadRow = new HTuple();
                    hv_CcutMLineQuadRow = hv_CcutMLineQuadRow.TupleConcat(TriPointY1);
                    hv_CcutMLineQuadRow = hv_CcutMLineQuadRow.TupleConcat(TriPointY2);
                    hv_CcutMLineQuadRow = hv_CcutMLineQuadRow.TupleConcat(TriPointY3);
                    hv_CcutMLineQuadRow = hv_CcutMLineQuadRow.TupleConcat(TriPointY4);
                    hv_CcutMLineQuadCol = new HTuple();
                    hv_CcutMLineQuadCol = hv_CcutMLineQuadCol.TupleConcat(TriPointX1);
                    hv_CcutMLineQuadCol = hv_CcutMLineQuadCol.TupleConcat(TriPointX2);
                    hv_CcutMLineQuadCol = hv_CcutMLineQuadCol.TupleConcat(TriPointX3);
                    hv_CcutMLineQuadCol = hv_CcutMLineQuadCol.TupleConcat(TriPointX4);

                    ho_MLineQuadRegion.Dispose();
                    HOperatorSet.GenRegionPolygonFilled(out ho_MLineQuadRegion, hv_CcutMLineQuadRow, hv_CcutMLineQuadCol);

                    //C角残留求取时，ROI区域按照smooth的区域所画，分成上下两个ROI单独进行FunBinary

                    FunROI funThROI = new FunROI();
                    ho_ThROI = funThROI.CreateOneROI(parRaisedEdgeSmooth.g_ParROI.g_ParROIExcute_L[0], htResult, out blResult, out anno);
                    HTuple hv_ROIRow1 = null, hv_ROIRow2 = null, hv_ROICol1 = null, hv_ROICol2 = null;

                    HOperatorSet.SmallestRectangle1(ho_ThROI, out hv_ROIRow1, out hv_ROICol1, out hv_ROIRow2, out hv_ROICol2);

                    HTuple hv_ROICenterRow = (hv_ROIRow1 + hv_ROIRow2) / 2;

                    ho_ROIUp.Dispose();
                    double upDwonRoiAdd = 15;
                    double LeftRightRoiAdd = 15;
                    HOperatorSet.GenRectangle1(out ho_ROIUp, hv_ROIRow1 - upDwonRoiAdd, hv_ROICol1 - LeftRightRoiAdd, hv_ROICenterRow + 20, hv_ROICol2 + LeftRightRoiAdd);
                    ho_ROIDown.Dispose();
                    HOperatorSet.GenRectangle1(out ho_ROIDown, hv_ROICenterRow - 20, hv_ROICol1 - LeftRightRoiAdd, hv_ROIRow2 + 50, hv_ROICol2 + LeftRightRoiAdd);
                    //将M直线生成的四边形区域填充至C角区域
                    ho_UpQuadPaintImage.Dispose();
                    HOperatorSet.PaintRegion(ho_MLineQuadRegion, ho_Image, out ho_UpQuadPaintImage, 0, "fill");

                    ho_ROIUpReduce.Dispose();
                    HOperatorSet.ReduceDomain(ho_UpQuadPaintImage, ho_ROIUp, out ho_ROIUpReduce);

                    ho_ROIDownReduce.Dispose();
                    HOperatorSet.ReduceDomain(ho_Image, ho_ROIDown, out ho_ROIDownReduce);

                    ImageAll imageAllUpReduced = new ImageAll();
                    imageAllUpReduced.Ho_Image = ho_ROIUpReduce;
                    imageAllUpReduced.BitmapSource = null;
                    //进行上半部分ROI的处理
                    //resultPreProcess = DealPreprocess(parRaisedEdgeSmooth, parRaisedEdgeSmooth.g_ParPreprocess, imageAllUpReduced, ho_Image, htResult);

                    ImageAll imageAllDownReduced = new ImageAll();
                    imageAllDownReduced.Ho_Image = ho_ROIDownReduce;
                    imageAllDownReduced.BitmapSource = null;
                    //进行下半部分的ROI处理
                    ResultBinary resultDownThRegion = DealBinary(ho_ROIDown, imageAllDownReduced);
                    if (resultDownThRegion.LevelError_e == LevelError_enum.Error)
                    {
                        result.LevelError_e = LevelError_enum.Error;
                        result.Annotation = parRaisedEdgeSmooth.NameCell.ToString() + parRaisedEdgeSmooth.Annotation + "下半部分预处理结果异常";
                        result.SetDefault();//赋值默认值
                        return result;
                    }


                    if (resultPreProcess == null)
                    {
                        result.LevelError_e = LevelError_enum.Error;
                        result.Annotation = parRaisedEdgeSmooth.NameCell.ToString() + parRaisedEdgeSmooth.Annotation + "上半部分预处理结果位Null";
                        result.SetDefault();//赋值默认值
                        return result;
                    }

                    //将预处理的中间过程赋值给结果类
                    foreach (string item in resultPreProcess.HtResultImage.Keys)
                    {
                        result.HtResultImage.Add(item, resultPreProcess.HtResultImage[item]);
                    }

                    //预处理警告
                    if (resultPreProcess.LevelError_e == LevelError_enum.Alarm)
                    {
                        result.LevelError_e = LevelError_enum.Alarm;
                        result.TypeErrorProcess_e = resultPreProcess.TypeErrorProcess_e;
                        result.Annotation = par.NameCell.ToString() + par.Annotation + "上半部分预处理警告:" + resultPreProcess.Annotation;
                    }

                    //赋值预处理结果类
                    result.g_ResultPreProcess = resultPreProcess;

                    //预处理不正确
                    if (!resultPreProcess.BlResult)
                    {
                        result.LevelError_e = LevelError_enum.Error;
                        result.TypeErrorProcess_e = resultPreProcess.TypeErrorProcess_e;
                        result.Annotation = parRaisedEdgeSmooth.NameCell.ToString() + parRaisedEdgeSmooth.Annotation + "上半部分预处理失败:" + resultPreProcess.Annotation;

                        result.SetDefault();//赋值默认值
                        return result;
                    }

                    //获取图像预处理结果，图像是二值化后再进行处理
                    //这个是上半部分结果
                    ho_binaryImageUp = result.g_ResultPreProcess.ImageResult.Ho_Image.Clone();
                    ho_binaryAreaUp = result.g_ResultPreProcess.resultBinary.RegionResult.Ho_Image.Clone();

                    ho_binaryImageDown = resultDownThRegion.BinaryResult.Ho_Image.Clone();
                    ho_binaryAreaDown = resultDownThRegion.RegionResult.Ho_Image.Clone();

                    ho_binaryAreaUnion.Dispose();
                    HOperatorSet.Union2(ho_binaryAreaUp, ho_binaryAreaDown, out ho_binaryAreaUnion);

                    ho_binaryArea.Dispose();
                    HOperatorSet.Connection(ho_binaryAreaUnion, out ho_binaryArea);

                    ho_binaryImage.Dispose();
                    HOperatorSet.RegionToBin(ho_binaryArea, out ho_binaryImage, 255, 0, width, height);

                }
                else
                {
                    //resultPreProcess = DealPreprocess(parRaisedEdgeSmooth, parRaisedEdgeSmooth.g_ParPreprocess, imageAll, ho_Image, htResult);

                    if (resultPreProcess == null)
                    {
                        result.LevelError_e = LevelError_enum.Error;
                        result.Annotation = parRaisedEdgeSmooth.NameCell.ToString() + parRaisedEdgeSmooth.Annotation + "预处理结果位Null";
                        result.SetDefault();//赋值默认值
                        return result;
                    }

                    //将预处理的中间过程赋值给结果类
                    foreach (string item in resultPreProcess.HtResultImage.Keys)
                    {
                        result.HtResultImage.Add(item, resultPreProcess.HtResultImage[item]);
                    }

                    //预处理警告
                    if (resultPreProcess.LevelError_e == LevelError_enum.Alarm)
                    {
                        result.LevelError_e = LevelError_enum.Alarm;
                        result.TypeErrorProcess_e = resultPreProcess.TypeErrorProcess_e;
                        result.Annotation = par.NameCell.ToString() + par.Annotation + "预处理警告:" + resultPreProcess.Annotation;
                    }

                    //赋值预处理结果类
                    result.g_ResultPreProcess = resultPreProcess;

                    //预处理不正确
                    if (!resultPreProcess.BlResult)
                    {
                        result.LevelError_e = LevelError_enum.Error;
                        result.TypeErrorProcess_e = resultPreProcess.TypeErrorProcess_e;
                        result.Annotation = parRaisedEdgeSmooth.NameCell.ToString() + parRaisedEdgeSmooth.Annotation + "预处理失败:" + resultPreProcess.Annotation;

                        result.SetDefault();//赋值默认值
                        return result;
                    }
                    //获取图像预处理结果，图像是二值化后再进行处理
                    ho_binaryImage = result.g_ResultPreProcess.ImageResult.Ho_Image.Clone();
                    ho_binaryArea = result.g_ResultPreProcess.resultBinary.RegionResult.Ho_Image.Clone();
                }

                #endregion 图像预处理
                #endregion 基础功能调用

                //C0行插入的是AMP
                ResultCalibWorld calibPar = (ResultCalibWorld)htResult["C0"];
                Amp = calibPar.AmpXY;

                ResultParProduct resultParProduct = (ResultParProduct)htResult["C195"]; //得到配置参数
                RefCirclePar refCirclePar1 = new RefCirclePar();
                RefCirclePar refCirclePar2 = new RefCirclePar();
                InnerCutPar innerCutPar = new InnerCutPar();

                //正常检测区域阈值
                NormalInspectTh = resultParProduct.ResultReference_L[14-2].Value / Amp;

                //卡控园半径
                double tol = resultParProduct.ResultReference_L[15-2].Value;

                switch (par.OutlineType)
                {
                    case 0://c角
                        refCirclePar1.RefRadius = resultParProduct.ResultReference_L[0].Value / Amp;
                        refCirclePar2.RefRadius = 0;  //C角只判断1个圆
                        innerCutPar.InnerCutH = resultParProduct.ResultReference_L[2-1].Value / Amp;
                        innerCutPar.InnerCutV = resultParProduct.ResultReference_L[3-1].Value / Amp;
                        break;
                    case 1://r角
                        refCirclePar1.RefRadius = resultParProduct.ResultReference_L[5-1].Value / Amp;
                        refCirclePar2.RefRadius = 0; //R角只判断1个圆
                        innerCutPar.InnerCutH = resultParProduct.ResultReference_L[7-2].Value / Amp;
                        innerCutPar.InnerCutV = resultParProduct.ResultReference_L[8-2].Value / Amp;
                        break;
                    case 2://u角内
                        refCirclePar1.RefRadius = resultParProduct.ResultReference_L[10-2].Value / Amp;
                        break;
                    case 3://U角外
                        refCirclePar2.RefRadius = resultParProduct.ResultReference_L[11-2].Value / Amp;
                        innerCutPar.InnerCutH = resultParProduct.ResultReference_L[12-2].Value / Amp;
                        break;
                }

                if (par.DefectType == "Shell") // 求崩缺，不用进行圆拟合计算，直接进finally进行平滑轮廓计算
                {
                    return result;
                }

                


                //if (par.OutlineType == 1) // 切角为圆，获得真实的残留检测选区（斜边为圆的三角圆）
                //{
                    #region 拟合圆
                    //获取拟合圆形的ROI
                    FunROI funCirROI = new FunROI();
                    ho_ThROI = funCirROI.CreateOneROI(parRaisedEdgeSmooth.g_ParROI.g_ParROIExcute_L[1], htResult, out blResult, out anno);

                    HOperatorSet.ErosionCircle(ho_ThROI, out ho_ThROI, 15);


                    ho_CircleImageReducedOpening.Dispose();
                    HOperatorSet.OpeningCircle(ho_binaryArea, out ho_CircleImageReducedOpening, 10);
                    ho_CircleImageReducedOpeningBin.Dispose();
                    HOperatorSet.RegionToBin(ho_CircleImageReducedOpening, out ho_CircleImageReducedOpeningBin, 255, 0, width, height);
                    HOperatorSet.ReduceDomain(ho_CircleImageReducedOpeningBin, ho_ThROI, out ho_CircleImageReduced);
///
                    //
                    //添加拟合圆的操作
                    //HOperatorSet.EdgesSubPix(ho_CircleImageReduced, out ho_CircleEdges, "canny", 3, 40, 60);
                    HOperatorSet.ThresholdSubPix(ho_CircleImageReduced, out ho_CircleEdges, 50);
                   
                    //ho_ContoursSplit.Dispose(); //进行segment容易找出现圆识别异常
                    //HOperatorSet.SegmentContoursXld(ho_CircleEdges, out ho_ContoursSplit, "lines_circles",
                    //    5, 4, 2);
                    ho_UnionContours.Dispose();

                    

                    HOperatorSet.UnionCocircularContoursXld(ho_CircleEdges, out ho_UnionContours,
                        0.9, 0.5, 0.5, 200, 50, 50, "true", 1);

////
                    HOperatorSet.LengthXld(ho_UnionContours, out hv_Length);
                    ho_LongestContour.Dispose();
                    HOperatorSet.SelectObj(ho_UnionContours, out ho_LongestContour, (((hv_Length.TupleSortIndex()
                        )).TupleSelect((new HTuple(hv_Length.TupleLength())) - 1)) + 1);

                    HOperatorSet.FitCircleContourXld(ho_LongestContour, "ahuber", -1, 0, 0, 3, 2,
                    out hv_RowCir, out hv_ColumnCir, out hv_RadiusCir, out hv_StartPhi, out hv_EndPhi,
                    out hv_PointOrder);
                    

                    #endregion 拟合圆    
                    
                    //如果拟合出的圆半径均不在给定的两个范围内，则认为拟合出错，即NG
                    bool isCirleFitNg = false;
                    if (par.OutlineType == 0 || par.OutlineType == 1) // C角和R角只拟合一个圆，所以只判断一次
                    {
                        isCirleFitNg = hv_RadiusCir.D < refCirclePar1.RefRadius * (1 - tol) || hv_RadiusCir.D > refCirclePar1.RefRadius * (1 + tol);
                    }
                    else //U角拟合两个圆，所以需要判断两次
                    {
                        isCirleFitNg = (hv_RadiusCir.D < refCirclePar1.RefRadius * (1 - tol) || hv_RadiusCir.D > refCirclePar1.RefRadius * (1 + tol))
                        && (hv_RadiusCir.D < refCirclePar2.RefRadius * (1 - tol) || hv_RadiusCir.D > refCirclePar2.RefRadius * (1 + tol));
                    }

                    if (isCirleFitNg)
                    {
                        string setValue =Math.Round(refCirclePar1.RefRadius*Amp,2).ToString();
                        string fitValue =Math.Round(hv_RadiusCir.D*Amp,2).ToString();

                        result.LevelError_e = LevelError_enum.Error;
                        result.TypeErrorProcess_e = TypeErrorProcess_enum.NumZero;
                        result.Annotation = par.NameCell.ToString() + ",拟合圆半径出错，设定值：" + setValue + " 拟合值:" + fitValue;
                        //result.SetDefault();
                        return result;
                    }
                    
                    //计算1.5倍圆和直线的交点
                    HOperatorSet.IntersectionLineCircle(resultCrossLine1.YBegin_L[0], resultCrossLine1.XBegin_L[0], resultCrossLine1.YEnd_L[0], resultCrossLine1.XEnd_L[0],
                    hv_RowCir, hv_ColumnCir, hv_RadiusCir * 1.5, 0, 6.28318, "positive", out hv_IntersectionP1Row,
                    out hv_IntersectionP1Col);
                    if (hv_IntersectionP1Row.Length != 2)
                    {
                        result.LevelError_e = LevelError_enum.Error;
                        result.TypeErrorProcess_e = TypeErrorProcess_enum.NumZero;
                        result.Annotation = par.NameCell.ToString() + ",直线1和切角的特征点求取失败";
                        //result.SetDefault();
                        return result;
                    }
                    HOperatorSet.IntersectionLineCircle(resultCrossLine1.YBegin_L[1], resultCrossLine1.XBegin_L[1], resultCrossLine1.YEnd_L[1], resultCrossLine1.XEnd_L[1],
                    hv_RowCir, hv_ColumnCir, hv_RadiusCir * 1.5, 0, 6.28318, "positive", out hv_IntersectionP2Row,
                    out hv_IntersectionP2Col);
                    if (hv_IntersectionP2Row.Length != 2)
                    {
                        result.LevelError_e = LevelError_enum.Error;
                        result.TypeErrorProcess_e = TypeErrorProcess_enum.NumZero;
                        result.Annotation = par.NameCell.ToString() + ",直线2和切角的特征点求取失败";
                        //result.SetDefault();
                        return result;
                    }


                    
                    //两个交点的中点就是和圆最近的点
                    hv_IntersectionCenterP1Row = ((hv_IntersectionP1Row.TupleSelect(0)) + (hv_IntersectionP1Row.TupleSelect(1))) / 2;
                    hv_IntersectionCenterP1Col = ((hv_IntersectionP1Col.TupleSelect(0)) + (hv_IntersectionP1Col.TupleSelect(1))) / 2;

                    hv_IntersectionCenterP2Row = ((hv_IntersectionP2Row.TupleSelect(0)) + (hv_IntersectionP2Row.TupleSelect(1))) / 2;
                    hv_IntersectionCenterP2Col = ((hv_IntersectionP2Col.TupleSelect(0)) + (hv_IntersectionP2Col.TupleSelect(1))) / 2;

                    //---新加的判断----直线交点---和圆交点的中心----连起来的线段，跟圆是否有交点，如果有，则使用这个交点，否则，使用和圆交点的中点
                    HOperatorSet.IntersectionSegmentCircle(resultCrossLine1.Y, resultCrossLine1.X, hv_IntersectionCenterP1Row,
                        hv_IntersectionCenterP1Col, hv_RowCir, hv_ColumnCir, hv_RadiusCir, 0, 6.28318, "positive",
                        out hv_SegmentCirCrossRow1, out hv_SegmentCirCrossCol1);
                    HOperatorSet.IntersectionSegmentCircle(resultCrossLine1.Y, resultCrossLine1.X, hv_IntersectionCenterP2Row,
                        hv_IntersectionCenterP2Col, hv_RowCir, hv_ColumnCir, hv_RadiusCir, 0, 6.28318,
                        "positive", out hv_SegmentCirCrossRow2, out hv_SegmentCirCrossCol2);

                    HOperatorSet.TupleLength(hv_SegmentCirCrossRow1, out hv_SegmentCirCrossLength1);
                    HOperatorSet.TupleLength(hv_SegmentCirCrossRow2, out hv_SegmentCirCrossLength2);

                    if ((int)(new HTuple(hv_SegmentCirCrossLength1.TupleNotEqual(0))) != 0)
                    {
                        hv_TriPointRow1 = hv_SegmentCirCrossRow1.Clone();
                        hv_TriPointCol1 = hv_SegmentCirCrossCol1.Clone();
                    }
                    else
                    {
                        hv_TriPointRow1 = hv_IntersectionCenterP1Row.Clone();
                        hv_TriPointCol1 = hv_IntersectionCenterP1Col.Clone();
                    }
                    
                    if ((int)(new HTuple(hv_SegmentCirCrossLength2.TupleNotEqual(0))) != 0)
                    {
                        hv_TriPointRow2 = hv_SegmentCirCrossRow2.Clone();
                        hv_TriPointCol2 = hv_SegmentCirCrossCol2.Clone();
                    }
                    else
                    {
                        hv_TriPointRow2 = hv_IntersectionCenterP2Row.Clone();
                        hv_TriPointCol2 = hv_IntersectionCenterP2Col.Clone();
                    }

                    //把1.5倍圆和直线的第一个交点--直线上和圆最近的点--圆心，三个点形成一个xld，然后再生成一个检测区域
                    //四个点形成一个检测区域--加上圆心共四个点
                    hv_rowA = new HTuple();
                    hv_rowA = hv_rowA.TupleConcat(hv_TriPointRow1);
                    hv_rowA = hv_rowA.TupleConcat(hv_RowCir);
                    hv_rowA = hv_rowA.TupleConcat(hv_TriPointRow2);
                    hv_rowA = hv_rowA.TupleConcat(resultCrossLine1.Y);

                    hv_colA = new HTuple();
                    hv_colA = hv_colA.TupleConcat(hv_TriPointCol1);
                    hv_colA = hv_colA.TupleConcat(hv_ColumnCir);
                    hv_colA = hv_colA.TupleConcat(hv_TriPointCol2);
                    hv_colA = hv_colA.TupleConcat(resultCrossLine1.X);
                    ho_ContourA.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_ContourA, hv_rowA, hv_colA);

                    //三角选区，在此区域内进行残留检测
                    ho_TriRegion.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_ContourA, out ho_TriRegion, "filled");

                    //圆形选区，在此选区外进行残留检测
                    //Complement,选区的补集
                    ho_CircleRegion.Dispose();
                    HOperatorSet.GenCircle(out ho_CircleRegion, hv_RowCir, hv_ColumnCir, hv_RadiusCir);
                    ho_CircleRegionOuterLimit.Dispose();
                    HOperatorSet.GenCircle(out ho_CircleRegionOuterLimit, hv_RowCir, hv_ColumnCir, hv_RadiusCir + 50);

                    ho_CircleRegionInnerLimit.Dispose();
                    HOperatorSet.GenCircle(out ho_CircleRegionInnerLimit, hv_RowCir, hv_ColumnCir, hv_RadiusCir - 50);
                    

                    ho_CircleRegionComplement.Dispose();


                    if (par.DefectType == "Fin") // 残留求的是圆外的部分
                    {
                        HOperatorSet.Complement(ho_CircleRegion, out ho_CircleRegionComplement);
                        HOperatorSet.Intersection(ho_CircleRegionComplement, ho_CircleRegionOuterLimit, out ho_CircleRegionComplement);
                    }
                    else //崩缺求的是圆内的部分
                    {
                        HOperatorSet.Complement(ho_CircleRegionInnerLimit, out ho_CircleRegionComplement);
                        HOperatorSet.Intersection(ho_CircleRegionComplement, ho_CircleRegion, out ho_CircleRegionComplement);
                    
                        //ho_CircleRegionComplement = ho_CircleRegion.Clone();
                    }

                    //生成残留检测选区
                    ho_TriCirIntersectRegion.Dispose();
                    HOperatorSet.Intersection(ho_CircleRegionComplement, ho_TriRegion, out ho_TriCirIntersectRegion);

                    #region 切角为三角形
                    //目前不考虑切角为三角形的情况，暂时pass
                //}
                //else //切角为三角形，获得真实的残留检测选区
                //{
                //    resultStraightLineC = (ResultStraightLine)htResult[nameCellPolygon2]; // 得到斜边的直线

                //    //计算直角边和斜边的交点
                //    HOperatorSet.IntersectionLines(resultCrossLine1.YBegin_L[0], resultCrossLine1.XBegin_L[0], resultCrossLine1.YEnd_L[0], resultCrossLine1.XEnd_L[0],
                //        resultStraightLineC.YBegin, resultStraightLineC.XBegin, resultStraightLineC.YEnd, resultStraightLineC.XEnd, out hv_RowC1, out hv_ColumnC1,
                //        out hv_IsOverlapping1);
                //    if (hv_RowC1.Length != 1)
                //    {
                //        result.LevelError_e = LevelError_enum.Error;
                //        result.TypeErrorProcess_e = TypeErrorProcess_enum.NumZero;
                //        result.Annotation = par.NameCell.ToString() + ",直线1和切角交点求取失败";
                //        result.SetDefault();
                //        return result;
                //    }
                //    HOperatorSet.IntersectionLines(resultCrossLine1.YBegin_L[1], resultCrossLine1.XBegin_L[1], resultCrossLine1.YEnd_L[1], resultCrossLine1.XEnd_L[1],
                //        resultStraightLineC.YBegin, resultStraightLineC.XBegin, resultStraightLineC.YEnd, resultStraightLineC.XEnd, out hv_RowC2, out hv_ColumnC2,
                //        out hv_IsOverlapping2);
                //    if (hv_RowC2.Length != 1)
                //    {
                //        result.LevelError_e = LevelError_enum.Error;
                //        result.TypeErrorProcess_e = TypeErrorProcess_enum.NumZero;
                //        result.Annotation = par.NameCell.ToString() + ",直线2和切角交点求取失败";
                //        result.SetDefault();
                //        return result;
                //    }

                //    if (par.DefectType == "Fin") // 生成残留检测选区
                //    {
                //        //把三条直线的三个交点形成一个xld，然后再生成一个检测区域
                //        hv_rowA = new HTuple();
                //        hv_rowA = hv_rowA.TupleConcat(hv_RowC1);
                //        hv_rowA = hv_rowA.TupleConcat(hv_RowC2);
                //        hv_rowA = hv_rowA.TupleConcat(resultCrossLine1.Y);
                //        hv_colA = new HTuple();
                //        hv_colA = hv_colA.TupleConcat(hv_ColumnC1);
                //        hv_colA = hv_colA.TupleConcat(hv_ColumnC2);
                //        hv_colA = hv_colA.TupleConcat(resultCrossLine1.X);
                //    }
                //    else  //生成崩缺检测选区
                //    {
                //        //崩缺检测，求三角形顶点关于斜边直线的镜像，生成镜像选区
                //        HTuple DistC1Cross = null, DistC2Cross = null;
                //        HOperatorSet.DistancePp(hv_RowC1, hv_ColumnC1, resultCrossLine1.Y, resultCrossLine1.X, out DistC1Cross);
                //        HOperatorSet.DistancePp(hv_RowC2, hv_ColumnC2, resultCrossLine1.Y, resultCrossLine1.X, out DistC2Cross);

                //        HTuple CrossCircleRows = null, CrossCircleCols = null, CirclesOverlapping = null;
                //        HOperatorSet.IntersectionCircles(hv_RowC1, hv_ColumnC1, DistC1Cross, 0, 6.28318, "positive",
                //            hv_RowC2, hv_ColumnC2, DistC2Cross, 0, 6.28318, "positive",
                //            out CrossCircleRows, out CrossCircleCols, out CirclesOverlapping);

                //        int CircleCrossNum = CrossCircleRows.Length;
                //        if (CrossCircleRows.Length != 2)
                //        {
                //            result.LevelError_e = LevelError_enum.Error;
                //            result.TypeErrorProcess_e = TypeErrorProcess_enum.NumZero;
                //            result.Annotation = par.NameCell.ToString() + ",崩缺选区求取异常";
                //            result.SetDefault();
                //            return result;
                //        }

                //        HTuple Distpp = null;
                //        HOperatorSet.DistancePp(CrossCircleRows[0], CrossCircleCols[0], resultCrossLine1.Y, resultCrossLine1.X, out Distpp);
                //        HTuple CrossPointMirrorRow = null, CrossPointMirrorCol = null;
                //        if (Distpp.D < 5) // 两个圆的交点就是待镜像的点，所以要再求一次
                //        {
                //            CrossPointMirrorRow = CrossCircleRows[1];
                //            CrossPointMirrorCol = CrossCircleCols[1];
                //        }
                //        else
                //        {
                //            CrossPointMirrorRow = CrossCircleRows[0];
                //            CrossPointMirrorCol = CrossCircleCols[0];
                //        }

                //        //把三条直线的三个交点形成一个xld，然后再生成一个检测区域
                //        hv_rowA = new HTuple();
                //        hv_rowA = hv_rowA.TupleConcat(hv_RowC1);
                //        hv_rowA = hv_rowA.TupleConcat(hv_RowC2);
                //        hv_rowA = hv_rowA.TupleConcat(CrossPointMirrorRow);
                //        hv_colA = new HTuple();
                //        hv_colA = hv_colA.TupleConcat(hv_ColumnC1);
                //        hv_colA = hv_colA.TupleConcat(hv_ColumnC2);
                //        hv_colA = hv_colA.TupleConcat(CrossPointMirrorCol);
                //    }
                //    //生成选区对应的XLD
                //    ho_ContourA.Dispose();
                //    HOperatorSet.GenContourPolygonXld(out ho_ContourA, hv_rowA, hv_colA);

                //    //三角选区，在此区域内进行残留检测
                //    ho_TriCirIntersectRegion.Dispose();
                //    HOperatorSet.GenRegionContourXld(ho_ContourA, out ho_TriCirIntersectRegion, "filled");
                    //}

                    #endregion 切角为三角形

                    ho_TriCirReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_binaryImage, ho_TriCirIntersectRegion, out ho_TriCirReduced
                    );
                ho_TriCirReducedThRegion.Dispose();
                HOperatorSet.Threshold(ho_TriCirReduced, out ho_TriCirReducedThRegion, 240,255);
                ho_TriCirReducedContours.Dispose();
                HOperatorSet.GenContourRegionXld(ho_TriCirReducedThRegion, out ho_TriCirReducedContours, "border");

                HTuple hv_Number = null;
                HOperatorSet.CountObj(ho_TriCirReducedContours, out hv_Number);//计算待检测残留区域的个数

                //内切选区的矩形边长的基数，具体长度需乘以个倍率
                double InnerCutRectShortEdge = 30;
                double InnerCutRectRatio = 4;
                //默认内切区域为0
                InterestROI1Index = -1;
                InterestROI2Index = -1;
                //只有一个内切区域
                if (innerCutPar.InnerCutH != 0) 
                {
                    //计算所有ROI内部的Region
                    //获取拟合圆形的ROI
                    //自动生成内切区域的ROI
                    ho_InterestROI1.Dispose();
                    HOperatorSet.GenRectangle1(out ho_InterestROI1, hv_TriPointRow1 - InnerCutRectShortEdge, hv_TriPointCol1 - InnerCutRectShortEdge * InnerCutRectRatio, hv_TriPointRow1 + InnerCutRectShortEdge, hv_TriPointCol1 + InnerCutRectShortEdge * InnerCutRectRatio);
                    
                    ho_TriCirRegionConnect1.Dispose();
                    HOperatorSet.Connection(ho_TriCirReducedThRegion, out ho_TriCirRegionConnect1);
                    ho_TriCirRegionIntersection1.Dispose();
                    HOperatorSet.Intersection(ho_TriCirRegionConnect1, ho_InterestROI1, out ho_TriCirRegionIntersection1
                        );
                    //找到最大的区域
                    HOperatorSet.AreaCenter(ho_TriCirRegionIntersection1, out hv_TriCirRegionArea, out hv_TriCirRegionRow, out hv_TriCirRegionColumn);
                    ho_TriCirBiggested1.Dispose();
                    HOperatorSet.SelectObj(ho_TriCirRegionIntersection1, out ho_TriCirBiggested1, (((hv_TriCirRegionArea.TupleSortIndex()
                        )).TupleSelect((new HTuple(hv_TriCirRegionArea.TupleLength())) - 1)) + 1);
                    //判断被内切选区截取的最大区域属于那个残留区域，这个区域就是内切的切角，需要使用特殊阈值
                    for (int i = 0; i < hv_Number; i++)
                    {
                        ho_TriCirRegionSelect.Dispose();
                        HOperatorSet.SelectObj(ho_TriCirRegionConnect1, out ho_TriCirRegionSelect, i + 1);
                        HOperatorSet.TestSubsetRegion(ho_TriCirBiggested1, ho_TriCirRegionSelect, out hv_IsSubset);
                        if ((int)(hv_IsSubset) != 0)
                        {
                            InterestROI1Index = i;
                            break;
                        }
                    }

                }
                //有两个内切区域
                if (innerCutPar.InnerCutV != 0) //
                {
                    //计算所有ROI内部的Region
                    //获取拟合圆形的ROI
                    //自动生成内切区域的ROI
                    ho_InterestROI2.Dispose();
                    HOperatorSet.GenRectangle1(out ho_InterestROI2, hv_TriPointRow2 - InnerCutRectShortEdge * InnerCutRectRatio, hv_TriPointCol2 - InnerCutRectShortEdge, hv_TriPointRow2 + InnerCutRectShortEdge * InnerCutRectRatio, hv_TriPointCol2 + InnerCutRectShortEdge);

                    ho_TriCirRegionConnect2.Dispose();
                    HOperatorSet.Connection(ho_TriCirReducedThRegion, out ho_TriCirRegionConnect2);
                    ho_TriCirRegionIntersection2.Dispose();
                    HOperatorSet.Intersection(ho_TriCirRegionConnect2, ho_InterestROI2, out ho_TriCirRegionIntersection2
                        );
                    //找到最大的区域
                    HOperatorSet.AreaCenter(ho_TriCirRegionIntersection2, out hv_TriCirRegionArea, out hv_TriCirRegionRow, out hv_TriCirRegionColumn);
                    ho_TriCirBiggested2.Dispose();
                    HOperatorSet.SelectObj(ho_TriCirRegionIntersection2, out ho_TriCirBiggested2, (((hv_TriCirRegionArea.TupleSortIndex()
                        )).TupleSelect((new HTuple(hv_TriCirRegionArea.TupleLength())) - 1)) + 1);
                    //判断被内切选区截取的最大区域属于那个残留区域，这个区域就是内切的切角，需要使用特殊阈值
                    HTuple hv_areaSel = null, hv_RowSel = null, hv_ColSel = null;
                    HOperatorSet.AreaCenter(ho_TriCirBiggested2, out hv_areaSel, out hv_RowSel, out hv_ColSel);
                    if (hv_areaSel.D != 0)
                    {
                        for (int i = 0; i < hv_Number; i++)
                        {
                            ho_TriCirRegionSelect.Dispose();
                            HOperatorSet.SelectObj(ho_TriCirRegionConnect2, out ho_TriCirRegionSelect, i + 1);
                            HOperatorSet.TestSubsetRegion(ho_TriCirBiggested2, ho_TriCirRegionSelect, out hv_IsSubset);
                            if ((int)(hv_IsSubset) != 0)
                            {
                                InterestROI2Index = i;
                                break;
                            }
                        }
                    }
                }


                HTuple hv_ContoursDis = null;

                HTuple hv_TriCirReducedRow = null, hv_TriCirReducedCol = null;


                double InspectTh = 1;
                for (int i = 0; i < hv_Number.I; i++)
                {
                    HOperatorSet.SelectObj(ho_TriCirReducedContours, out ho_TriCirReducedContoursSelect, i + 1);
                    //得到所选残留部分的xld对应的坐标点
                    HOperatorSet.GetContourXld(ho_TriCirReducedContoursSelect, out hv_TriCirReducedRow, out hv_TriCirReducedCol);

                    //if (par.OutlineType == 1) // 切角为圆，获得真实的残留检测选区（斜边为圆的三角圆）
                    //{
                        //计算残留区域边缘和切角圆的几何距离
                        HOperatorSet.DistEllipseContourPointsXld(ho_TriCirReducedContoursSelect, "signed", 0,
                        hv_RowCir, hv_ColumnCir, 0, hv_RadiusCir, hv_RadiusCir, out hv_ContoursDis);
                    //}
                    //else //切角为直线
                    //{
                    //    //计算残留区域边缘和切角直线的几何距离
                    //    HOperatorSet.DistancePl(hv_TriCirReducedRow, hv_TriCirReducedCol, resultStraightLineC.YBegin, resultStraightLineC.XBegin, resultStraightLineC.YEnd,
                    //    resultStraightLineC.XEnd, out hv_ContoursDis);
                    //}

                    int PointNum = hv_ContoursDis.Length;
                    double[] ContoursDis_Arr = hv_ContoursDis.ToDArr();  //记录xld上每个点的距离
                    double[] TriCirReducedRow_Arr = hv_TriCirReducedRow.ToDArr(); //记录xld上每个点的row坐标
                    double[] TriCirReducedCol_Arr = hv_TriCirReducedCol.ToDArr(); //记录xld上每个点的col坐标

                    double maxValue =0;
                    int maxValueIndex = 0;
                    //if (par.DefectType == "Fin"||par.OutlineType==0) // 求残留，或者切角为直线的时候，都按如下进行计算
                    if (par.DefectType == "Fin") // 求残留，或者切角为直线的时候，都按如下进行计算
                    {
                        //计算残留区域最大高度值
                        maxValue = ContoursDis_Arr.Max();
                        maxValueIndex = Array.IndexOf(ContoursDis_Arr, maxValue);
                    }
                    else
                    {
                        //计算崩缺区域最大高度值----崩缺区域在圆内侧，计算出的高度是负数
                        maxValue =Math.Abs(ContoursDis_Arr.Min());
                        maxValueIndex = Array.IndexOf(ContoursDis_Arr, -maxValue);
                    }

                    //最大高度值对应的坐标
                    double maxValueRow = TriCirReducedRow_Arr[maxValueIndex];
                    double maxValueCol = TriCirReducedCol_Arr[maxValueIndex];

                    if (InterestROI1Index == i) //内切区域1，使用单独的阈值
                    {
                        InspectTh = innerCutPar.InnerCutH;
                    }
                    else if (InterestROI2Index == i) //内切区域2，使用单独的阈值
                    {
                        InspectTh = innerCutPar.InnerCutV;
                    }
                    else //非内切区域部分--使用FunSmooth函数求取，所以这个部分的阈值设为无穷大
                    {
                        //InspectTh = NormalInspectTh;
                        InspectTh = double.MaxValue;
                    }
                    double ThPixel = InspectTh;

                    //对高度小于阈值边缘都不计入残留
                    if (maxValue > ThPixel)
                    {
                        double x = 0, y = 0;
                        //添加残留最远点的坐标到数组
                        x = Math.Round(maxValueCol, 1);
                        y = Math.Round(maxValueRow, 1);
                        result.X_L.Add(x);
                        result.Y_L.Add(y);
                        result.Area_L.Add(0);//求取面积
                        result.Rectangularity_L.Add(0);
                        result.Circularity_L.Add(0);
                        #region 求取包络的集合

                        result.Radius_L.Add(0);
                        result.R_L.Add(0);
                        result.Height_L.Add(Math.Round(maxValue,1));
                        result.Width_L.Add(0);
                    }
                    #endregion 求取包络的集合

                }
                

                return result;
            }
            catch (Exception ex)
            {
                result.LevelError_e = LevelError_enum.Error;
                result.Annotation = par.NameCell.ToString() + ex.Message.ToString();
                //result.SetDefault();
                return result;
            }
            finally
            {
                #region 调用平行轮廓求取
                if (parRaisedEdgeSmooth.BlTestRun)
                {
                    RecordImage_Record(par, result, "FunPhoto", "CameraImage", imageAll);//默认记录
                }              

                parRaisedEdgeSmooth.g_ParSmooth.Position = par.Position;    //检测位置
                parRaisedEdgeSmooth.g_ParSmooth.DefectType = par.DefectType;//检测类型
                parRaisedEdgeSmooth.g_ParSmooth.NormalTh = NormalInspectTh; //正常区域检测阈值

                FunSmoothing fun = new FunSmoothing();
                fun.DealSmoothing(parRaisedEdgeSmooth, ho_binaryImage.Clone(), ho_binaryArea.Clone(), result, htResult);

                //区域个数
                result.Num = result.X_L.Count;
                if (result.Num == 0)
                {
                    result.LevelError_e = LevelError_enum.OK;
                    result.Annotation = par.NameCell.ToString() + "FinShell个数为0";
                    result.SetDefault();
                }

                //添加显示
                AddDisplay(parRaisedEdgeSmooth.g_ParOutput, result);

                #endregion 调用平行轮廓求取

                //对结果进行综合处理
                SetComprehensiveResult(result, parRaisedEdgeSmooth, htResult, false);

                //校准
                DealCalibResult(parRaisedEdgeSmooth, result, htResult);
                #region 记录
                //进行无关区域削减后的区域
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_CcutRegionReduceXld", ho_CcutRegionReduceXld);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_CcutRegionReduceRegion", ho_CcutRegionReduceRegion);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_CcutRegionReduceImage", ho_CcutRegionReduceImage);

                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_MLineQuadRegion", ho_MLineQuadRegion);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_UpQuadPaintImage", ho_UpQuadPaintImage);

                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_binaryArea", ho_binaryArea);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_binaryImage", ho_binaryImage);

                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_binaryAreaUp", ho_binaryAreaUp);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_binaryImageUp", ho_binaryImageUp);

                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_binaryAreaDown", ho_binaryAreaDown);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_binaryImageDown", ho_binaryImageDown);

                #region 圆拟合部分
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_CircleEdges", ho_CircleEdges);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_LongestContour", ho_LongestContour);
                #endregion 圆拟合部分

                #region 切角选区求取部分
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_ContourA", ho_ContourA);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_CircleRegion", ho_CircleRegion);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_TriCirIntersectRegion", ho_TriCirIntersectRegion);
                #endregion 切角选区求取部分

                #region 内切区域求取部分
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_InterestROI1", ho_InterestROI1);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_TriCirRegionConnect1", ho_TriCirRegionConnect1);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_TriCirRegionIntersection1", ho_TriCirRegionIntersection1);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_TriCirBiggested1", ho_TriCirBiggested1);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_InterestROI2", ho_InterestROI2);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_TriCirRegionConnect2", ho_TriCirRegionConnect2);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_TriCirRegionIntersection2", ho_TriCirRegionIntersection2);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_TriCirBiggested2", ho_TriCirBiggested2);
                #endregion 内切区域求取部分

                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_TriCirReduced", ho_TriCirReduced);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_TriCirReducedThRegion", ho_TriCirReducedThRegion);
                RecordHoject(parRaisedEdgeSmooth, result, NameClass, "ho_TriCirReducedContours", ho_TriCirReducedContours);

                #endregion 记录

                if (ho_TriCirReducedContoursSelect != null)
                {
                    ho_TriCirReducedContoursSelect.Dispose();
                }
                if (ho_ThROI != null)
                {
                    ho_ThROI.Dispose();
                }
                if (ho_CircleEdges != null)
                {
                    ho_CircleEdges.Dispose();
                }
                if (ho_CircleImageReduced != null)
                {
                    ho_CircleImageReduced.Dispose();
                }
                if (ho_ContoursSplit != null)
                {
                    ho_ContoursSplit.Dispose();
                }
                if (ho_TriRegion != null)
                {
                    ho_TriRegion.Dispose();
                }
                if (ho_CircleRegionComplement != null)
                {
                    ho_CircleRegionComplement.Dispose();
                }
                if (ho_TriCirRegionSelect != null)
                {
                    ho_TriCirRegionSelect.Dispose();
                }
                if (ho_UnionContours != null)
                {
                    ho_UnionContours.Dispose();
                }
                if (ho_CircleRegionOuterLimit != null)
                {
                    ho_CircleRegionOuterLimit.Dispose();
                }
                if (ho_CircleRegionInnerLimit != null)
                {
                    ho_CircleRegionInnerLimit.Dispose();
                }
                if (ho_CircleImageReducedOpening != null)
                {
                    ho_CircleImageReducedOpening.Dispose();
                }
                if (ho_CircleImageReducedOpeningBin != null)
                {
                    ho_CircleImageReducedOpeningBin.Dispose();
                }

                if (ho_ROIUp != null)
                {
                    ho_ROIUp.Dispose();
                }
                if (ho_ROIDown != null)
                {
                    ho_ROIDown.Dispose();
                }

            }
        }



        /// <summary>
        /// 求取二值化，专门给C角图像下半部分使用的二值化，参数写死了
        ///
        public ResultBinary DealBinary(HObject ho_ROI, ImageAll imageAll)
        {
            #region 定义
            ResultBinary result = new ResultBinary();

            HTuple num_Obj = 0;
            HTuple width = 0;
            HTuple height = 0;

            HObject ho_Image = imageAll.Ho_Image;//获取图像
            HObject ho_ImageMean = null;
            HObject ho_RegionTh = null;

            HObject ho_RegionDilation = null;
            HObject ho_RegionIntersection = null;
            HObject ho_RegionOpen = null;
            HObject ho_RegionClose = null;
            HObject ho_RegionFillup = null;
            HObject ho_RegionConnect = null;
            HObject ho_SelectShape = null;
            HObject ho_RegionResult = null;
            HObject ho_ImageResult = null;
            HObject ho_BinaryResult = null;

            HOperatorSet.GenEmptyObj(out ho_ImageMean);
            HOperatorSet.GenEmptyObj(out ho_RegionTh);
            HOperatorSet.GenEmptyObj(out ho_ImageResult);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            HOperatorSet.GenEmptyObj(out ho_RegionOpen);
            HOperatorSet.GenEmptyObj(out ho_RegionClose);
            HOperatorSet.GenEmptyObj(out ho_RegionFillup);
            HOperatorSet.GenEmptyObj(out ho_RegionConnect);
            HOperatorSet.GenEmptyObj(out ho_SelectShape);
            HOperatorSet.GenEmptyObj(out ho_RegionResult);
            #endregion 定义

            try
            {
                #region Record
                stopWatch.Restart();//记录运行时间
                #endregion Record

                #region 求取通道个数
                HTuple num = 0;
                HOperatorSet.CountChannels(ho_Image, out num);//计算图像通道数
                if (num != 1)
                {
                    result.LevelError_e = LevelError_enum.Alarm;
                    result.Annotation = "二值化输入不是单通道图像";
                }
                HOperatorSet.GetImageSize(ho_Image, out width, out height);//求取图像尺寸
                #endregion 求取通道个数

                #region 二值化
                //固定阈值
                HOperatorSet.Threshold(ho_Image, out ho_RegionTh, 0, 220);
                HOperatorSet.CountObj(ho_RegionTh, out num_Obj);
                if (num_Obj == 0)
                {
                    result.LevelError_e = LevelError_enum.Error;
                    result.TypeErrorProcess_e = TypeErrorProcess_enum.NumZero;
                    result.Annotation = "RegionTh(Binary二值化)为空";
                    result.SetDefault();
                    return result;
                }
                #endregion 二值化

                #region 开运算
                ho_RegionOpen.Dispose();
                HOperatorSet.OpeningRectangle1(ho_RegionTh, out ho_RegionOpen, 5, 5);
                HOperatorSet.CountObj(ho_RegionOpen, out num_Obj);
                if (num_Obj == 0)
                {
                    result.LevelError_e = LevelError_enum.Error;
                    result.TypeErrorProcess_e = TypeErrorProcess_enum.NumZero;
                    result.Annotation = "RegionOpen(Binary开运算)为空";
                    result.SetDefault();
                    return result;
                }
                #endregion 开运算

                #region 闭运算
                ho_RegionClose.Dispose();
                HOperatorSet.ClosingRectangle1(ho_RegionOpen, out ho_RegionClose, 5, 5);
                HOperatorSet.CountObj(ho_RegionClose, out num_Obj);
                if (num_Obj == 0)
                {
                    result.LevelError_e = LevelError_enum.Error;
                    result.TypeErrorProcess_e = TypeErrorProcess_enum.NumZero;
                    result.Annotation = "RegionClose(Binary闭运算)为空";
                    result.SetDefault();
                    return result;
                }
                #endregion 闭运算

                #region 内部填充
                //HOperatorSet.FillUp(ho_RegionClose, out ho_RegionFillup);
                ho_RegionFillup = ho_RegionClose.Clone();
                HOperatorSet.CountObj(ho_RegionFillup, out num_Obj);
                if (num_Obj == 0)
                {
                    result.LevelError_e = LevelError_enum.Error;
                    result.TypeErrorProcess_e = TypeErrorProcess_enum.NumZero;
                    result.Annotation = "RegionFillup(Binary填充)为空";
                    result.SetDefault();
                    return result;
                }
                #endregion 内部填充

                #region 连接细小断开1
                ho_RegionDilation = ho_RegionFillup.Clone();
                #endregion 连接细小断开1

                #region 求取联通域
                ho_RegionConnect.Dispose();
                HOperatorSet.Connection(ho_RegionDilation, out ho_RegionConnect);

                HOperatorSet.CountObj(ho_RegionConnect, out num_Obj);
                if (num_Obj == 0)
                {
                    result.LevelError_e = LevelError_enum.Error;
                    result.TypeErrorProcess_e = TypeErrorProcess_enum.NumZero;
                    result.Annotation = "RegionFillup(Binary连通域)为空";
                    result.SetDefault();
                    return result;
                }
                #endregion 求取联通域

                #region 连接细小断开2
                ho_RegionIntersection = ho_RegionConnect.Clone();
                #endregion 连接细小断开2

                #region 筛选面积，高度，宽度
                ho_SelectShape.Dispose();
                HOperatorSet.SelectShape(ho_RegionIntersection, out ho_SelectShape,
                    (new HTuple("area")).TupleConcat("height").TupleConcat("width"), "and",
                    new HTuple(2000).TupleConcat(10).TupleConcat(10),
                    new HTuple(5000000).TupleConcat(999999).TupleConcat(999999));

                HOperatorSet.CountObj(ho_SelectShape, out num_Obj);
                if (num_Obj == 0)
                {
                    result.LevelError_e = LevelError_enum.Error;
                    result.TypeErrorProcess_e = TypeErrorProcess_enum.NumZero;
                    result.Annotation = "SelectShape(Binary面积筛选)为空";
                    result.SetDefault();
                    return result;
                }
                #endregion 筛选面积

                //region 结果
                ho_RegionResult = ho_SelectShape.Clone();
                result.RegionResult = new ImageAll(ho_RegionResult);

                //图像结果
                HObject ho_ROIInverse = null;
                HObject ho_Back = null;
                HOperatorSet.Complement(ho_ROI, out ho_ROIInverse);
                HOperatorSet.RegionToBin(ho_ROIInverse, out ho_Back, 255, 0, width, height);
                HOperatorSet.RegionToBin(ho_SelectShape, out ho_ImageResult, 255, 0, width, height);
                HOperatorSet.AddImage(ho_Back, ho_ImageResult, out ho_ImageResult, 1, 0);
                result.ImageResult = new ImageAll(ho_ImageResult);
                if (ho_ROIInverse != null)
                {
                    ho_ROIInverse.Dispose();
                }
                if (ho_Back != null)
                {
                    ho_Back.Dispose();
                }
                //输出图像或者区域

                result.BinaryResult = result.RegionResult;
                ho_BinaryResult = ho_RegionResult;


                //result.BinaryResult = result.ImageResult;
                //ho_BinaryResult = ho_ImageResult;


                return result;
            }
            catch (Exception ex)
            {
                //LogError(NameClass, , ex);
                result.LevelError_e = LevelError_enum.Error;
                result.Annotation = "二值化处理跳转到异常";
                result.SetDefault();
                return result;
            }
            finally
            {
                if (ho_ImageMean != null)
                {
                    ho_ImageMean.Dispose();
                }
                if (ho_RegionTh != null)
                {
                    ho_RegionTh.Dispose();
                }
                if (ho_ImageResult != null)
                {
                    ho_ImageResult.Dispose();
                }
                if (ho_RegionDilation != null)
                {
                    ho_RegionDilation.Dispose();
                }
                if (ho_RegionIntersection != null)
                {
                    ho_RegionIntersection.Dispose();
                }

                if (ho_RegionOpen != null)
                {
                    ho_RegionOpen.Dispose();
                }
                if (ho_RegionClose != null)
                {
                    ho_RegionClose.Dispose();
                }
                if (ho_RegionFillup != null)
                {
                    ho_RegionFillup.Dispose();
                }
                if (ho_RegionConnect != null)
                {
                    ho_RegionConnect.Dispose();
                }
                if (ho_SelectShape != null)
                {
                    ho_SelectShape.Dispose();
                }
            }
        }

        /// <summary>
        /// 从一副图像求取给定参数的圆，若求取成功，返回1，其它返回2？3？99？
        /// </summary>
        /// <param name="ho_Image">输入图像</param>
        /// <param name="width">图像宽度</param>
        /// <param name="height">图像高度</param>
        /// <param name="refRadius">参考半径</param>
        /// <param name="refRadiusTol">查找到的圆的半径容差</param>
        /// <param name="refThH">参考阈值高</param>
        /// <param name="refThL">参考阈值低</param>
        /// <param name="closingRadius">闭运算半径</param>
        /// <param name="openingRadius">开运算半径</param>
        /// <param name="FilterSmoothPar">边缘平滑参数</param>
        int GenCirCleFromImage(HObject ho_Image,double width,double height, double refRadius, double refRadiusTol,double refThL, double refThH, double closingRadius, double openingRadius, 
            double FilterSmoothPar,out double[] CirclePar)
        {
            CirclePar = new double[3];

            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            HObject ho_Region = null, ho_RegionFillUp = null, ho_RegionClosing = null;
            HObject ho_RegionOpening = null, ho_ConnectedRegions = null, ho_SelectedRegions = null;
            HObject ho_BinImage = null, ho_Rectangle1Smaller = null, ho_ImageReduced1;
            HObject ho_Edges = null, ho_ContoursSplit = null, ho_Lines = null, ho_Circles = null;
            HObject ho_Contour = null;
            HObject ho_UnionContours = null, ho_UnionContCircle = null;


            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_Rectangle1Smaller);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);

            HOperatorSet.GenEmptyObj(out ho_Edges);
            HOperatorSet.GenEmptyObj(out ho_ContoursSplit);
            HOperatorSet.GenEmptyObj(out ho_Lines);
            HOperatorSet.GenEmptyObj(out ho_Circles);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_UnionContCircle);
            HOperatorSet.GenEmptyObj(out ho_UnionContours);
            try
            {

                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_Image, out ho_Region, refThL, refThH);
                //上边缘进行拟合之前最好closing下
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_Region, out ho_RegionFillUp);
                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_RegionFillUp, out ho_RegionClosing, closingRadius);

                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_RegionClosing, out ho_RegionOpening, openingRadius);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                    "and", (width * height) * 0.3, width * height);
                ho_BinImage.Dispose();
                HOperatorSet.RegionToBin(ho_SelectedRegions, out ho_BinImage, 255, 0, width, height);

                //因为openingCirle会让边缘变得平滑，所以需要将ROI缩小，去掉边缘的锐角区域
                ho_Rectangle1Smaller.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle1Smaller, 0 + openingRadius, 0 + openingRadius,
                    height - openingRadius, width - openingRadius);
                ho_ImageReduced1.Dispose();
                HOperatorSet.ReduceDomain(ho_BinImage, ho_Rectangle1Smaller, out ho_ImageReduced1);

                //如果残留对最终的拟合有影响，可以把edges_sub_pix和segment_contours_xld中的平滑调大
                ho_Edges.Dispose();
                HOperatorSet.EdgesSubPix(ho_ImageReduced1, out ho_Edges, "canny", FilterSmoothPar, 20, 40);

                //将xld中的所有直线和圆弧分段，smooth和边缘容差调大一些，就可以显著减少分段个数
                ho_ContoursSplit.Dispose();
                HOperatorSet.SegmentContoursXld(ho_Edges, out ho_ContoursSplit, "lines_circles",
                    FilterSmoothPar, 11, 9);

                HTuple hv_Number = null;
                HOperatorSet.CountObj(ho_ContoursSplit, out hv_Number);
                ho_Lines.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Lines);
                ho_Circles.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Circles);

                //直线和圆弧部分通过属性分开处理
                HTuple end_val73 = hv_Number;
                HTuple step_val73 = 1;
                for (int i = 1; i<=hv_Number.I; i++)
                {
                    ho_Contour.Dispose();
                    HOperatorSet.SelectObj(ho_ContoursSplit, out ho_Contour, i);
                    HTuple hv_Type = null;
                    HOperatorSet.GetContourGlobalAttribXld(ho_Contour, "cont_approx", out hv_Type);
                    if ((int)(new HTuple(hv_Type.TupleEqual(-1))) != 0)//hv_type =-1,表面其为直线
                    {
                        HOperatorSet.ConcatObj(ho_Lines, ho_Contour, out OTemp[0]);
                        ho_Lines.Dispose();
                        ho_Lines = OTemp[0];
                    }
                    else//hv_type !=-1,表面其为圆弧
                    {
                        HOperatorSet.ConcatObj(ho_Circles, ho_Contour, out OTemp[0]);
                        ho_Circles.Dispose();
                        ho_Circles = OTemp[0];
                    }
                }

                //进行圆弧的拟合
                HTuple hv_Row = null, hv_Column = null, hv_Radius = null, hv_StartPhi = null, hv_EndPhi = null, hv_PointOrder = null;

                //对圆弧部分进行合并
                ho_UnionContours.Dispose();
                HOperatorSet.UnionCocircularContoursXld(ho_Circles, out ho_UnionContours, 1,
                    0.2, 0.2, 400, 200, 400, "true", 1);
                //拟合圆的算子不会对xld进行分段，它只进行拟合，对每个xld输出一个对应的圆
                HOperatorSet.FitCircleContourXld(ho_UnionContours, "atukey", -1, 2, 20, 5,
                    2, out hv_Row, out hv_Column, out hv_Radius, out hv_StartPhi, out hv_EndPhi,
                    out hv_PointOrder);
                ho_UnionContCircle.Dispose();
                HOperatorSet.GenCircleContourXld(out ho_UnionContCircle, hv_Row, hv_Column,
                    hv_Radius, 0, 6.28318, "positive", 1);

                if (hv_Radius.Length == 0)
                {
                    CirclePar[0] = 0;
                    CirclePar[1] = 0;
                    CirclePar[2] = 0;
                    return 2;
                }
                double RadiusTolTemp=double.MaxValue;
                int RadiusTolTempIndex = 0;
                for (int i = 0; i < hv_Radius.Length; i++)
                {
                    //寻找和参考半径最接近的圆弧
                    if (Math.Abs(hv_Radius.DArr[i] - refRadius) < RadiusTolTemp)
                    {
                        RadiusTolTemp = Math.Abs(hv_Radius.DArr[i] - refRadius);
                        RadiusTolTempIndex = i;
                    }
                }
                //如果求出和参考圆半径最接近的圆，和参考圆还是差比较多，就ReturnFalse
                if (RadiusTolTemp < refRadius - refRadiusTol || RadiusTolTemp > refRadius + refRadiusTol)
                {
                    CirclePar[0] = 0;
                    CirclePar[1] = 0;
                    CirclePar[2] = 0;
                    return 3;
                }
                CirclePar[0] = hv_Row.DArr[RadiusTolTempIndex];
                CirclePar[1] = hv_Column.DArr[RadiusTolTempIndex];
                CirclePar[2] = hv_Radius.DArr[RadiusTolTempIndex];
                return 1;//返回1为OK
            }
            catch
            {
                CirclePar[0] = 0;
                CirclePar[1] = 0;
                CirclePar[2] = 0;
                return 99;
            }
            finally
            {
                if (ho_Region != null)
                {
                    ho_Region.Dispose();
                }
                if (ho_RegionFillUp != null)
                {
                    ho_RegionFillUp.Dispose();
                }
                if (ho_RegionClosing != null)
                {
                    ho_RegionClosing.Dispose();
                }
                if (ho_Rectangle1Smaller != null)
                {
                    ho_Rectangle1Smaller.Dispose();
                }
                if (ho_ImageReduced1 != null)
                {
                    ho_ImageReduced1.Dispose();
                }
                if (ho_Edges != null)
                {
                    ho_Edges.Dispose();
                }
                if (ho_ContoursSplit != null)
                {
                    ho_ContoursSplit.Dispose();
                }
                if (ho_Lines != null)
                {
                    ho_Lines.Dispose();
                }
                if (ho_Circles != null)
                {
                    ho_Circles.Dispose();
                }

                if (ho_Contour != null)
                {
                    ho_Contour.Dispose();
                }
                if (ho_UnionContCircle != null)
                {
                    ho_UnionContCircle.Dispose();
                }
                if (ho_UnionContours != null)
                {
                    ho_UnionContours.Dispose();
                }
            }
        }


        //int GenLineFromImage()
        //{

        //}


        /// <summary>
        /// 根据输入的轮廓生成相应的平行轮廓
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
                HOperatorSet.GetContourXld(ho_ContourOuter,out hv_RowOuter,out hv_ColOuter);
                HOperatorSet.GetContourXld(ho_ContourOuterShift,out hv_RowOuterShift,out hv_ColOuterShift);
                HOperatorSet.GetContourXld(ho_ContourInner,out hv_RowInner,out hv_ColInner);
                HOperatorSet.GetContourXld(ho_ContourInnerShift, out hv_RowInnerShift, out hv_ColInnerShift);

                //Inverse
                HOperatorSet.TupleInverse(hv_RowOuter, out hv_RowOuterInverse);
                HOperatorSet.TupleInverse(hv_ColOuter, out hv_ColOuterInverse);
                HOperatorSet.TupleInverse(hv_RowInner, out hv_RowInnerInverse);
                HOperatorSet.TupleInverse(hv_ColInner, out hv_ColInnerInverse);

                //concat
                HOperatorSet.TupleConcat(hv_RowOuterInverse,hv_RowOuterShift,out hv_RowOuterConcat);
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



        //进行线段插值
        void GenSegmentInterpola(double[] PX, double[] PY, int Num, out double[] InterpolaX, out double[] InterpolaY)
        {
            InterpolaX = new double[Num * (PX.Length - 1)];
            InterpolaY = new double[Num * (PX.Length - 1)];
            try
            {
                for (int j = 0; j < PX.Length-1; j++)
                {
                    
                    double P1X = PX[j];
                    double P1Y = PY[j];
                    double P2X = PX[j+1];
                    double P2Y = PY[j+1];
                    FunGeometry funGeometry = new FunGeometry();
                    ParLineGeometry resultLine = funGeometry.GetLine(P1X, P1Y, P2X, P2Y);
                    double[] XArry = new double[Num];
                    double[] YArry = new double[Num];
                    for (int i = 1; i < Num + 1; i++)
                    {
                        //XArry[i] = P1X + (P2X - P1X) * i / Num + 1;
                        //YArry[i] = resultLine.k * XArry[i] + resultLine.b;

                        InterpolaX[j * Num + i-1] = P1X + (P2X - P1X) * i / (Num + 1);
                        InterpolaY[j * Num + i - 1] = resultLine.k * InterpolaX[j * Num + i - 1] + resultLine.b;
                    }
                }
            }
            catch
            {

            }
        }
    }

    //拟合圆的参数
    public class RefCirclePar
    {
        private double refRadius;
        private double refRadiusTol;
        private double refThL;
        private double refThH;
        private double closingRadius;
        private double openingRadius;
        private double filterSmoothPar;

        public double RefRadius
        {
            get { return refRadius; }
            set { refRadius = value; }
        }
        public double RefRadiusTol
        {
            get { return refRadiusTol; }
            set { refRadiusTol = value; }
        }
        public double RefThL
        {
            get { return refThL; }
            set { refThL = value; }
        }
        public double RefThH
        {
            get { return refThH; }
            set { refThH = value; }
        }
        public double ClosingRadius
        {
            get { return closingRadius; }
            set { closingRadius = value; }
        }
        public double OpeningRadius
        {
            get { return openingRadius; }
            set { openingRadius = value; }
        }
        public double FilterSmoothPar
        {
            get { return filterSmoothPar; }
            set { filterSmoothPar = value; }
        }
    }

    public class InnerCutPar
    {
        public double InnerCutH = 0;
        public double InnerCutV = 0;


    }
}
