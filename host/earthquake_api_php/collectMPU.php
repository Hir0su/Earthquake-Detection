<?php 

include_once('connects.php');
$d = $_GET['dateVal'];
$t = $_GET['timeVal'];
$i = $_GET['intensityVal'];

$result = mysqli_query($con,"INSERT INTO sensortbl (dateVal,timeVal,intensityVal) VALUES('$d','$t','$i')");
echo "sent";

?>