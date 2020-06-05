create table Products
(
	[ProductID] INT NOT NULL PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(100) NOT NULL,
	[Description] NVARCHAR(500) NOT NULL,
	[Categroy] NVARCHAR(50) NOT NULL,
	[Price] DECIMAL(16,2) NOT NULL
)

select * from Products

insert into Products 
select 'Lifejacket', 'Protective and fashionable', 'Watersports', 48.95
union
select 'Soccer Ball', 'FIFA-approved size and weight', 'soccer', 19.50
union
select 'Corner Flags', 'Give your playing field a professional touch', 'soccer', 34.95
union
select 'Stadium', 'Flat-packed 35.000-seat stadium', 'soccer', 79500
union
select 'Thinking Cap', 'Improve your brain efficiency by 75%', 'Chess', 16.00
union
select 'Unsteady Chair', 'Secretly give your opponent a Chess', 'Chess', 29.95
union
select 'Human Chess Board', 'A fun game for the family', 'Chess', 75.00
union
select 'Bling-Bling King', 'Glod-plated, diamond-studded King', 'Chess', 1200.00
