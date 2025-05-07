IF DB_ID('inventario') IS NULL
BEGIN
    CREATE DATABASE inventario;
END
GO

USE inventario;
GO


IF OBJECT_ID('categorias', 'U') IS NULL
BEGIN
    CREATE TABLE categorias (
        idCategoria INT PRIMARY KEY IDENTITY,
        descrip VARCHAR(50)
    );

    INSERT INTO categorias (descrip) VALUES 
        ('General'),
        ('Electrónica'),
        ('Alimentos');
END
GO


IF OBJECT_ID('productos', 'U') IS NULL
BEGIN
    CREATE TABLE productos (
        codigo VARCHAR(30) PRIMARY KEY,
        nombre VARCHAR(100),
        descripcion NVARCHAR(255),
        precio DECIMAL(10,2),
        stock INT,
        idCategoria INT,
        FOREIGN KEY (idCategoria) REFERENCES categorias(idCategoria)
    );

    INSERT INTO productos (codigo, nombre, descripcion, precio, stock, idCategoria) VALUES 
        ('P001', 'Auriculares Bluetooth', 'Auriculares inalámbricos con micrófono', 5500.00, 15, 2),
        ('P002', 'Caja de cereales', 'Cereal integral con frutas secas', 1200.00, 30, 3),
        ('P003', 'Lámpara LED', 'Lámpara de bajo consumo con luz blanca', 3500.00, 0, 1);
END
GO
