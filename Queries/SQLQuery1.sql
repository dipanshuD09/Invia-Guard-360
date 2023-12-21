CREATE DATABASE Live_ProjectDb;
USE Live_ProjectDb;

-- Plans table
CREATE TABLE plans
  (
     id            INT PRIMARY KEY,
     plan_name     NVARCHAR(255) NOT NULL,
     plan_validity INT NOT NULL,
     plan_amount   DECIMAL(10, 2) NOT NULL,
     max_rounds    INT NOT NULL
  ); 

  select * from Users

-- Customer Table
CREATE TABLE customers
  (
     id            INT PRIMARY KEY IDENTITY (1, 1),
     email         NVARCHAR(255) NOT NULL UNIQUE,
     first_name    NVARCHAR(255) NOT NULL,
     last_name     NVARCHAR(255) NOT NULL,
     city          NVARCHAR(255),
     country       NVARCHAR(255),
     address       NVARCHAR(255),
     current_plan  INT,
     transactionid INT,
     planid        INT,
     organization  NVARCHAR(255),
     created_date  DATETIME
  );

-- Transactions table
CREATE TABLE transactions
  (
     id             INT PRIMARY KEY IDENTITY (1, 1),
     transaction_id INT NOT NULL,
     plan_start     DATETIME NOT NULL,
     plan_end       DATETIME NOT NULL,
     current_round  INT NOT NULL,
     islatest       BIT NOT NULL,
     customerid     INT NOT NULL,
     planid         INT NOT NULL
  ); 

-- Users table
CREATE TABLE users
  (
     id         INT PRIMARY KEY IDENTITY (1, 1),
     username   NVARCHAR(255) NOT NULL,
     password   NVARCHAR(255) NOT NULL,
     iscustomer BIT NOT NULL,
     isadmin    BIT NOT NULL,
     isemployee BIT NOT NULL,
     customerid INT,
     isactive   BIT NOT NULL,
  );

-- Enquires table
CREATE TABLE enquires
  (
     id           INT PRIMARY KEY IDENTITY (1, 1),
     enquiry_date DATETIME NOT NULL,
     isresolved   BIT NOT NULL,
     message      VARCHAR(max),
     customerid   INT,
     planid       INT,
  ); 


--Domain table
CREATE TABLE [dbo].[domains]
  (
     id             UNIQUEIDENTIFIER NOT NULL,
     [name]         [NVARCHAR](500) NOT NULL,
     [title]        [VARCHAR](1000) NULL,
     [regdate]      [DATE] NULL,
     [expdate]      [DATE] NULL,
     [registrar]    [VARCHAR](250) NULL,
     [size]         [VARCHAR](50) NULL,
     [webserver]    [VARCHAR](250) NULL,
     [country]      [NVARCHAR](255) NULL,
     [openport]     [VARCHAR](max) NULL,
     [criticalport] [VARCHAR](max) NULL,
     [ipaddress]    [VARCHAR](50) NULL,
     [customer_id]  [INT] NOT NULL,
     [round]        [INT] NOT NULL,
	 [transaction_Id][] null
  )

--Sub-Domain table
CREATE TABLE [dbo].[subdomains]
  (
     [name]      [NVARCHAR](500) NOT NULL,
     [domainid]  [UNIQUEIDENTIFIER] NULL,
     [ipaddress] [VARCHAR](50) NULL
  )

--RansomwareSusceptibilityTests table
CREATE TABLE [dbo].[ransomwaresusceptibilitytests]
  (
     [affectedapplication] [VARCHAR](500) NOT NULL,
     [ransomwaretype]      [VARCHAR](150) NULL,
     [domainid]            [UNIQUEIDENTIFIER] NOT NULL
  ) 


--Vulnerabilities table
CREATE TABLE [dbo].[vulnerabilities]
  (
     [id]           [UNIQUEIDENTIFIER] NOT NULL,
     [name]         [NVARCHAR](500) NOT NULL,
     [description]  [VARCHAR](max) NULL,
     [path]         [VARCHAR](500) NULL,
     [severityrank] [INT] NULL,
     [remidiation]  [VARCHAR](max) NULL,
     [domainid]     [UNIQUEIDENTIFIER] NULL,
     [ipaddress]    [VARCHAR](50) NULL
  ) 


 -- Procedure for signin 
