CREATE PROCEDURE [dbo].[sp_CreateUpdateUser] 
    @id INT,
    @firstName VARCHAR(50),
    @lastName VARCHAR(50),
    @email VARCHAR(50),
    @mobileNo VARCHAR(50),
    @password VARCHAR(50),
    @ipAddress VARCHAR(50),
    @role VARCHAR(50),
    @designation VARCHAR(50),
    @userId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM [dbo].[SuperAdminUsers] WHERE [Id] = @id)
    BEGIN
        UPDATE [dbo].[SuperAdminUsers]
        SET 
            [FirstName] = @firstName,
            [LastName] = @lastName,
            [Email] = @email,
            [MobileNo] = @mobileNo,
            [Role] = @role,
            [Password] = @password,
            [IPAddress] = @ipAddress,
            [Designation] = @designation,
            [UpdatedOn] = dbo.fn_getCurrentISTTime(),
            [UpdatedBy] = @userId
        WHERE [Id] = @id;

        SELECT @id AS UserId;
    END
    ELSE
    BEGIN
        INSERT INTO [dbo].[SuperAdminUsers]
        (
            [FirstName],
            [LastName],
            [Email],
            [MobileNo],
            [Role],
            [Password],
            [IPAddress],
            [Designation],
            [IsActive],
            [CreatedOn],
            [CreatedBy]
        )
        VALUES
        (
            @firstName,
            @lastName,
            @email,
            @mobileNo,
            @role,
            @password,
            @ipAddress,
            @designation,
            1,
            dbo.fn_getCurrentISTTime(),
            @userId
        );

        SELECT SCOPE_IDENTITY() AS UserId;
    END
END