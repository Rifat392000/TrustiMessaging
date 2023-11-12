create database OnlineMessageManagement;
use OnlineMessageManagement;

create table SocialService(
ServiceId int identity,
ServiceName varchar(50) not null,
ServiceStatus int not null default 1,
  CONSTRAINT ck_status CHECK (ServiceStatus IN (1,0))
);
----StoredProcedure-----


CREATE PROCEDURE GetAllSocialServices
AS
BEGIN
    SELECT ServiceId, ServiceName,ServiceStatus
    FROM SocialService;
END


---------------------------------------------------------


CREATE PROCEDURE AddSocialService  @ServiceName varchar(50)
AS
BEGIN
insert into SocialService (ServiceName) values (@ServiceName)
END


---------------------------------

CREATE PROCEDURE UpdateServiceStatus(
    @ServiceId INT,
    @ServiceStatus INT)
AS
BEGIN
    UPDATE SocialService
    SET ServiceStatus = @ServiceStatus
    WHERE ServiceId = @ServiceId;
END

------------------------------------------
CREATE PROCEDURE UpdateService(
    @ServiceId INT,
    @ServiceName varchar(50))
AS
BEGIN
    UPDATE SocialService
    SET ServiceName = @ServiceName
    WHERE ServiceId = @ServiceId;
END

--------------------------------------------

CREATE PROCEDURE DeleteService(
    @ServiceId INT)
AS
BEGIN
     DELETE FROM SocialService
    WHERE ServiceId = @ServiceId;
END

----------------------------------------------------

CREATE PROCEDURE GetServiceById( @ServiceId INT)
AS
BEGIN
select  ServiceId, ServiceName
FROM SocialService
    WHERE ServiceId = @ServiceId;
end

-------------------------------------------------------------

create procedure chkserviceisrunning @ServiceId int
AS
BEGIN
select ServiceStatus
from SocialService
where ServiceId=@ServiceId
END

--EXEC chkserviceisrunning 2

