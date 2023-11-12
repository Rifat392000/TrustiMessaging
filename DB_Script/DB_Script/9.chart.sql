--Message Status
--0 = Procesing
--1 = Pending
--2 = Success
--(-1) = Failed

CREATE PROCEDURE singleMsgInfoByService
AS
BEGIN
select ServiceName, count(1) as total,
count(CASE WHEN messageStatus=0 THEN 1 END) as process,
count(CASE WHEN messageStatus=1 AND checkedmsg=1 THEN 1 END) as pending,
count(CASE WHEN messageStatus=2 AND checkedmsg=1 THEN 1 END) as success,
count(CASE WHEN messageStatus=-1 AND checkedmsg=1 THEN 1 END) as failed
from singleMessage 
group by ServiceId,ServiceName
order by ServiceId asc
END



-----------------------------------------------------------------------------



CREATE PROC bulkMsgInfoByService
AS
BEGIN
select ServiceName, count(1) as total,
count(CASE WHEN messageStatus=0 THEN 1 END) as process,
count(CASE WHEN messageStatus=1 AND checkedmsg=1 THEN 1 END) as pending,
count(CASE WHEN messageStatus=2 AND checkedmsg=1 THEN 1 END) as success,
count(CASE WHEN messageStatus=-1 AND checkedmsg=1 THEN 1 END) as failed
from bulkMessage 
group by ServiceId,ServiceName
order by ServiceId asc
END


-----------------------------------------------------------------------------------


create proc messageSendTimeBulk
AS
BEGIN
SELECT FORMAT(msgSend, 'yyyy-MM-dd HH:mm') AS distinct_date,count(1) AS num_msg_per_min
FROM bulkMessage
where FORMAT(msgSend, 'yyyy-MM-dd')=FORMAT(GETDATE(),'yyyy-MM-dd') AND checkedmsg=1 AND messageStatus=2 
group by FORMAT(msgSend, 'yyyy-MM-dd HH:mm')
order by FORMAT(msgSend, 'yyyy-MM-dd HH:mm') desc
END

EXEC messageSendTimeBulk
------------------------------------------------------------------


create proc messageSendTimeSingle
AS
BEGIN
SELECT FORMAT(msgSend, 'yyyy-MM-dd HH:mm') AS distinct_date,count(1) AS num_msg_per_min
FROM singleMessage 
where FORMAT(msgSend, 'yyyy-MM-dd')=FORMAT(GETDATE(),'yyyy-MM-dd') AND checkedmsg=1 AND messageStatus=2
group by FORMAT(msgSend, 'yyyy-MM-dd HH:mm')
order by FORMAT(msgSend, 'yyyy-MM-dd HH:mm') desc
END

EXEC messageSendTimeSingle
----------------------------------------------------------------------------


