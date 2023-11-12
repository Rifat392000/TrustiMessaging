
create table userType
(
userId int not null primary key ,
userRoll varchar(50)  not null unique
)




create table Logpannel(
login_id numeric(4) not null primary key identity(1000,1),
name varchar(250) not null,
userId int not null,
password nvarchar(100) not null,
  CONSTRAINT FK_userType FOREIGN KEY (userId) REFERENCES userType (userId)
)



-------------------------------------------------------------------------

CREATE PROCEDURE userLogin
(@login_id numeric(4),@password nvarchar(100))
AS
BEGIN
select name   from Logpannel
where login_id=@login_id AND password=@password
END

--------------------------------------------------------


CREATE PROCEDURE userLoginfo
(@login_id numeric(4))
AS
BEGIN
select name,login_id,userId  from Logpannel
where login_id=@login_id 
END


---------------------------------------------------------------------------------

