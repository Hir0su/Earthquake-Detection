<?php

include_once('connects.php');

$stateVal = $_GET['stateVal'];
$dateVal = $_GET['dateVal'];
$timeVal = $_GET['timeVal'];

$result = mysqli_query($con,"INSERT INTO `stateTBL`(`stateVal`, `dateVal`, `timeVal`) VALUES ('$stateVal','$dateVal','$timeVal')");

echo "Data Inserted";

?>