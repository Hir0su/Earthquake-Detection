<?php
$conn = mysqli_connect('localhost', 'root', '', 'earthquake');
if (!$conn) {
    echo 'Connection Error: ' . mysqli_connect_error();
    exit;
}

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    if (isset($_POST['dateVal']) && isset($_POST['timeVal'])) {
        $dateVal = mysqli_real_escape_string($conn, $_POST['dateVal']);
        $timeVal = mysqli_real_escape_string($conn, $_POST['timeVal']);

        $sql = "DELETE FROM sensortbl WHERE dateVal = '$dateVal' AND timeVal = '$timeVal'";
        $result = mysqli_query($conn, $sql);

        if ($result) {
            echo "Data deleted successfully";
            // redirect to index.php
            header("Location: index.php");
            exit;
        } else {
            echo "Error deleting data: " . mysqli_error($conn);
        }
    }
}

mysqli_close($conn);
?>