CREATE PROCEDURE dbo.CheckUser @Username NVARCHAR(255), 
@Password NVARCHAR(255) AS BEGIN DECLARE @UserId INT;
SELECT 
  * 
FROM 
  Users 
WHERE 
  Username = @Username 
  AND Password = @Password;
END;

  -- Procedure create new customer
  CREATE OR ALTER PROCEDURE sp_Create_Customer @Email NVARCHAR(255),
  @First_Name NVARCHAR(255),
  @Last_Name NVARCHAR(255),
  @City NVARCHAR(255),
  @Country NVARCHAR(255),
  @Address NVARCHAR(255),
  @Current_Plan INT,
  @TransactionId INT,
  @PlanId INT,
  @Organization NVARCHAR(255)
  AS
  BEGIN
    DECLARE @Validity INT;
    DECLARE @Password NVARCHAR(MAX);
    DECLARE @RandomNumber INT;
    DECLARE @Cus_Id INT;
    DECLARE @Plan_Start DATETIME;
    DECLARE @Plan_End DATETIME;
    DECLARE @Created_Date DATETIME;
    SET @RandomNumber = CAST(RAND() * 9000 + 1000 AS INT);
    SET @Created_Date = GETDATE();

    INSERT INTO Customers (Email, First_Name, Last_Name, City, Country, [Address], Current_Plan, TransactionId, PlanId, Organization, Created_Date)
      VALUES (@Email, @First_Name, @Last_Name, @City, @Country, @Address, @Current_Plan, @TransactionId, @PlanId, @Organization, @Created_Date);

    SET @Validity = (SELECT
        Plan_Validity
      FROM Plans
      WHERE Id = @PlanId);
    SET @Password = @First_Name + @Last_Name + CAST(@RandomNumber AS NVARCHAR(4));
    SET @Password = REPLACE(@Password, ' ', '');
    SET @Cus_Id = SCOPE_IDENTITY();
    SET @Plan_Start = GETDATE();
    SET @Plan_End = DATEADD(DAY, @Validity, @Plan_Start);

    EXEC Sp_Create_Transaction @TransactionId
                              ,@Plan_Start
                              ,@Plan_End
                              ,1
                              ,1
                              ,@Cus_Id
                              ,@PlanId

    EXEC Sp_Create_User @Email
                       ,@Password
                       ,1
                       ,0
                       ,0
                       ,@Cus_Id
  END

    --Sp for creating a new transaction
    CREATE OR ALTER PROCEDURE Sp_Create_Transaction @Transaction_Id INT,
    @Plan_Start DATETIME,
    @Plan_End DATETIME,
    @Current_Round INT,
    @IsLatest BIT,
    @CustomerId INT,
    @PlanId INT
    AS
    BEGIN
      IF EXISTS (SELECT
            1
          FROM Transactions
          WHERE CustomerId = @CustomerId)
      BEGIN

        UPDATE Transactions
        SET IsLatest = 0
        WHERE CustomerId = @CustomerId;

        INSERT INTO Transactions (Transaction_Id, Plan_Start, Plan_End, Current_Round, IsLatest, CustomerId, PlanId)
          VALUES (@Transaction_Id, @Plan_Start, @Plan_End, @Current_Round, @IsLatest, @CustomerId, @PlanId);

        UPDATE Customers
        SET Current_Plan = @PlanId
           ,TransactionId = @Transaction_Id
        WHERE Id = @CustomerId;
      END
      ELSE
      BEGIN
        INSERT INTO Transactions (Transaction_Id, Plan_Start, Plan_End, Current_Round, IsLatest, CustomerId, PlanId)
          VALUES (@Transaction_Id, @Plan_Start, @Plan_End, @Current_Round, @IsLatest, @CustomerId, @PlanId);
      END
    END

      -- Procedure create User
      CREATE OR ALTER PROCEDURE Sp_Create_User @Email NVARCHAR(255),
      @Password NVARCHAR(255),
      @IsCustomer BIT,
      @IsAdmin BIT,
      @IsEmployee BIT,
      @CustomerId INT
      AS
      BEGIN
        DECLARE @UserName VARCHAR(50)
        DECLARE @IsActive BIT
        SET @IsActive = 1
        SET @UserName = SUBSTRING(@Email, 1, CHARINDEX('@', @Email) - 1)
        INSERT INTO Users
          VALUES (@UserName, @Password, @IsCustomer, @IsAdmin, @IsEmployee, @CustomerId, @IsActive)
        EXEC sp_Welcome_Mail @Email
                            ,@UserName
                            ,@Password
      END


        ----Procedure GetAllCustomerData
        CREATE OR ALTER PROC sp_GetAllCustomerData
        AS
        BEGIN
          SELECT
            C.Id
           ,C.Email
           ,C.First_Name
           ,C.Last_Name
           ,C.City
           ,C.Country
           ,C.Address
           ,C.Current_Plan
           ,C.Organization
           ,P.Plan_Name
          FROM Customers C
          LEFT JOIN Plans P
            ON C.PlanId = P.Id;
        END


          -- Procedure for sending Welcome Email
          CREATE OR ALTER PROCEDURE sp_Welcome_Mail @User_Email NVARCHAR(255),
          @Username NVARCHAR(50),
          @Password NVARCHAR(50)
          AS
          BEGIN
            DECLARE @Subject NVARCHAR(255)
            DECLARE @Message NVARCHAR(MAX)

            SET @Subject = 'Welcome to Invia Connect - Your Enterprise Security Solution'
            SET @Message =
            '<html>
  <head>
    <style>
      body {
        font-size: 16px;
        font-family: Arial, sans-serif;
        line-height: 1.6;
        margin: 0;
        padding: 0;
      }
      img {
        display: block;
        width: 100%;
        height: 250px;
		margin: 20px 0;
		border-radius: 10px;
      }
      table {
        border: 1px solid #000;
        border-collapse: collapse;
        margin: 20px 0;
      }
      table td {
        border: 1px solid #000;
        padding: 5px;
      }
      p {
        margin: 20px 0;
      }
      h3 {
        color: #0047AB;
      }
      strong {
        font-weight: bold;
      }
    </style>
  </head>
  <body>
    <img src="https://i.ibb.co/Q8fLQWk/INVIA.png" alt="INVIA" border="0">
    <p>We are thrilled to have you on board with us!</p>
    
    <p>To get started, please use the following credentials:</p>
    
    <table>
      <tr>
        <td style="font-weight: bold;">Username:</td>
        <td>' + @Username + '</td>
      </tr>
      <tr>
        <td style="font-weight: bold;">Password:</td>
        <td>' + @Password + '</td>
      </tr>
    </table>
    
    <p>Thank you for choosing us. We look forward to serving you and to a productive and prosperous partnership. Welcome to the Invia family!</p>
    
    <p>Warm regards,</p>
    <h3>INVIA</h3>
    <strong>"Connecting People, Bridging Worlds."</strong>
  </body>
  </html>'


            EXEC msdb.dbo.sp_send_dbmail @profile_name = 'SQLAlerts'
                                        ,@recipients = @User_Email
                                        ,@subject = @Subject
                                        ,@body = @Message
                                        ,@body_format = 'HTML';
          END


            --Procedure for Sending Enquiry
            CREATE OR ALTER PROCEDURE sp_AddEnquiry @Id INT,
            @CustomerId INT
            AS
            BEGIN
              DECLARE @PlanName NVARCHAR(255);
              SELECT
                @PlanName = Plan_Name
              FROM Plans
              WHERE Id = @Id;

              IF EXISTS (SELECT
                    1
                  FROM Customers
                  WHERE Id = @CustomerId)
              BEGIN
                INSERT INTO Enquires (Enquiry_Date, isResolved, Message, CustomerId, PlanId)
                  VALUES (GETDATE(), 0, 'Hi, I need to know more about the ' + @PlanName, @CustomerId, @Id);
              END
            END


              -- sp_CustomerPlanDetails stored procedure
              CREATE OR ALTER PROCEDURE sp_CustomerPlanDetails 
			  @customerId INT
              AS
              BEGIN
                SELECT
                  c.Id
                 ,c.First_Name
                 ,c.Last_Name
                 ,c.Email
                 ,c.City
                 ,c.[Address]
                 ,c.Organization
                 ,p.Plan_Name
                 ,t.Current_Round
                 ,t.Plan_Start
                 ,t.Plan_End
                 ,p.Id AS planId
                FROM Customers c
                JOIN Transactions t
                  ON t.CustomerId = c.Id
                JOIN Plans p
                  ON p.Id = c.Current_Plan
                WHERE c.Id = @customerId
                ORDER BY T.IsLatest DESC, T.Plan_Start DESC;
              END

                -- Stored procedure customeTransactions Details
                CREATE OR ALTER PROCEDURE sp_CustomerTransactions @customerId INT
                AS
                BEGIN
                  SELECT
                    c.Id
                   ,t.Transaction_Id
                   ,t.Plan_Start
                   ,t.Plan_End
                   ,p.Plan_Name
                   ,p.Id AS planId
                  FROM Transactions t
                  JOIN Customers c
                    ON t.CustomerId = c.Id
                  JOIN Plans p
                    ON p.Id = t.PlanId
                  WHERE c.Id = @customerId order by t.IsLatest desc 
                END

                  --Stored procedure for Deleting User
                  CREATE OR ALTER PROCEDURE Sp_Delete_Customer @Customer_Id INT
                  AS
                  BEGIN
                    DECLARE @Subject NVARCHAR(255) = 'Account Deletion Notification';
                    DECLARE @Body NVARCHAR(MAX) = 'Dear Customer, we regret to inform you that your account has been deleted. Thank you for being our valuable customer.';
                    DECLARE @MailProfile NVARCHAR(128) = 'SQLAlerts';
                    DECLARE @CustomerMail NVARCHAR(255);

                    SELECT
                      @CustomerMail = c.Email
                    FROM Customers c
                    WHERE c.Id = @Customer_Id;

                    DELETE FROM Users
                    WHERE CustomerId = @Customer_Id;
                    DELETE FROM Transactions
                    WHERE CustomerId = @Customer_Id;
                    DELETE FROM Customers
                    WHERE Id = @Customer_Id;
                    DELETE FROM Enquires
                    WHERE CustomerId = @Customer_Id;

                    EXEC msdb.dbo.sp_send_dbmail @profile_name = @MailProfile
                                                ,@recipients = @CustomerMail
                                                ,@subject = @Subject
                                                ,@body = @Body;
                  END


