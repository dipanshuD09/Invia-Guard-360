
--*************************************************** for generating random customer ***************************************************
DECLARE @counter INT = 1;
WHILE @counter <= 20
BEGIN
    DECLARE @Email NVARCHAR(255) = 'user' + CAST(@counter AS NVARCHAR(10)) + '@example.com';
    DECLARE @First_Name NVARCHAR(255) = 'FirstName' + CAST(@counter AS NVARCHAR(10));
    DECLARE @Last_Name NVARCHAR(255) = 'LastName' + CAST(@counter AS NVARCHAR(10));
    DECLARE @City NVARCHAR(255) = 'City' + CAST(@counter AS NVARCHAR(10));
    DECLARE @Country NVARCHAR(255) = 'Country' + CAST(@counter AS NVARCHAR(10));
    DECLARE @Address NVARCHAR(255) = 'Address' + CAST(@counter AS NVARCHAR(10));
    DECLARE @PlanId INT = ABS(CHECKSUM(NEWID())) % 3 + 1; -- Random plan Id between 1 and 3
    DECLARE @TransactionId INT = ABS(CHECKSUM(NEWID())) % 900000 + 100000; -- Random 6-digit transaction ID
    DECLARE @Organization NVARCHAR(255) = 'Organization' + CAST(@counter AS NVARCHAR(10));

    -- Set Current_Plan to the same value as PlanId
    DECLARE @Current_Plan INT = @PlanId;

    EXEC sp_Create_Customer 
        @Email,
        @First_Name,
        @Last_Name,
        @City,
        @Country,
        @Address,
        @Current_Plan,
        @TransactionId,
        @PlanId,
        @Organization;

    SET @counter = @counter + 1;
END;


-- *************************************************** random domain data ***************************************************

DECLARE @counter INT = 1;

WHILE @counter <= 200
BEGIN
    DECLARE @CustomerId INT = ABS(CHECKSUM(NEWID())) % 20 + 27; -- Random value between 27 and 46
    DECLARE @CurrentRound INT = (SELECT MAX(Round) FROM Domains WHERE Customer_Id = @CustomerId);

    -- Check if the customer has no domains in the current round
    IF @CurrentRound IS NULL OR @CurrentRound < (SELECT MAX_Rounds FROM Plans WHERE Id IN (SELECT PlanId FROM Transactions WHERE CustomerId = @CustomerId and Transactions.IsLatest = 1))
    BEGIN
        DECLARE @Id INT = ABS(CHECKSUM(NEWID())) % 20 + 27; -- Random value between 27 and 46
        DECLARE @Title VARCHAR(1000) = 'testsite' + CAST(@counter AS VARCHAR(5));
        DECLARE @Name NVARCHAR(500) = 'testsite' + CAST(@counter AS NVARCHAR(5)) + '.com';
        DECLARE @IpAddress VARCHAR(50) = '127.' + CAST(ABS(CHECKSUM(NEWID())) % 256 AS VARCHAR(3)) + '.' + CAST(ABS(CHECKSUM(NEWID())) % 256 AS VARCHAR(3)) + '.' + CAST(ABS(CHECKSUM(NEWID())) % 256 AS VARCHAR(3));
        DECLARE @CriticalPort VARCHAR(MAX) = CAST(ABS(CHECKSUM(NEWID())) % 10000 AS VARCHAR(5)) + ',' + CAST(ABS(CHECKSUM(NEWID())) % 10000 AS VARCHAR(5)) + ',' + CAST(ABS(CHECKSUM(NEWID())) % 10000 AS VARCHAR(5));
        DECLARE @OpenPort VARCHAR(MAX) = CAST(ABS(CHECKSUM(NEWID())) % 10000 AS VARCHAR(5)) + ',' + CAST(ABS(CHECKSUM(NEWID())) % 10000 AS VARCHAR(5)) + ',' + CAST(ABS(CHECKSUM(NEWID())) % 10000 AS VARCHAR(5));
        DECLARE @WebServer VARCHAR(250) = CASE ABS(CHECKSUM(NEWID())) % 3
                                            WHEN 0 THEN 'Nginx'
                                            WHEN 1 THEN 'Apache'
                                            WHEN 2 THEN 'IIS'
                                         END;

        EXEC sp_Add_Domain
            @Id,
            @Title,
            @Name,
            @IpAddress,
            @CriticalPort,
            @OpenPort,
            @WebServer;

        SET @counter = @counter + 1;
    END
    ELSE
    BEGIN
        SET @counter = @counter + 1;
    END
END;

-- ************************************************** Inserting vulneribilities ****************************************************

DECLARE @Counter INT = 1;

WHILE @Counter <= 5
BEGIN
    INSERT INTO [dbo].[Vulnerabilities] ([Id], [Name], [Description], [Path], [SeverityRank], [Remidiation], [DomainId], [IpAddress])
    SELECT NEWID(),
           'Vuln' + CAST(@Counter AS NVARCHAR(5)),
           'Description' + CAST(ABS(CHECKSUM(NEWID(), GETDATE())) % 100 AS NVARCHAR(3)),
           'Path' + CAST(ABS(CHECKSUM(NEWID(), GETDATE())) % 100 AS NVARCHAR(3)),
           ABS(CHECKSUM(NEWID(), GETDATE())) % 10 + 1,
           'Remediation' + CAST(ABS(CHECKSUM(NEWID(), GETDATE())) % 100 AS NVARCHAR(3)),
           D.Id,
           '192.168.' + CAST(ABS(CHECKSUM(NEWID(), GETDATE())) % 256 AS NVARCHAR(3)) + '.'
           + CAST(ABS(CHECKSUM(NEWID(), GETDATE())) % 256 AS NVARCHAR(3))
    FROM [dbo].[Domains] D;

    SET @Counter = @Counter + 1;
END;



