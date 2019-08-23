#include "Keyboard.h"

/*qqqqwwwwewqewqeqwewqqqqqAAaaaaaaaaaa
   Layer Change Keys
   Change to layer 0 206
   Change to layer 1 207
   Change to layer 2 208
   Change to layer 3 209
   Change to layer 4 210
*/

byte none = 0;
byte changeLayer0 = 1;
byte changeLayer1 = 2;
byte changeLayer2 = 3;
byte changeLayer3 = 5;
byte changeLayer4 = 5;
char ctrlKey = KEY_LEFT_GUI;
byte layer = 0;  // max of 5 layers
int layers[5][6][8] =
{
  // Layer 000qqqqq
  {
    {KEY_ESC,  KEY_F1,  KEY_F2,  KEY_F3,  KEY_F4,  KEY_F5,  KEY_F6,  0},
    {96,  49,   50,   51,   52,   53,   54,   0},
    {KEY_TAB,  113,  119,  101,  114,  116,  0,    0},
    {KEY_CAPS_LOCK,  97,   115,  100,  102,  103,  178,  0},
    {KEY_LEFT_SHIFT,  122,  120,  99,   118,  98,   179,  0},
    {KEY_LEFT_CTRL,  131,  130,  0,    176,  32,   179,  0}
  },
  // Layer 1
  {
    {KEY_F7,  KEY_F8,  KEY_F9,  KEY_F10,  KEY_F11,  KEY_F12,  199,  0},
    {126,  49,   50,   51,   52,   53,   54,   0},
    {179,  113,  119,  101,  114,  116,  0,    0},
    {193,  97,   115,  100,  102,  103,  178,  0},
    {133,  122,  120,  99,   118,  98,   179,  0},
    {128,  131,  130,  0,    176,  32,   179,  0}
  },
  // Layer 2
  {
    {177,  194,  195,  196,  197,  198,  199,  0},
    {126,  49,   50,   51,   52,   53,   54,   0},
    {179,  113,  119,  101,  114,  116,  0,    0},
    {193,  97,   115,  100,  102,  103,  178,  0},
    {133,  122,  120,  99,   118,  98,   179,  0},
    {128,  131,  130,  0,    176,  32,   179,  0}
  },
  // Layer 3
  {
    {177,  194,  195,  196,  197,  198,  199,  0},
    {126,  49,   50,   51,   52,   53,   54,   0},
    {179,  113,  119,  101,  114,  116,  0,    0},
    {193,  97,   115,  100,  102,  103,  178,  0},
    {133,  122,  120,  99,   118,  98,   179,  0},
    {128,  131,  130,  0,    176,  32,   179,  0}
  },
  // Layer 4
  {
    {177,  194,  195,  196,  197,  198,  199,  0},
    {126,  49,   50,   51,   52,   53,   54,   0},
    {179,  113,  119,  101,  114,  116,  0,    0},
    {193,  97,   115,  100,  102,  103,  178,  0},
    {133,  122,  120,  99,   118,  98,   179,  0},
    {128,  131,  130,  0,    176,  32,   179,  0}
  }
};

int rows[6] = {A5, A4, A3, A2, A1, A0};

unsigned long waitTime[6][8] =
{
  {micros(), micros(), micros(), micros(), micros(), micros(), micros(), micros()},
  {micros(), micros(), micros(), micros(), micros(), micros(), micros(), micros()},
  {micros(), micros(), micros(), micros(), micros(), micros(), micros(), micros()},
  {micros(), micros(), micros(), micros(), micros(), micros(), micros(), micros()},
  {micros(), micros(), micros(), micros(), micros(), micros(), micros(), micros()},
  {micros(), micros(), micros(), micros(), micros(), micros(), micros(), micros()}
};
unsigned long waitwait = 100000;  // Using Gateron Green found 100ms to be a enough of a debouncing time.  This could be reduced for linear switches.


void setup()
{
  // open the serial port:
  Serial.begin(9600);
  //while (!Serial)
  { // wait for serial connection to be setup.
    ;
  }
  Serial.setTimeout(100);  // 10ms timeout instead of 1s

  // initialize control over the keyboard:
  Keyboard.begin();

  pinMode(A0, OUTPUT);
  pinMode(A1, OUTPUT);
  pinMode(A2, OUTPUT);
  pinMode(A3, OUTPUT);
  pinMode(A4, OUTPUT);
  pinMode(A5, OUTPUT);

  pinMode(2, INPUT_PULLUP);
  pinMode(3, INPUT_PULLUP);
  pinMode(4, INPUT_PULLUP);
  pinMode(5, INPUT_PULLUP);
  pinMode(6, INPUT_PULLUP);
  pinMode(7, INPUT_PULLUP);
  pinMode(8, INPUT_PULLUP);
  pinMode(9, INPUT_PULLUP);

  pinMode(13, OUTPUT);

  digitalWrite(13, HIGH);
  digitalWrite(A0, HIGH);
  digitalWrite(A1, HIGH);
  digitalWrite(A2, HIGH);
  digitalWrite(A3, HIGH);
  digitalWrite(A4, HIGH);
  digitalWrite(A5, HIGH);
}
//
void loop()
{
  /*
     Every 70 minuites of keyboard function there will be a short reset
     This reset is set to be 100ms long
     Reducing this number could cause issues of keyboard not being functinoal for 70 minuites.
  */
  if (micros() < 100000)
  {
    for (byte i = 0; i < 6; i++)
    {
      for (byte j = 0; j < 8; j++)
      {
        waitTime[i][j] = micros();
      }
    }
    Serial.write("Reset time");
  }
  
  if (Serial.available())
  {
    String command = Serial.readString();
    if (command.substring(0, 4) == "Name")
    {
      Serial.println("Left Keyboard"); 
    }
  }

  // This goes through of the rows, and scans it across each of the colums.
  for (byte i = 0; i <= 6; i++)
  {
    digitalWrite(rows[i], LOW);
    for (byte j = 2; j < 10; j++)
    {
      if (digitalRead(j) == 0)
      {
        if ((micros() - waitTime[i][j]) > waitwait)
        {
          Keyboard.press(layers[layer][i][j - 2]);
          waitTime[i][j] = micros();
          Serial.println("A button was pressed as ");
        }
      }
      else
      {
        Keyboard.release(layers[0][i][j - 2]);
      }
    }
    digitalWrite(rows[i], HIGH);
  }
}