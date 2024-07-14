<?php

$conn = mysqli_connect('localhost', 'root', '', 'earthquake');
if(!$conn){
    echo 'Connection Error: ' . mysqli_connect_error();
}
// Check if a level button is clicked
$levelFilter = ''; // Initialize the level filter variable
$orderBy = 'ORDER BY dateVal'; // Default sorting by DateTime

// Check if a level button is clicked
if (isset($_GET['level'])) {
    $selectedLevel = $_GET['level'];

    // Validate the selected level
    if ($selectedLevel === '1' || $selectedLevel === '2' || $selectedLevel === '3') {
        $levelFilter = "WHERE intensityVal = '$selectedLevel'";
        $orderBy = 'ORDER BY dateVal DESC'; // Sort by DateTime in descending order for the selected level
    }
}

// write query with level filter and sorting
$sql = "SELECT dateVal, timeVal, intensityVal FROM sensortbl $levelFilter $orderBy";

// make & get result
$result = mysqli_query($conn, $sql);
// fetch results as array
$info = mysqli_fetch_all($result, MYSQLI_ASSOC);
$rowcount = mysqli_num_rows($result);

// close connection
mysqli_free_result($result);
mysqli_close($conn);
?>


<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Earth Detection</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">
    <link rel="stylesheet" href="css/style.css">
</head>
<body>

<nav class="navbar navbar-expand-lg navbar-dark" style="background-color: #9C0A07;">
  <a class="navbar-brand" href="#"><b>Earthquake Detector</b></a>
  <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
    <span class="navbar-toggler-icon"></span>
  </button>
  <div class="collapse navbar-collapse" id="navbarSupportedContent">
    <ul class="navbar-nav mr-auto">
      <li class="nav-item active">
        <a class="nav-link" href="index.php">Home <span class="sr-only">(current)</span></a>
      </li>
        <ul class="navbar-nav mr-auto">
        <li class="nav-item">
        <a class="nav-link" href="index.php?level=1">Level 1</a>
      </li>
      <li class="nav-item">
        <a class="nav-link" href="index.php?level=2">Level 2</a>
      </li>
      <li class="nav-item">
        <a class="nav-link" href="index.php?level=3">Level 3</a>
      </li>
    </ul>
    <span class="navbar-text">
          <?php echo "| $rowcount Records Found" ?>
        </span>
      </li>
    </ul>
  </div>
</nav>

<h3 class="text-center grey-text"><b>Detected Earthquakes</b></h4>
<div class="container">
    <div class="row">

            <?php
            $counter = 0; // Initialize counter variable
            foreach ($info as $sensortbl) {
                if ($counter % 5 === 0 && $counter !== 0) {
                    echo '</div><div class="row">'; // Start a new row after every 5 items
              }
              ?>

            <div class="col s6 md3">
                <div class="card z-depth-0">
                    <div class="card-header text-center">
                        <h6>Level: <?php echo htmlspecialchars($sensortbl['intensityVal']); ?></h6>
                    </div>
                    <div class="card-content text-center">
                        <h5>Date: <?php echo htmlspecialchars($sensortbl['dateVal']); ?></h5>
                        <div>Time: <?php echo htmlspecialchars($sensortbl['timeVal']); ?></div>
                        <?php if (isset($_GET['level'])) { // Check if a level button is clicked ?>
                            <div class="card-action right-align">
                              <a class="btn btn-primary" href="details.php?id=<?php echo $sensortbl['dateVal']; ?>">More</a>
                            </div>
                        <?php } else { ?>
                            <div class="card-action right-align">
                              <a class="btn btn-primary" href="details.php?id=<?php echo $sensortbl['dateVal']; ?>">More</a>
                            </div>
                        <?php $counter++;} ?>
                    </div>
                </div>
            </div>

        <?php } ?>
    </div>
</div>

<script src="https://ajax/googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>    
</body>
</html>