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

select* from Usuario;
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

------------------------------------------------- //SP PRODUCTO//------------------------------------------------------

create procedure sp_RegistrarProducto
(
	@Nombre varchar(100),
	@Descripcion varchar(100),
	@IdMarca varchar(100),
	@IdCategoria varchar(100),
	@Precio decimal(10,2),
	@Stock int,
	@Activo bit,
	@Mensaje varchar(500) output,
	@Resultado int output
)
as
begin
	set @Resultado = 0

	if not exists (select * from Producto where Nombre = @Nombre)

	begin

		insert into Producto(Nombre,Descripcion,IdMarca,IdCategoria,Precio,Stock,Activo)values
		(@Nombre,@Descripcion,@IdMarca,@IdCategoria,@Precio,@Stock,@Activo)

		set @Resultado = SCOPE_IDENTITY()

	end

	else
	set @Mensaje = 'El producto ya existe'	
	
end

create procedure sp_EditarProducto
(
	@IdProducto int,
	@Nombre varchar(500),
	@Descripcion varchar(500),
	@IdMarca int,
	@IdCategoria int,
	@Precio decimal(10,2),
	@Stock int,
	@Activo bit,
	@Mensaje varchar(500) output,
	@Resultado bit output
)
as
begin
	set @Resultado = 0
	if not exists (select * from Producto where Nombre = @Nombre and IdProducto != @IdProducto)
	begin
		UPDATE top (1) Producto set
		Nombre = @Nombre,
		Descripcion = @Descripcion,
		IdMarca = @IdMarca,
		IdCategoria = @IdCategoria,
		Precio = @Precio,
		Stock = @Stock,
		Activo = @Activo
		where IdProducto = @IdProducto

		set @Resultado = 1
	end
	else
	set @Mensaje = 'El producto ya existe'	
end

create procedure sp_EliminarProducto
(
	@IdProducto int,
	@Mensaje varchar(500) output,
	@Resultado bit output
)
as
begin
	set @Resultado = 0
	if not exists (select * from Detalle_Venta dv
	inner join Producto p on p.IdProducto = dv.IdProducto
	where p.IdProducto = @IdProducto)
	begin
		delete top (1) from Producto 
		where IdProducto = @IdProducto

		set @Resultado = 1
	end
	else
	set @Mensaje = 'El producto se encuentra relacionado a una venta'		
end

select p.IdProducto,p.Nombre,p.Descripcion,
m.IdMarca,m.Descripcion[DesMarca],
c.IdCategoria,c.Descripcion[DesCategoria],
p.Precio,p.Stock,p.RutaImagen,p.NombreImagen,p.Activo
from Producto p 
inner join Marca m on m.IdMarca = p.IdMarca 
inner join Categoria c on c.IdCategoria = p.IdCategoria

select * from Producto

------------------------------------------------- //SP REPORTES//------------------------------------------------------

create procedure sp_ReporteDashboard
as
begin
	select
	(select COUNT(*) from Cliente) [TotalCliente],
	(select ISNULL(SUM(Cantidad),0) from Detalle_Venta) [TotalVenta],
	(select COUNT(*) from Producto) [TotalProducto]
end
exec sp_ReporteDashboard

--------------------------------------- //SP Ventas por Cliente// -----------------------------------------------------

create procedure sp_ReporteVenta
(
	@FechaInicio varchar(10),
	@FechaFin varchar(10),
	@IdTransaccion varchar(50)
)
as
begin

	set dateformat dmy;

	select CONVERT(char(10),v.FechaVenta,103)[FechaVenta],CONCAT(c.Nombres,' ',c.Apellidos)[Cliente],
	p.Nombre[Producto],p.Precio,dv.Cantidad,dv.Total,v.IdTransaccion
	from Detalle_Venta dv
	inner join Producto p on dv.IdProducto = p.IdProducto
	inner join Venta v on v.IdVenta = dv.IdVenta
	inner join Cliente c on c.IdCliente = v.IdCliente
	where CONVERT(date, v.FechaVenta) between @FechaInicio and @FechaFin
	and v.IdTransaccion = IIF(@IdTransaccion ='',v.IdTransaccion,@IdTransaccion)
