<?php
    //Account variables
    $username = $_REQUEST["Username"];
    $password = sha1($_REQUEST["Password"]);
    $timestamp = $_REQUEST["Timestamp"];

    include 'connection.php';
    //Look for account
    $sql = "SELECT user_name
            FROM logins
            WHERE user_name = ?
            AND pass = ?";

    if(mysqli_stmt_prepare($stmt, $sql))
    {
        mysqli_stmt_bind_param($stmt, 'ss', $username, $password);
        mysqli_stmt_execute($stmt);
        $result = mysqli_stmt_store_result($stmt);

        if ($stmt->num_rows != 1)
        {
            echo "mismatch";
        }
        else
        {
            echo "success";
            //Set Timestamp
            $sql = "UPDATE logins SET last_login = '$timestamp' WHERE user_name = '$username'";

            mysqli_query($conn, $sql);
        }
    }

    mysqli_close($conn);
?>
