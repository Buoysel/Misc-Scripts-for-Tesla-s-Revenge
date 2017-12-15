<?php
    //Uploads the player's save file to the file server

    $directory = "Saves";

    if ($_FILES["SaveFile"]["error"] > 0)
    {
        echo "Error: " . $_FILES["SaveFile"]["error"] . "<br />";
    }
    else
    {
        $file = basename($_FILES["SaveFile"]["name"]);
        $tmp_name = $_FILES["SaveFile"]["tmp_name"];

        if (move_uploaded_file($tmp_name, "$directory/$file"))
        {
            echo "File uploaded Successfully";
        }
    }
?>
