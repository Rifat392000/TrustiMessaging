
--Message Status
--0 = Procesing
--1 = Pending
--2 = Success
--(-1) = Failed
--ctrl k+c  for command Visual Studio and SSMS

create table singleMessage(
    logSingleMessage int identity not null primary key,
	checkedmsg int not null default 0,
    Cid int not null,
	Cname varchar(300) not null,
    PhoneNumber VARCHAR(11) NOT NULL,
	ServiceName varchar(50) not null,
	ServiceId int not null,
	CustomerMessage nvarchar(max),
	messageStatus int not null default 0,
	msgCreated DATETIME DEFAULT GETDATE(),
    msgSend DATETIME  DEFAULT GETDATE(),
	FOREIGN KEY (Cid) REFERENCES Customer(Cid),
	CONSTRAINT ck_singleMessage CHECK (checkedmsg  IN (1,0))
	
);




select * from singleMessage

------------------------------------------------------------------------

CREATE PROCEDURE InsertSingleMessage
(
    @Cid INT,
    @Cname VARCHAR(300),
    @PhoneNumber VARCHAR(11),
    @ServiceName VARCHAR(50),
	@ServiceId INT,
    @CustomerMessage NVARCHAR(MAX)
   
)
AS
BEGIN
    INSERT INTO singleMessage (Cid, Cname, PhoneNumber, ServiceName,ServiceId, CustomerMessage)
    VALUES (@Cid, @Cname, @PhoneNumber, @ServiceName,@ServiceId, @CustomerMessage)
END
GO


--------------------------------------------------------------------------------------------

CREATE PROCEDURE GetSingleMessage
AS
BEGIN
select Cid,Cname,PhoneNumber,ServiceName,CustomerMessage,messageStatus,msgCreated,msgSend
from singleMessage
order by msgSend desc
END
GO


EXECUTE GetSingleMessage 



-------------------------------------------------------------------------------------------





CREATE PROCEDURE SingleMessageUnchecked
AS
BEGIN
select logSingleMessage,Cid,Cname,PhoneNumber,ServiceName,CustomerMessage,messageStatus,msgCreated
from singleMessage
where checkedmsg=0 AND messageStatus=0
order by msgCreated desc
END


EXEC SingleMessageUnchecked


--------------------------------------------------------------------------

CREATE PROCEDURE SingleMessagechecked (@logSingleMessage int,@messageStatus int,@checkedmsg int)
AS
BEGIN
UPDATE singleMessage
SET checkedmsg=1,messageStatus=1
where logSingleMessage=@logSingleMessage  AND @checkedmsg=0 AND @messageStatus=0
END





-------------------------------------------------------------------------


CREATE PROC MsgSendSingle
AS
BEGIN
select TOP 20 logSingleMessage,PhoneNumber,CustomerMessage,ServiceId
from singleMessage
where checkedmsg=1 and messageStatus=1 and ServiceId=1
order by logSingleMessage asc
END

exec MsgSendSingle

--------------------------------------------------------------

CREATE PROC FinalStatusMsgSingle @logSingleMessage int, @messageStatus int
AS
BEGIN
update singleMessage
set messageStatus=@messageStatus, msgSend=GETDATE()
where logSingleMessage=@logSingleMessage AND checkedmsg=1 AND messageStatus=1 AND ServiceId=1
END


--exec FinalStatusMsgSingle 16 ,2