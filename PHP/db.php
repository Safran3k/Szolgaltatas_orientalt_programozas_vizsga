<?php

$host="localhost";
$port=3306;
$socket="";
$user="root";
$password="";
$dbname="filmek_db";

$connection = new mysqli($host, $user, $password, $dbname, $port, $socket)
    or die ("Connect failed: ".mysqli_connect_error());
    
if ($connection -> connect_errno > 0) {
    printf("Connect failed: %s\n", $connection -> connect_error);
    exit();
}