-- stored procedure for next round start
 CREATE OR ALTER PROCEDURE Sp_RoundUpdate @Customer_Id INT
 AS
 BEGIN
   DECLARE @Round INT
          ,@Max_Round INT
  
SELECT @Round = t.Current_Round
   FROM Transactions t
   JOIN Customers c
     ON t.CustomerId = c.Id
   WHERE t.CustomerId = @Customer_Id and t.IsLatest = 1;
	
   SELECT
     @Max_Round = p.Max_Rounds
   FROM Plans p
   JOIN Transactions t
     ON t.PlanId = p.Id
   WHERE t.CustomerId = @Customer_Id AND t.IsLatest = 1;

   IF (@Round < @Max_Round)
   BEGIN
     SET @Round += 1
     UPDATE Transactions
     SET Current_Round = @Round
     WHERE CustomerId = @Customer_Id;
   END
 END

--Procedure for customer Details
CREATE OR ALTER PROCEDURE sp_update_customer
  @Customer_Id   INT,
  @Email         NVARCHAR(255),
  @First_Name    NVARCHAR(255),
  @Last_Name     NVARCHAR(255),
  @City          NVARCHAR(255),
  @Address       NVARCHAR(255),
  @Current_Plan  INT,
  @TransactionId INT,
  @Organization  NVARCHAR(255)
