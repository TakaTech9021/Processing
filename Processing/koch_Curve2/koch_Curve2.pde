int koch_step;
float koch_angle;
float koch_length;
int base_time=0;
int s=0;

void setup(){
  size(800,600);
  koch_angle=60;
  koch_length=400.0;
  koch_angle=radians(koch_angle);
  background(255);
  noStroke();
  fill(0,0,255);
  triangle(200,150,600,150,400,150+200*sqrt(3));
}

void newKoch(int step){
  // line1
  Koch_C(600.0,150.0,200.0,150.0,step);
  // line2
  Koch_C(400.0,150.0+200.0*sqrt(3),600.0,150.0,step);
  // line3
  Koch_C(200.0,150.0,400.0,150.0+200.0*sqrt(3),step);
}

void draw(){
  int time=millis()-base_time;
  if(time>=2000){
    newKoch(s);
    base_time=millis();
    s++;
  }
  if(s>=7){
    s=0;
    background(255);
    fill(0,0,255);
    triangle(200,150,600,150,400,150+200*sqrt(3));
  }
  if(frameCount<=750){
    saveFrame("frames/####.png"); 
  }
}

void Koch_C(float ax, float ay, float bx, float by, int n){
  float sx,sy,tx,ty,ux,uy;
  if(n>0){
    sx=ax+(bx-ax)/3;
    sy=ay+(by-ay)/3;
    tx=ax+(bx-ax)*2/3;
    ty=ay+(by-ay)*2/3;
    ux=sx+(tx-sx)*cos(koch_angle)-(ty-sy)*sin(koch_angle);
    uy=sy+(tx-sx)*sin(koch_angle)+(ty-sy)*cos(koch_angle);
    noStroke();
    fill(0,0,255);
    triangle(sx,sy,tx,ty,ux,uy);
    Koch_C(ax,ay,sx,sy,n-1);
    Koch_C(sx,sy,ux,uy,n-1);
    Koch_C(ux,uy,tx,ty,n-1);
    Koch_C(tx,ty,bx,by,n-1);
  }
}
