/* =========================
   0) CREATE & USE DATABASE
   ========================= */
IF DB_ID(N'CarVipPro') IS NULL
BEGIN
    CREATE DATABASE CarVipPro;
END
GO
USE CarVipPro;
GO

-- 1. CarCompany
CREATE TABLE dbo.CarCompany (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    CatalogName   NVARCHAR(150) NOT NULL,
    [Description] NVARCHAR(500) NULL,
    IsActive      BIT NOT NULL DEFAULT 1
);

-- 2. VehicleCategory
CREATE TABLE dbo.VehicleCategory (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName  NVARCHAR(100) NOT NULL,
    IsActive      BIT NOT NULL DEFAULT 1
);

-- 3. ElectricVehicle (thuộc CarCompany + VehicleCategory)
CREATE TABLE dbo.ElectricVehicle (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    CarCompanyId    INT NOT NULL,
    CategoryId      INT NOT NULL,
    [Model]         NVARCHAR(120) NOT NULL,
    [Version]       NVARCHAR(80)  NULL,
    Price           DECIMAL(18,2) NOT NULL CHECK (Price >= 0),
    [Specification] NVARCHAR(MAX) NULL,
    ImageUrl        NVARCHAR(500) NULL,
    [Color]         NVARCHAR(60)  NULL,
    IsActive        BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_EV_Company  FOREIGN KEY (CarCompanyId) REFERENCES dbo.CarCompany(Id),
    CONSTRAINT FK_EV_Category FOREIGN KEY (CategoryId)   REFERENCES dbo.VehicleCategory(Id)
);

-- 4. Account (nhân sự: Staff/Manager)
CREATE TABLE dbo.[Account] (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    Email      NVARCHAR(255) NOT NULL,
    Phone      NVARCHAR(30)  NULL,
    FullName   NVARCHAR(120) NOT NULL,
    [Password] NVARCHAR(255) NOT NULL,
    [Role]     NVARCHAR(30)  NOT NULL,   -- Staff | Manager
    IsActive   BIT NOT NULL DEFAULT 1,
    CONSTRAINT UQ_Account_Email UNIQUE(Email),
    CONSTRAINT CK_Account_Role CHECK ([Role] IN (N'Staff', N'Manager'))
);

-- 5. Customer
CREATE TABLE dbo.Customer (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    Email        NVARCHAR(255) NOT NULL,
    Phone        NVARCHAR(30)  NULL,
    FullName     NVARCHAR(120) NOT NULL,
    IdentityCard NVARCHAR(60)  NULL,
    [Address]    NVARCHAR(255) NULL,
    ZipCode      NVARCHAR(20)  NULL,
    CONSTRAINT UQ_Customer_Email UNIQUE(Email)
);

-- 6. Promotion (giảm giá đơn hàng)
CREATE TABLE dbo.Promotion (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    [Code]        NVARCHAR(40)  NOT NULL,
    [Description] NVARCHAR(300) NULL,
    Discount      DECIMAL(5,2)  NOT NULL CHECK (Discount >= 0), -- % hoặc số tiền tùy logic app
    IsActive      BIT NOT NULL DEFAULT 1,
    CONSTRAINT UQ_Promotion_Code UNIQUE([Code])
);

-- 7. Order (đơn hàng) – liên kết Customer, Account (người tạo/xử lý), Promotion (tùy chọn)
CREATE TABLE dbo.[Order] (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId    INT NOT NULL,
    AccountId     INT NOT NULL,
    PromotionId   INT NULL,
    [DateTime]    DATETIME2(0) NOT NULL DEFAULT SYSUTCDATETIME(),
    Total         DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (Total >= 0),
    PaymentMethod NVARCHAR(30)  NOT NULL, -- CASH | MOMO | INSTALLMENT
    [Status]      NVARCHAR(20)  NOT NULL, -- PENDING | COMPLETED | CANCELLED
    CONSTRAINT FK_Order_Customer  FOREIGN KEY (CustomerId)  REFERENCES dbo.Customer(Id),
    CONSTRAINT FK_Order_Account   FOREIGN KEY (AccountId)   REFERENCES dbo.[Account](Id),
    CONSTRAINT FK_Order_Promotion FOREIGN KEY (PromotionId) REFERENCES dbo.Promotion(Id),
    CONSTRAINT CK_Order_Payment CHECK (PaymentMethod IN (N'CASH', N'MOMO', N'INSTALLMENT')),
    CONSTRAINT CK_Order_Status  CHECK ([Status] IN (N'PENDING', N'COMPLETED', N'CANCELLED'))
);

-- 8. OrderDetail (chi tiết đơn) – mỗi dòng gắn 1 ElectricVehicle
CREATE TABLE dbo.OrderDetail (
    Id                INT IDENTITY(1,1) PRIMARY KEY,
    OrderId           INT NOT NULL,
    ElectricVehicleId INT NOT NULL,
    Quantity          INT NOT NULL CHECK (Quantity > 0),
    TotalPrice        DECIMAL(18,2) NOT NULL CHECK (TotalPrice >= 0),
    CONSTRAINT FK_OrderDetail_Order  FOREIGN KEY (OrderId)           REFERENCES dbo.[Order](Id) ON DELETE CASCADE,
    CONSTRAINT FK_OrderDetail_EV     FOREIGN KEY (ElectricVehicleId) REFERENCES dbo.ElectricVehicle(Id)
);

-- 9. DriveSchedule (đặt lịch lái thử) – EV + Customer + Account (nhân sự phụ trách)
CREATE TABLE dbo.DriveSchedule (
    Id                INT IDENTITY(1,1) PRIMARY KEY,
    ElectricVehicleId INT NOT NULL,
    CustomerId        INT NOT NULL,
    AccountId         INT NOT NULL,
    StartTime         DATETIME2(0) NOT NULL,
    EndTime           DATETIME2(0) NOT NULL,
    [Status]          NVARCHAR(20) NOT NULL, -- COMING_SOON | IS_HAPPENING | CANCELLED
    CONSTRAINT FK_DriveSchedule_EV       FOREIGN KEY (ElectricVehicleId) REFERENCES dbo.ElectricVehicle(Id),
    CONSTRAINT FK_DriveSchedule_Customer FOREIGN KEY (CustomerId)        REFERENCES dbo.Customer(Id),
    CONSTRAINT FK_DriveSchedule_Account  FOREIGN KEY (AccountId)         REFERENCES dbo.[Account](Id),
    CONSTRAINT CK_Drive_Status CHECK ([Status] IN (N'COMING_SOON', N'IS_HAPPENING', N'CANCELLED')),
    CONSTRAINT CK_Drive_Time CHECK (EndTime > StartTime)
);

-- 10. Feedback (ý kiến khách) – gắn Customer
CREATE TABLE dbo.Feedback (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId   INT NOT NULL,
    FeedbackType NVARCHAR(20) NOT NULL, -- REPORT | OPINION | OTHER
    [Content]    NVARCHAR(MAX) NOT NULL,
    [DateTime]   DATETIME2(0) NOT NULL DEFAULT SYSUTCDATETIME(),
    IsActive     BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Feedback_Customer FOREIGN KEY (CustomerId) REFERENCES dbo.Customer(Id),
    CONSTRAINT CK_Feedback_Type CHECK (FeedbackType IN (N'REPORT', N'OPINION', N'OTHER'))
);