AS
  BEGIN
  DECLARE @Plan_Start DATETIME,
     @Plan_End         DATETIME,
     @Validity         INT	  IF (@Current_Plan !=
   (
          SELECT current_plan
          FROM   customers
          WHERE  id = @Customer_Id))
   BEGIN
     SET @Plan_Start = Getdate();
     SET @Validity =
     (
            SELECT plan_validity
            FROM   plans
            WHERE  id = @Current_Plan);
     SET @Plan_End = Dateadd(day,@Validity,@Plan_Start);
   	EXEC Sp_create_transaction
       @TransactionId,
       @Plan_Start,
       @Plan_End,
       1,
       1,
       @Customer_Id,
       @Current_Plan;
   END
	UPDATE customers
   SET    email = @Email,
          first_name = @First_Name,
          last_name = @Last_Name,
          city = @City,
          address = @Address,		   PlanId = Current_Plan,
          current_plan = @Current_Plan,
          organization = @Organization
   WHERE  id = @Customer_Id;END


 -- To select all the Domains of particular customer
 select c.Id,p.Plan_Name,d.Title,d.Id,d.Name, d.IpAddress,d.CriticalPort,d.OpenPort,d.WebServer,d.Round,p.Max_Rounds,t.Current_Round from Customers c
 INNER JOIN Transactions t on t.PlanId = c.Current_Plan
 inner join Plans p on p.Id = t.PlanId
 inner join Domains d on d.Customer_Id = c.Id
 where c.Id = 35 and t.IsLatest = 1 order by d.Round desc


 -- To select all sub-domains of that particular customer
  select c.Id as CustomerId,s.Name,s.IpAddress,d.Name as Domain from Customers c
  join Domains d on d.Customer_Id = c.Id
  join Subdomains s on s.DomainId = d.Id
 where c.Id = 3027


 -- To get all result of Ransomware suspectibility test of a customer 
 select c.Id, d.Name, d.IpAddress, r.AffectedApplication, r.RansomwareType as type from Customers c
  join Domains d on d.Customer_Id = c.Id
  join RansomwareSusceptibilityTests r on r.DomainId = d.Id
  where c.Id = 3027


 -- to get all Vulnerabilities in domain of a customer
