float[] xs;
float[] ys;

float[] u;
float[] k;
float[] v;

float[] r;
float[] g;
float[] b;

void setup(){
  size(800,800);
  xs = new float[300];
  ys = new float[300];
  
  u = new float[300];
  k = new float[300];
  v = new float[300];
  
  r = new float[300];
  g = new float[300];
  b = new float[300];
  
  for (int i=0; i<300; i++){
    float s=random(800);
    float t=random(800);
    xs[i]=s;
    ys[i]=t;
  }
  for (int i=0; i<300; i++){
    u[i]=random(-1,1);
    k[i]=random(-1,1);
    v[i]=random(-1,1);
    r[i]=random(255);
    g[i]=random(255);
    b[i]=random(255);
  }
}

void draw_eraser(){
  stroke(0);
  strokeWeight(1);
  fill(255);
  rect(mouseX,mouseY,160,75);
  fill(0,0,250);
  rect(mouseX+55,mouseY,125,25);
  fill(255);
  rect(mouseX+55,mouseY+25,125,25);
  fill(0);
  rect(mouseX+55,mouseY+50,125,25);
}

void draw(){
  background(255);

  for (int i=0; i<300; i++){
    stroke(r[i],g[i],b[i]);
    strokeWeight(2);
    if (u[i]>0){
      if (k[i]>0){
        if (v[i]>0){
          line(xs[i],ys[i],xs[i]+20,ys[i]+20);
        }else{
          line(xs[i],ys[i],xs[i]-20,ys[i]+20);
        }
      }else{
        if (v[i]>0){
          line(xs[i],ys[i],xs[i],ys[i]-20);
        }else{
          line(xs[i],ys[i],xs[i],ys[i]+20);
        }
      }
    }else{
      if (k[i]>0){
        if (v[i]>0){
          line(xs[i],ys[i],xs[i]+20,ys[i]-20);
        }else{
          line(xs[i],ys[i],xs[i]-20,ys[i]-20);          
        }
      }else{
        if (v[i]>0){
          line(xs[i],ys[i],xs[i]+20,ys[i]);
        }else{
          line(xs[i],ys[i],xs[i]-20,ys[i]);
        }
      }
    }
    if (mouseX-20<xs[i] && xs[i]<mouseX && mouseY-20<ys[i] && ys[i]<mouseY){
      r[i]=255;
      g[i]=255;
      b[i]=255;
    }
  }
  draw_eraser();
   if(frameCount<=2000){
    saveFrame("frames/####.png"); 
  }
}
