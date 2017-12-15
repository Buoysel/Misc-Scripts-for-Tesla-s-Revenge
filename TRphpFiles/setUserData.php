<?php
    /*Will need to fix up this code. Will eventually give each game its own table to track its records, attempts, and victory conditions.*/

    error_reporting(E_ALL);
    ini_set('display_errors', 1);

    $taskType = $_REQUEST["Task"];
    $username = $_REQUEST["Username"];

    include 'connection.php';

    switch ($taskType) {
        case "GET_RECORD":
              $output = "";   //Should receive the 0's or 1's from database (false or true)
              $values = "";   //String of all 0's and 1's sepa
              $gameCounter = 0;

              $games = array("game", "quotron", "wordsearch",
                          "pipes", "powerbox", "bookcart",
                          "terminal", "runner");

              foreach ($games as $game) {
                  $sql = "SELECT $game" . "_beaten
                          FROM   $game" . "_records
                          WHERE user_name = ?";

                  $stmt = $conn->prepare($sql);
                  $stmt->bind_param('s', $username);
                  $stmt->execute();
                  $stmt->store_result();
                  $stmt->bind_result($output);
                  $stmt->fetch();

                  $values .= $output;
                  $gameCounter++;
                  if ($gameCounter < 8){
                      $values .= ":";
                  }

                  mysqli_error($conn);
              }

              echo $values;
              break; //END GET_RECORD

        case "UPDATE_WIN":
            $game = $_REQUEST["Minigame"];
            $gamebeaten = $game . "_beaten";
            $gametable = $game . "_records";
            $gameattemptwon = $game . "_attempts_won";


            $sql = "UPDATE $gametable SET $gamebeaten = 1, $gameattemptwon = $gameattemptwon + 1 WHERE user_name = ?";
            $stmt = $conn->prepare($sql);
            $stmt->bind_param('s', $username);
            $stmt->execute();

            $time = $_REQUEST["Time"];
            $timerecord = $game . "_record";
            $temptime = "";

            $sql = "SELECT $timerecord FROM $gametable WHERE user_name = ?";
            $stmt = $conn->prepare($sql);
            $stmt->bind_param('s', $username);
            $stmt->execute();
            $stmt->store_result();
            $stmt->bind_result($temptime);
            $stmt->fetch();

            if ($time < $temptime || $temptime == NULL){
                $sql = "UPDATE $gametable SET $timerecord = '$time' WHERE user_name = ?";
                $stmt = $conn->prepare($sql);
                $stmt->bind_param('s', $username);
                $stmt->execute();
            }

            mysqli_error($conn);
            break; //END UPDATE_WIN

        case "UPDATE_ATTEMPT":
            $game = $_REQUEST["Minigame"];
            $gametable = $game . "_records";
            $gameattempt = $game . "_attempts";

            $sql = "UPDATE $gametable SET $gameattempt = $gameattempt + 1 WHERE user_name = ?";

            $stmt = $conn->prepare($sql);
            $stmt->bind_param('s', $username);
            $stmt->execute();

            mysqli_error($conn);
            break; //END UPDATE_ATTEMPT;

        case "UPDATE_LOSS":
            $game = $_REQUEST["Minigame"];
            $gametable = $game . "_records";
            $gameattemptlost = $game . "_attempts_lost";

            $sql = "UPDATE $gametable SET $gameattemptlost = $gameattemptlost + 1 WHERE user_name = ?";

            $stmt = $conn->prepare($sql);
            $stmt->bind_param('s', $username);
            $stmt->execute();

            mysqli_error($conn);
            break; //END UPDATE_LOSS

        case "SAVE_DATA":
            $saveData = $_REQUEST["SaveData"];


            $sql = "UPDATE save_data
                    SET save_text = '$saveData'
                    WHERE user_name = ?";

            $stmt = $conn->prepare($sql);
            $stmt->bind_param('s', $username);
            $stmt->execute();

            mysqli_error($conn);
            break;

       case "DOWNLOAD_SAVE":
            $saveData = "";

            $sql = "SELECT save_text
                    FROM save_data
                    WHERE user_name = ?";

            $stmt = $conn->prepare($sql);
            $stmt->bind_param('s', $username);
            $stmt->execute();
            $stmt->store_result();
            $stmt->bind_result($saveData);
            $stmt->fetch();

            if ($saveData == "" || $saveData == NULL)
                echo "Save data not found.";
            else
                echo $saveData;

            mysqli_error($conn);
            break;
    }

    $stmt -> close();
    $conn -> close();
?>
