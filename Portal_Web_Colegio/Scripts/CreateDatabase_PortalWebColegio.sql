
CREATE DATABASE IF NOT EXISTS `Portal_Web_Colegio`
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

USE `Portal_Web_Colegio`;

SET FOREIGN_KEY_CHECKS = 0;

SET FOREIGN_KEY_CHECKS = 1;

CREATE TABLE `Alumnos` (
  `AlumnoId` INT NOT NULL AUTO_INCREMENT,
  `Nombre` TEXT NOT NULL,
  `Apellido` TEXT NOT NULL,
  `FechaNacimiento` DATETIME NOT NULL,
  `Grado` TEXT NOT NULL,
  PRIMARY KEY (`AlumnoId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `Materias` (
  `MateriaId` INT NOT NULL AUTO_INCREMENT,
  `NombreMateria` TEXT NOT NULL,
  `Docente` TEXT NOT NULL,
  PRIMARY KEY (`MateriaId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `Expedientes` (
  `ExpedienteId` INT NOT NULL AUTO_INCREMENT,
  `AlumnoId` INT NOT NULL,
  `MateriaId` INT NOT NULL,
  `NotaFinal` FLOAT NOT NULL,
  `Observaciones` TEXT NOT NULL,
  PRIMARY KEY (`ExpedienteId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX `IX_Expedientes_AlumnoId` ON `Expedientes` (`AlumnoId`);
CREATE INDEX `IX_Expedientes_MateriaId` ON `Expedientes` (`MateriaId`);

ALTER TABLE `Expedientes`
  ADD CONSTRAINT `FK_Expedientes_Alumnos_AlumnoId`
  FOREIGN KEY (`AlumnoId`) REFERENCES `Alumnos` (`AlumnoId`) ON DELETE CASCADE;

ALTER TABLE `Expedientes`
  ADD CONSTRAINT `FK_Expedientes_Materias_MateriaId`
  FOREIGN KEY (`MateriaId`) REFERENCES `Materias` (`MateriaId`) ON DELETE CASCADE;
