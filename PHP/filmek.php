<?php

include("./db.php");

$request = $_SERVER['REQUEST_METHOD'];

switch ($request) {
    case "GET": //select
        if(empty($_GET["FelhasznaloId"])) {			
		    $filmek = filmekListaja();
        }
        else {
            $filmek = filmekListajaFelhSzerint($_GET["FelhasznaloId"]);
        }
        echo json_encode($filmek);
		break;

    case "POST": // insert
        if (!aktFelhasznalo($_POST["FelhasznaloNev"], $_POST["Jelszo"])) {
			header('HTTP/1.0 401 Unauthorized ');
			break;
		}else {
			filmHozzaad();
		}
        break;

    case "PUT": // update (MÁR MŰKÖDIK)
        $content = file_get_contents('php://input');
		$data = json_decode($content, true);
		// header('Content-Type: application/json');
        // echo json_encode($data);
        // echo $data["felhasznalonev"];
        // echo $data["jelszo"];

		if (empty($data["felhasznalonev"]) || 
			empty($data["jelszo"]) || 
			!aktFelhasznalo($data["felhasznalonev"], $data["jelszo"])) {
				echo json_encode( 
				array(
					'error' => 1,
					'message' => 'Bejelentkezes szukseges film modositasahoz.'
				)
			);
			exit;
		}else {
            filmModositas($data["id"], $data["felhasznaloid"], $data["filmcime"], $data["rendezo"], $data["mufaj"], $data["premierdatuma"]);
        }
        break;

    case "DELETE": // delete
        $id = intval($_GET["id"]);
        $felhasznaloNev = $_GET["FelhasznaloNev"];
        $jelszo = $_GET["Jelszo"];

        if (!empty($felhasznaloNev) && !empty($jelszo) && !aktFelhasznalo($felhasznaloNev, $jelszo)) {
            header('HTTP/1.0 401 Unauthorized ');
			break;
        }elseif (empty($id)) {
            echo json_encode( 
				array(
					'error' => 1,
					'message' => 'Nincs film kivalasztva (hianyzo id).'
				)
            );
        }else {
            filmTorlese($id);
        }
        break;

    default:
        header('HTTP/1.1 405 Method Not Allowed');
        break;
}


function aktFelhasznalo($felhNev, $jelszo) {
	global $connection;
	// Itt már nem hash-elem, csak összehasonlítom a felhasználónevet és a jelszót
	$result = count($connection -> query("SELECT * FROM felhasznalok WHERE FelhasznaloNev = '$felhNev' AND Jelszo = '$jelszo';") -> fetch_all());
	return $result > 0;
}

function filmHozzaad() {
	global $connection;
	
    $felhasznaloId = $_POST["FelhasznaloId"];
    $filmCime = $_POST["FilmCime"]; 
    $rendezo = $_POST["Rendezo"];
	$mufaj = $_POST["Mufaj"];
	$premierDatuma = $_POST["PremierDatuma"];

    echo $query="INSERT INTO filmek SET FelhasznaloId='$felhasznaloId', FilmCime='$filmCime', Rendezo='$rendezo', Mufaj='$mufaj', PremierDatuma='$premierDatuma' ";

    if(mysqli_query($connection, $query))
    {
       $response = array(
             'status' => 1,
             'status_message' =>'Film hozzaadva.'
              );
    }
    else
    {
       $response = array(
             'status' => 0,
             'status_message' =>'Hiba film hozzaadasa kozben.'
             );
    }
    header('Content-Type: application/json');
    echo json_encode($response);
}

function filmModositas($id, $felhasznaloId, $filmCime, $rendezo, $mufaj, $premierDatuma){
    global $connection;

	if (!empty($id) &&
    !empty($felhasznaloId) && 
    !empty($filmCime) && 
    !empty($rendezo) && 
    !empty($mufaj) && 
    !empty($premierDatuma)) {
		$connection -> query("UPDATE filmek SET FelhasznaloId=$felhasznaloId, FilmCime='$filmCime', Rendezo='$rendezo', Mufaj='$mufaj', PremierDatuma='$premierDatuma' WHERE id = $id ");
		echo json_encode(
			array(
				'error' => 0,
				'message' => 'Film modositva!'
			)
		);
	}
	else {
		echo json_encode(array(
				'error' => 1,
				'message' => 'Minden adat megadasa kotelezo!'
			)
		);
	}
    
    /*
    global $connection;
    
    $query = "UPDATE filmek SET FelhasznaloId='$felhasznaloId', FilmCime='$filmCime', Rendezo='$rendezo', Mufaj='$mufaj', PremierDatuma='$premierDatuma' WHERE id = '$id' ";

    if(mysqli_query($connection, $query))
    {
        $response = array(
            'status' => 1,
            'status_message' =>'Film modositva.'
        );
    }
    else
    {
        $response = array(
            'status' => 0,
            'status_message' =>'Hiba modositas kozben.'
        );
    }
    header('Content-Type: application/json');
    echo json_encode($response);
    */
}

function filmTorlese($id) {
    global $connection;

    $query = "DELETE FROM filmek WHERE id = '$id' ";

    if(mysqli_query($connection, $query))
    {
        echo json_encode(
            array(
            	'error' => 0,
            	'message' => 'Film torolve!'
            )
        );
    }
    else
    {
        echo json_encode(
            array(
            	'error' => 0,
            	'message' => 'Hiba film torlese soran!'
            )
        );
    }
    header('Content-Type: application/json');
}

function filmekListajaFelhSzerint($id) {
    global $connection;

    $result = $connection -> query("SELECT * FROM filmek WHERE FelhasznaloId='$id'");
	
	return $result->fetch_all(MYSQLI_ASSOC);
}

function filmekListaja() {
	global $connection;
	
	$result = $connection -> query("SELECT * FROM filmek;");
	
	return $result->fetch_all(MYSQLI_ASSOC);
}



?>