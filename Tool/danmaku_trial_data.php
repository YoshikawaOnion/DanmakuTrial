<?php

// Setting
if (isset($_POST['jsonData'])){
	$file = './' . $_POST['title'] . '.json';
	if (!file_exists($file)) {
		touch($file);
	}
	$current = stripslashes($_POST['jsonData']);
	file_put_contents($file, $current);
}

// Log
if (isset($_POST['logData'])){
	$file = './Log_'.date("YmdHis").'_.txt';
	if (!file_exists($file)) {
		touch($file);
	}
	$current = stripslashes($_POST['logData']);
	file_put_contents($file, $current);
}

?>