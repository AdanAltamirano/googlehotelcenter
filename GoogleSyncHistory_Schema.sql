CREATE TABLE [dbo].[GoogleSyncHistory] (
    [IdSync]         INT            IDENTITY (1, 1) NOT NULL,
    [IdHotel]        INT            NOT NULL,
    [RequestXML]     XML            NULL,
    [ResponseXML]    XML            NULL,
    [Status]         VARCHAR (50)   NOT NULL, -- 'Pending', 'Success', 'Failed'
    [Timestamp]      DATETIME       DEFAULT (getdate()) NOT NULL,
    [Usuario]        VARCHAR (100)  NULL,
    [TipoOperacion]  VARCHAR (100)  NOT NULL, -- 'CreateRatePlan', 'UpdateRate', 'DeleteRate', etc.
    [RatePlanId]     VARCHAR (100)  NULL,
    [RoomId]         INT            NULL,
    [ErrorMessage]   NVARCHAR (MAX) NULL,
    [CorrelationId]  UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    PRIMARY KEY CLUSTERED ([IdSync] ASC)
);

CREATE INDEX IX_GoogleSyncHistory_IdHotel ON [dbo].[GoogleSyncHistory] ([IdHotel]);
CREATE INDEX IX_GoogleSyncHistory_Status ON [dbo].[GoogleSyncHistory] ([Status]);
CREATE INDEX IX_GoogleSyncHistory_Timestamp ON [dbo].[GoogleSyncHistory] ([Timestamp]);
