USE SH_OBD
GO
-- OBD数据表
IF OBJECT_ID(N'SH_OBD.dbo.OBDUser') IS NOT NULL
    DROP TABLE SH_OBD.dbo.OBDUser
GO
CREATE TABLE SH_OBD.dbo.OBDUser (
    ID int IDENTITY PRIMARY KEY NOT NULL, -- ID, 自增, 主键
    UserName varchar(20) NOT NULL, -- 用户名
    PassWord varchar(32) NOT NULL, -- 密码
)
GO

-- 插入字段备注
EXEC sp_addextendedproperty N'MS_Description', N'ID', N'USER', N'dbo', N'TABLE', N'OBDUser', N'COLUMN', N'ID'
EXEC sp_addextendedproperty N'MS_Description', N'用户名', N'USER', N'dbo', N'TABLE', N'OBDUser', N'COLUMN', N'UserName'
EXEC sp_addextendedproperty N'MS_Description', N'密码', N'USER', N'dbo', N'TABLE', N'OBDUser', N'COLUMN', N'PassWord'
GO

-- 测试: 插入数据
INSERT SH_OBD.dbo.OBDUser
    VALUES (
        'admin',
        '897F3ECBECFD3C2A32D29ACBD8D10ADD' -- 默认密码 JMCOBD
    )
GO
