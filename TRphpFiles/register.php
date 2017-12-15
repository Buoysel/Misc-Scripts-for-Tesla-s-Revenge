<?php
    //Account Variables
    $username = $_REQUEST["Username"];
    $firstName = $_REQUEST["FirstName"];
    $lastName = $_REQUEST["LastName"];
    $password = sha1($_REQUEST["Password"]);

    //Games array
    $games = array("game", "quotron", "wordsearch",
                    "pipes", "powerbox", "bookcart",
                    "terminal", "runner");


    include 'connection.php';


    //Check if account is used.
    $sql = "SELECT user_name FROM logins WHERE user_name = ?";
    if(mysqli_stmt_prepare($stmt, $sql))
    {
        mysqli_stmt_bind_param($stmt, 's', $username);
        mysqli_stmt_execute($stmt);
        $result = mysqli_stmt_store_result($stmt);

        if ($stmt->num_rows != 0)
        {
            echo "used";
        }
        else
        {
            $sql = "INSERT INTO logins (user_name, first_name, last_name, pass) VALUES (?, ?, ?, ?)";

            if(mysqli_stmt_prepare($stmt, $sql))
            {
                mysqli_stmt_bind_param($stmt, 'ssss', $username, $firstName, $lastName, $password);
                mysqli_stmt_execute($stmt);

                echo "success";
            }

            //Set the user's Game data to their "blank" states
            foreach ($games as $game) {
                $sql = "INSERT INTO " . $game . "_records (user_name) VALUES (?)";

                if (mysqli_stmt_prepare($stmt, $sql))
                {
                    mysqli_stmt_bind_param($stmt, 's', $username);
                    mysqli_stmt_execute($stmt);

                    echo mysqli_error($conn);
                }
                else
                {
                    echo "Something went wrong";
                }
            }

            //Create a save slot in the database.
            $sql = "INSERT INTO save_data (user_name) VALUES (?)";

            if (mysqli_stmt_prepare($stmt, $sql))
            {
              mysqli_stmt_bind_param($stmt, 's', $username);
              mysqli_stmt_execute($stmt);

              echo mysqli_error($conn);
            }
            else
            {
                echo "Something went wrong";
            }
        }
    }
    mysqli_stmt_close($stmt);
    mysqli_close($conn);
?>
