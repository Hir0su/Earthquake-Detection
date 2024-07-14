<?php

include_once('connects.php');

$query = "SELECT * FROM `stateTBL` ORDER BY `stateTBL`.`dateVal` DESC, `stateTBL`.`timeVal` DESC";
$check = mysqli_query($con, $query);

if ($check === FALSE) {
    echo json_encode(array(
        "error" => "Database query failed"
    ));
    exit;
}

$row = mysqli_fetch_array($check);

$state = $row['stateVal'] ?? "0"; // Assign 0 if stateVal is null

$data = array(
    "state" => $state
);



header('Content-Type: application/json');
echo json_encode($data);

?>