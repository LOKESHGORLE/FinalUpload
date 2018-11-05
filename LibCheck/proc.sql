create or alter procedure Proc_InsertFilePermissions (@pFileID int, 
														@pUserName nvarchar(100),
														@pFilePermission nvarchar(50))
as
declare @UserId int;
declare @FilePermissionId int;
begin
select  @UserId=UserID from DomainUsers where name like @pUserName;
select  @FilePermissionId=Id from FolderPermissions where PermissionLevel=@pFilePermission;
insert into UserFilePermissions (FileID,UserID,PermissionLevelId) values (@pFileID,@UserId, @FilePermissionId);
end

create or alter procedure Proc_InsertFolderPermissions (@pFolderID int, 
														@pUserName nvarchar(100),
														@pFolderPermission nvarchar(50))
as
declare @UserId int;
declare @FolderPermissionId int;
begin
select  @UserId=UserID from DomainUsers where name like @pUserName;
select @FolderPermissionId=Id from FolderPermissions where PermissionLevel=@pFolderPermission;
insert into UserFolderPermissions (SubFolderID,UserID,PermissionLevelId) values (@pFolderID,@UserId,@FolderPermissionId);
end