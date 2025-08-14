int p_x,p_y;
IntList tx;
IntList ty;
int n=0;
void setup(){
  tx = new IntList();
  ty = new IntList();
  frameRate(4);
  size(800,800);
  background(255,248,220);
  p_x=255;
  p_y=235;
  tx.append(p_x);
  ty.append(p_y);
  noStroke();
  fill(100,150);

}

void draw(){
 fill(100,150);
 float r,s;
 r=random(-1.0,1.0);
 s=random(-1.0,1.0);
 int k;
 if (r>0){
   if (s>0){
     k=1;
   }else{
     k=2;
   }
 }else{
   if (s>0){
     k=3;
   }else{
     k=4;
   }
 }
 n+=1;
 if (n%2==0){
    p_x+=40;
    p_y+=20;
    noStroke();
    ellipse(p_x,p_y,18,18);
    ellipse(p_x,p_y-15,5,5);
    ellipse(p_x+10,p_y-10,5,5);
    ellipse(p_x+15,p_y,5,5);
 }else{
    p_x+=20;
    p_y-=20*k;
    noStroke();
    ellipse(p_x,p_y,18,18);
    ellipse(p_x,p_y-15,5,5);
    ellipse(p_x+10,p_y-10,5,5);
    ellipse(p_x+15,p_y,5,5);
 }
 tx.append(p_x);
 ty.append(p_y);
 if(tx.size()==30){
   int x,y;
   x=tx.get(0);
   y=ty.get(0);
   noStroke();
   fill(255,248,220);
   ellipse(x,y,19,19);
   ellipse(x,y-15,6,6);
   ellipse(x+10,y-10,6,6);
   ellipse(x+15,y,6,6);
   tx.remove(0);
   ty.remove(0);
 }
 if(p_y<0){
   p_y=895;
 }
 if(p_x>780){
   p_x=-5;
 }
 if(frameCount<=750){
   saveFrame("frames/####.png"); 
 }
}
