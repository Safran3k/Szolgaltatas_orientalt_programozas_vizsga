-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2023. Jan 08. 11:43
-- Kiszolgáló verziója: 10.4.25-MariaDB
-- PHP verzió: 8.1.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `filmek_db`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `felhasznalok`
--

CREATE TABLE `felhasznalok` (
  `id` int(11) UNSIGNED NOT NULL,
  `FelhasznaloNev` varchar(100) NOT NULL,
  `Jelszo` varchar(64) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- A tábla adatainak kiíratása `felhasznalok`
--

INSERT INTO `felhasznalok` (`id`, `FelhasznaloNev`, `Jelszo`) VALUES
(1, 'teszt', '6c90aa3760658846a86a263a4e92630e'),
(2, 'teszt1', 'fe282d20bc8dcc12647088d0fe0ca7d7'),
(3, 'teszt2', 'e970707c584b0c4574564ad239301c01'),
(4, 'teszt3', 'f6529e2a4a0fcfb629ac78007f71379c'),
(5, 'ujfelhasznalo', 'bfd59291e825b5f2bbf1eb76569f8fe7'),
(6, 'asdasd', 'a8f5f167f44f4964e6c998dee827110c');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `filmek`
--

CREATE TABLE `filmek` (
  `id` int(10) UNSIGNED NOT NULL,
  `FelhasznaloId` int(10) UNSIGNED NOT NULL,
  `FilmCime` varchar(150) NOT NULL,
  `Rendezo` varchar(60) NOT NULL,
  `Mufaj` varchar(50) NOT NULL,
  `PremierDatuma` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- A tábla adatainak kiíratása `filmek`
--

INSERT INTO `filmek` (`id`, `FelhasznaloId`, `FilmCime`, `Rendezo`, `Mufaj`, `PremierDatuma`) VALUES
(1, 1, 'The Hateful Eight', 'Quentin Tarantino', 'amerikai western', '2015-09-17'),
(2, 2, 'Prometheus', 'Ridley Scott', 'amerikai-angol fantasztikus kalandfilm', '2012-07-20'),
(12, 6, 'Black Adam', 'Jaume Collet-Serra', 'amerikai sci-fi akciófilm', '2022-12-02'),
(14, 1, 'Prey', 'Dan Trachtenberg', 'amerikai akciófilm, kalandfilm', '2022-10-13'),
(15, 1, 'teszt film címe', 'teszt rendező', 'teszt műfaj', '2022-09-15');

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `felhasznalok`
--
ALTER TABLE `felhasznalok`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `filmek`
--
ALTER TABLE `filmek`
  ADD PRIMARY KEY (`id`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `felhasznalok`
--
ALTER TABLE `felhasznalok`
  MODIFY `id` int(11) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT a táblához `filmek`
--
ALTER TABLE `filmek`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
