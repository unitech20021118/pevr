<?php
$filename = $_REQUEST['filename'];
$file_handle = fopen($filename, "r") or die("file not found");
while (!feof($file_handle)) {
   $line = fgets($file_handle);
   echo $line;
}
fclose($file_handle);
?>