void setup(){
  size(500,500);
  background(255);
  colorMode(HSB);
}
void draw(){
  loadPixels();
  for(int i=0; i<width; i++){
    for(int j=0; j<height; j++){
      float a = (float(i)/250)+x;
      float b = (float(j)/250)+y;
      int r = keisan(a,b);
      if (r==-1){
        //stroke(0,255,0);
        //point(i,j);
        pixels[j*width+i]=color(0);
      }else{
        pixels[j*width+i]=color(map(r,1,50,0,255),255,255);
      }
    }
  }
  updatePixels();
}
float x=-1.5,y=-1;
int keisan(float a,float b){
  int result=-1;
  float d=0,e=0;
  for (int i=0; i<60; i++){
    float z_d=d,z_e=e;
    d=sq(z_d)-sq(z_e)+a;
    e=2*z_d*z_e+b;
    if(sqrt(sq(d)+sq(e))>2){
      result=i+1;
      break;
    }
  }
  return result;
}
    
