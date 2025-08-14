float init=10;
float[] o={0,0};
float[] next_o={init,0};
float fib = init;
float next_fib=init;
float t=0;
int n=0;

void setup(){
  size(890,550);
  colorMode(HSB,1);
  stroke(0);
}

void draw(){
  if (t%10==0 && n<11){
    drawFibonacciRect();
  }
  t++;
}

void drawFibonacciRect(){
  float x = o[0];
  float y = o[1];
  float length=fib;
  fill(n/15.0,0.5,0.9);
  rect(x,y,length,length);
  o[0]=next_o[0];
  o[1]=next_o[1];
  if(n%2==1){
    next_o[0]=x+length;
    next_o[1]=y;
  }else{
    next_o[0]=x;
    next_o[1]=y+length;
  }
  fib=next_fib;
  next_fib+=length;
  n++;
 if(frameCount<=750){
   saveFrame("frames/####.png"); 
 }
}
