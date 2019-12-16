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
    EGRCOMP int NULL, -- EVAP 监测完成次数
    EGRCOND int NULL, -- EVAP 符合监测条件次数
    GPFCOMP1 int NULL, -- GPF 组1 监测完成次数
    GPFCOND1 int NULL, -- GPF 组1 符合监测条件次数
    GPFCOMP2 int NULL, -- GPF 组2 监测完成次数
    GPFCOND2 int NULL, -- GPF 组2 符合监测条件次数
)
GO

-- 插入字段备注
EXEC sp_addextendedproperty N'MS_Description', N'ID', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'ID'
EXEC sp_addextendedproperty N'MS_Description', N'写入时间', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'WriteTime'
EXEC sp_addextendedproperty N'MS_Description', N'车辆VIN号', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'VIN'
EXEC sp_addextendedproperty N'MS_Description', N'ECU Response ID', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'ECU_ID'
EXEC sp_addextendedproperty N'MS_Description', N'CAL_ID', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'CAL_ID'
EXEC sp_addextendedproperty N'MS_Description', N'CVN', N'USER', N'dbo', N'TABLE', N'OBDIUPR', N'COLUMN', N'CVN'
GO
