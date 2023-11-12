
--App User
--A column function produces a single value for a group of rows
--learn from Anwar Bhi
create proc AppUser (@serviceId int)
as
begin
SELECT 
Cid,Cname, PhoneNumber,ServiceName,ServiceId
    FROM ServiceUser
	cross apply(select value from string_split(ServiceUse,','))T, SocialService
 where value=@serviceId and SocialService.ServiceId=@serviceId
 end
GO

--WhatsApp
exec AppUser 1;

