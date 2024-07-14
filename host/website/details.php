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
        $levelFilter = "WHERE levels = '$selectedLevel'";
        $orderBy = 'ORDER BY dateVal DESC'; // Sort by DateTime in descending order for the selected level
    }
}

$info = null; //initialize $info variable

if(isset($_GET['id'])){
  $id = mysqli_real_escape_string($conn, $_GET['id']);
  $sql = "SELECT * FROM sensortbl WHERE dateVal = '$id'";
  $result = mysqli_query($conn, $sql);
  $info = mysqli_fetch_assoc($result);

  mysqli_free_result($result);
  mysqli_close($conn);
}
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

  <script>
  function deleteData(dateTime) {
      if (confirm("Are you sure you want to delete this data?")) {
          // Make an AJAX request to delete the data
          $.ajax({
              type: "POST",
              url: "delete.php",
              data: { dateTime: dateTime },
              success: function (response) {
                  // Refresh the page after successful deletion
                  location.reload();
              },
              error: function (xhr, status, error) {
                  console.error(xhr.responseText);
              }
          });
      }
  }
</script>
</head>
<body>

<nav class="navbar navbar-expand-lg navbar-dark bg-dark">
  <a class="navbar-brand" href="#">Earthquake Detector</a>
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
      </li>
    </ul>
  </div>
</nav>

<div class="card" style="width: 18rem; margin-left: auto;
    margin-right: auto; vertical-align: middle">
  <div class="card z-depth-0">
  <div class="col s6 md3">
    <?php if($info): ?>
      <div class="card-header">
      <h4>Level: <?php echo htmlspecialchars($info['intensityVal']); ?></h4>
      </div>
        <h4>Date: <?php echo htmlspecialchars($info['dateVal']); ?></h4>
        <h4>Time: <?php echo htmlspecialchars($info['timeVal']); ?></h4>
        <form id="delete-form" method="POST" action="delete.php">
          <input type="hidden" name="dateVal" value="<?php echo $info['dateVal']; ?>">
          <input type="hidden" name="timeVal" value="<?php echo $info['timeVal']; ?>">
          <button class="btn btn-danger" type="submit" onclick="return confirm('Are you sure you want to delete this data?')">Delete</button>
        </form>


    <?php else: ?>
      <h1>Data Not Found</h1>
    <?php endif; ?>
    </div>
    </div>
</div>

<script src="https://ajax/googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>  
</body>
</html>