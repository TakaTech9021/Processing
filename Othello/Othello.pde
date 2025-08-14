int board[][] = new int[10][10]; // 10x10 board
int bw;      //color of stone  first move is 1 second move is -1
int pass,side;    //number of pass, length of grid
int num0,numB,numW;   //num of place where you can put stone, num of black, num of white

void setup(){
  size(400,400);
  side=height/8;  // length of grid
  
  startPosition();  //initialize setting
  showBoard();    //draw board
}

// initialize setting
void startPosition() {
  bw=1;  // first move (black) is 1, second move (white) is -1
  
  //initialize stone position
  for (int i=0; i<=9; i++){
    for (int j=0; j<=9; j++){
      if ((i==4&&j==5) || (i==5&&j==4)) {board[i][j]=1;}  // black
      else if ((i==4&&j==4) || (i==5&&j==5)) {board[i][j]=-1;}  // white
      else if (i==0||j==0||i==9||j==9) {board[i][j]=2;}   // Around board is 2
      else {board[i][j]=0;}  // empty grid is 0
    }
  }
}

//draw board and stone
void showBoard(){
  //board(background and grid)
  background(0,160,0);
  stroke(0);
  for(int i=1;i<=8;i++){
    line(i*side,0, i*side,height); //vertical line
    line(0,i*side, width,i*side); //horizontal line
  }
  
  //draw stone, mark place where you can put stone
  noStroke();
  num0=numB=numW=0;
  for (int i=1;i<=8;i++){
    for (int j=1;j<=8;j++){
      if (board[i][j]==1){  // black(1)
        fill(0);
        ellipse((i-1)*side + side/2, (j-1)*side + side/2, 0.9*side, 0.9*side);
        numB++;
      }
      else if (board[i][j]==-1){  // white(-1)
        fill(255);
        ellipse((i-1)*side + side/2, (j-1)*side + side/2, 0.9*side, 0.9*side);
        numW++;
      }
      else if (validMove(i,j)){  // legal move
        pass=0;  // reset num of pass
        num0++;  // count legal move
        
        if(bw==-1){fill(255, 255, 255, 200);}
        else if(bw==1){fill(0, 0, 0, 200);}
        ellipse((i-1)*side + side/2, (j-1)*side + side/2, side/3, side/3);
      }
    }
  }     
}
void mousePressed(){
  int i = floor(mouseX/side +1);  // define position of grid
  int j = floor(mouseY/side +1);  // truncate decimal point
  
  //when you can put stone
  if (validMove(i,j)){
    movePiece(i,j);
    bw = -bw;  // change color of stone
    showBoard();  // call function which draw board
  }
}
//judge whether you can put stone, scanning 8 direction around grid
boolean validMove(int i, int j){
  if(i<1|8<i || j<1||8<j) {return false;} // you can't put board out
  if (board[i][j]!=0) {return false;} // you can't put without empty 
  
  //judge whether you can put stone, scanning 8 direction around grid
  int ri, rj;  // variable to scan 8 direction around grid
  for (int di=-1; di<=1; di++){  //horizontal direction
    for (int dj=-1; dj<=1; dj++){  //vertical direction
      ri=i+di; rj=j+dj;  // initialize scan grid
      
      // If scanning grid is opponent stone, continue scanning
      while (board[ri][rj]==-bw){
        ri+=di; rj+=dj;  // move next grid
        
        //when you can put stone
        if (board[ri][rj]==bw) {return true;} 
      }
    }
  }
  
  return false;
}
//put stone, reverse stone in 8 direction
void movePiece(int i, int j){
  board[i][j] = bw; // put stone
  
  int ri, rj;  //variable to scan 8 direction around grid
  for (int di=-1; di<=1; di++){  //horizontal direction
    for (int dj=-1; dj<=1; dj++){  //vertical direction
      ri=i+di; rj=j+dj;  //initialize scanning grid
      
      // If scanning grid is opponent stone, continue scanning
      while (board[ri][rj]==-bw){
        ri+=di; rj+=dj;  // move next grid
        
        // when facing equal color stone, reverse stone 
        if (board[ri][rj]==bw){
          ri-=di; rj-=dj;  // back 1 grid
          
          while (!(i==ri&&j==rj)){ // back to base grid
            board[ri][rj] = bw;
            ri-=di; rj-=dj; // back 1 grid
          }
        }
      }
    }
  }   
}
void draw(){
  passCheck();  //judge pass
}
//judge pass
void passCheck(){
  // If you can't put stone anywhere, Pass is valid
  if (num0==0 && pass<=1){
    pass++;  // count num of pass
    bw = -bw;  // reverse color of stone
    showBoard();
  }
  // If pass is two times, judge win or lose
  if (pass==2){
    fill(255,0,0);
    textSize(1.0*side);  // size of character
    textAlign(CENTER);
    
    if (numW<numB){text("Black win", width/2,height/2);} // win first move
    else if (numB<numW){text("White win", width/2,height/2);}  // win second move
    else {text("Draw", width/2,height/2);}  //draw
  }
}