SELECT v.id           AS VulnerabilityId,
       v.NAME         AS VulnerabilityName,
       v.description  AS VulnerabilityDescription,
       v.path         AS VulnerabilityPath,
       v.severityrank AS SeverityRanking,
       v.remidiation  AS RemediationInfo,
       d.id           AS DomainId,
       d.NAME         AS DomainName,
       d.title        AS DomainTitle,
       d.registrar    AS DomainRegistrar,
       d.webserver    AS DomainWebServer,
       d.country      AS DomainCountry,
       d.openport     AS DomainOpenPort,
       d.criticalport AS DomainCriticalPort,
       d.ipaddress    AS DomainIpAddress,
       c.id           AS CustomerId,
       c.first_name   AS CustomerName
FROM   domains d
       JOIN customers c
         ON d.customer_id = c.id
       JOIN vulnerabilities v
         ON v.domainid = d.id
WHERE  c.id = 32
ORDER  BY v.severityrank DESC 


-- Creating random number for transaction
  CREATE or ALTER PROC RandNum
@TransactionId int,
@returnval int OUTPUT
AS
BEGIN
	DECLARE @in int,@out int
	SET @in = @TransactionId;
	IF EXISTS(SELECT Transaction_Id From Transactions WHERE Transaction_Id = @in)
	BEGIN
		SET @in = CAST(RAND() * 900000 + 1000 AS INT)
		exec RandNum @in, @out OUTPUT;
		SET @returnval = @out
	END
	ELSE
	BEGIN
		SET @returnval = @in
	END
END


--STORED PROCEDURE TO UPDATE CUSTOMER
CREATE OR ALTER PROCEDURE update_customer
  @Customer_Id  INT,
  @Email        NVARCHAR(255),
  @First_Name   NVARCHAR(255),
  @Last_Name    NVARCHAR(255),
  @City         NVARCHAR(255),
  @Address      NVARCHAR(255),
  @Current_Plan INT,
  @Organization NVARCHAR(255)
