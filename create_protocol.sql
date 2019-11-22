USE SH_OBD
GO

-- 车型表用于存储对应车型含有的CAL_ID和CVN
IF OBJECT_ID(N'SH_OBD.dbo.OBDProtocol') IS NOT NULL
    DROP TABLE SH_OBD.dbo.OBDProtocol
GO
Create TABLE SH_OBD.dbo.OBDProtocol (
    ID int IDENTITY PRIMARY KEY NOT NULL, -- ID, 自增, 主键
    CAR_MODEL varchar(20) NOT NULL, -- 车型
    Protocol_NUM varchar(1) NOT NULL, -- ELM327连接OBD协议代号
)
GO

-- 插入字段备注
EXEC sp_addextendedproperty N'MS_Description', N'ID', N'USER', N'dbo', N'TABLE', N'OBDProtocol', N'COLUMN', N'ID'
EXEC sp_addextendedproperty N'MS_Description', N'车型', N'USER', N'dbo', N'TABLE', N'OBDProtocol', N'COLUMN', N'CAR_MODEL'
EXEC sp_addextendedproperty N'MS_Description', N'ELM327连接OBD协议代号', N'USER', N'dbo', N'TABLE', N'OBDProtocol', N'COLUMN', N'Protocol_NUM'
GO

-- 插入数据
INSERT SH_OBD.dbo.OBDProtocol VALUES ('testmodel', '6')
GO
