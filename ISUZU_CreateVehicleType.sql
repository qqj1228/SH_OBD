USE SH_OBD
GO

-- 车型表用于存储对应车型含有的CAL_ID和CVN
IF OBJECT_ID(N'SH_OBD.dbo.VehicleType') IS NOT NULL
    DROP TABLE SH_OBD.dbo.VehicleType
GO
Create TABLE SH_OBD.dbo.VehicleType (
    ID int IDENTITY PRIMARY KEY NOT NULL, -- ID, 自增, 主键
    Project varchar(20) NOT NULL, -- 项目号
    Type varchar(20) NOT NULL, -- 车型代码
    ECU_ID varchar(8) NOT NULL, -- 与排放相关的ECU Response ID
    CAL_ID varchar(50) NULL,
    CVN varchar(50) NULL,
)
GO

-- 插入字段备注
EXEC sp_addextendedproperty N'MS_Description', N'ID', N'USER', N'dbo', N'TABLE', N'VehicleType', N'COLUMN', N'ID'
EXEC sp_addextendedproperty N'MS_Description', N'项目号', N'USER', N'dbo', N'TABLE', N'VehicleType', N'COLUMN', N'Project'
EXEC sp_addextendedproperty N'MS_Description', N'车型代码', N'USER', N'dbo', N'TABLE', N'VehicleType', N'COLUMN', N'Type'
EXEC sp_addextendedproperty N'MS_Description', N'ECU Response ID', N'USER', N'dbo', N'TABLE', N'VehicleType', N'COLUMN', N'ECU_ID'
EXEC sp_addextendedproperty N'MS_Description', N'CAL_ID', N'USER', N'dbo', N'TABLE', N'VehicleType', N'COLUMN', N'CAL_ID'
EXEC sp_addextendedproperty N'MS_Description', N'CVN', N'USER', N'dbo', N'TABLE', N'VehicleType', N'COLUMN', N'CVN'
GO