end

------------------------------------------------- //SP Cliente// ------------------------------------------------------

create procedure sp_RegistrarCliente
(
	@Nombres varchar(100),
	@Apellidos varchar(100),
	@Correo varchar(100),
	@Clave varchar(150),
	@Mensaje varchar(500) output,
	@Resultado int output
)
as
begin
	set @Resultado = 0
	if not exists (select * from Cliente where Correo = @Correo)
	begin

		insert into Cliente(Nombres,Apellidos,Correo,Clave,Reestablecer)values
		(@Nombres,@Apellidos,@Correo,@Clave,0)

		set @Resultado = SCOPE_IDENTITY()

	end
	else
	set @Mensaje = 'El correo del cliente ya existe'
end

select * from Cliente
------------------------------------------- //Select Categoria// ------------------------------------------------------

select distinct m.IdMarca, m.Descripcion from Producto p
inner join Categoria c on c.IdCategoria = p.IdCategoria
inner join Marca m on m.IdMarca = p.IdMarca and m.Activo = 1
where c.IdCategoria = IIF(@IdCategoria = 0,c.IdCategoria,@IdCategoria)

--Es la misma solo con mas validaciones
select distinct m.IdMarca, m.Descripcion 
from Producto p
inner join Categoria c on c.IdCategoria = p.IdCategoria 
    and c.Activo = 1 -- <--- Validamos que la Categoría esté activa
inner join Marca m on m.IdMarca = p.IdMarca 
    and m.Activo = 1 -- <--- Validamos que la Marca esté activa
where c.IdCategoria = IIF(@IdCategoria = 0, c.IdCategoria, @IdCategoria)
  and p.Activo = 1 -- <--- (Opcional) Validamos que el Producto esté activo

  select * from Marca
  select * from Categoria
  select * from Producto where IdCategoria = 13 and Activo = 1


SELECT IdMarca, COUNT(*) as Total 
FROM Producto 
GROUP BY IdMarca

