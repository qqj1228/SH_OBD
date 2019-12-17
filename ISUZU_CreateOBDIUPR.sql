USE SH_OBD
GO

-- 车型表用于存储对应车型含有的CAL_ID和CVN
IF OBJECT_ID(N'SH_OBD.dbo.OBDIUPR') IS NOT NULL
    DROP TABLE SH_OBD.dbo.OBDIUPR
GO
Create TABLE SH_OBD.dbo.OBDIUPR (
    ID int IDENTITY PRIMARY KEY NOT NULL, -- ID, 自增, 主键
    WriteTime datetime NOT NULL default(getdate()), -- 写入时间
    VIN varchar(17) NOT NULL, -- 车辆VIN号
    ECU_ID varchar(8) NOT NULL, -- 与排放相关的ECU Response ID
    -- 0908
    CATCOMP1 int NULL, -- 催化器 组1 监测完成次数
    CATCOND1 int NULL, -- 催化器 组1 符合监测条件次数
    CATCOMP2 int NULL, -- 催化器 组2 监测完成次数
    CATCOND2 int NULL, -- 催化器 组2 符合监测条件次数
    O2SCOMP1 int NULL, -- 前氧传感器 组1 监测完成次数
    O2SCOND1 int NULL, -- 前氧传感器 组1 符合监测条件次数
    O2SCOMP2 int NULL, -- 前氧传感器 组2 监测完成次数
    O2SCOND2 int NULL, -- 前氧传感器 组2 符合监测条件次数
    SO2SCOMP1 int NULL, -- 后氧传感器 组1 监测完成次数
    SO2SCOND1 int NULL, -- 后氧传感器 组1 符合监测条件次数
    SO2SCOMP2 int NULL, -- 后氧传感器 组2 监测完成次数
    SO2SCOND2 int NULL, -- 后氧传感器 组2 符合监测条件次数
    EVAPCOMP int NULL, -- EVAP 监测完成次数
    EVAPCOND int NULL, -- EVAP 符合监测条件次数
    EGRCOMP_08 int NULL, -- EGR和VVT 监测完成次数
    EGRCOND_08 int NULL, -- EGR和VVT 符合监测条件次数
    PFCOMP1 int NULL, -- GPF 组1 监测完成次数
    PFCOND1 int NULL, -- GPF 组1 符合监测条件次数
    PFCOMP2 int NULL, -- GPF 组2 监测完成次数
    PFCOND2 int NULL, -- GPF 组2 符合监测条件次数
    AIRCOMP int NULL, -- 二次空气喷射系统 监测完成次数
    AIRCOND int NULL, -- 二次空气喷射系统 符合监测条件次数
    -- 090B
    HCCATCOMP int NULL, -- NMHC催化器 监测完成次数
    HCCATCOND int NULL, -- NMHC催化器 符合监测条件次数
    NCATCOMP int NULL, -- NOx催化器 监测完成次数
    NCATCOND int NULL, -- NOx催化器 符合监测条件次数
    NADSCOMP int NULL, -- NOx吸附器 监测完成次数
    NADSCOND int NULL, -- NOx吸附器 符合监测条件次数
    PMCOMP int NULL, -- PM捕集器 监测完成次数
    PMCOND int NULL, -- PM捕集器 符合监测条件次数
    EGSCOMP int NULL, -- 废气传感器 监测完成次数
    EGSCOND int NULL, -- 废气传感器 符合监测条件次数
    EGRCOMP_0B int NULL, -- EGR和VVT 监测完成次数
    EGRCOND_0B int NULL, -- EGR和VVT 符合监测条件次数
    BPCOMP int NULL, -- 增压压力 监测完成次数
    BPCOND int NULL, -- 增压压力 符合监测条件次数
)
GO

