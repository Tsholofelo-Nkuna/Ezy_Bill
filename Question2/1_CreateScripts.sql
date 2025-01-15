Create Table Facilitators (
 Id bigint Primary Key Identity(1,1),
 FirstName nvarchar(128),
 LastName nvarchar(128),
 Email nvarchar(128),
 Phone nvarchar(128)
);

create Table Clients (
 [Name] nvarchar(128),
 [Id] bigint Primary Key Identity(1,1),
 ClientType int, --indicates type of client (business or individual)
 Email nvarchar(128),
 Phone nvarchar(128)
)

Create Table Students (
 Id bigint Primary Key Identity(1,1),
 FirstName nvarchar(128),
 LastName nvarchar(128), 
 Email nvarchar(128),
 Phone nvarchar(128),
 ClientId bigint references Clients(Id), 
)

Create Table Courses (
Id bigint Primary Key Identity(1,1),
MaxStudentIntake int, 
[Name] nvarchar(128), 
[Description] nvarchar(max),
FacilitatorId bigint Foreign Key References Facilitators(Id),
StartDate DateTime,
EndDate DateTime
);

Create Table X_StudentCourses(
  Id bigint Primary Key Identity(1,1),
  StudentId bigint Foreign Key References Students(Id),
  CourseId bigint Foreign Key References Courses(Id)
)

Create Table X_StudentCourseResults(
 Id bigint Primary Key Identity(1,1),
 StudendId bigint Foreign Key References Students(Id),
 CourseId bigint Foreign Key References Courses(Id),
 Competent bit not null default(0)
)

Create Table CourseCertificate(
  Id bigint Primary Key Identity(1,1),
  DateIssued DateTime,
  ExpirationDate DateTime,
  IssuedTo bigint Foreign Key References Students(Id),
  CourseId bigint Foreign Key References Courses(Id)
)
