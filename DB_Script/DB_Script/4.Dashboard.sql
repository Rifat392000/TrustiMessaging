
create proc totalServiceUserCount
as
begin
select count(Cid) as totalServiceUser
from ServiceUser
end

exec totalServiceUserCount;

------------------------------------------------------------
--learn from Anwar bhi (TBL) and Sohel Bhi (Flora)


SELECT value,
Cid,Cname, PhoneNumber, ServiceUse, CreatedAt, UpdatedAt
    FROM ServiceUser
	cross apply(select value from string_split(ServiceUse,','))T


create proc differentServices
as
begin
SELECT  
value,
COUNT(value) as totalcount
    FROM ServiceUser
	cross apply(select value from string_split(ServiceUse,','))T
	group by value
end

exec differentServices