-- 插入字段备注
EXEC sp_addextendedproperty N'MS_Description', N'ID', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'ID'
EXEC sp_addextendedproperty N'MS_Description', N'写入时间', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'WriteTime'
EXEC sp_addextendedproperty N'MS_Description', N'车辆VIN号', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'VIN'
EXEC sp_addextendedproperty N'MS_Description', N'ECU Response ID', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'ECU_ID'
EXEC sp_addextendedproperty N'MS_Description', N'催化器 组1 监测完成次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'CATCOMP1'
EXEC sp_addextendedproperty N'MS_Description', N'催化器 组1 符合监测条件次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'CATCOND1'
EXEC sp_addextendedproperty N'MS_Description', N'催化器 组2 监测完成次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'CATCOMP2'
EXEC sp_addextendedproperty N'MS_Description', N'催化器 组2 符合监测条件次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'CATCOND2'
EXEC sp_addextendedproperty N'MS_Description', N'前氧传感器 组1 监测完成次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'O2SCOMP1'
EXEC sp_addextendedproperty N'MS_Description', N'前氧传感器 组1 符合监测条件次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'O2SCOND1'
EXEC sp_addextendedproperty N'MS_Description', N'前氧传感器 组2 监测完成次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'O2SCOMP2'
EXEC sp_addextendedproperty N'MS_Description', N'前氧传感器 组2 符合监测条件次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'O2SCOND2'
EXEC sp_addextendedproperty N'MS_Description', N'后氧传感器 组1 监测完成次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'SO2SCOMP1'
EXEC sp_addextendedproperty N'MS_Description', N'后氧传感器 组1 符合监测条件次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'SO2SCOND1'
EXEC sp_addextendedproperty N'MS_Description', N'后氧传感器 组2 监测完成次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'SO2SCOMP2'
EXEC sp_addextendedproperty N'MS_Description', N'后氧传感器 组2 符合监测条件次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'SO2SCOND2'
EXEC sp_addextendedproperty N'MS_Description', N'EVAP 监测完成次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'EVAPCOMP'
EXEC sp_addextendedproperty N'MS_Description', N'EVAP 符合监测条件次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'EVAPCOND'
EXEC sp_addextendedproperty N'MS_Description', N'EGR和VVT 监测完成次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'EGRCOMP_08'
EXEC sp_addextendedproperty N'MS_Description', N'EGR和VVT 符合监测条件次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'EGRCOND_08'
EXEC sp_addextendedproperty N'MS_Description', N'GPF 组1 监测完成次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'PFCOMP1'
EXEC sp_addextendedproperty N'MS_Description', N'GPF 组1 符合监测条件次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'PFCOND1'
EXEC sp_addextendedproperty N'MS_Description', N'GPF 组2 监测完成次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'PFCOMP2'
EXEC sp_addextendedproperty N'MS_Description', N'GPF 组2 符合监测条件次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'PFCOND2'
EXEC sp_addextendedproperty N'MS_Description', N'二次空气喷射系统 监测完成次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'AIRCOMP'
EXEC sp_addextendedproperty N'MS_Description', N'二次空气喷射系统 符合监测条件次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'AIRCOND'
EXEC sp_addextendedproperty N'MS_Description', N'NMHC催化器 监测完成次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'HCCATCOMP'
EXEC sp_addextendedproperty N'MS_Description', N'NMHC催化器 符合监测条件次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'HCCATCOND'
EXEC sp_addextendedproperty N'MS_Description', N'NOx催化器 监测完成次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'NCATCOMP'
EXEC sp_addextendedproperty N'MS_Description', N'NOx催化器 符合监测条件次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'NCATCOND'
EXEC sp_addextendedproperty N'MS_Description', N'NOx吸附器 监测完成次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'NADSCOMP'
EXEC sp_addextendedproperty N'MS_Description', N'NOx吸附器 符合监测条件次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'NADSCOND'
EXEC sp_addextendedproperty N'MS_Description', N'PM捕集器 监测完成次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'PMCOMP'
EXEC sp_addextendedproperty N'MS_Description', N'PM捕集器 符合监测条件次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'PMCOND'
EXEC sp_addextendedproperty N'MS_Description', N'废气传感器 监测完成次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'EGSCOMP'
EXEC sp_addextendedproperty N'MS_Description', N'废气传感器 符合监测条件次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'EGSCOND'
EXEC sp_addextendedproperty N'MS_Description', N'EGR和VVT 监测完成次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'EGRCOMP_0B'
EXEC sp_addextendedproperty N'MS_Description', N'EGR和VVT 符合监测条件次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'EGRCOND_0B'
EXEC sp_addextendedproperty N'MS_Description', N'增压压力 监测完成次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'BPCOMP'
EXEC sp_addextendedproperty N'MS_Description', N'增压压力 符合监测条件次数', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'BPCOND'
GO
