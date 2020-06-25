Create Table UserInfo(
UserId Int Identity(1,1) Not null Primary Key,
FirstName Varchar(50) Not null,
LastName Varchar(50) Not null,
UserName Varchar(30) Not null,
Email Varchar(100) Not null,
Password Varchar(128) Not null,
CreatedDate DateTime Default(GetDate()) Not Null)
GO
Insert Into UserInfo(FirstName, LastName, UserName, Email, Password) 
Values ('System', 'Admin', 'SysAdmin', 'admin@abcSystems.com', '$adminAbcd!')
