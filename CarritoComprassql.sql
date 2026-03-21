-- DESKTOP-JJ9DM3F\SQLEXPRESS

create database dbCarritoCompras
go

use dbCarritoCompras
go

-------------------------------------------------CATEGORIA--------------------------------------------------------

create table Categoria
(
	IdCategoria int identity(1,1),
	Descripcion varchar(100),
	Activo bit default 1,
	FechaRegistro datetime default getdate(),
	constraint PK_Categoria primary key (IdCategoria)
)
go

-------------------------------------------------MARCA------------------------------------------------------------

create table Marca
(
	IdMarca int identity(1,1),
	Descripcion varchar(100),
	Activo bit default 1,
	FechaRegistro datetime default getdate(),
	constraint PK_Marca primary key (IdMarca)
)
go

-------------------------------------------------PRODUCTO----------------------------------------------------------

create table Producto
(
	IdProducto int identity(1,1),
	Nombre varchar(500),
	Descripcion varchar(500),
	IdMarca int,
	IdCategoria int,
	Precio decimal(10,2) default 0,
	Stock int,
	RutaImagen varchar(100),
	NombreImagen varchar(100),
	Activo bit default 1,
	FechaRegistro datetime default getdate(),
	constraint PK_Producto primary key (IdProducto),
	constraint FK_MarcaProducto foreign key (IdMarca) references Marca(IdMarca),
	constraint FK_CategoriaProducto foreign key (IdCategoria) references Categoria(IdCategoria)
)
go

-------------------------------------------------CLIENTE----------------------------------------------------------

create table Cliente
(
	IdCliente int identity(1,1),
	Nombres varchar(100),
	Apellidos varchar(100),
	Correo varchar(100),
	Clave varchar(150),
	Reestablecer bit default 0,
	FechaRegistro datetime default getdate(),
	constraint PK_Cliente primary key (IdCliente)
)
go

-------------------------------------------------CARRITO----------------------------------------------------------

create table Carrito
(
	IdCarrito int identity(1,1),
	IdCliente int,
	IdProducto int,
	Cantidad int,
	constraint PK_Carrito primary key (IdCarrito),
	constraint FK_ClienteCarrito foreign key (IdCliente) references Cliente(IdCliente),
	constraint FK_ProductoCarrito foreign key (IdProducto) references Producto(IdProducto)
)
go

-------------------------------------------------VENTA------------------------------------------------------------

create table Venta
(
	IdVenta int identity(1,1),
	IdCliente int,
	TotalProducto int,
	MontoTotal decimal(10,2),
	Contacto varchar(50),
	IdDistrito varchar(10),
	Telefono varchar(50),
	Direccion varchar(500),
	IdTransaccion varchar(50),
	FechaVenta datetime default getdate(),
	constraint PK_Venta primary key (IdVenta),
	constraint FK_ClienteVenta foreign key (IdCliente) references Cliente(IdCliente)
)
go


-------------------------------------------------DETALLE_VENTA----------------------------------------------------

create table Detalle_Venta
(
	IdDetalleVenta int identity(1,1),
	IdVenta int,
	IdProducto int,
	Cantidad int,
	Total decimal(10,2),
	constraint PK_DetalleVenta primary key (IdDetalleVenta),
	constraint FK_VentaDetalleVenta foreign key (IdVenta) references Venta(IdVenta),
	constraint FK_ProductoDetalleVenta foreign key (IdProducto) references Producto(IdProducto)
)
go

-------------------------------------------------USUARIO----------------------------------------------------------

create table Usuario
(
	IdUsuario int identity(1,1),
	Nombres varchar(100),
	Apellidos varchar(100),
	Correo varchar(100),
	Clave varchar(150),
	Reestablecer bit default 1,
	Activo bit default 1,
	FechaRegistro datetime default getdate(),
	constraint PK_Usuario primary key (IdUsuario)
)
go

-------------------------------------------------DEPARTAMENTO-----------------------------------------------------

