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

create or ALTER PROCEDURE Proc_GetUserRoleforFile(@pFilePath nvarchar(250) )
AS
BEGIN

    SET NOCOUNT ON;

    declare @TimeRanges as TABLE (UserName nvarchar(100), RoleType nvarchar(100));

     select TimeRanges= (select u.Name,r.RoleType from UserFilePermissions uf,
														DomainUsers u,
														FolderToSPPermissions fsp,
														RoleTypes r where u.UserId=uf.userId and
																			r.Id=fsp.RoleTypesId and
																			fsp.FolderPermissionsId=uf.PermissionLevelId and 
															FileID=(select Id from fileinfo where path='D:\UploadFolderTest\lev1sf1New Microsoft PowerPoint Presentation'));
     
END

select TimeRanges= (select u.NAme,r.RoleType from UserFilePermissions uf, DomainUsers u, FolderToSPPermissions fsp, RoleTypes r where u.UserId=uf.userId and r.Id=fsp.RoleTypesId and fsp.FolderPermissionsId=uf.PermissionLevelId and FileID=(select Id from fileinfo where path+Name='D:\UploadFolderTest\lev1sf1New Microsoft PowerPoint Presentation')) END

delete  from UserFilePermissions where Userid!=289
update  UserFilePermissions set fileId=2 where FileId=1