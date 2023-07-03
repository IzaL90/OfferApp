CREATE TABLE IF NOT EXISTS `bids` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(150) NOT NULL,
  `Description` TEXT NULL,
  `Created` DATETIME NOT NULL,
  `Updated` DATETIME DEFAULT NULL,
  `Count` INT(11) NOT NULL,
  `FirstPrice` DECIMAL(14,4) NOT NULL,
  `LastPrice` DECIMAL(14,4) NULL,
  `Published` TINYINT(1) NULL,
  PRIMARY KEY (`Id`),
  INDEX `idx_bids_title` (`Name`),
  INDEX `idx_bids_created` (`Created`)
)
