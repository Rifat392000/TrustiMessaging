create table Customer(
Cid int identity(501,1) primary key,
Cname varchar(300) not null
);

select * from Customer;

--------------------------------------------

CREATE PROCEDURE GetAvailableCustomers
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Cid, Cname
    FROM Customer
    WHERE Cid NOT IN (SELECT Cid FROM ServiceUser)
END


---------------------------------------------

CREATE PROCEDURE GetCustomerById
    @Cid INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Cid, Cname
    FROM Customer
    WHERE Cid = @Cid
END