AS
  BEGIN
  DECLARE @Plan_Start DATETIME,
      @Plan_End         DATETIME,
      @Validity         INT,
      @TransactionId    INT,
      @r                INT 
	  SET @TransactionId = Cast(Rand() * 900000 + 1000 AS INT)EXEC Randnum
      @TransactionId,
      @r output
	  SET @TransactionId = @r
	  IF (@Current_Plan !=
    (
           SELECT current_plan
           FROM   customers
           WHERE  id = @Customer_Id))
    BEGIN
      SET @Plan_Start = Getdate();
      SET @Validity =
      (
             SELECT plan_validity
             FROM   plans
             WHERE  id = @Current_Plan);
      SET @Plan_End = Dateadd(day,@Validity,@Plan_Start);
      EXEC Sp_create_transaction
        @TransactionId,
        @Plan_Start,
        @Plan_End,
        1,
        1,
        @Customer_Id,
        @Current_Plan;

    DECLARE @Subject NVARCHAR(255) = 'Plan Change Notification';
    DECLARE @Body NVARCHAR(MAX);
    DECLARE @MailProfile NVARCHAR(128) = 'SQLAlerts';
    DECLARE @CustomerMail NVARCHAR(255);

    SELECT @CustomerMail = Email
    FROM customers
    WHERE id = @Customer_Id;

    DECLARE @NewPlanName NVARCHAR(255);
    DECLARE @NewPlanValidity INT;

    SELECT
      @NewPlanName = p.Plan_Name,
      @NewPlanValidity = p.Plan_Validity
    FROM plans p
    WHERE id = @Current_Plan;

    IF (@Current_Plan > (SELECT t.PlanId FROM Transactions t WHERE t.CustomerId = @Customer_Id and t.IsLatest = 1))
      SET @Body = 'Dear Customer, your plan has been updated. You have upgraded your plan.';
    ELSE
      SET @Body = 'Dear Customer, your plan has been updated. You have downgraded your plan.';

    SET @Body = @Body + CHAR(13) + CHAR(10) +
                'New Plan Details:' + CHAR(13) + CHAR(10) +
                'Plan Name: ' + @NewPlanName + CHAR(13) + CHAR(10) +
                'Validity: ' + CONVERT(NVARCHAR, @NewPlanValidity) + ' days';

    -- Send email
    EXEC msdb.dbo.sp_send_dbmail
      @profile_name = @MailProfile,
      @recipients = @CustomerMail,
      @subject = @Subject,
      @body = @Body;

    END

	UPDATE customers
    SET    email = @Email,
           first_name = @First_Name,
           last_name = @Last_Name,
           city = @City,
           address = @Address,
           current_plan = @Current_Plan,
           organization = @Organization
    WHERE  id = @Customer_Id;
	END


-- stored procedure for chart(threat level Dougnet chart)
CREATE OR ALTER PROCEDURE getvulnerabilitycounts
  @customer_Id   INT,
  @Low_threat    INT output,
  @Medium_threat INT output,
  @High_threat   INT output
AS
  BEGIN
  SELECT @Low_threat = Count(v.severityrank)
    FROM   customers c
    JOIN   domains d
    ON     d.customer_id = c.id
    JOIN   vulnerabilities v
    ON     v.domainid = d.id
    WHERE  v.severityrank <= 4
    AND    c.id = @customer_Id

	SELECT @Medium_threat = Count(v.severityrank)
    FROM   customers c
    JOIN   domains d
    ON     d.customer_id = c.id
    JOIN   vulnerabilities v
    ON     v.domainid = d.id
    WHERE  v.severityrank <= 6
    AND    v.severityrank > 4
    AND    c.id = @customer_Id

	SELECT @High_threat = Count(v.severityrank)
    FROM   customers c
    JOIN   domains d
    ON     d.customer_id = c.id
    JOIN   vulnerabilities v
    ON     v.domainid = d.id
    WHERE  v.severityrank <= 10
    AND    v.severityrank > 6
    AND    c.id = @customer_Id
  end

Declare @low int
Declare @medium int
Declare @high int
exec GetVulnerabilityCounts 38,@low OUTPUT,@medium OUTPUT,@high OUTPUT
select @low as 'Low Severity',@medium  as 'Medium Severity',@high  as 'High Severity'


-- stored procedure for adding domain
create or alter procedure Add_New_Domain
@Id int,
@Title varchar(1000),
@Name nvarchar (500),
@IpAddress varchar(50),
@CriticalPort varchar(max),
@OpenPort varchar(max),
@WebServer varchar(250)
as
begin
DECLARE @Round int,@RegDate Datetime,@ExpDate Datetime,@Registrar nvarchar(250),@Size nvarchar(50),@Country nvarchar(50)
SET @RegDate = GETDATE()
SET @ExpDate = DATEADD(YEAR, 1, @RegDate)
SET @Registrar = 'GoDaddy'
SET @Size = 'large'
SET @Country = 'India'
SET @Round = (SELECT Current_Round FROM Transactions WHERE CustomerId = @Id AND IsLatest = 1)
 Insert into Domains(Id,Name,Title,RegDate,ExpDate,Registrar,Size,WebServer,Country,OpenPort,CriticalPort,IpAddress,Customer_Id,[Round])
 values(NEWID(),@Name,@Title,@RegDate,@ExpDate,@Registrar,@Size,@WebServer,@Country,@OpenPort,@CriticalPort,@IpAddress,@Id,@Round)
