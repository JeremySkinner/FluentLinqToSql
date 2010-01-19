create table Customer(
	Id int primary key identity,
	Name nvarchar(250)
)
GO
create table Customer2(
	Id int primary key identity,
	CustomerName nvarchar(250)
)
GO
create table [Order](
	Id int primary key identity,
	ProductName nvarchar(250),
	Amount money,
	CustomerId int not null
)
GO