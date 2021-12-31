CREATE DATABASE $(RDMG_DB);
GO
USE $(RDMG_DB);
GO
CREATE LOGIN $(RDMG_USER) WITH PASSWORD = '$(RDMG_PASSWORD)';
GO
CREATE USER $(RDMG_USER) FOR LOGIN $(RDMG_USER);
GO
ALTER SERVER ROLE sysadmin ADD MEMBER [$(RDMG_USER)];
GO