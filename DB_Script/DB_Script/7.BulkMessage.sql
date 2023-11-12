
--Message Status
--0 = Procesing
--1 = Pending
--2 = Success
--(-1) = Failed


create table bulkMessage(
 logSingleMessage int identity not null primary key,
 checkedmsg int not null default 0,
    Cid int,
	Cname varchar(300) not null,
    PhoneNumber VARCHAR(11) NOT NULL,
   ServiceName varchar(50) not null,
   ServiceId int,
   CustomerMessage nvarchar(max),
   messageStatus int not null default 0,
	msgCreated DATETIME DEFAULT GETDATE(),
    msgSend DATETIME  DEFAULT GETDATE(),
	FOREIGN KEY (Cid) REFERENCES Customer(Cid),
	CONSTRAINT ck_bulkMessage CHECK (checkedmsg  IN (1,0))
   )

   select * from bulkMessage


---------------------------------------------------------------------------

create proc BulkAppUser (@serviceId int)
as
begin
SELECT 
Cid,Cname, PhoneNumber,ServiceName,ServiceId
    FROM ServiceUser
	cross apply(select value from string_split(ServiceUse,','))T, SocialService
 where value=@serviceId and SocialService.ServiceId=@serviceId
 end
GO



--------------------------------------------------------------------------

   create proc InsertbulkMessage(@ServiceId int,@CustomerMessage nvarchar(max))
   as
   begin
   insert into bulkMessage(Cid,Cname,PhoneNumber,ServiceName,ServiceId) exec BulkAppUser @ServiceId;

   update bulkMessage
   set CustomerMessage = @CustomerMessage
   where CustomerMessage is null
   end

   ----------------------------------------------------------------------------------------------
  -- exec InsertbulkMessage 1,'Hello This Whatsapp'

  -- exec InsertbulkMessage 2,'Hello This Viber'

   -----------------------------------------------------------------------------------------

   CREATE PROCEDURE GetbulkMessage
AS
BEGIN
select Cid,Cname,PhoneNumber,ServiceName,CustomerMessage,messageStatus,msgCreated,msgSend
from bulkMessage
order by msgSend desc
END
GO


EXECUTE GetbulkMessage 



-------------------------------------------------------------------------------------------





CREATE PROCEDURE bulkMessageUnchecked
AS
BEGIN
select logSingleMessage,Cid,Cname,PhoneNumber,ServiceName,CustomerMessage,messageStatus,msgCreated
from bulkMessage
where checkedmsg=0 AND messageStatus=0
order by msgCreated desc
END


EXEC bulkMessageUnchecked


--------------------------------------------------------------------------

CREATE PROCEDURE bulkMessagechecked (@logSingleMessage int,@messageStatus int,@checkedmsg int)
AS
BEGIN
UPDATE bulkMessage
SET checkedmsg=1,messageStatus=1
where logSingleMessage=@logSingleMessage  AND @checkedmsg=0 AND @messageStatus=0
END





-------------------------------------------------------------------------


CREATE PROC MsgSendBulk
AS
BEGIN
select TOP 20 logSingleMessage,PhoneNumber,CustomerMessage,ServiceId
from bulkMessage
where checkedmsg=1 and messageStatus=1 and ServiceId=1
order by logSingleMessage asc
END

exec MsgSendBulk

--------------------------------------------------------------------------

CREATE PROC FinalStatusMsgBulk @logSingleMessage int, @messageStatus int
AS
BEGIN
update bulkMessage
set messageStatus=@messageStatus, msgSend=GETDATE()
where logSingleMessage=@logSingleMessage AND checkedmsg=1 AND messageStatus=1 AND ServiceId=1
END

----------------------------------------------------------

