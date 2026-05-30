USE [TaskFlowProDb]
GO

-- ==============================================================================
-- STEP 1: Alter columns from NVARCHAR(MAX) to NVARCHAR(256) to allow indexing
-- ==============================================================================
ALTER TABLE [dbo].[Profiles] 
ALTER COLUMN [EmailAddress] NVARCHAR(256) NOT NULL;
GO

ALTER TABLE [dbo].[Profiles] 
ALTER COLUMN [FullName] NVARCHAR(256) NOT NULL;
GO

-- ==============================================================================
-- STEP 2: Create ProjectTasks Indexes
-- ==============================================================================

-- Task List (Assigned User + Status)
CREATE INDEX IX_ProjectTasks_AssignedUserId_StatusId
ON ProjectTasks (AssignedUserId, StatusId);
GO

-- Created By User
CREATE INDEX IX_ProjectTasks_CreatedById
ON ProjectTasks (CreatedById);
GO

-- Dashboard Date Filter
CREATE INDEX IX_ProjectTasks_CreatedOn
ON ProjectTasks (CreatedOn);
GO

-- Status + Updated Date
CREATE INDEX IX_ProjectTasks_StatusId_UpdatedOn
ON ProjectTasks (StatusId, UpdatedOn);
GO

-- ==============================================================================
-- STEP 3: Create ActivityLogs Indexes
-- ==============================================================================

-- Task Activities
CREATE INDEX IX_ActivityLogs_TaskId_CreatedOn
ON ActivityLogs (TaskId, CreatedOn);
GO

-- User Activity History
CREATE INDEX IX_ActivityLogs_CreatedByUserId
ON ActivityLogs (CreatedByUserId);
GO

-- ==============================================================================
-- STEP 4: Create Profiles Indexes
-- ==============================================================================


-- User Search
CREATE INDEX IX_Profiles_FullName
ON Profiles (FullName);
GO