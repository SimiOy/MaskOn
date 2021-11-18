# MathPlotFun - documentation
by Simionescu Andrei and Aaron Panaitescu

# Purpose
Through this project we hope to stimulate highschool students to play around with complex
mathematical analysis functions, maybe even have a bit of fun with it!

# TLDR
By plotting a function, a rocket will follow the player's trajectory trying to avoid all obstacles.
The goal of the game is to navigate the xOy graphical coordinate system, by compounding a series of functions,
while avoiding obstacles in place. The game allows all possible compounds of functions. (f+g,f-g,f*g,f(g),f(g+c),f(g*c),f(g-c))
Besides solving the already existing levels, users are allowed to create their own levels, to challenge other
players to try them out as well.

# Mechanics

## 1. The xOy plane

The player can explore the plane by moving the cursor (similar to Desmos) around or using the (WASD) keys
Similar to Desmos, it is possible to zoom into or out of a function infinitely many times without getting into any issues.

## 2. Modifying the function

The player can open a panel in which he can to the following operations on a function f:
- compound functions (+ | - | / | ^)
- unary composition of a function ( fog(x) )
- function ramifications on any arbitrary number of points
```
f(x) = { h(x) , x ∈ H    , H ∪ G =  F , H ∩ G = {}
       { g(x) , x ∈ G
```
- access to basic elementary functions ( sin , cos, tg, ctg, arctrigo, e^x, ln x, x^a, ct)

The implementation of these functions was done using an appropiate color scheme, to make the
game more user-friendly (turcoise looks like a good color for ln(x))

## 3. RunTime/Pause/Reset

When theplayer wants to verify that the function that he has completed verifies (and avoids all obstacles placed),
he can just run the game. A custom object (a rocket) will traverse the player's inputed function and
calculate if any collsiion is being made. If the rocket reaches the end of the level, that means that 
the level was passed.

During runtime the player can pause the game at any given time and check for any possible incosistencies
with the function. He can see and modify it on the spot, and reset the game afterwards.

## 4. Platform and Interfaces

To start this mathematical adventure the player is being given two options:
- A guest user that mentains current progress via cookies (but if the cookies are reset 
  the current progress is lost!)
- Log in to a google account using the google api. This way the progress is saved during multiple
  run times of the game. (Developer google accounts are distinguished since they have multiple
  accesses to special commands to modify exisiting levels or update the databes.)

# Instalation process

Since we are begginers we chose not to host the game on the web, even though our infrastucture would allow us to.
    (To host a website costs money, which we currently lack.)
But with a bit of patience you can follow these easy steps:
- unarchive the rar: SecretProject.rar
- install XAMPP for the decode of the php and sql files
- place into this folder: '\xampp\mysql\data'; this folder 'test' from the archive
- place in this folder '\xampp\htdocs' the following :
    - file: index.php && connect.php
    - the SecretProject folder
- from Xampp's control panel start the apache and the sql server
- open in browser http://localhost/Secret%20Project/index.php

You are all set! try it out!

# Techinical challenges faced

## 0. Libraries and programming languages used:
- languages: javascript, php/html/css, sql 
- libraries: p5.js , math.js, google-api

## 1. The xOy grid

```javascript

class Grid{
    constructor(){ ... }
    update(){ ... }
    draw(){ ... }
}

```

The calculations necessary for traversing the xOy plane are done in the 'grid.js' file.
Grid class contains:
- constructor which initialises variables of position, scale, and other variables used.

```javascript
	constructor(){
		this.norms = [1, 2, 5];
		this.normCount = 0;
		this.grid_norm = 1;
		this.offsetX = 0;
		this.offsetY = 0;
		this.scaleX = 0;
		this.scaleY = 0;
		this.decimalNumber = 10;
		this.endX = 0;
		this.endY = 0;
		this.startX = 0;
		this.startY = 0;
	}
```

- a method update which listens to user input and modifies the above-mentioned variables accordingly

