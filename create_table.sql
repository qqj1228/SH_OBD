USE SH_OBD
GO
-- OBD数据表
IF OBJECT_ID(N'SH_OBD.dbo.OBDData') IS NOT NULL
    DROP TABLE SH_OBD.dbo.OBDData
GO
CREATE TABLE SH_OBD.dbo.OBDData (
    ID int IDENTITY PRIMARY KEY NOT NULL, -- ID, 自增, 主键
    WriteTime datetime NOT NULL default(getdate()), -- 写入时间
    VIN varchar(17) NOT NULL, -- 车辆VIN号，0902
    ECU_ID varchar(8) NOT NULL, -- 车型代码与排放相关的ECU Response ID
    MIL varchar(50) default('不适用'), -- MIL灯状态，0101
    MIL_DIST varchar(50) default('不适用'), -- MIL亮后行驶里程（km），0121
    OBD_SUP varchar(50) default('不适用'), -- OBD型式检验类型，011C
    ODO varchar(50) default('不适用'), -- 总累计里程ODO（km），01A6
    DTC03 varchar(50) default('--'), -- 存储DTC，03
    DTC07 varchar(50) default('--'), -- 未决DTC，07
    DTC0A varchar(50) default('--'), -- 永久DTC，0A
    MIS_RDY varchar(50) default('不适用'), -- 失火监测，0101
    FUEL_RDY varchar(50) default('不适用'), -- 燃油系统监测，0101
    CCM_RDY varchar(50) default('不适用'), -- 综合组件监测，0101
    CAT_RDY varchar(50) default('不适用'), -- 催化剂监测，0101
    HCAT_RDY varchar(50) default('不适用'), -- 加热催化剂监测，0101
    EVAP_RDY varchar(50) default('不适用'), -- 燃油蒸发系统监测，0101
    AIR_RDY varchar(50) default('不适用'), -- 二次空气系统监测，0101
    ACRF_RDY varchar(50) default('不适用'), -- 空调系统制冷剂监测，0101
    O2S_RDY varchar(50) default('不适用'), -- 氧气传感器监测，0101
    HTR_RDY varchar(50) default('不适用'), -- 加热氧气传感器监测，0101
    EGR_RDY varchar(50) default('不适用'), -- EGR/VVT系统监测，0101
    HCCAT_RDY varchar(50) default('不适用'), -- NMHC催化剂监测，0101
    NCAT_RDY varchar(50) default('不适用'), -- NOx/SCR后处理监测，0101
    BP_RDY varchar(50) default('不适用'), -- 增压系统监测，0101
    EGS_RDY varchar(50) default('不适用'), -- 废气传感器监测，0101
    PM_RDY varchar(50) default('不适用'), -- PM过滤监测，0101
    ECU_NAME varchar(50) default('不适用'), -- ECU名称，090A
    CAL_ID varchar(50) default('不适用'), -- CAL_ID，0904
    CVN varchar(50) default('不适用'), -- CVN，0906
	Result int default(0), -- OBD检测结果，1 - 合格，0 - 不合格
)
GO

-- 插入字段备注
EXEC sp_addextendedproperty N'MS_Description', N'ID', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'ID'
EXEC sp_addextendedproperty N'MS_Description', N'写入时间', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'WriteTime'
EXEC sp_addextendedproperty N'MS_Description', N'车辆VIN号', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'VIN'
EXEC sp_addextendedproperty N'MS_Description', N'ECU Response ID', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'ECU_ID'
EXEC sp_addextendedproperty N'MS_Description', N'MIL灯状态', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'MIL'
EXEC sp_addextendedproperty N'MS_Description', N'MIL亮后行驶里程（km）', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'MIL_DIST'
EXEC sp_addextendedproperty N'MS_Description', N'OBD型式检验类型', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'OBD_SUP'
EXEC sp_addextendedproperty N'MS_Description', N'总累计里程ODO（km）', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'ODO'
EXEC sp_addextendedproperty N'MS_Description', N'存储DTC', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'DTC03'
EXEC sp_addextendedproperty N'MS_Description', N'未决DTC', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'DTC07'
EXEC sp_addextendedproperty N'MS_Description', N'永久DTC', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'DTC0A'
EXEC sp_addextendedproperty N'MS_Description', N'失火监测', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'MIS_RDY'
EXEC sp_addextendedproperty N'MS_Description', N'燃油系统监测', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'FUEL_RDY'
EXEC sp_addextendedproperty N'MS_Description', N'综合组件监测', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'CCM_RDY'
EXEC sp_addextendedproperty N'MS_Description', N'催化剂监测', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'CAT_RDY'
EXEC sp_addextendedproperty N'MS_Description', N'加热催化剂监测', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'HCAT_RDY'
EXEC sp_addextendedproperty N'MS_Description', N'燃油蒸发系统监测', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'EVAP_RDY'
EXEC sp_addextendedproperty N'MS_Description', N'二次空气系统监测', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'AIR_RDY'
EXEC sp_addextendedproperty N'MS_Description', N'空调系统制冷剂监测', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'ACRF_RDY'
EXEC sp_addextendedproperty N'MS_Description', N'氧气传感器监测', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'O2S_RDY'
EXEC sp_addextendedproperty N'MS_Description', N'加热氧气传感器监测', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'HTR_RDY'
EXEC sp_addextendedproperty N'MS_Description', N'EGR/VVT系统监测', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'EGR_RDY'
EXEC sp_addextendedproperty N'MS_Description', N'NMHC催化剂监测', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'HCCAT_RDY'
EXEC sp_addextendedproperty N'MS_Description', N'NOx/SCR后处理监测', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'NCAT_RDY'
EXEC sp_addextendedproperty N'MS_Description', N'增压系统监测', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'BP_RDY'
EXEC sp_addextendedproperty N'MS_Description', N'废气传感器监测', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'EGS_RDY'
EXEC sp_addextendedproperty N'MS_Description', N'PM过滤监测', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'PM_RDY'
EXEC sp_addextendedproperty N'MS_Description', N'ECU名称', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'ECU_NAME'
EXEC sp_addextendedproperty N'MS_Description', N'CAL_ID', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'CAL_ID'
EXEC sp_addextendedproperty N'MS_Description', N'CVN', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'CVN'
EXEC sp_addextendedproperty N'MS_Description', N'OBD检测结果', N'USER', N'dbo', N'TABLE', N'OBDData', N'COLUMN', N'Result'
GO

-- 测试: 插入数据
INSERT SH_OBD.dbo.OBDData
    VALUES (
        '2222-09-16 18:18:18',
        'testvincode012345',
        '7E0',
        'OFF',
        '0',
        'CN-OBD-6',
        '0',
        '--',
        '--',
        '--',
        '完成',
        '完成',
        '完成',
        '完成',
        '完成',
        '完成',
        '完成',
        '完成',
        '完成',
        '完成',
        '完成',
        '不适用',
        '不适用',
        '不适用',
        '不适用',
        '不适用',
        'ECM-EngineControl',
        'JMB*36761500,JMB*47872611',
        '1791BC82,16E062BE',
		'1'
    )
GO
-- 测试: 修改数据
UPDATE SH_OBD.dbo.OBDData
    SET ECU_ID = '7E8'
    WHERE VIN = 'testvincode012345'
GO
-- 测试: 查询数据
SELECT VIN, ECU_ID
    FROM SH_OBD.dbo.OBDData
GO