create table Departamento
(
	IdDepartamento varchar(2) not null,
	Descripcion varchar(45) not null
)
go

-------------------------------------------------PROVINCIA--------------------------------------------------------

create table Provincia
(
	IdProvincia varchar(4) not null,
	Descripcion varchar(45) not null,
	IdDepartamento varchar(2) not null
)
go

-------------------------------------------------DISTRITO---------------------------------------------------------

create table Distrito
(
	IdDistrito varchar(6) not null,
	Descripcion varchar(45) not null,
	IdProvincia varchar(4) not null,
	IdDepartamento varchar(2) not null
)
go

delete from Usuario
truncate table Usuario
select * from Usuario
insert into Usuario(Nombres,Apellidos,Correo,Clave) 
values 
('test02 nombre','test02 apellido','user02@example.com','ecd71870d1963316a97e3ac3408c9835ad8cf0f3c1bc703527c30265534f75ae')
go

delete from Categoria
truncate table Categoria
select * from Categoria
insert into Categoria(Descripcion) 
values 
('Tecnologia'),
('Muebles'),
('Dormitorio'),
('Deportes')
go

select * from Marca
insert into Marca(Descripcion) 
values
('SONYTE'),
('HPTE'),
('LGTE'),
('HYUNDAITE'),
('CANONTE'),
('ROBERTA ALLENTE')
go

select * from Departamento
insert into Departamento(IdDepartamento,Descripcion)
values 
('01','Arequipa'),
('02','Ica'),
('03','Lima')
go

select * from Provincia
insert into Provincia(IdProvincia,Descripcion,IdDepartamento)
values
('0101','Arequipa','01'),
('0102','Camaná','01'),

--ICA - PROVINCIAS
('0201', 'Ica ', '02'),
('0202', 'Chincha ', '02'),

--LIMA - PROVINCIAS
('0301', 'Lima ', '03'),
('0302', 'Barranca ', '03')
go

select * from Distrito
insert into Distrito(IdDistrito,Descripcion,IdProvincia,IdDepartamento) 
values 
('010101','Nieva','0101','01'),
('010102', 'El Cenepa', '0101', '01'),
('010201', 'Camaná', '0102', '01'),
('010202', 'José María Quimper', '0102', '01'),

--ICA - DISTRITO
('020101', 'Ica', '0201', '02'),
('020102', 'La Tinguiña', '0201', '02'),
('020201', 'Chincha Alta', '0202', '02'),
('020202', 'Alto Laran', '0202', '02'),


--LIMA - DISTRITO
('030101', 'Lima', '0301', '03'),
('030102', 'Ancón', '0301', '03'),
('030201', 'Barranca', '0302', '03'),
('030202', 'Paramonga', '0302', '03')

select IdUsuario,Nombres,Apellidos,Correo,Clave,Reestablecer,Activo from Usuario
update Usuario set Activo = 0 where IdUsuario=2
--------------------------------------------//////SP USUARIO///////-----------------------------------------------------

create procedure sp_RegistrarUsuario
(
	@Nombres varchar(100),
	@Apellidos varchar(100),
	@Correo varchar(100),
	@Clave varchar(150),
	@Activo bit,
	@Mensaje varchar(500) output,
	@Resultado int output
)
as
begin
	set @Resultado = 0
	if not exists (select * from Usuario where Correo = @Correo)
	begin
		insert into Usuario(Nombres,Apellidos,Correo,Clave,Activo)values
		(@Nombres,@Apellidos,@Correo,@Clave,@Activo)
		set @Resultado = SCOPE_IDENTITY()
	end
	else
	set @Mensaje = 'El correo del usuario ya existe'
end

create procedure sp_EditarUsuario
(
	@IdUsuario int,
	@Nombres varchar(100),
	@Apellidos varchar(100),
	@Correo varchar(100),
	@Activo bit,
	@Mensaje varchar(500) output,
	@Resultado bit output
)
as
begin
	set @Resultado = 0
	if not exists (select * from Usuario where Correo = @Correo and IdUsuario != @IdUsuario)
	begin
		update top (1) Usuario set
		Nombres = @Nombres,
		Apellidos = @Apellidos,
		Correo = @Correo,
		Activo = @Activo
		where IdUsuario = @IdUsuario

		set @Resultado = 1
	end
	else
	set @Mensaje = 'El correo del usuario ya existe'