In the following the WASD movement is portrayed:
```javascript
update(){
    ...
    if(!typing){
            if(keyIsDown(16))
                speed = 32;
            else
                speed = 10;
            if(keyIsDown(68))
                offsetX+=speed/scaleX;
            else if(keyIsDown(65))
                offsetX-=speed/scaleX;
            if(keyIsDown(87))
                offsetY-=speed/scaleY;
            else if(keyIsDown(83))
                offsetY+=speed/scaleY;
            }
    ... // next is scaling and calculating numerical indices for the xOy plane
}
```

- A method draw which draws on the website all the information provided each frame in the update method
 
```javascript
draw() {
    ...
    for(let x = 0; x<this.endX; x+=this.grid_norm) //desenam liniile verticale din cadranele 1 si 4
            {
                let p = worldToScreen(x, 0);

                stroke('black');
                strokeWeight(1);
                line(p.x, 0, p.x, windowHeight);
                strokeWeight(0.2);
                for(let j = x+this.grid_norm/5; j<x+this.grid_norm; j+=this.grid_norm/5)
                {
                    let sj = worldToScreen(j, 0).x;
                    line(sj, 0, sj, windowHeight);
                }
                if(x){
                    p.y = max(p.y, 0);
                    p.y = min(p.y, windowHeight - 30);
                    noStroke();
                    fill('white');
                    ellipse(p.x, p.y + 15, 10, 13);
                    fill('black');
                    textSize(15);
                    textAlign(CENTER);
                    if(this.grid_norm>=1){
                        text( x.toString(), p.x, p.y +20 );
                    }
                    else{
                        var rx = round(x, this.decimalNumber);
                        text( rx.toString(), p.x, p.y +20 );
                    }
                }
            }
    ...     // same for the second and third quadrant
}
``` 