end

exec sp_Add_New_Domain 35,'mydomain','mydomain_4_8','232.2324.12','4343,34343','3434,3434','ingnx'


-- STORED PROCEDUR EXECUTION
 EXEC sp_GetAllCustomerData
EXEC sp_Create_Customer 'kunalshokeen051@gmail.com',
	'kunal',
	'shokeen',
	'Gurgoan',
	'India',
	'jharsa gurgaon',
	3,
	1234556,
	3,
	'Tower Research'

                      EXEC sp_AddEnquiry 2
                                        ,1
                      EXEC sp_CustomerTransactions 3012
                      EXEC sp_CustomerPlanDetails 4
                      EXEC Sp_Create_Transaction 324324
                                                ,'2023-10-28 18:17:08.350'
                                                ,'2024-04-28 18:17:08.350'
                                                ,1
                                                ,1
                                                ,3007
                                                ,2
                      EXEC Sp_Create_User 'kunalshokeen99@gmail.com'
                                         ,'admin123'
                                         ,0
                                         ,1
                                         ,1
                                         ,0
                      EXEC Sp_Delete_Customer 3024
                      EXEC Sp_RoundUpdate 3025		  
                   exec Sp_Update_Customer 1,'ddgangwar09@gmail.com','Dipanshu','Gangwar','Noida','Noida, Sector-59',1,'Google'

                      --Calling Tables
                      SELECT
                        *
                      FROM Plans
                      SELECT
                        *
                      FROM Customers
                      SELECT
                        *
                      FROM Transactions
                      SELECT
                        *
                      FROM Users
                      SELECT
                        *
                      FROM Enquires
					  select
					  *
					  from [Domains]
					  select
					  *
					  from [Subdomains]
					  select
					  *
					  from [RansomwareSusceptibilityTests]
					  select
					  *
					  from [Vulnerabilities]


-- --------------------------------------------------------------------------------------------------------------------------------		

-- to get domain count
select COUNT(*) from Domains where Customer_Id = 27


-- Schedule task for plan validity checking
EXEC msdb.dbo.sp_add_schedule
    @schedule_name = N'DailySchedule',
    @enabled = 1,
    @freq_type = 4,  
    @freq_interval = 1;  

EXEC msdb.dbo.sp_attach_schedule
    @job_name = N'CheckPlanEndJob',
    @schedule_name = N'DailySchedule';

EXEC msdb.dbo.sp_add_job
    @job_name = N'CheckPlanEndJob',
    @enabled = 1;

EXEC msdb.dbo.sp_add_jobstep
    @job_name = N'CheckPlanEndJob',
    @step_name = N'CheckPlanEndStep',
    @subsystem = N'TSQL',
    @command = 
    N'
    DECLARE @Today DATE = GETDATE();

    UPDATE Users
    SET IsActive = 0
    FROM UserTable u
    join Customers c on u.CustomerId = c.Id
    JOIN Transactions t ON  T.CustomerId = C.Id
	JOIN Plans P ON C.Current_Plan =  P.Id 
	 WHERE p.plan_end < @Today AND T.IsLatest = 1;

    -- Send email to customers whose plan is expiring today
    EXEC msdb.dbo.sp_send_dbmail
        @profile_name = ''sqlalerts'',
        @recipients = (SELECT Email FROM CustomerTable c JOIN PlanTable p ON c.CustomerID = p.CustomerID WHERE p.plan_end = @Today),
        @subject = ''Your plan is expiring today'',
        @body = ''Please renew your plan.'';
    ';

EXEC msdb.dbo.sp_add_jobserver
    @job_name = N'CheckPlanEndJob';

-- For testing this Job
EXEC msdb.dbo.sp_start_job N'CheckPlanEndJob';


