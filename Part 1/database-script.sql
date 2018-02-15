-- MySQL Script generated by MySQL Workbench
-- Sat Dec  2 18:02:15 2017
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `mydb` DEFAULT CHARACTER SET utf8 ;
USE `mydb` ;

-- -----------------------------------------------------
-- Table `mydb`.`Owner`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Owner` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Owner` (
  `OwnerId` CHAR(36) NOT NULL,
  `Name` NVARCHAR(60) NOT NULL,
  `DateOfBirth` DATE NOT NULL,
  `Address` NVARCHAR(100) NOT NULL,
  PRIMARY KEY (`OwnerId`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Account`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Account` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Account` (
  `AccountId` CHAR(36) NOT NULL,
  `DateCreated` DATE NOT NULL,
  `AccountType` NVARCHAR(45) NOT NULL,
  `OwnerId` CHAR(36) NOT NULL,
  PRIMARY KEY (`AccountId`),
  INDEX `fk_Account_Owner_idx` (`OwnerId` ASC),
  CONSTRAINT `fk_Account_Owner`
    FOREIGN KEY (`OwnerId`)
    REFERENCES `mydb`.`Owner` (`OwnerId`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;