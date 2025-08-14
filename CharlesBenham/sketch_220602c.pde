float t,v;
int cnt=0;
void setup(){
  size(500,500);
  t=0;
  v=0.05;
}
void draw(){
  background(255);
  if (cnt%2==1){
    t+=v;
    if (9<=t && 25>=t){
      v+=0.02;
    }
  }else{
    //reset
    t=0;
    v=0.05;
  }
  fill(255);
  ellipse(250,250,300,300);
  fill(0);
  //rotate
  arc(250,250,300,300,t,PI+t);
  fill(255);
  ellipse(250,250,6,6);
  // arc
  strokeWeight(2);
  arc(250,250,285,285,-PI+t,-3*PI/4+t);
  arc(250,250,270,270,-PI+t,-3*PI/4+t);
  arc(250,250,255,255,-PI+t,-3*PI/4+t);
  arc(250,250,240,240,-PI+t,-3*PI/4+t);
  arc(250,250,225,225,-3*PI/4+t,-PI/2+t);
  arc(250,250,210,210,-3*PI/4+t,-PI/2+t);
  arc(250,250,195,195,-3*PI/4+t,-PI/2+t);
  arc(250,250,180,180,-3*PI/4+t,-PI/2+t);
  arc(250,250,165,165,-PI/2+t,-PI/4+t);
  arc(250,250,150,150,-PI/2+t,-PI/4+t);
  arc(250,250,135,135,-PI/2+t,-PI/4+t);
  arc(250,250,120,120,-PI/2+t,-PI/4+t);
  arc(250,250,105,105,-PI/4+t,t);
  arc(250,250,90,90,-PI/4+t,t);
  arc(250,250,75,75,-PI/4+t,t);
  arc(250,250,60,60,-PI/4+t,t);
  if(frameCount<=1000){
    saveFrame("frames/####.png");
  }
}
void keyPressed(){
  if(keyCode == ENTER){
    cnt+=1;
  }
}
void mousePressed(){
  cnt+=1;
}
