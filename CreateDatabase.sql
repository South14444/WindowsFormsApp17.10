create database TeaShop
go

use TeaShop
go

create table TypeOfTea(
	ID int identity(1,1) not null primary key,
	ColorOfTea nvarchar(30) not null constraint UQcolor unique
)

create table Countries(
	ID int identity(1,1) not null primary key,
	Name nvarchar(30) not null constraint UQcountry unique
)

create table TeaInfo(
	ID int identity not null primary key,
	Name nvarchar(30) not null,
	CountryId int not null constraint FKCid foreign key
									(CountryId) references Countries(ID),
	TypeId int not null constraint FKTid foreign key
									(TypeId) references TypeOfTea(ID),
	Grammes int not null,
	Cost money not null
)