end

------------------------------------------//////SP CATEGORIA///////-----------------------------------------------------
create procedure sp_RegistrarCategoria
(
	@Descripcion varchar(100),
	@Activo bit,
	@Mensaje varchar(500) output,
	@Resultado bit output
)
as
begin 
	set @Resultado = 0
	if not exists (select * from Categoria where Descripcion = @Descripcion)
	begin
		insert into Categoria(Descripcion,Activo)values
		(@Descripcion,@Activo)
		set @Resultado = SCOPE_IDENTITY()
	end
	else
	set @Mensaje = 'La categoria ya existe'	
end

create procedure sp_EditarCategoria
(
	@IdCategoria int,
	@Descripcion varchar(100),
	@Activo bit,
	@Mensaje varchar(500) output,
	@Resultado bit output
)
as
begin 
	set @Resultado = 0
	if not exists (select * from Categoria where Descripcion = @Descripcion and IdCategoria != @IdCategoria)
	begin
		UPDATE top (1) Categoria set
		Descripcion = @Descripcion,
		Activo = @Activo
		where IdCategoria = @IdCategoria

		set @Resultado = 1
	end
	else
	set @Mensaje = 'La categoria ya existe'	
end

create procedure sp_EliminarCategoria
(
	@IdCategoria int,
	@Mensaje varchar(500) output,
	@Resultado bit output
)
as
begin 
	set @Resultado = 0
	if not exists (
	select * from Producto p 
	inner join Categoria c 
	on c.IdCategoria = p.IdCategoria
	where p.IdCategoria = @IdCategoria)
	begin
		delete top (1) from Categoria 
		where IdCategoria = @IdCategoria

		set @Resultado = 1
	end
	else
	set @Resultado = 0
	set @Mensaje = 'La categoria se encuentra relacionada a un producto'	
end

SELECT * FROM Categoria

---------------------------------------------//////SP MARCA///////------------------------------------------------------
create procedure sp_RegistrarMarca
(
	@Descripcion varchar(100),
	@Activo bit,
	@Mensaje varchar(500) output,
	@Resultado bit output
)
as
begin 
	set @Resultado = 0
	if not exists (select * from Marca where Descripcion = @Descripcion)
	begin
		insert into Marca(Descripcion,Activo)values
		(@Descripcion,@Activo)
		set @Resultado = SCOPE_IDENTITY()
	end
	else
	set @Mensaje = 'La marca ya existe'	
end

create procedure sp_EditarMarca
(
	@IdMarca int,
	@Descripcion varchar(100),
	@Activo bit,
	@Mensaje varchar(500) output,
	@Resultado bit output
)
as
begin 
	set @Resultado = 0
	if not exists (select * from Marca where Descripcion = @Descripcion and IdMarca != @IdMarca)
	begin
		UPDATE top (1) Marca set
		Descripcion = @Descripcion,
		Activo = @Activo
		where IdMarca = @IdMarca

		set @Resultado = 1
	end
	else
	set @Mensaje = 'La marca ya existe'	
end

create procedure sp_EliminarMarca
(
	@IdMarca int,
	@Mensaje varchar(500) output,
	@Resultado bit output
)
as
begin 
	set @Resultado = 0

	if not exists 
	(
	select * from Producto p 
	inner join Marca m 
	on m.IdMarca = p.IdMarca
	where p.IdMarca = @IdMarca
	)
	begin
		delete top (1) from Marca 
		where IdMarca = @IdMarca

		set @Resultado = 1
	end
	else
	set @Resultado = 0
	set @Mensaje = 'La Marca se encuentra relacionada a un producto'	
end

SELECT * FROM Marca