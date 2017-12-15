<?php
  //MySQL Connection
  $conn = mysqli_connect("168.16.222.104:3306", "TRUser", "TRPass87","teslas_revenge");
  $stmt = mysqli_stmt_init($conn);
  $sql = "";
  echo "Using localhost I should see this.";
?>
