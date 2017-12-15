<?php
    $user_name = $_REQUEST["Username"];
    $file = "Saves\\$user_name - savedgame0.txt";

    if (file_exists($file))
    {
        echo readfile($file);
    }
    else 
    {
        echo "File not found";
    }
?>