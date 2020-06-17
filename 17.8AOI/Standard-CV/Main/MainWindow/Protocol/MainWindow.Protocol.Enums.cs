using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    #region robot std
    public enum BotStd
    {
        /// <summary>
        /// 粗定位取片位置
        /// </summary>
        PickPos = 1,
        /// <summary>
        /// 精定位位置
        /// </summary>
        PrecisePos,
        /// <summary>
        /// 抛料位置
        /// </summary>
        DownPos,
    }
    #endregion

    #region robot adj
    public enum BotAdj
    {
        /// <summary>
        /// 粗定位取片位置
        /// </summary>
        PickPos = 1,
        /// <summary>
        /// 精定位位置
        /// </summary>
        PrecisePos,
        /// <summary>
        /// 抛料位置
        /// </summary>
        DownPos,
    }
    #endregion

    /// <summary>
    /// 配方寄存器 d2020
    /// </summary>
    public enum RecipeRegister
    {
        /// <summary>
        /// 玻璃X
        /// </summary>
        GlassX,
        /// <summary>
        /// 玻璃Y
        /// </summary>
        GlassY,
        /// <summary>
        /// 玻璃厚度
        /// </summary>
        Thickness,
        /// <summary>
        /// 二维码x
        /// </summary>
        CodeX = 7,
        /// <summary>
        /// 二维码y
        /// </summary>
        CodeY,
        /// <summary>
        /// mark间距
        /// </summary>
        DisMark,
        /// <summary>
        /// 取片平台产品方向
        /// </summary>
        DIR_PICK,
        /// <summary>
        /// 放aoi产品方向
        /// </summary>
        DIR_PLACETOAOI = 20,
        /// <summary>
        /// 卡塞行数
        /// </summary>
        CSTRows = 41,
        /// <summary>
        /// 卡塞列数
        /// </summary>
        CSTCols,
        /// <summary>
        /// 第一列龙骨间距
        /// </summary>
        DisCol1,
        /// <summary>
        /// 龙骨间距
        /// </summary>
        KeelInterval,
        /// <summary>
        /// 龙骨层间距
        /// </summary>
        LayerSpacing,
    }

    /// <summary>
    /// 数据寄存器1，d2500
    /// </summary>
    public enum DataRegister1
    {
        PCAlarm = 1,
        /// <summary>
        /// 配方写入成功
        /// </summary>
        RecipeOK = 3,
        /// <summary>
        /// 皮带线系数
        /// </summary>
        CodeResult = 4,
        /// <summary>
        /// 已插栏数
        /// </summary>
        NumInCST,
        /// <summary>
        /// pc->plc，二维码，第一位是长度
        /// </summary>
        Code,
        /// <summary>
        /// 插栏数据确认
        /// </summary>
        InsertDataConfirm = 11,
        /// <summary>
        /// 粗定位补偿X
        /// </summary>
        OffsetX,
        /// <summary>
        /// 粗定位补偿y
        /// </summary>
        OffsetY,
        /// <summary>
        /// 粗定位补偿r
        /// </summary>
        OffsetR,
        /// <summary>
        /// 粗定位偏差确认
        /// </summary>
        OffsetConfirm,
        /// <summary>
        /// 单目偏差x
        /// </summary>
        MonoOffsetX,
        /// <summary>
        /// 单目偏差Y
        /// </summary>
        MonoOffsetY,
        /// <summary>
        /// 单目偏差R
        /// </summary>
        MonoOffsetR,
        /// <summary>
        /// 单目偏差确认信号
        /// </summary>
        MonoOffsetConfirm,
    }

    /// <summary>
    /// 数据寄存器2，d2540
    /// </summary>
    public enum DataRegister2
    {
        /// <summary>
        /// 插栏1基准值
        /// </summary>
        StdCSTPos1,
        /// <summary>
        /// 插栏2基准值
        /// </summary>
        StdCSTPos2,
        /// <summary>
        /// 插栏3基准值
        /// </summary>
        StdCSTPos3,
        /// <summary>
        /// 插栏4基准值
        /// </summary>
        StdCSTPos4,
        /// <summary>
        /// 上游平台取片t轴角度
        /// </summary>
        AxisT_PickFromPlat = 11,
        /// <summary>
        /// 双目定位t轴角度
        /// </summary>
        AxisT_Precise,
        /// <summary>
        /// 放aoi工位1 t轴角度
        /// </summary>
        AxisT_PlaceToAOI1,
        /// <summary>
        /// 放aoi工位2 t轴角度
        /// </summary>
        AxisT_PlaceToAOI2,
        /// <summary>
        /// 放aoi工位3 t轴角度
        /// </summary>
        AxisT_PlaceToAOI3,
        /// <summary>
        /// 放aoi工位4 t轴角度
        /// </summary>
        AxisT_PlaceToAOI4,
        /// <summary>
        /// 取aoi工位1 t轴角度
        /// </summary>
        AxisT_PickFromAOI1,
        /// <summary>
        /// 取aoi工位2 t轴角度
        /// </summary>
        AxisT_PickFromAOI2,
        /// <summary>
        /// 取aoi工位3 t轴角度
        /// </summary>
        AxisT_PickFromAOI3,
        /// <summary>
        /// 取aoi工位4 t轴角度
        /// </summary>
        AxisT_PickFromAOI4,
        /// <summary>
        /// 放片到下游平台t轴角度
        /// </summary>
        AxisT_PlaceToPlat,
        /// <summary>
        /// 旋转中心标定角度
        /// </summary>
        AxisT_CalibRC
    }

    /// <summary>
    /// 数据寄存器3，d2600
    /// </summary>
    public enum DataRegister3
    {
        /// <summary>
        /// 插栏基准补偿，相机和fork之间的差，发给plc用于数据校验时剔除
        /// </summary>
        InsertStdCom,
        /// <summary>
        /// 插栏x数据
        /// </summary>
        InsertData,
        /// <summary>
        /// 插栏1第1列高度补偿
        /// </summary>
        InsertComZ1,
        /// <summary>
        /// 插栏1第1列层间距
        /// </summary>
        KeelSpacing1 = 8,
    }

    /// <summary>
    /// 报警
    /// </summary>
    public enum PCAlarm_Enum
    {
        标定失败 = 1,
        卡塞计算失败 = 2,
    }
}