-- 插入数据
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA18', 'JXW1032FSA', '7E8', '2564ENF1JM564011', 'B2BA3E6F')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA18', 'JXW1032FSA', '7E8', '2564EMF1JM564021', '31369D5A')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA18', 'JXW1033FSB', '7E8', '2564ENF1JM564011', 'B2BA3E6F')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA18', 'JXW1033FSB', '7E8', '2564EMF1JM564021', '31369D5A')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA18', 'JXW1032FSAA', '7E8', '2564ENF1JM564011', 'B2BA3E6F')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA18', 'JXW1032FSAA', '7E8', '2564EMF1JM564021', '31369D5A')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA18', 'JXW1033FSBA', '7E8', '2564ENF1JM564011', 'B2BA3E6F')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA18', 'JXW1033FSBA', '7E8', '2564EMF1JM564021', '31369D5A')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA18', 'JXW5032XXYFSG', '7E8', '2564ENF1JM564011', 'B2BA3E6F')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA18', 'JXW5032XXYFSG', '7E8', '2564EMF1JM564021', '31369D5A')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA18', 'JXW5033XXYFSG', '7E8', '2564ENF1JM564011', 'B2BA3E6F')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA18', 'JXW5033XXYFSG', '7E8', '2564EMF1JM564021', '31369D5A')
GO
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA19', 'JXW1033CSG', '7E8', 'MD1CS089 01', 'A59F33E8')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA19', 'JXW1034CSG', '7E8', 'MD1CS089 01', 'A59F33E8')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA19', 'JXW5032XXYCSG', '7E8', 'MD1CS089 01', 'A59F33E8')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA19', 'JXW5033XXYCSG', '7E8', 'MD1CS089 01', 'A59F33E8')
INSERT SH_OBD.dbo.VehicleType VALUES ('PT017', 'JXW1030CSGB', '7E8', 'MD1CS089 01', 'A59F33E8')
INSERT SH_OBD.dbo.VehicleType VALUES ('PT017', 'JXW1031CSGB', '7E8', 'MD1CS089 01', 'A59F33E8')
INSERT SH_OBD.dbo.VehicleType VALUES ('PT017', 'JXW5030XXYCSG', '7E8', 'MD1CS089 01', 'A59F33E8')
INSERT SH_OBD.dbo.VehicleType VALUES ('PT017', 'JXW5031XXYCSG', '7E8', 'MD1CS089 01', 'A59F33E8')
GO
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA06', 'JXW1030CSG', '7E8', 'MD1CS089 MT 01', 'A4E1462E')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA06', 'JXW1031CSG', '7E8', 'MD1CS089 MT 01', 'A4E1462E')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA06', 'JXW1031CSG', '7E8', 'MD1CS089 MT 01', 'CAD05AB0')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA06', 'JXW1030CSGA', '7E8', 'MD1CS089 AT 01', '73BF92F7')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA06', 'JXW1030CSGA', '7EA', '99383-07444', '0000F10C')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA06', 'JXW1031CSGA', '7E8', 'MD1CS089 AT 01', '73BF92F7')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA06', 'JXW1031CSGA', '7EA', '99383-07443', '00007320')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA06', 'JXW5030XXYCSGA', '7E8', 'MD1CS089 MT 01', 'A4E1462E')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA06', 'JXW5030XXYCSGB', '7E8', 'MD1CS089 AT 01', '73BF92F7')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA06', 'JXW5030XXYCSGB', '7EA', '99383-07444', '0000F10C')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA06', 'JXW5031XXYCSGA', '7E8', 'MD1CS089 MT 01', 'A4E1462E')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA06', 'JXW5031XXYCSGA', '7E8', 'MD1CS089 MT 01', 'CAD05AB0')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA06', 'JXW5031XXYCSGB', '7E8', 'MD1CS089 AT 01', '73BF92F7')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA06', 'JXW5031XXYCSGB', '7EA', '99383-07443', '00007320')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA06', 'JXW5031XXYCSGC', '7E8', 'MD1CS089 AT 01', '73BF92F7')
INSERT SH_OBD.dbo.VehicleType VALUES ('PPA06', 'JXW5031XXYCSGC', '7EA', '99383-07443', '00007320')
GO
INSERT SH_OBD.dbo.VehicleType VALUES ('PSA07', 'JXW6480CSE', '7E8', 'MD1CS089 AT 01', 'FFB1EF17')
INSERT SH_OBD.dbo.VehicleType VALUES ('PSA07', 'JXW6480CSE', '7EA', '99383-07444', '0000F10C')
INSERT SH_OBD.dbo.VehicleType VALUES ('PSA07', 'JXW6480CSE', '7E8', 'MD1CS089 AT 01', '73BF92F7')
INSERT SH_OBD.dbo.VehicleType VALUES ('PSA07', 'JXW6480CSEA', '7E8', 'MD1CS089 AT 01', 'FFB1EF17')
INSERT SH_OBD.dbo.VehicleType VALUES ('PSA07', 'JXW6480CSEA', '7EA', '99383-07444', '0000F10C')
INSERT SH_OBD.dbo.VehicleType VALUES ('PSA07', 'JXW6480CSEA', '7E8', 'MD1CS089 AT 01', '73BF92F7')
INSERT SH_OBD.dbo.VehicleType VALUES ('PSA07', 'JXW6481CSE', '7E8', 'MD1CS089 AT 01', 'FFB1EF17')
INSERT SH_OBD.dbo.VehicleType VALUES ('PSA07', 'JXW6481CSE', '7EA', '99383-07443', '00007320')
INSERT SH_OBD.dbo.VehicleType VALUES ('PSA07', 'JXW6481CSE', '7E8', 'MD1CS089 AT 01', '73BF92F7')
INSERT SH_OBD.dbo.VehicleType VALUES ('PSA07', 'JXW6481CSEA', '7E8', 'MD1CS089 AT 01', 'FFB1EF17')
INSERT SH_OBD.dbo.VehicleType VALUES ('PSA07', 'JXW6481CSEA', '7EA', '99383-07443', '00007320')
INSERT SH_OBD.dbo.VehicleType VALUES ('PSA07', 'JXW6481CSEA', '7E8', 'MD1CS089 AT 01', '73BF92F7')
GO