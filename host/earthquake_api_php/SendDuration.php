<?php

include_once('connects.php');

$durationVal = $_GET['durationVal'];
$dateVal = $_GET['dateVal'];
$timeVal = $_GET['timeVal'];

$result = mysqli_query($con,"INSERT INTO `durationTBL`(`durationVal`, `dateVal`, `timeVal`) VALUES ('$durationVal','$dateVal','$timeVal')");

echo "Data Inserted";

?>