INSERT INTO Producto(Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
VALUES 
('Audífonos Sony XM5', 'Cancelación de ruido líder en la industria', 1, 13, 350.00, 10, 1),
('Laptop HP Envy', 'Procesador i7 con 16GB de RAM', 2, 13, 1200.00, 5, 1),
('Monitor LG UltraGear', '27 pulgadas, 144Hz para gaming', 3, 13, 300.00, 8, 1),
('Laptop Hyundai Thinnote', 'Económica y ligera para oficina', 4, 13, 250.00, 15, 1),
('Cámara Canon EOS R6', 'Cámara Mirrorless profesional', 5, 13, 2200.00, 3, 1);
INSERT INTO Producto(Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
VALUES 
('Juego de Sábanas Premium', 'Algodón egipcio 600 hilos', 6, 15, 80.00, 20, 1),
('Edredón Térmico', 'Ideal para climas fríos', 6, 15, 120.00, 12, 1);
INSERT INTO Producto(Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
VALUES ('Mesa de Centro Geek', 'Mesa con luces LED', 1, 14, 150.00, 5, 1)

INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, RutaImagen, NombreImagen, Activo)
VALUES
-- TECNOLOGÍA (IdCategoria: 13)
('Sony Bravia 55"', 'Smart TV 4K Ultra HD OLED', 1, 13, 850.00, 15, '', '', 1),
('Laptop HP Pavilion 15', 'Procesador Ryzen 7, 16GB RAM', 2, 13, 750.00, 10, '', '', 1),
('Monitor LG 27" Gaming', '144Hz, 1ms, Panel IPS', 3, 13, 299.99, 20, '', '', 1),
('Tablet Hyundai HyTab', '10 pulgadas, 64GB almacenamiento', 4, 13, 120.00, 25, '', '', 1),
('Cámara Canon EOS R10', 'Cámara Mirrorless 24.2 MP', 5, 13, 980.00, 5, '', '', 1),
('Impresora HP LaserJet', 'Impresora láser blanco y negro', 2, 13, 150.00, 12, '', '', 1),
('Audífonos Sony WH-CH520', 'Inalámbricos con Bluetooth', 1, 13, 59.00, 40, '', '', 1),
('Smartphone LG K62', 'Pantalla 6.6", 128GB ROM', 3, 13, 180.00, 8, '', '', 1),

-- MUEBLES (IdCategoria: 14)
('Escritorio para Oficina', 'Madera noble con acabados modernos', 6, 14, 210.00, 7, '', '', 1),
('Silla Ergonómica Pro', 'Ajuste lumbar y reposacabezas', 6, 14, 185.00, 14, '', '', 1),
('Estante para Libros HPStyle', 'Estilo industrial madera y metal', 2, 14, 95.00, 10, '', '', 1),
('Mesa de Centro Minimalista', 'Vidrio templado y base de madera', 6, 14, 130.00, 6, '', '', 1),
('Sillón Reclinable LG-Confort', 'Cuero sintético negro espacioso', 3, 14, 450.00, 4, '', '', 1),

-- DORMITORIO (IdCategoria: 15)
('Colchón Queen Size', 'Resortes ensacados y memory foam', 6, 15, 600.00, 5, '', '', 1),
('Juego de Sábanas Algodón', '600 hilos, King Size, Blanco', 6, 15, 45.00, 30, '', '', 1),
('Lámpara de Noche SonyLED', 'Luz cálida con puerto USB', 1, 15, 35.00, 22, '', '', 1),
('Mesa de Noche Moderna', 'Dos cajones, acabado roble', 6, 15, 75.00, 18, '', '', 1),
('Almohada Ortopédica', 'Espuma con memoria para cuello', 6, 15, 25.00, 50, '', '', 1),
('Cómoda de 4 Cajones', 'Gran capacidad para ropa', 6, 15, 220.00, 9, '', '', 1),

-- DEPORTES (IdCategoria: 16 - Nota: En tu script dice Activo=0, cámbialo a 1 si quieres probar)
('Bicicleta de Montaña 29"', 'Frenos de disco y 21 velocidades', 4, 16, 450.00, 3, '', '', 1),
('Set de Pesas 20kg', 'Mancuernas ajustables de acero', 4, 16, 85.00, 15, '', '', 1),
('Smartwatch Sony Sport', 'GPS y sensor de ritmo cardíaco', 1, 16, 190.00, 12, '', '', 1),
('Pelota de Básquet Pro', 'Grip profesional para exteriores', 4, 16, 30.00, 40, '', '', 1),

-- NIÑOS (IdCategoria: 18)
('Cuna Convertible', 'Madera de pino, se vuelve cama', 6, 18, 320.00, 5, '', '', 1),
('Tablet Hyundai Kids', 'Protector de silicona azul', 4, 18, 95.00, 15, '', '', 1),
('Juego de Bloques Construcción', '500 piezas de colores', 6, 18, 40.00, 20, '', '', 1),
('Cámara Canon Kids Edition', 'Resistente a golpes y agua', 5, 18, 110.00, 10, '', '', 1),
('Audífonos LG Kids Safe', 'Limitador de volumen integrado', 3, 18, 25.00, 18, '', '', 1),
('Carrito Montable Hyundai', 'Diseño deportivo para niños', 4, 18, 140.00, 6, '', '', 1),
('Mochila Escolar Sony Hero', 'Compartimento para tablet', 1, 18, 45.00, 25, '', '', 1);

-------------------------------------------------------------------
-- 1. NUEVAS CATEGORÍAS (Siguiendo tu identidad 1,1)
-------------------------------------------------------------------
INSERT INTO Categoria(Descripcion) 
VALUES 
('Electrohogar'),   -- ID 19 (aprox)
('Cuidado Personal'),-- ID 20
('Videojuegos'),     -- ID 21
('Libros y Papelería');-- ID 22

-------------------------------------------------------------------
-- 2. NUEVAS MARCAS
-------------------------------------------------------------------
INSERT INTO Marca(Descripcion) 
VALUES 
('SAMSUNGTE'),   -- ID 7
('PHILIPSTE'),   -- ID 8
('NINTENDOTE'),  -- ID 9
('LOGITECHTE'),  -- ID 10
('MABETE'),      -- ID 11
('WHIRLPOOLTE'); -- ID 12

-------------------------------------------------------------------
-- 3. MÁS PRODUCTOS (30+ adicionales para cubrir todo el espectro)
-------------------------------------------------------------------

-- 1. TECNOLOGIA
INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Monitor LG UltraGear', '24 pulgadas 144Hz IPS', m.IdMarca, c.IdCategoria, 280.00, 15, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'LGTE' AND c.Descripcion = 'Tecnologia';

INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Impresora HP LaserJet', 'Impresión láser blanco y negro', m.IdMarca, c.IdCategoria, 150.00, 10, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'HPTE' AND c.Descripcion = 'Tecnologia';

INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Cámara Canon EOS R6', 'Mirrorless Profesional 4K', m.IdMarca, c.IdCategoria, 2100.00, 3, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'CANONTE' AND c.Descripcion = 'Tecnologia';

-- 2. MUEBLES
INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Escritorio Gamer Pro', 'Madera con soporte para cables', m.IdMarca, c.IdCategoria, 240.00, 8, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'ROBERTA ALLENTE' AND c.Descripcion = 'Muebles';

INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Mesa de Centro LG-Style', 'Vidrio templado y metal', m.IdMarca, c.IdCategoria, 110.00, 5, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'LGTE' AND c.Descripcion = 'Muebles';

-- 3. DORMITORIO
INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Cómoda de 6 Cajones', 'Acabado en roble moderno', m.IdMarca, c.IdCategoria, 320.00, 6, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'ROBERTA ALLENTE' AND c.Descripcion = 'Dormitorio';

INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Lámpara de Noche Samsung', 'Luz LED con carga inalámbrica', m.IdMarca, c.IdCategoria, 45.00, 20, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'SAMSUNGTE' AND c.Descripcion = 'Dormitorio';

-- 4. DEPORTES
INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Bicicleta Hyundai MTB', 'Aro 29 con frenos hidráulicos', m.IdMarca, c.IdCategoria, 550.00, 4, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'HYUNDAITE' AND c.Descripcion = 'Deportes';

INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Smartwatch Sony Sport', 'GPS y sensor ritmo cardíaco', m.IdMarca, c.IdCategoria, 195.00, 12, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'SONYTE' AND c.Descripcion = 'Deportes';

-- 5. NIÑOS
INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Tablet Hyundai Kids', 'Protector anticaídas incluido', m.IdMarca, c.IdCategoria, 95.00, 15, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'HYUNDAITE' AND c.Descripcion = 'Niños';

INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Cuna convertible Roberta', 'Madera de pino certificada', m.IdMarca, c.IdCategoria, 420.00, 3, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'ROBERTA ALLENTE' AND c.Descripcion = 'Niños';

-- 6. ELECTROHOGAR
INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Lavadora Whirlpool 18kg', 'Carga frontal sistema ahorro', m.IdMarca, c.IdCategoria, 890.00, 7, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'WHIRLPOOLTE' AND c.Descripcion = 'Electrohogar';

INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Cocina Mabe 4 hornillas', 'Acero inoxidable con horno', m.IdMarca, c.IdCategoria, 350.00, 10, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'MABETE' AND c.Descripcion = 'Electrohogar';

-- 7. CUIDADO PERSONAL
INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Rasuradora Philips 5000', 'Uso seco y húmedo inalámbrica', m.IdMarca, c.IdCategoria, 85.00, 25, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'PHILIPSTE' AND c.Descripcion = 'Cuidado Personal';

INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Secadora Philips Pro', 'Motor AC con difusor', m.IdMarca, c.IdCategoria, 60.00, 18, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'PHILIPSTE' AND c.Descripcion = 'Cuidado Personal';

-- 8. VIDEOJUEGOS
INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Zelda: Tears of Kingdom', 'Juego físico Nintendo Switch', m.IdMarca, c.IdCategoria, 60.00, 40, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'NINTENDOTE' AND c.Descripcion = 'Videojuegos';

INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Headset Logitech G733', 'Inalámbrico RGB LightSync', m.IdMarca, c.IdCategoria, 145.00, 12, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'LOGITECHTE' AND c.Descripcion = 'Videojuegos';

INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Samsung Odyssey G5', 'Monitor Curvo 144Hz 2K', m.IdMarca, c.IdCategoria, 420.00, 6, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'SAMSUNGTE' AND c.Descripcion = 'Videojuegos';

-- 9. LIBROS Y PAPELERIA
INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Set Marcadores Canon', 'Impresión de fotos premium', m.IdMarca, c.IdCategoria, 30.00, 50, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'CANONTE' AND c.Descripcion = 'Libros y Papelería';

INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Agenda Cuero Roberta', 'Diseño exclusivo artesanal', m.IdMarca, c.IdCategoria, 35.00, 100, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'ROBERTA ALLENTE' AND c.Descripcion = 'Libros y Papelería';

-- 10. REFUERZOS ALEATORIOS
INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Smartphone Galaxy A54', 'Pantalla 120Hz 5G', m.IdMarca, c.IdCategoria, 450.00, 9, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'SAMSUNGTE' AND c.Descripcion = 'Tecnologia';

INSERT INTO Producto (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
SELECT 'Mouse HP Wireless', 'Sensor óptico ergonómico', m.IdMarca, c.IdCategoria, 25.00, 60, 1
FROM Marca m, Categoria c WHERE m.Descripcion = 'HPTE' AND c.Descripcion = 'Tecnologia';

select * from Producto
UPDATE Producto 
SET RutaImagen = 'C:\Users\tomas\Downloads\EjerciciosDeProgramacion\CarritoComprasCE\FOTOS_CARRITO', 
    NombreImagen = '2.jpg'  -- <--- IMPORTANTE: Pon el nombre de una foto que SÍ tengas en esa carpeta
WHERE RutaImagen IS NULL OR RutaImagen = '' OR NombreImagen IS NULL;


------------------------------------------- // CARRITO // ------------------------------------------------------
CREATE procedure sp_ExisteCarrito
(
	@IdCliente int,
	@IdProducto int,
	@Resultado bit out
)
as
begin
	if exists(select * from Carrito where IdCliente = @IdCliente and IdProducto = @IdProducto)
		set @Resultado = 1
	else
		set @Resultado = 0
end

create procedure sp_OperacionCarrito
(
	@IdCliente int,
	@IdProducto int,
	@Sumar bit,
	@Mensaje varchar(500) output,
	@Resultado bit output
)
as
begin
	set @Resultado = 1
	set @Mensaje = ''
	
	declare @ExisteCarrito bit = iif(exists(select * from Carrito where IdCliente = @IdCliente and IdProducto = @IdProducto),1,0)
	declare @StockProducto int = (select Stock from Producto where IdProducto = @IdProducto)
	declare @CantidadEnCarrito int = (select Cantidad from Carrito where IdCliente = @IdCliente and IdProducto = @IdProducto)

	begin try
		begin transaction OPERACION
		if(@Sumar = 1)
		begin 
			if(@StockProducto > 0)
			begin
				if(@ExisteCarrito = 1)
					update Carrito set Cantidad = Cantidad + 1 where IdCliente = @IdCliente and IdProducto = @IdProducto
				else
					insert into Carrito(IdCliente,IdProducto,Cantidad)values(@IdCliente,@IdProducto,1)
				
				update Producto set Stock = Stock - 1 where IdProducto = @IdProducto
			end
			else
			begin
				set @Resultado = 0
				set @Mensaje = 'El producto no cuenta con stock disponible'
			end
		end
		else
		begin
			-- MEJORA: Si solo queda 1, lo borramos en lugar de dejarlo en 0
			if(@CantidadEnCarrito > 1)
				update Carrito set Cantidad = Cantidad - 1 where IdCliente = @IdCliente and IdProducto = @IdProducto
			else
				delete from Carrito where IdCliente = @IdCliente and IdProducto = @IdProducto
			
			update Producto set Stock = Stock + 1 where IdProducto = @IdProducto
		end
		commit transaction OPERACION
	end try
	begin catch
		set @Resultado = 0
		set @Mensaje = ERROR_MESSAGE()
		rollback transaction OPERACION
	end catch
end

create function fn_ObtenerCarritoCliente
(
	@IdCliente int
)
returns table
as
return
(
	select p.IdProducto,m.Descripcion[DesMarca],p.Nombre,p.Precio,c.Cantidad,p.RutaImagen,p.NombreImagen
	from Carrito c
	inner join Producto p on p.IdProducto = c.IdProducto
	inner join Marca m on m.IdMarca = p.IdMarca
	where c.IdCliente = @IdCliente
)

alter procedure sp_EliminarCarrito
(
	@IdCliente int,
	@IdProducto int,
	@Resultado bit output
)
as
begin
	set @Resultado = 0
	declare @CantidadProducto int = (select Cantidad from Carrito where IdCliente = @IdCliente and IdProducto = @IdProducto)

	begin try
		begin transaction OPERACION

		update Producto set Stock = Stock + @CantidadProducto where IdProducto = @IdProducto
		delete top(1) from Carrito where IdCliente = @IdCliente and IdProducto = @IdProducto
		set @Resultado = 1

		commit transaction OPERACION

	end try
	begin catch
		set @Resultado = 0
		rollback transaction OPERACION
	end catch
end


------------------------------------------- // REGISTRAR VENTA // ------------------------------------------------------

CREATE TYPE EDetalle_Venta as table
(
	IdProducto int null,
	Cantidad int null,
	Total decimal(18,2) null
)

create procedure usp_RegistrarVenta
(
	@IdCliente int,
	@TotalProducto int,
	@Montototal decimal(18,2),
	@Contacto varchar(100),
	@IdDistrito varchar(6),
	@Telefono varchar(10),
	@Direccion varchar(100),
	@IdTransaccion varchar(50),
	@DetalleVenta [EDetalle_Venta] readonly,
	@Resultado bit output,
	@Mensaje varchar(500) output
)
as
begin
	begin try
		declare @idventa int = 0
		set @Resultado = 1
		set @Mensaje = ''

		begin transaction registro

		insert into Venta(IdCliente,TotalProducto,MontoTotal,Contacto,IdDistrito,Telefono,Direccion,IdTransaccion)
		values(@IdCliente,@TotalProducto,@Montototal,@Contacto,@IdDistrito,@Telefono,@Direccion,@IdTransaccion)

		set @idventa = SCOPE_IDENTITY()

		insert into Detalle_Venta(IdVenta,IdProducto,Cantidad,Total)
		select @idventa,IdProducto,Cantidad,Total from @DetalleVenta

		delete from Carrito where IdCliente = @IdCliente

		commit transaction registro
	end try

	begin catch
		set @Resultado = 0
		set @Mensaje = ERROR_MESSAGE()
		rollback transaction registro
	end catch
end
select * from Cliente
select * from Venta
select * from Detalle_Venta

alter function fn_ListarCompra
(
	@IdCliente int
)
returns table
as
return
(
	select p.Nombre, p.RutaImagen, p.NombreImagen, p.Precio, dv.Cantidad, dv.Total, v.IdTransaccion 
	from Detalle_Venta dv
	inner join Producto p on p.IdProducto = dv.IdProducto
	inner join Venta v on v.IdVenta = dv.IdVenta
	where v.IdCliente = @IdCliente
)
go