<?php

include_once('connects.php');

$sqlCheck = "SELECT COUNT(*) FROM `sensorTBL`";

$result = $con->query($sqlCheck);
$row = $result->fetch_row();
$rowCount = $row[0];

if ($rowCount > 0) {
    // The table has data, truncate it
    $sqlTruncate = "TRUNCATE TABLE `sensorTBL`";
    
    if ($con->query($sqlTruncate) === true) {
        echo "Table data deleted successfully.";
    } else {
        echo "Error deleting table data: " . $con->error;
    }
} else {
    echo "Table is already empty. No data to delete.";
}

?>