![Image](https://raw.githubusercontent.com/NemoInfo/SecretProject/main/grid.png)

## 2. Function drawing

Organised into a class such that the calling and updating of these is as easy as possible

(in ui.js)

```javascript
	p.setup = function()
	{
		inputNo = 0;
		inputs = [-1, -1];
		functionImg = [ {img:'blue', law:(x) => sin(x)},
					{img:'red' ,     law:(x) => cos(x)},
					{img:'green',    law:(x) => log(x)},
					{img:'lime',     law:(x) => exp(x)}, 
					{img:'yellow',  law:(x) => asin(x)},
					{img:'cyan',     law:(x) => tan(x)},
					{img:'orange',  law:(x) => acos(x)},
					{img:'pink',    law:(x) => sqrt(x)},
					{img:'purple',  law:(x) => atan(x)},
					{img:'white',    law:(x) => ct(x)}];

		intervalLaws[0] = functionImg[0];
		draggIndex = -1;
		s = 1-editMode;
	}
```

For the screen drawing, we had some issues and we made two functions for plotting
(in main.js)

```javascript
function plot( a, b, law, color ) // first variant of drawing, evaluates the function
                                 // pixel by pixel and makes the necessary connections
                                 // PROBLEM: type 2 discontinuites, and vertical asymptotes; INEFFICIENT
{
	var screenA = worldToScreen(a, 0).x;
	var screenB = worldToScreen(b, 0).x;
	beginShape();
	noFill();
	stroke(color);
	strokeWeight(3);
	var pwy = -law(a);
	for(let sx = screenA; sx<=screenB; sx++)
	{
		let wx = screenToWorld(sx, 0).x;
		let wy = -law(wx);
		if(wy){
			if( abs(pwy - wy)>50 && pwy*wy<0 )
			{
				endShape();
				beginShape();
			}
			let sy = worldToScreen(0, wy).y;
			sy = min(sy, windowHeight + 10);
			sy = max(sy, -10);
			vertex(sx, sy);
			pwy = wy;
		}
	}
	endShape();
}

function omegaPlot(a, b, law, color) // the final version which SUCCEDDED:
                                    // Evaluate based on the rate of change (first order derivative)
                                   // when the function modifies slighlty -> use less points
                                  // when the function modifies abruptly -> use many points
                                 // as such, we fix the issues with type 1 and type 2 discontinuites at a given point (i.e tan(x) at x = k*pi/2)
                                // in essence the more the function is "detailed" the more points we draw (a liniar function would take least amount of points)
{
	let wx = a;
	stroke(color);
	strokeWeight(3);
	beginShape();
	noFill();
	let pwy = -law(a);
	while(wx<b){
		let wy = -law(wx);
		if( abs(pwy - wy)>50 && pwy*wy<0 )
		{
			endShape();
			beginShape();
		}
		let s = worldToScreen(wx, wy);
		s.y = max( min(s.y,  windowHeight), -3 );
		vertex(s.x, s.y);
		let dx = min( 1/scaleX , abs(1/derivative(law)(wx)));
		dx = max( dx, 6.123233921028994e-4 );
		wx += dx;
		pwy = wy;
	}
	endShape();
}

function derivative(law){ // numeric evaluation of a derivative at a given point
		return x0 => {
			let y0 = law(x0);
			let x = x0 + 1/100000000;
			let y = law(x); 
			return (y - y0) / (x - x0);
		};
}
```


## 3. Obstacles (in obstacles.js)

Shared under multiple classes, united in a single poly-morphic class that contains all of them

```javascript
class SceneObject // clasa polimorfica de obstacole
{
	constructor(fileXPos,fileYPos, scalex, scaley)
    {
        this.scalex = scalex;
        this.scaley = scaley;
        this.xPos = fileXPos;
        this.yPos = fileYPos;
        this.type = -1;
    }
}	

class Rect extends SceneObject{ // obstacolul dreptunghiular
	draw(){
		this.type = 1;
		let shape= worldToScreen(this.xPos,this.yPos);
		noStroke();
		fill(212, 61, 192);
		rect(shape.x,shape.y,this.scalex*scaleX,this.scaley*scaleY);
	}
	collide(x, y){
		return ( x>=this.xPos && x<=this.xPos + this.scalex && y>=this.yPos && y<=this.yPos + this.scaley );
	}
}

class Ellipse extends SceneObject{ // obstacol eliptic
	draw(){
		this.type = 0;
		let shape= worldToScreen(this.xPos,this.yPos);
		noStroke();
		fill(101, 173, 172);
		ellipse(shape.x, shape.y, this.scalex*scaleX, this.scaley*scaleY );
	}
	collide(x, y){
		return ( pow((x-this.xPos)/this.scalex2, 2) + pow((y-this.yPos)/this.scaley2, 2) <= 1 )
	}
}
```

Every object constructed using these functions:
- draw() which simply draws it on to the screen
- collide() which returns whether or not a point is inside an object

## 4. Animations (in animation.js)

When the player runs the game, the central focus of the camera is locked (by default, but can be changed by pressing spacebar)
on the actual object following the trajectory
The entire process follows all mathematical rules:
	- object speeds up when going to infinity on a vertical asysmptote 
	  (we con't want to wait infinte seconds, a few milsieconds is enough)
	- the camera focus is bounded by invisible lines, such that the speed of the camera follows 
	  an offset of the speed of the object (has some bounds in which it doesn't need to move at all for example)
	- The orientation of the rocket (such that it's always going forward), is given by the slope of the function
	  (first order derivative)

One crucial optimization: 
the preCalculate() function "knows" if your rocket will crash before the actual start of the game (we just don't tell the player),
such that we don't have to do manual checks for the collision each frame, we just store the x position of the rocket for the first "impact",
if any.

```javascript
preCalculate()
	{
		this.color = 'yellow';
		var x = obstacles.length;
		var dx = 0.01;
		this.wx = this.a;
		this.deathPos = this.a-1;
		if(x==0)
			return;
		while(this.wx < this.b)
		{
			this.wx += dx;
			this.wy = -this.law(this.wx);
			for(let i=0;i<x;i++)
			{
				if(obstacles[i].collide(this.wx,this.wy))
				{
					this.deathPos = this.wx;
					this.wx = this.a;
					return;
				}
			}
		}
		this.wx = this.a;
		return;
	}
```

Method update() calculates the current position and orientation of the object and the location of the possible "crash"; 
(possible crash might not be any, meaning that the rocket will be able to travers and avoid all obstacels)

```javascript
update()
	{
		if(this.wx >= this.b || this.wx <= this.a)
			this.speed *= -1;
		var dx = 1/this.speed;
		if(this.wx<this.deathPos || this.deathPos==this.a - 1)
			this.wx += dx;
		if(this.wx > this.b)
			this.wx = this.b;
		else if(this.wx < this.a)
			this.wx = this.a;
		this.wy = this.law(this.wx);
		this.sx = worldToScreen(this.wx, 0).x;
		this.wy = min(100, this.wy);
		this.wy = max(-100, this.wy);
		this.wy = -this.wy;
		this.sy = worldToScreen(0, this.wy).y;
		if(this.deathPos !=0)
		{
			if(abs(this.deathPos-this.wx)<=0.01)
			{
				this.color = 'red';
			}
		} 
	}
```

## 5. UI (in ui.js)

The interface might be one of the most appealing and important parts of a game/application.
We tried to preserve a minimal, modern design, using the contents of the screen as little as possible
As such, all menus glide from the sides, and disappear when the cursor is no longer on them.

We used a combiantion of html panels, css style codes, and javascript calls to make the user-experience
as seamless as possible.

For example, when the function pause() is called, all active processes of the game are stopped, to minimalise
background usage.

```javascript
function pause()
{
	paused = 1-paused;
	bottomList.style.top = bottomListOptions[menuOn*(1-paused)];

	if(editMode)
		document.getElementById("shape_selector").style.right = rightOptions[menuOn*(1-paused)];

	document.getElementById("frame").style.filter = blurOptions[paused];
	playButton.style.display = playButtonOptions[paused];
	homeButton.style.display = playButtonOptions[paused];
	settingsButton.style.display = playButtonOptions[paused];
}
```

Meniul de modificare a functiei este cea mai complexa componenta a interfatei. El insusi este un nou canvas P5 suprapus cu cel principal
Intervalele pot fi miscate modificate, compuse. Bara de 2d de previzualizare reprezinta axa Ox, poate fi marita si explorata.

Pentru a desena diferitele intervale in meniul secundar iteram prin toate capetele de intervale si atribuim functia aleasa de jucator


(i.e: in the method p.draw() attributed to menu 'bottom_bar')
```javascript
for(let i=1; i<intervals.length-1; i++)
		{
			var sx = p.worldToScreen(intervals[i]);

			p.strokeWeight(2);
			p.stroke(46, 46, 46);
			p.line(sx, cheight/8, sx, cheight);

			p.fill('orange');
			p.stroke('black');
			p.strokeWeight(1);
			p.triangle(sx, 0, sx-6, cheight/8, sx+6, cheight/8);

			if(intervalLaws[i])
			{
				p.stroke(intervalLaws[i].img);
				p.strokeWeight(2);
				p.line(sx, cheight/16, p.worldToScreen(intervals[i+1]), cheight/16);
				var x = p.worldToScreen( intervals[i]) +15;
				var y = cheight/8 + 10;
				var w = p.worldToScreen( intervals[i+1] ) -15- x; 
				var h = cheight-cheight/8*3;

				if(w>0){
					p.fill(intervalLaws[i].img);
					p.rect( x, y, w, h, w/4);		
				}		
			}
```

## 6. Main (in main.js)

The main.js files links all existing files and classes to run the application.
Contains a series of methods that are called via user inputs, which in turn call other functions
Creates the p5 canvas, updates it and draws it every frame.

To make the transition from the SCREEN space (actual screen pixels) to the ABSTARCT space (plane) xOy (theoretical space),
we use two functions that convert coordinates in realtion to SCALE, POSITION and the xOy plane's origin, screen dimensions and
the webpage dimensions itself.

```javascript
function worldToScreen( worldX, worldY ){
		return { 
			x: (worldX - offsetX)*scaleX ,
			y: (worldY - offsetY)*scaleY }  ;
}
function screenToWorld( screenX, screenY ){
		return { 
			x: (screenX/scaleX + offsetX) ,
		 	y: (screenY/scaleY + offsetY) };
}
```

The main file also contains a series of events that define game-state variables.

```javascript
function keyPressed()
{
	if(typing)
		return;

	if(keyCode == 32 && gamePlaying){ //spacebar
		mode= !mode;
	}
	else if(keyCode == 8) //backspace begins game
    {
        startGame();
    }
    else if( keyCode == 69 ){ //E
    	allow = !allow;
    	toggleMenu();
    }
    else if(keyCode == 80 && editMode==1)//P
    	saveLevel();
    else if(keyCode == 72)//M
    	toggleHints();
    else if(keyCode == 13)
    	pause();
}

function mousePressed()
{
	if(editMode==0)
		return;
	if(mouseButton === LEFT && currentShape!=-1){ // A good example of process: rendering all obstacles at the start of any level
		let wmouse = screenToWorld(mouseX, mouseY);  
		if(currentShape==0)
			obstacles[obstacles.length] = new Ellipse(wmouse.x,wmouse.y, previewScaleX, previewScaleY); 
		else if(currentShape==1) // Se apleaza functiile de obstacole externe descrise mai sus
			obstacles[obstacles.length] = new Rect(wmouse.x - previewScaleX/2, wmouse.y -previewScaleY/2, previewScaleX, previewScaleY);
		obstaclePlaced = true;
	}
	else if(mouseButton === RIGHT)
	{
		if(currentShape!=-1)
			currentShape = -1;
		else{
			var w =  screenToWorld(mouseX, mouseY);
			for(let x = obstacles.length-1; x>=0 ; x--)
			{
				if(obstacles[x].collide(w.x, w.y)){
					obstacles.splice(x, 1);
					x--;
					return;
				}
			}
		}
	}
}
```

## 7. Backend (PHP, SQL, google-api)

We have to rememebr information about players, obstacles levels and already passed levels.
To do this we use a SQL table whihc we update and retrieve information from using a series of php calls

- Data base:

Players table:
```
+-----------+------------------+------+-----+---------+----------------+
| Field     | Type             | Null | Key | Default | Extra          |
+-----------+------------------+------+-----+---------+----------------+
| name      | varchar(30)      | NO   |     | NULL    |                |
| email     | varchar(30)      | YES  |     | NULL    |                |
| password  | varchar(30)      | NO   |     | NULL    |                |
| max_level | int(10) unsigned | NO   |     | 1       |                |
| player_id | int(10) unsigned | NO   | PRI | NULL    | auto_increment |
| session   | varchar(100)     | NO   |     | NULL    |                |
+-----------+------------------+------+-----+---------+----------------+
```

Objects table:
```
+----------+-----------------------+------+-----+---------+-------+
| Field    | Type                  | Null | Key | Default | Extra |
+----------+-----------------------+------+-----+---------+-------+
| number   | mediumint(8) unsigned | NO   | PRI | NULL    |       |
| xPos     | float                 | NO   |     | NULL    |       |
| yPos     | float                 | NO   |     | NULL    |       |
| xSize    | float                 | NO   |     | NULL    |       |
| ySize    | float                 | NO   |     | NULL    |       |
| shape    | tinyint(4)            | NO   |     | NULL    |       |
| level_id | int(10) unsigned      | NO   | PRI | NULL    |       |
+----------+-----------------------+------+-----+---------+-------+
```
Run command:
```
SELECT * FROM objects WHERE level_id = 1;
```
ie.: Data stored for the first level.
```
+--------+---------+----------+-------+-------+-------+----------+
| number | xPos    | yPos     | xSize | ySize | shape | level_id |
+--------+---------+----------+-------+-------+-------+----------+
|      0 | 7.65462 | -5.20508 |   1.2 |   1.2 |     1 |        1 |
|      1 | 8.86926 | -2.56029 |   1.2 |   1.2 |     1 |        1 |
|      2 | 7.88372 | 0.980017 |   1.2 |   1.2 |     0 |        1 |
|      3 | 6.12419 | -2.58474 |   1.2 |   1.2 |     0 |        1 |
+--------+---------+----------+-------+-------+-------+----------+
```
levels tabel:
```
+-------------+------------------+------+-----+---------+----------------+
| Field       | Type             | Null | Key | Default | Extra          |
+-------------+------------------+------+-----+---------+----------------+
| level_id    | int(10) unsigned | NO   | PRI | NULL    | auto_increment |
| level_start | float            | NO   |     | 0       |                |
| level_end   | float            | NO   |     | 20      |                |
| speed       | float            | NO   |     | 1       |                |
+-------------+------------------+------+-----+---------+----------------+
```

In our connect.php, we connect our applciation to the sql database "test"

```php
<?php 

DEFINE('DB_USER', 'playerweb');
DEFINE('DB_PASSWORD',  'nemo');
DEFINE('DB_HOST', 'localhost');
DEFINE('DB_NAME', 'test');

$dbc = @mysqli_connect(DB_HOST, DB_USER, DB_PASSWORD, DB_NAME)
OR die('Could not connect ' .
	mysqli_connect_error() );

?>
```

To save and modify data about tables or progress we use the update.php file

```php
<?php 
	session_start();
	require_once('../connect.php');

	if($_POST)
	{
		$id = $_COOKIE['id'];
		$max_level = $_POST['maxLevel'] ?? 0;
		if( $_SESSION['max_level'] < $max_level ){
			$query = "UPDATE players SET max_level = '$max_level' WHERE player_id = \"$id\"";
			mysqli_query($dbc, $query);
		}
	}
 ?>
```

In the save.js file we control the saving and loading of current existing progress made.
```javascript
function loadLevel()
{
	var u = "map.php?levelNo=" + levelNo +"&type=0";
	$.ajax({
	    url : u,
	    type: "GET",
	    dataType : "json",
	    success: function(data){
	    	if(data[0]){
		    	intervals = [Number(data[0]["level_start"]), Number(data[0]["level_end"])];
		    	animation.a = Number(data[0]["level_start"]);
		    	animation.b = Number(data[0]["level_end"]);
	    	}
	    	else{
	    		intervals = [-10, 10];
	    	}
	    	obstacles = [];
	    	while(data[1][obstacles.length])
	    	{
	    		if(data[1][obstacles.length]["shape"]==0)
	    			obstacles[obstacles.length] = new Ellipse(Number(data[1][obstacles.length]["xPos"]), Number(data[1][obstacles.length]["yPos"]), 
													Number(data[1][obstacles.length]["xSize"]), Number(data[1][obstacles.length]["ySize"]));
	    		else if(data[1][obstacles.length]["shape"]==1)
	    			obstacles[obstacles.length] = new Rect(Number(data[1][obstacles.length]["xPos"]), Number(data[1][obstacles.length]["yPos"]), 
													Number(data[1][obstacles.length]["xSize"]), Number(data[1][obstacles.length]["ySize"]));
	    	}
	    }
	});
}

function saveLevel()
{
	var u = "map.php?levelNo=" + levelNo +"&type=1";
	var save = [];	
	for(let i=0; i<obstacles.length; i++)
	{
		save[i] = [ i, obstacles[i].xPos, obstacles[i].yPos,
					obstacles[i].scalex, obstacles[i].scaley, obstacles[i].type];
	}
	console.log(save);
	$.ajax({
		url: u,
		type: "post",
		data:{
			objects: save,
			objectsNumber: obstacles.length,
			level: [intervals[0], intervals[intervals.length-1]]
		},
		success:function(data)
		{
			console.log(data);
		},
		error:function(xhr, status, error)
		{
			console.error(xhr);
		}
	});
```

File controller.php centarlises and stores cookies and data related to active user playing.

```php
<?php 
	require_once("controller.Class.php");
	require_once("config.php");
	
	if(isset($_GET["code"]))
		$token = $gClient->fetchAccessTokenWithAuthCode($_GET["code"]);
	else{


		$Controller = new Controller;
		echo $Controller ->insertGuest();

		header('Location: index.php');
		exit();
	}

	if(isset($token["error"]) != "invalid_grant"){
		$oAuth = new Google_Service_Oauth2($gClient);
		$userData = $oAuth->userinfo_v2_me->get();

		//insert data
		$Controller = new Controller;
		echo $Controller->insertData(array(
			'email' => $userData["email"],
			'name' => $userData["givenName"],	 
		 ));
	}
	else{
		header("Location: index.php");
		exit();
	}
 ?>
```

In file index.php we do user login (via guest or google-api) and possible (dev-only) restricted access to modifying existing levels

```php
<?php 
	require_once("config.php");
 ?>

<!DOCTYPE html>
<html>
	<head>
		<meta charset="UTF-8">
		<title>GAME_NAME</title>
		<link rel="stylesheet" href="index_style.css">
		<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css">
		<script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.2.1.min.js"></script > 
		<script src="https://cdn.jsdelivr.net/npm/js-cookie@rc/dist/js.cookie.min.js"></script>
		<script type="text/javascript" language="javascript" src="index_script.js"></script>
	</head>
	<body>
		<div id="loader"></div>
		<div id="cont">
		<button id="play" onclick="play()" onmouseout="reset()">PLAY</button>
		<button id="settings">SETTINGS</button>
		<button id="edit"  onclick="edit()"  onmouseout="reset()">EDIT</button>
		</div>
		<div id="icon" onclick="toggle()">
			<img id="icon_image" src="sprites/user.png">
		</div>
		<div id="pannel">

		<?php if( ! ( isset($_COOKIE['id']) || isset($_COOKIE['sess']) ) ){ ?>

			<button onclick="window.location ='<?php echo $login_url; ?>' "  type="button" id="ggl">
			<i class="fa fa-google" aria-hidden="true"></i> Login </button>
			<p> Or </p>
			<button onclick="window.location = 'controller.php' " id="guest" type="button">Guest Login</button>

		<?php }else{ 
			require_once('../connect.php');

			$id = $_COOKIE['id'];
			$query = "SELECT name FROM players WHERE player_id = \"$id\" ";
			$response = @mysqli_query($dbc, $query);
			$row = mysqli_fetch_array($response);

			echo "<p style='color:rgb(45, 46, 43);'>Logged in as:</p>";
			?>
			<form id="changeName" action="name.php" onsubmit="return test()" method="get">
				<input spellcheck="false" autocomplete="off" type="text" id="name" name="name" value = '<?php echo $row["name"] ?>'></input>
				<input type="submit" id="ghost"></form>
			</form>

			<button id="logout" onclick="window.location = 'logout.php'">Logout</button>

		<?php } ?>
		</div>
	</body>
</html>
```

File map.php makes the actual link between the data extracted from the database and the arrays exsting in our application

```php
?php
	session_start();
	require_once('../connect.php');

	$level = $_GET['levelNo'] + 1;
	if($_GET['type'] == 0){
		$query = "select * from objects where level_id = $level";
		$response = @mysqli_query($dbc, $query);
		$rows = [];
		while($row = mysqli_fetch_array($response))
		{
			$rows[] = $row;
		}
		$query = "select level_start, level_end from levels where level_id = $level";
		$response = @mysqli_query($dbc, $query);
		$row = mysqli_fetch_array($response);

		$total = [$row, $rows];
		$myJson = json_encode($total);
		echo $myJson;
	}
	else if($_POST && $_GET['type'] == 1){
		$query = "delete from objects where level_id = $level;";
		mysqli_query($dbc, $query);

		if($_POST["objectsNumber"]!=0){
			foreach ($_POST["objects"] as $object) {
				$query = "INSERT INTO objects VALUES ($object[0], $object[1], $object[2], $object[3], $object[4], $object[5], $level)";
				mysqli_query($dbc, $query);
			}
		}
		$ls = $_POST['level'][0];
		$le = $_POST['level'][1];
		$query = "UPDATE levels SET level_start = $ls, level_end = $le WHERE level_id = $level";
		mysqli_query($dbc, $query) or die;

		echo "Success";
	}

 ?>
```

## 8. The rest

The remaining files in the project are not about the complexity of the project; insignifficant and only takes care of fromatting and other
useful realtions. We tried to portray a serious work-environment by adding additional comments on the sides (albeit in romanian), and naming
variables accordingly to make the collaboratrion of the application as seamless as possible. (just like at a company)
