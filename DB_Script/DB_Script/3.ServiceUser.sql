CREATE TABLE ServiceUser (
    Cid int,
	Cname varchar(300) not null,
    PhoneNumber VARCHAR(11) NOT NULL,
    ServiceUse VARCHAR(50) NOT NULL ,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME,
    CONSTRAINT PK_ServiceUser PRIMARY KEY (Cid),
    CONSTRAINT FK_ServiceUser_Customerid FOREIGN KEY (Cid) REFERENCES Customer (Cid),
    CONSTRAINT UQ_ServiceUser_PhoneNumber UNIQUE (PhoneNumber)
);


----StoredProcedure-----

--Read--
CREATE PROCEDURE GetAllServiceUsers
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Cid,Cname, PhoneNumber, ServiceUse, CreatedAt, UpdatedAt
    FROM ServiceUser
	order by CreatedAt desc
END

---------------------------------

CREATE PROCEDURE GetServiceUserById
    @Cid INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Cid,Cname, PhoneNumber, ServiceUse, CreatedAt, UpdatedAt
    FROM ServiceUser
    WHERE Cid = @Cid
END

----------------------------------------

CREATE PROCEDURE CreateServiceUser
    @CustomerCid INT,
	@CustomerCname varchar(300),
    @PhoneNumber VARCHAR(11),
    @ServiceUse VARCHAR(50)
   
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO ServiceUser (Cid,Cname, PhoneNumber, ServiceUse, CreatedAt, UpdatedAt)
    VALUES (@CustomerCid,@CustomerCname, @PhoneNumber, @ServiceUse, GETDATE(), GETDATE())
END

------------------------------------------------------

CREATE PROCEDURE UpdateServiceUser
    @Cid INT,
	
    @PhoneNumber VARCHAR(11),
    @ServiceUse VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ServiceUser
    SET PhoneNumber = @PhoneNumber, ServiceUse = @ServiceUse, UpdatedAt = GETDATE()
    WHERE Cid = @Cid
END

------------------------------------------------------


CREATE PROCEDURE DeleteServiceUser
    @Cid INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM ServiceUser
    WHERE Cid = @Cid
END
