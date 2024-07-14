<?php

include_once('connects.php');

$query = "SELECT * FROM `sensorTBL` ORDER BY `sensorTBL`.`dateVal` DESC, `sensorTBL`.`timeVal` DESC LIMIT 10" ;
$check = mysqli_query($con, $query);

if ($check === FALSE) {
    echo json_encode(array(
        "error" => "Database query failed"
    ));
    exit;
}

$data = array();

while ($row = mysqli_fetch_array($check)) {
    $data[] = array(
        "date" => $row['dateVal'],
        "time" => $row['timeVal'],
        "intensity" => $row['intensityVal']
    );
}

header('Content-Type: application/json');
echo json_encode($data);



// $row = mysqli_fetch_array($check);
// $data = array(
//     "date" => $row['dateVal'],
//     "time" => $row['timeVal'],
//     "intensity" => $row['intensityVal']
// );

// header('Content-Type: application/json');
// echo json_encode($data);



// while($row = mysqli_fetch_array($check)){
//     $data = array(
//         "date" => $row['dateVal'],
//         "time" => $row['timeVal'],
//         "intensity" => $row['intensityVal']
//     );
//     header('Content-Type: application/json');
//     echo json_encode($data);
// }




?>