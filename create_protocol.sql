USE SH_OBD
GO

-- 车型表用于存储对应车型含有的CAL_ID和CVN
IF OBJECT_ID(N'SH_OBD.dbo.OBDProtocol') IS NOT NULL
    DROP TABLE SH_OBD.dbo.OBDProtocol
GO
Create TABLE SH_OBD.dbo.OBDProtocol (
    ID int IDENTITY PRIMARY KEY NOT NULL, -- ID, 自增, 主键
    CAR_CODE varchar(20) NOT NULL, -- 车型代码
    Engine varchar(20), -- 匹配发动机
    Stage varchar(20), -- 排放阶段
    Fuel varchar(20), -- 燃油方式
    Model varchar(20), -- 车型
    Protocol varchar(50) NOT NULL, -- OBD连接协议
)
GO

-- 插入字段备注
EXEC sp_addextendedproperty N'MS_Description', N'ID', N'USER', N'dbo', N'TABLE', N'OBDProtocol', N'COLUMN', N'ID'
EXEC sp_addextendedproperty N'MS_Description', N'车型代码', N'USER', N'dbo', N'TABLE', N'OBDProtocol', N'COLUMN', N'CAR_CODE'
EXEC sp_addextendedproperty N'MS_Description', N'匹配发动机', N'USER', N'dbo', N'TABLE', N'OBDProtocol', N'COLUMN', N'Engine'
EXEC sp_addextendedproperty N'MS_Description', N'排放阶段', N'USER', N'dbo', N'TABLE', N'OBDProtocol', N'COLUMN', N'Stage'
EXEC sp_addextendedproperty N'MS_Description', N'燃油方式', N'USER', N'dbo', N'TABLE', N'OBDProtocol', N'COLUMN', N'Fuel'
EXEC sp_addextendedproperty N'MS_Description', N'车型', N'USER', N'dbo', N'TABLE', N'OBDProtocol', N'COLUMN', N'Model'
EXEC sp_addextendedproperty N'MS_Description', N'OBD连接协议', N'USER', N'dbo', N'TABLE', N'OBDProtocol', N'COLUMN', N'Protocol'
GO

-- 插入数据
INSERT SH_OBD.dbo.OBDProtocol VALUES ('JX6490TB5', '4USG', '国五', '柴油', '撼路者', 'ISO 15031 CAN')
INSERT SH_OBD.dbo.OBDProtocol VALUES ('JX6491TA5', '4USG', '国五', '柴油', '撼路者', 'ISO 15031 CAN')
INSERT SH_OBD.dbo.OBDProtocol VALUES ('JX1031TFA5', 'DURATORQ4D205L', '国五', '柴油', 'V362', 'ISO 15031 CAN')
GO
