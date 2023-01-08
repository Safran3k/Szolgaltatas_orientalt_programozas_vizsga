<?php

include("./db.php");

$request = $_SERVER['REQUEST_METHOD'];

switch ($request) {
	case "GET":
		if (!empty($_GET["FelhasznaloNev"]) && !empty($_GET["Jelszo"])) {

			$felhasznalok = login($_GET["FelhasznaloNev"], $_GET["Jelszo"]);

			echo json_encode($felhasznalok);
		}
		elseif(!empty($_GET["FelhasznaloNev"]) && empty($_GET["Jelszo"])) { 

			$felhasznalok = felhasznalokLekerdezese($_GET["FelhasznaloNev"]);
			echo json_encode($felhasznalok);
		}
		else {
			echo json_encode(array(
					'error' => 1,
					'message' => 'Sikertelen bejelentkezes!'
				)
			);
		}
		break;
	case "POST":
		if(!empty($_POST["FelhasznaloNev"]) && !empty($_POST["Jelszo"])) {
			
			regisztracio();
		}else {
			echo json_encode(array(
					'error' => 1,
					'message' => 'Sikertelen regisztráció, hiányzó felhasználónév, vagy jelszó!'
				)
			);
		}
		break;
	case "PATCH":
		// SESSION kezelést kivettem
		logout();
		break;
	default:
		header('HTTP/1.1 405 Method Not Allowed');
		header('Allow: GET, POST, PATCH');
		break;
}



function login($felh, $jelszo) {
	global $connection;
	
	$result = $connection -> query("SELECT * FROM felhasznalok WHERE FelhasznaloNev = '$felh' AND jelszo = MD5('$jelszo')");
	
	return $result->fetch_all(MYSQLI_ASSOC)[0];
}

function logout() {
	session_start();
	unset($_SESSION["FelhasznaloNev"]);
	unset($_SESSION["Jelszo"]);
	session_destroy();
}

// Regisztrációnál lekérdezem hogy az adott felhasználónéven regisztrált-e már valaki
function felhasznalokLekerdezese($felh) {
	global $connection;
	
	$result = $connection -> query("SELECT COUNT(id) FROM felhasznalok WHERE FelhasznaloNev = '$felh'");
	
	return $result->fetch_all(MYSQLI_ASSOC)[0];
}

function regisztracio() {
	global $connection;

    $felhasznaloNev = $_POST["FelhasznaloNev"]; 
    $jelszo = $_POST["Jelszo"];

    echo $query="INSERT INTO felhasznalok SET FelhasznaloNev='$felhasznaloNev', Jelszo=MD5('$jelszo')";

    if(mysqli_query($connection, $query))
    {
       echo json_encode(array(
				'error' => 1,
				'message' => 'Sikeres regisztráció!'
			)
		);
    }
    else
    {
		echo json_encode(array(
				'error' => 1,
				'message' => 'Sikertelen regisztráció!'
			)
		);
    }
    header('Content-Type: application/json');
}