

#include <Adafruit_NeoPixel.h>


byte g_ledPin = 6;
byte g_ledNum = 48;
byte g_rate = 200; // 200 rounds before colors change 
byte g_round = 1; // This would move up to rate.

struct led
{
  byte type;
  byte startColor[3];
  byte endColor[3];
} leds[6][8] =  
{
  { {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}} },
  { {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}} },
  { {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}} },
  { {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}} },
  { {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}} },
  { {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}}, {0, {255, 255, 255}, {0,0,0}} }
};
Adafruit_NeoPixel strip(g_ledNum, g_ledPin, NEO_GRB + NEO_KHZ800);


void setup()
{
  // Serial setup
  Serial.begin(9600);
  //Serial.setTimeout(10);

  strip.begin();
}

void loop()
{
  // Serial processing
  if (Serial.available() > 0)
  {
    String input = Serial.readString();
    if (input.startsWith("Name"))
    {
      Serial.println("Left Keyboard");
    }
    else if (input.startsWith("Rate"))
    {
      // Change animation speed
      
    }
    else if (input.startsWith("Set"))
    {
      
    }
    else if (input.startsWith("Get"))
    {
      
    }
    
  }
  
  // LED processing
  for (int i = 0; i < 6; i++)
  {
    for (int j = 0; j <8; j++)
    {
      uint32_t mycolor;
      
      if (leds[i][j].type == 0)
      { // Solid color 
        mycolor= strip.Color(leds[i][j].startColor[0], leds[i][j].startColor[1], leds[i][j].startColor[2]);
      }
      else if (leds[i][j].type == 1)
      { // Breathing
        // This breaths between two different colors. By default this second color is black or off.
        
        int green;
        int red;
        int blue;
        int y1;
        int y2;
        int diff;

        // Green        
        y1 = leds[i][j].startColor[0];
        y2  = leds[i][j].endColor[0];
        diff = abs(g1-g2)/2;
        green = (diff * cos(2*PI*((g_round/g_rate)) + (diff + min(g1, g2));
        // Red
        y1 = leds[i][j].startColor[1];
        y2  = leds[i][j].endColor[1];
        diff = abs(g1-g2)/2;
        red = (diff * cos(2*PI*((g_round/g_rate)) + (diff + min(g1, g2));
        // Blue
        y1 = leds[i][j].startColor[2];
        y2  = leds[i][j].endColor[2];
        diff = abs(g1-g2)/2;
        blue = (diff * cos(2*PI*((g_round/g_rate)) + (diff + min(g1, g2));

        mycolor= strip.Color(green, red, blue);
      }
      else if (leds[i][j].type == 3)
      {// Rainbow????
      }
      else if (leds[i][j].type == 4)
      {// Rainfall???
      }
      
      byte n = i*8 + j*6;
      //mycolor = strip.Color(leds[i][j].startColor[0], leds[i][j].startColor[1], leds[i][j].startColor[2]);
      strip.setPixelColor(n, mycolor);
    }
  }
  
  Serial.println("We did a cycle");
}



void colorWipe(uint32_t color) 
{
  for (int i = 0; i < strip.numPixels(   ); i++) 
  {
    strip.setPixelColor(i, (255, 0, i));
    strip.show();
  }
}

void rainbow() 
{
  for(long firstPixelHue = 0; firstPixelHue < 5*65536; firstPixelHue += 256) 
  {
    for(int i=0; i<strip.numPixels(); i++) 
    {
      int pixelHue = firstPixelHue + (i * 65536L / strip.numPixels());
      strip.setPixelColor(i, strip.gamma32(strip.ColorHSV(pixelHue)));
    }
    strip.show();
  }
}
