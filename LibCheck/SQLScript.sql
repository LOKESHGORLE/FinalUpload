create table RootFolder (
	Id int identity(1,1) primary key,
	Name nvarchar(100),
	Path nvarchar(1000),
	CreatedBy nvarchar(100),
	ModifiedBy nvarchar(100),
	CreatedOn datetime,
	ModifiedOn datetime
)
select * from Rootfolder

create table SubFolder(
	Id int identity(1,1) primary key,
	RootFolderId int foreign key references RootFolder(Id),
	Name nvarchar(100),
	Path nvarchar(1000),
	CreatedBy nvarchar(100),
	ModifiedBy nvarchar(100),
	CreatedOn datetime,
	ModifiedOn datetime
	
)
select * from SubFolder
create table FileInfo(
	Id int identity(1,1) primary key,
	SubFolderId int foreign key references SubFolder(Id),
	Name nvarchar(100),
	Path nvarchar(1000),
	Type varchar(100),
	Size varchar(100),
	CreatedBy nvarchar(100),
	ModifiedBy nvarchar(100),
	FileAccessed datetime,
	CreatedOn datetime,
	ModifiedOn datetime,
	IsFilePathOK bit,
	IsFileUnsupported bit 
)
select * from FileInfo

create table FolderPermissions(
	Id int identity(1,1) primary key,
	PermissionLevel nvarchar(30) unique
)

create table SPPermissions(
	Id int identity(1,1) primary key,
	PermissionLevel nvarchar(30)
)
select * from FolderPermissions
select * from RoleTypes
create table RoleTypes(
	Id int identity(1,1) primary key,
	RoleType nvarchar(30)
)

create table FolderToSPPermissions(
	
	FolderPermissionsId int foreign key references FolderPermissions(Id),
	RoleTypesId int foreign key references RoleTypes(Id),
)

create table DomainUsers(
UserID int identity(1,1) Primary key,
Name nvarchar(100)
)

create table UserFolderPermissions(
SubFolderID int  foreign key references SubFolder(Id),
UserID int foreign key references DomainUsers(UserId),
PermissionLevelId int foreign key references FolderPermissions(Id)
)

create table UserFilePermissions(
FileID int  foreign key references FileInfo(Id),
UserID int foreign key references DomainUsers(UserId),
PermissionLevelId int foreign key references FolderPermissions(Id)
)

insert into RootFolder values ('UploadFolderTest','D:\','Lokesh','Lokesh1',SYSDATETIME(),SYSDATETIME());
select * from Rootfolder;
select * from SubFolder;

insert into SubFolder values(1,'lev1sf1','D:\UploadFolderTest','Lokesh','Lokesh1',SYSDATETIME(),SYSDATETIME());
insert into SubFolder values(1,'Lev1Sf2','D:\UploadFolderTest','Lokesh','Lokesh1',SYSDATETIME(),SYSDATETIME());
insert into SubFolder values(1,'New Microsoft Word Document','D:\UploadFolderTest','Lokesh','Lokesh1',SYSDATETIME(),SYSDATETIME());
insert into SubFolder values(1,'New Microsoft PowerPoint Presentation','D:\UploadFolderTest\lev1sf1','Lokesh','Lokesh1',SYSDATETIME(),SYSDATETIME());
insert into SubFolder values(1,'Lev1Sf1F1','D:\UploadFolderTest\Lev1Sf2','Lokesh','Lokesh1',SYSDATETIME(),SYSDATETIME());
insert into SubFolder values(1,'New Microsoft Word Document','D:\UploadFolderTest\Lev1Sf2','Lokesh','Lokesh1',SYSDATETIME(),SYSDATETIME());
insert into SubFolder values(1,'New Microsoft Word Document','D:\UploadFolderTest\Lev1Sf2\Lev1Sf1F1','Lokesh','Lokesh1',SYSDATETIME(),SYSDATETIME());

select concat(Path,'/',Name) as x from FileInfo;
insert into FileInfo(SubFolderId,Name,Path) values(1,'New Microsoft Word Document','D:\UploadFolderTest');
insert into FileInfo(SubFolderId,Name,Path) values(2,'New Microsoft PowerPoint Presentation','D:\UploadFolderTest\lev1sf1');
insert into FileInfo(SubFolderId,Name,Path) values(2,'New Microsoft Word Document','D:\UploadFolderTest\Lev1Sf2');
insert into FileInfo(SubFolderId,Name,Path) values(3,'New Microsoft Word Document','D:\UploadFolderTest\Lev1Sf2\Lev1Sf1F1');

select REPLACE(Path,'\','/')+'/'+Name from FileInfo;

select * from FolderPermissions;
select * from SPPermissions;
select * from RoleTypes;
select r.RoleType from RoleTypes r where r.Id in ( 
select RoleTypesId from FolderToSPPermissions where FolderPermissionsId=(select Id from FolderPermissions where PermissionLevel='Read'));
select * from FolderToSPPermissions  where FolderPermissionsId=18

select RoleType from RoleTypes  where Id in (select RoleTypesId from FolderToSPPermissions where FolderPermissionsId = (select Id from FolderPermissions where PermissionLevel = 'ReadAndExecute')); 

create table x as select * from FolderToSPPermissions;
SELECT * INTO x FROM  FolderToSPPermissions;
select * from x;
select * from DomainUsers;
insert into  DomainUsers(Name) values('Lokesh');
SELECT name, COUNT(name) 
FROM DomainUsers
GROUP BY name
HAVING COUNT(name) > 1;
delete from DomainUsers where name='';
select * from UserFilePermissions;
select * from FolderPermissions;
select Id from FolderPermissions where PermissionLevel=' Synchronize';
select * from UserFolderPermissions;


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


DELETE TOP (10)
FROM  UserFilePermissions
insert into FolderToSPPermissions values(13,3)