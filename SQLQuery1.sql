drop table if exists StudentTbl;
create table StudentTbl
(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Name nvarchar(255),
	Class nvarchar(50),
	Gender char(1),
	DOB date
);

INSERT INTO StudentTbl VALUES('Ab', '12', 'M', '2013-10-23');

select * from StudentTbl;


