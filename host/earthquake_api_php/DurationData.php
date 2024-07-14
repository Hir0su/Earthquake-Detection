<?php

include_once('connects.php');

$query = "SELECT * FROM `durationTBL` ORDER BY `durationTBL`.`dateVal` DESC, `durationTBL`.`timeVal` DESC";
$check = mysqli_query($con, $query);

if ($check === FALSE) {
    echo json_encode(array(
        "error" => "Database query failed"
    ));
    exit;
}

$row = mysqli_fetch_array($check);

$duration = $row['durationVal'] ?? "1"; // Assign 0 if stateVal is null

$data = array(
    "duration" => $duration
);



header('Content-Type: application/json');
echo json_encode($data);

?>