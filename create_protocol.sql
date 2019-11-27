USE SH_OBD
GO

-- 车型表用于存储对应车型含有的CAL_ID和CVN
IF OBJECT_ID(N'SH_OBD.dbo.OBDProtocol') IS NOT NULL
    DROP TABLE SH_OBD.dbo.OBDProtocol
GO
Create TABLE SH_OBD.dbo.OBDProtocol (
    ID int IDENTITY PRIMARY KEY NOT NULL, -- ID, 自增, 主键
    CAR_CODE varchar(20) NOT NULL, -- 车型代码
    Protocol varchar(50) NOT NULL, -- OBD连接协议
)
GO

-- 插入字段备注
EXEC sp_addextendedproperty N'MS_Description', N'ID', N'USER', N'dbo', N'TABLE', N'OBDProtocol', N'COLUMN', N'ID'
EXEC sp_addextendedproperty N'MS_Description', N'车型代码', N'USER', N'dbo', N'TABLE', N'OBDProtocol', N'COLUMN', N'CAR_CODE'
EXEC sp_addextendedproperty N'MS_Description', N'OBD连接协议', N'USER', N'dbo', N'TABLE', N'OBDProtocol', N'COLUMN', N'Protocol'
GO

-- 插入数据
INSERT SH_OBD.dbo.OBDProtocol VALUES ('JX6490TB5', 'ISO 15765-4 CAN / ISO 15031')
INSERT SH_OBD.dbo.OBDProtocol VALUES ('JX6491TA5', 'ISO 15765-4 CAN / ISO 15031')
INSERT SH_OBD.dbo.OBDProtocol VALUES ('JX1031TFA5', 'ISO 15765-4 CAN / ISO 15031')
GO
